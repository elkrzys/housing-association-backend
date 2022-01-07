using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HousingAssociation.Repositories
{
    public class DocumentsRepository
    {
        private readonly DbSet<Document> _documents;
        public DocumentsRepository(AppDbContext dbContext)
        {
            _documents = dbContext.Documents;
        }
        public async Task AddAsync(Document document) => await _documents.AddAsync(document);
        public async Task<List<Document>> FindAllAsync() => await _documents.ToListAsync();
        public async Task<Document> FindByIdAsync(int id) => await _documents.FindAsync(id);
        public async Task<List<Document>> FindAllByAuthorIdAsync(int authorId)
            => await _documents
                .Include(document => document.Author)
                .Where(document => document.AuthorId == authorId)
                .ToListAsync();
        public async Task<List<Document>> FindAllWhereReceiversNotEmpty()
            => await _documents
                .Include(document => document.Author)
                .Include(document => document.Receivers)
                .Where(document => document.Receivers.Count > 0)
                .ToListAsync();
        public async Task<List<Document>> FindAllWhereReceiversEmpty()
            => await _documents
                .Include(document => document.Author)
                .Include(document => document.Receivers)
                .Where(document => document.Receivers.Count == 0)
                .ToListAsync();
        public async Task<List<Document>> FindAllByReceiverAsync(int receiverId)
            => await _documents
                .Include(document => document.Author)
                .Include(document => document.Receivers)
                .Where(document => document.Receivers.Any(r => r.Id == receiverId))
                .ToListAsync();
        public async Task<Document> FindByHashAsync(string hash)
            => await _documents
                .Include(document => document.Author)
                .Include(document => document.Receivers)
                .SingleOrDefaultAsync(document => document.Md5.Equals(hash));
        public async Task<List<string>> FindAllDocumentHashes()
            => await _documents.Select(d => d.Md5).ToListAsync();
        public void DeleteDocument(Document document) => _documents.Remove(document);
    }
}