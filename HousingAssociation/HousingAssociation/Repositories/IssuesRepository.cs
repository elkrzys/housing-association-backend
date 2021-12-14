using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HousingAssociation.Repositories
{
    public class IssuesRepository
    {
        private readonly DbSet<Issue> _issues;
        //public AppDbContext Context { get; }

        public IssuesRepository(AppDbContext dbContext)
        {
            //Context = dbContext;
            _issues = dbContext.Issues;
        }
        public async Task<bool> CheckIfExistsAsync(Issue issue)
            => await _issues.AnyAsync(i =>
                i.Title.Equals(issue.Title) &&
                i.Content.Equals(issue.Content) &&
                i.SourceLocalId == issue.SourceLocalId &&
                i.SourceBuildingId == issue.SourceBuildingId);
        public async Task AddAsync(Issue issue)
        {
            await _issues.AddAsync(issue);
            //await Context.SaveChangesAsync();
        }

        public void Update(Issue issue) => _issues.Update(issue);
        public void Delete(Issue issue) => _issues.Remove(issue);
        public async Task<Issue> FindByIdAsync(int id) => await _issues.FindAsync(id);
        public async Task<List<Issue>> FindAllAsync() => await _issues.ToListAsync();
        public async Task<List<Issue>> FindAllNotCancelledAsync() 
            => await _issues
                .Where(issue => !issue.IsCancelled)
                .ToListAsync();
        public async Task<List<Issue>> FindAllBySourceBuildingIdAsync(int buildingId) =>
            await _issues.Where(issue => issue.SourceBuildingId == buildingId).ToListAsync();
        public async Task<List<Issue>> FindAllByAuthorIdAsync(int authorId) =>
            await _issues.Where(issue => issue.AuthorId == authorId).ToListAsync();
    }
}