﻿using HousingAssociation.Repositories;

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

        public void Dispose()
        {
            Context.Dispose();
        }

        private AddressesRepository _addressesRepository;
        private BuildingsRepository _buildingsRepository;
        private UsersRepository _usersRepository;
        private UserCredentialsRepository _usersCredentialsRepository;
        private RefreshTokensRepository _refreshTokensRepository;
        private AnnouncementsRepository _announcementsRepository;
        private IssuesRepository _issuesRepository;
        public AddressesRepository AddressesRepository => _addressesRepository ??= new AddressesRepository(Context);
        public BuildingsRepository BuildingsRepository => _buildingsRepository ??= new BuildingsRepository(Context);
        public UsersRepository UsersRepository => _usersRepository ??= new UsersRepository(Context);
        public UserCredentialsRepository UserCredentialsRepository => _usersCredentialsRepository ??= new UserCredentialsRepository(Context);
        public RefreshTokensRepository RefreshTokensRepository => _refreshTokensRepository ??= new RefreshTokensRepository(Context);
        public AnnouncementsRepository AnnouncementsRepository => _announcementsRepository ??= new AnnouncementsRepository(Context);
        public IssuesRepository IssuesRepository => _issuesRepository ??= new IssuesRepository(Context);
    }
}