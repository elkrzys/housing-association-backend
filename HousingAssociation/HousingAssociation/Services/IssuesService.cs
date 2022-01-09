using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.ExceptionHandling.Exceptions;
using HousingAssociation.Models.DTOs;
using HousingAssociation.Utils.Extensions;
using Serilog;

namespace HousingAssociation.Services
{
    public class IssuesService
    {
        private readonly IUnitOfWork _unitOfWork;
        public IssuesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddIssue(IssueDto issueDto)
        {
            if (await _unitOfWork.IssuesRepository.CheckIfExistsAsync(issueDto))
            {
                Log.Warning($"Issue already exists.");
                throw new BadRequestException("Issue already exists");
            }
            if (issueDto.Author is null)
            {
                Log.Warning($"Issue author is null");
                throw new BadRequestException("Issue author is null");
            }
            
            var issue = new Issue
            {
                Title = issueDto.Title,
                Content = issueDto.Content,
                SourceLocalId = issueDto.SourceLocalId,
                AuthorId = issueDto.Author.Id
            };
            await _unitOfWork.IssuesRepository.AddAsync(issue with {Created = DateTime.Now});
            await _unitOfWork.CommitAsync();
        }
        
        public async Task UpdateIssue(IssueDto issueDto)
        {
            if (issueDto.Id is null)
            {
                Log.Warning("Bad request: issue Id is null.");
                throw new BadRequestException("Issue incorrect");
            }
            if (issueDto.Author is null)
            {
                Log.Warning("Bad request: author is null.");
                throw new BadRequestException("Issue author incorrect");
            }
            
            var issue = new Issue
            {
                Title = issueDto.Title,
                Content = issueDto.Content,
                SourceLocalId = issueDto.SourceLocalId,
                AuthorId = issueDto.Author.Id,
                PreviousIssueId = issueDto.Id
            };
            
            await _unitOfWork.OuterTransaction(async () =>
            {
                await CancelIssue(issueDto.Id!.Value);
                await _unitOfWork.IssuesRepository.AddAsync(issue with {Created = DateTime.Now});
            });
        }

        public async Task<IssueDto> GetById(int id)
        {
            var issue = await _unitOfWork.IssuesRepository.FindByIdAsyncWithDetails(id) ?? throw new NotFoundException();
            return issue.AsDto();
        }
        
        public async Task<List<IssueDto>> GetAllActual()
        {
            var issues = await _unitOfWork.IssuesRepository.FindAllNotCancelledAsync();
            return GetIssuesAsDtos(issues);
        }
        
        public async Task<List<IssueDto>> GetAllByAuthorId(int authorId)
        {
            var author = await _unitOfWork.UsersRepository.FindByIdAndIncludeAllLocalsAsync(authorId);
            if (author is null)
            {
                Log.Warning($"Issue author with id = {authorId} not found.");
                throw new NotFoundException();
            }
            var localsIds = author.ResidedLocals.Select(local => local.Id);
            var issues = await _unitOfWork.IssuesRepository.FindAllByResidentLocals(localsIds);
            return GetIssuesAsDtos(issues);
        }
        
        public async Task CancelIssue(int id)
        {
            var issue = await _unitOfWork.IssuesRepository.FindByIdAsync(id);
            if(issue is null)
            {
                Log.Information($"Issue with id = {id} doesn't exist.");
                throw new NotFoundException();
            }
            issue.Cancelled = DateTimeOffset.Now;
            await _unitOfWork.CommitAsync();
        }
        
        public async Task ResolveIssue(int id)
        {
            var issue = await _unitOfWork.IssuesRepository.FindByIdAsync(id);
            if (issue is null)
            {
                Log.Warning($"Issue with id = {id} doesn't exist.");
                throw new NotFoundException();
            }

            issue.Resolved = DateTimeOffset.Now;
            await _unitOfWork.CommitAsync();
        }
        
        private List<IssueDto> GetIssuesAsDtos(List<Issue> issues)
        {
            return issues.Select(issue => issue.AsDto()).ToList();
        }
    }
}