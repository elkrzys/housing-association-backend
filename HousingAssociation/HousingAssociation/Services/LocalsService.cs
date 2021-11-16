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

        public async Task<Local> AddLocals(List<Local> locals)
        {
            if (!locals.Any()) throw new BadRequestException("Cannot add empty list of locals.");

            if (locals.Count == 1)
            {
                await _unitOfWork.LocalsRepository.AddIfNotExistsAsync(locals.First());
            }
            else
            {
               // await _unitOfWork.LocalsRepository.AddRange(locals);
            }
            _unitOfWork.Commit();
            return null;
        }

        public async Task<List<Local>> FindAllFromBuilding(int buildingId)
        {
            var building = await _unitOfWork.BuildingsRepository.FindByIdAsync(buildingId) 
                           ?? throw new BadRequestException("Building doesn't exists.");

            return building.Locals;

            //return await _unitOfWork.LocalsRepository.GetAllByBuildingIdAsync(buildingId);
        }
    }
}