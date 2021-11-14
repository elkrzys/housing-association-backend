using System;
using HousingAssociation.Repositories;

namespace HousingAssociation.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        AppDbContext Context { get;  } 
        AddressesRepository AddressesRepository { get; }
        BuildingsRepository BuildingsRepository { get; }
        UsersRepository UsersRepository { get; }
        UserCredentialsRepository UserCredentialsRepository { get; }
        RefreshTokensRepository RefreshTokensRepository { get; }
        AnnouncementsRepository AnnouncementsRepository { get; }
        IssuesRepository IssuesRepository { get; }
        void Commit();
    }
}