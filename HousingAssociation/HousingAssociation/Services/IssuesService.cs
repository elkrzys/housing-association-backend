using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.ExceptionHandling.Exceptions;
using HousingAssociation.Models.DTOs;
using HousingAssociation.Repositories;
using HousingAssociation.Utils.Extensions;

namespace HousingAssociation.Services
{
    public class IssuesService
    {
        private readonly IUnitOfWork _unitOfWork;
        // private readonly IssuesRepository _issues;
        // private readonly AppDbContext _dbContext;

        public IssuesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            // _dbContext = dbContext;
            // _issues = issues;
        }

        public async Task AddIssue(IssueDto issueDto)
        {
            var issue = new Issue
            {
                Title = issueDto.Title,
                Content = issueDto.Content,
                SourceBuildingId = issueDto.SourceBuildingId,
                SourceLocalId = issueDto.SourceLocalId,
                AuthorId = issueDto.AuthorId
            };

            //_dbContext.Database?.BeginTransactionAsync();
            if (await _unitOfWork.IssuesRepository.CheckIfExistsAsync(issue))
            {
                throw new BadRequestException("Issue already exists");
            }

            await _unitOfWork.IssuesRepository.AddAsync(issue with {CreatedAt = DateTime.Now});
            //_dbContext.Database?.CommitTransactionAsync();
            await _unitOfWork.CommitAsync();
        }

        // public async Task<IssueDto> GetById(int id)
        // {
        //     var issue = await _unitOfWork.IssuesRepository.FindByIdAsync(id) ?? throw new NotFoundException();
        //     return issue.AsDto();
        // }
        //
        // public async Task<List<IssueDto>> GetAllNotCancelled()
        // {
        //     var issues = await _unitOfWork.IssuesRepository.FindAllNotCancelledAsync();
        //     return GetIssuesAsDtos(issues);
        // }
        //
        // public async Task<List<IssueDto>> GetAllByAuthorId(int authorId)
        // {
        //     var issues = await _unitOfWork.IssuesRepository.FindAllByAuthorIdAsync(authorId);
        //     return GetIssuesAsDtos(issues);
        // }
        //
        // public async Task CancelIssue(int id)
        // {
        //     
        // }
        //
        // public async Task ResolveIssue(int id)
        // {
        //     
        // }
        //
        // private List<IssueDto> GetIssuesAsDtos(List<Issue> issues)
        // {
        //     List<IssueDto> issuesDtos = new();
        //     issues.ForEach(i => issuesDtos.Add(i.AsDto()));
        //     return issuesDtos;
        // }
    }
}