﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using HousingAssociation.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HousingAssociation.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        public AppDbContext Context { get; }

        public UnitOfWork(AppDbContext context)
        {
            Context = context;
        }
        public void Commit()
        {
            Context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await Context.SaveChangesAsync();
        }

        public void SetModified<T>(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }
        public void SetModified<T>(List<T> entities)
        {
            entities.ForEach(entity => Context.Entry(entity).State = EntityState.Modified);
        }

        public async Task OuterTransaction(Func<Task> methodToInvoke)
        {
            await Context.Database.BeginTransactionAsync();
            await methodToInvoke();
            await Context.SaveChangesAsync();
            await Context.Database.CommitTransactionAsync();
        }

        private AddressesRepository _addressesRepository;
        private BuildingsRepository _buildingsRepository;
        private LocalsRepository _localsRepository;
        private UsersRepository _usersRepository;
        private UserCredentialsRepository _usersCredentialsRepository;
        private AnnouncementsRepository _announcementsRepository;
        private DocumentsRepository _documentsRepository;
        private IssuesRepository _issuesRepository;
        public AddressesRepository AddressesRepository => _addressesRepository ??= new AddressesRepository(Context);
        public BuildingsRepository BuildingsRepository => _buildingsRepository ??= new BuildingsRepository(Context);
        public LocalsRepository LocalsRepository => _localsRepository ??= new LocalsRepository(Context);
        public UsersRepository UsersRepository => _usersRepository ??= new UsersRepository(Context);
        public UserCredentialsRepository UserCredentialsRepository => _usersCredentialsRepository ??= new UserCredentialsRepository(Context);
        public AnnouncementsRepository AnnouncementsRepository => _announcementsRepository ??= new AnnouncementsRepository(Context);
        public DocumentsRepository DocumentsRepository => _documentsRepository ??= new DocumentsRepository(Context);
        public IssuesRepository IssuesRepository => _issuesRepository ??= new IssuesRepository(Context);
    }
}