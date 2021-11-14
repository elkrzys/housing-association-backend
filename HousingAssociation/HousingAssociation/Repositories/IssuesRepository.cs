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

        public IssuesRepository(AppDbContext dbContext)
        {
            _issues = dbContext.Issues;
        }

        public async Task Add(Issue issue) => await _issues.AddAsync(issue);
        
        public async Task Update(Issue issue)
        {
            var existingIssue = await _issues.FindAsync(issue.Id);
            if (existingIssue is not null)
            {
                _issues.Update(issue);
            }
        }

        public void Delete(Issue issue) => _issues.Remove(issue);

        public async Task<Issue> FindIssueById(int id) => await _issues.FindAsync(id);

        public async Task<List<Issue>> FindAll() => await _issues.ToListAsync();

        public async Task<List<Issue>> FindAllBySourceBuildingId(int buildingId) =>
            await _issues.Where(issue => issue.SourceBuildingId == buildingId).ToListAsync();

    }
}