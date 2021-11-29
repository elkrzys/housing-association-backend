using System.Threading.Tasks;
using HousingAssociation.DataAccess;

namespace HousingAssociation.Services
{
    public class AnnouncementsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnnouncementsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddAnnouncement()
        {
            
        }

        public async Task UpdateAnnouncement()
        {
            
        }

        public async Task DeleteAnnouncement()
        {
            
        }

        public async Task GetAllAnnouncements()
        {
            
        }

        public async Task GetAllAnnouncementsByBuilding()
        {
            
        }
    }
}