using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HousingAssociation.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HousingAssociation.DataAccess
{
    public interface IUnitOfWork
    {
        AddressesRepository AddressesRepository { get; }
        BuildingsRepository BuildingsRepository { get; }
        LocalsRepository LocalsRepository { get; }
        UsersRepository UsersRepository { get; }
        UserCredentialsRepository UserCredentialsRepository { get; }
        AnnouncementsRepository AnnouncementsRepository { get; }
        DocumentsRepository DocumentsRepository { get; }
        IssuesRepository IssuesRepository { get; }
        Task CommitAsync();
        void SetModified<T>(T entity);
        void SetModified<T>(List<T> entities);
        Task OuterTransaction(Func<Task> methodToInvoke);
    }
}