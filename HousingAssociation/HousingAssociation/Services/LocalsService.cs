using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.ExceptionHandling.Exceptions;
using HousingAssociation.Models.DTOs;
using HousingAssociation.Utils.Extensions;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace HousingAssociation.Services
{
    public class LocalsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public LocalsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddLocal(LocalDto localDto)
        {
            if (localDto is null) 
                throw new BadRequestException("Local mustn't be null.");

            var local = new Local
            {
                Number = localDto.Number,
                BuildingId = localDto.BuildingId,
                Area = localDto.Area,
                IsFullyOwned = localDto.IsFullyOwned ?? false
            };

            if (await _unitOfWork.LocalsRepository.CheckIfExists(local))
                throw new BadRequestException("Local already exists");

            await _unitOfWork.LocalsRepository.AddAsync(local);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<LocalDto>> FindAllFromBuilding(int buildingId)
        {
            if(await _unitOfWork.BuildingsRepository.FindByIdWithAddressAsync(buildingId) is null)
                throw new BadRequestException("Building doesn't exists.");
            
            var locals= await _unitOfWork.LocalsRepository.FindAllByBuildingIdAsync(buildingId);
            return GetLocalsAsDtos(locals);
        }

        public async Task AddResidentToLocal(int localId, int residentId)
        {
            var local = await _unitOfWork.LocalsRepository.FindByIdAsync(localId) ?? throw new NotFoundException();
            var resident = await _unitOfWork.UsersRepository.FindById(residentId) ?? throw new NotFoundException();
            
            local.Residents.Add(resident);
            _unitOfWork.LocalsRepository.Update(local);
            await _unitOfWork.CommitAsync();
        }
        
        public async Task RemoveResidentFromLocal(int localId, int residentId)
        {
            var local = await _unitOfWork.LocalsRepository.FindByIdAsync(localId) ?? throw new NotFoundException();
            var resident = await _unitOfWork.UsersRepository.FindById(residentId) ?? throw new NotFoundException();
            
            local.Residents.Remove(resident);
            _unitOfWork.LocalsRepository.Update(local);
            await _unitOfWork.CommitAsync();
        }
        public async Task UpdateLocal(LocalDto localDto)
        {
            var local = await _unitOfWork.LocalsRepository
                .FindByIdAsync(localDto.Id ?? throw new BadRequestException("No Id given."));

            local.Area = localDto.Area;
            local.Number = localDto.Number;
            local.BuildingId = localDto.BuildingId;
            local.IsFullyOwned = localDto.IsFullyOwned ?? local.IsFullyOwned;
            
            _unitOfWork.LocalsRepository.Update(local);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<LocalDto>> GetAllByResidentId(int residentId)
        {
            var locals = await _unitOfWork.LocalsRepository.FindAllByResidentId(residentId);
            return GetLocalsAsDtos(locals);
        }

        public async Task DeleteLocalById(int id)
        {
            var local = await _unitOfWork.LocalsRepository.FindByIdAsync(id) ?? throw new BadRequestException("Local doesn't exist");
            _unitOfWork.LocalsRepository.Delete(local);
            await _unitOfWork.CommitAsync();
        }
        private List<LocalDto> GetLocalsAsDtos(List<Local> locals)
        {
            List<LocalDto> localDtos = new();
            locals.ForEach(local => localDtos.Add(local.AsDto()));
            return localDtos;
        }
    }
}