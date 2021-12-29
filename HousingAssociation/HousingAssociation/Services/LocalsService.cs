using System.Collections.Generic;
using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.ExceptionHandling.Exceptions;
using HousingAssociation.Models.DTOs;
using HousingAssociation.Utils.Extensions;
using Serilog;

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
            {
                Log.Warning("LocalDto is null");
                throw new BadRequestException("Local mustn't be null.");
            }
            if (localDto.BuildingId is null)
            {
                Log.Warning("BuildingId is null");
                throw new BadRequestException("Local must be assigned to building");
            }
            
            var local = new Local
            {
                Number = localDto.Number,
                BuildingId = localDto.BuildingId!.Value,
                Area = localDto.Area,
                IsFullyOwned = localDto.IsFullyOwned ?? false
            };

            if (await _unitOfWork.LocalsRepository.CheckIfExists(local))
            {
                Log.Warning($"Local already exists");
                throw new BadRequestException("Local already exists");
            }
            await _unitOfWork.LocalsRepository.AddAsync(local);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<LocalDto>> FindAllFromBuilding(int buildingId)
        {
            if(await _unitOfWork.BuildingsRepository.FindByIdWithDetailsAsync(buildingId) is null)
            {
                Log.Warning($"Building with id = {buildingId} doesn't exist.");
                throw new BadRequestException("Building doesn't exists.");
            }
            
            var locals= await _unitOfWork.LocalsRepository.FindAllByBuildingIdAsync(buildingId);
            return GetLocalsAsDtos(locals);
        }

        public async Task AddResidentToLocal(int localId, int residentId)
        {
            var local = await _unitOfWork.LocalsRepository.FindByIdAsync(localId);
            if (local is null)
            {
                Log.Warning($"Local with id = {localId} not found");
                throw new NotFoundException();
            }

            var resident = await _unitOfWork.UsersRepository.FindByIdAsync(residentId);
            if (resident is null)
            {
                Log.Warning($"User with id = {residentId} not found");
                throw new NotFoundException();
            }
            
            if (local.Residents.Exists(r => r.Id == residentId))
            {
                Log.Warning($"Resident with id = {residentId} is already assigned to local with id = {local.Id}");
                throw new BadRequestException("User is already assigned to the local");
            }
            
            local.Residents.Add(resident);
            _unitOfWork.LocalsRepository.Update(local);
            await _unitOfWork.CommitAsync();
        }
        
        public async Task RemoveResidentFromLocal(int localId, int residentId)
        {
            var local = await _unitOfWork.LocalsRepository.FindByIdAsync(localId);
            if (local is null)
            {
                Log.Warning($"Local with id = {localId} not found.");
                throw new NotFoundException();
            }

            var resident = await _unitOfWork.UsersRepository.FindByIdAndIncludeAllLocalsAsync(residentId);
            if (resident is null)
            {
                Log.Warning($"User with id = {residentId} not found.");
                throw new NotFoundException();
            }
            //_unitOfWork.SetUnchanged(resident);

            local.Residents.Remove(resident);
            _unitOfWork.SetModified(local.Residents);
            //_unitOfWork.LocalsRepository.Update(local);
            await _unitOfWork.CommitAsync();
        }
        public async Task UpdateLocal(LocalDto localDto)
        {
            var local = await _unitOfWork.LocalsRepository
                .FindByIdAsync(localDto.Id ?? throw new BadRequestException("No Id given."));

            local.Area = localDto.Area;
            local.Number = localDto.Number;
            local.IsFullyOwned = localDto.IsFullyOwned ?? local.IsFullyOwned;
            
            _unitOfWork.LocalsRepository.Update(local);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<LocalDto>> GetAllByResidentId(int residentId)
        {
            var locals = await _unitOfWork.LocalsRepository.FindAllByResidentId(residentId);
            return GetLocalsAsDtos(locals);
        }
        
        public async Task<int> GetLocalIdByLocalDetails(LocalDto localDto)
        {
            var local = await _unitOfWork.LocalsRepository.FindByDetailsAsync(localDto);
            if (local is null)
            {
                Log.Warning($"Trying to add local with current details: {localDto}");
                throw new NotFoundException();
            }
            return local.Id;
        }

        public async Task DeleteLocalById(int id)
        {
            var local = await _unitOfWork.LocalsRepository.FindByIdAsync(id);
            if (local is null)
            {
                Log.Warning($"Local with id = {id} doesn't exist.");
                throw new BadRequestException("Local doesn't exist");
            }
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