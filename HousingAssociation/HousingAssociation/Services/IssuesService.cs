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

            if (await _unitOfWork.IssuesRepository.CheckIfExistsAsync(issue))
            {
                throw new BadRequestException("Issue already exists");
            }

            await _unitOfWork.IssuesRepository.AddAsync(issue with {Created = DateTime.Now});
            await _unitOfWork.CommitAsync();
        }

        public async Task<IssueDto> GetById(int id)
        {
            var issue = await _unitOfWork.IssuesRepository.FindByIdAsync(id) ?? throw new NotFoundException();
            return issue.AsDto();
        }
        
        public async Task<List<IssueDto>> GetAllNotCancelled()
        {
            var issues = await _unitOfWork.IssuesRepository.FindAllNotCancelledAsync();
            return await GetIssuesAsDtos(issues);
        }
        
        public async Task<List<IssueDto>> GetAllByAuthorId(int authorId)
        {
            var issues = await _unitOfWork.IssuesRepository.FindAllByAuthorIdAsync(authorId);
            return await GetIssuesAsDtos(issues);
        }
        
        public async Task CancelIssue(int id)
        {
            var issue = await _unitOfWork.IssuesRepository.FindByIdAsync(id) ?? throw new NotFoundException();
            _unitOfWork.IssuesRepository.Update(issue with {Cancelled = DateTimeOffset.Now});
            await _unitOfWork.CommitAsync();
        }
        
        public async Task ResolveIssue(int id)
        {
            var issue = await _unitOfWork.IssuesRepository.FindByIdAsync(id) ?? throw new NotFoundException();
            _unitOfWork.IssuesRepository.Update(issue with {Resolved = DateTimeOffset.Now});
            await _unitOfWork.CommitAsync();
        }
        
        private async Task<List<IssueDto>> GetIssuesAsDtos(List<Issue> issues)
        {
            List<IssueDto> issuesDtos = new();
            foreach (var issue in issues)
            {
                var issueBuilding = await _unitOfWork.BuildingsRepository.FindByIdAsync(issue.SourceBuildingId);
                issuesDtos.Add(issue.AsDto());
            }
            return issuesDtos;
        }
    }
}