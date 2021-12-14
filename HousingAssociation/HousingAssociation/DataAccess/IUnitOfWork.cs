﻿using System;
using System.Threading.Tasks;
using HousingAssociation.Repositories;

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
        void Commit();
        Task CommitAsync();
    }
}