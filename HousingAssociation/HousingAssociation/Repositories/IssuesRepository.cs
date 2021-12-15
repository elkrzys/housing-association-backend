using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace HousingAssociation.Repositories
{
    public class IssuesRepository
    {
        private readonly DbSet<Issue> _issues;
        public IssuesRepository(AppDbContext dbContext)
        {
            _issues = dbContext.Issues;
        }
        public async Task<bool> CheckIfExistsAsync(IssueDto issueDto)
            => await _issues
                .Include(i => i.Local)
                .AnyAsync(i =>
                i.Title.Equals(issueDto.Title) &&
                i.Content.Equals(issueDto.Content) &&
                i.SourceLocalId == issueDto.SourceLocalId &&
                i.Local.BuildingId == issueDto.SourceBuildingId);
        public async Task AddAsync(Issue issue) => await _issues.AddAsync(issue);
        public void Update(Issue issue) => _issues.Update(issue);
        public void Delete(Issue issue) => _issues.Remove(issue);
        public async Task<Issue> FindByIdAsync(int id)
            => await _issues
                .Include(issue => issue.Local)
                    .ThenInclude(l => l.Building)
                    .ThenInclude(b => b.Address)
                .SingleOrDefaultAsync(issue => issue.Id == id);
        public async Task<List<Issue>> FindAllAsync() => await _issues.ToListAsync();
        public async Task<List<Issue>> FindAllNotCancelledAsync() 
            => await _issues
                .Include(issue => issue.Local)
                    .ThenInclude(l => l.Building)
                        .ThenInclude(b => b.Address)
                .Where(issue => issue.Cancelled == null)
                .ToListAsync();
        public async Task<List<Issue>> FindAllBySourceBuildingIdAsync(int buildingId) =>
            await _issues
                .Include(issue => issue.Local)
                    .ThenInclude(issue => issue.Building)
                        .ThenInclude(b => b.Address)
                .Where(issue => issue.Local.BuildingId == buildingId)
                .ToListAsync();
        public async Task<List<Issue>> FindAllByAuthorIdAsync(int authorId) =>
            await _issues
                .Include(issue => issue.Local)
                    .ThenInclude(issue => issue.Building)
                        .ThenInclude(b => b.Address)
                .Where(issue => issue.AuthorId == authorId)
                .ToListAsync();
    }
}