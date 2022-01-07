using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using HousingAssociation.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HousingAssociation.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public async Task CommitAsync() => await _context.SaveChangesAsync();
        public void SetModified<T>(T entity) => _context.Entry(entity).State = EntityState.Modified;
        public void SetModified<T>(List<T> entities) => entities.ForEach(entity => _context.Entry(entity).State = EntityState.Modified);

        public async Task OuterTransaction(Func<Task> methodToInvoke)
        {
            await _context.Database.BeginTransactionAsync();
            await methodToInvoke();
            await _context.SaveChangesAsync();
            await _context.Database.CommitTransactionAsync();
        }

        private AddressesRepository _addressesRepository;
        private BuildingsRepository _buildingsRepository;
        private LocalsRepository _localsRepository;
        private UsersRepository _usersRepository;
        private UserCredentialsRepository _usersCredentialsRepository;
        private AnnouncementsRepository _announcementsRepository;
        private DocumentsRepository _documentsRepository;
        private IssuesRepository _issuesRepository;
        public AddressesRepository AddressesRepository => _addressesRepository ??= new AddressesRepository(_context);
        public BuildingsRepository BuildingsRepository => _buildingsRepository ??= new BuildingsRepository(_context);
        public LocalsRepository LocalsRepository => _localsRepository ??= new LocalsRepository(_context);
        public UsersRepository UsersRepository => _usersRepository ??= new UsersRepository(_context);
        public UserCredentialsRepository UserCredentialsRepository => _usersCredentialsRepository ??= new UserCredentialsRepository(_context);
        public AnnouncementsRepository AnnouncementsRepository => _announcementsRepository ??= new AnnouncementsRepository(_context);
        public DocumentsRepository DocumentsRepository => _documentsRepository ??= new DocumentsRepository(_context);
        public IssuesRepository IssuesRepository => _issuesRepository ??= new IssuesRepository(_context);
    }
}