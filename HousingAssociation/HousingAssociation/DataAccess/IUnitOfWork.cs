using System;
using System.Threading.Tasks;
using HousingAssociation.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HousingAssociation.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        AppDbContext Context { get;  } 
        AddressesRepository AddressesRepository { get; }
        BuildingsRepository BuildingsRepository { get; }
        LocalsRepository LocalsRepository { get; }
        UsersRepository UsersRepository { get; }
        UserCredentialsRepository UserCredentialsRepository { get; }
        AnnouncementsRepository AnnouncementsRepository { get; }
        IssuesRepository IssuesRepository { get; }
        void Commit();
        Task CommitAsync();
    }
}