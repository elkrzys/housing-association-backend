using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.ExceptionHandling.Exceptions;

namespace HousingAssociation.Services
{
    public class LocalsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public LocalsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddLocals(List<Local> locals)
        {
            if (!locals.Any()) throw new BadRequestException("Cannot add empty list of locals.");
            locals.ForEach(async local => await AddLocalOrThrowBadRequestIfAlreadyExists(local));
            _unitOfWork.Commit();
        }

        public async Task<List<Local>> FindAllFromBuilding(int buildingId)
        {
            if(await _unitOfWork.BuildingsRepository.FindByIdAsync(buildingId) is null)
                throw new BadRequestException("Building doesn't exists.");

            return await _unitOfWork.LocalsRepository.GetAllByBuildingIdAsync(buildingId);
        }

        public async Task UpdateLocal(Local local)
        {
            if (await _unitOfWork.LocalsRepository.FindByIdAsync(local.Id) is null)
                throw new BadRequestException("Local doesn't exist");

            _unitOfWork.LocalsRepository.Update(local);
            _unitOfWork.Commit();
        }

        public async Task DeleteLocalById(int id)
        {
            var local = await _unitOfWork.LocalsRepository.FindByIdAsync(id);
            if(local is null)
                throw new BadRequestException("Local doesn't exist");
            
            _unitOfWork.LocalsRepository.Delete(local);
        }

        private async Task AddLocalOrThrowBadRequestIfAlreadyExists(Local local)
        {
            local = await _unitOfWork.LocalsRepository.AddIfNotExistsAsync(local);
            if(local is null)
                throw new BadRequestException("At least one added local already exists");
        }
    }
}