﻿using System;
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
            var issue = new Issue
            {
                Title = issueDto.Title,
                Content = issueDto.Content,
                SourceLocalId = issueDto.SourceLocalId,
                AuthorId = issueDto.AuthorId
            };
            await _unitOfWork.IssuesRepository.AddAsync(issue with {Created = DateTime.Now});
            await _unitOfWork.CommitAsync();
        }

        public async Task<IssueDto> GetById(int id)
        {
            var issue = await _unitOfWork.IssuesRepository.FindByIdAsync(id) ?? throw new NotFoundException();
            return issue.AsDto();
        }
        
        public async Task<List<IssueDto>> GetAllActual()
        {
            var issues = await _unitOfWork.IssuesRepository.FindAllNotCancelledAsync();
            return GetIssuesAsDtos(issues);
        }
        
        public async Task<List<IssueDto>> GetAllByAuthorId(int authorId)
        {
            var issues = await _unitOfWork.IssuesRepository.FindAllByAuthorIdAsync(authorId);
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
            _unitOfWork.IssuesRepository.Update(issue with {Cancelled = DateTimeOffset.Now});
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
            _unitOfWork.IssuesRepository.Update(issue with {Resolved = DateTimeOffset.Now});
            await _unitOfWork.CommitAsync();
        }
        
        private List<IssueDto> GetIssuesAsDtos(List<Issue> issues)
        {
            return issues.Select(issue => issue.AsDto()).ToList();
        }
    }
}