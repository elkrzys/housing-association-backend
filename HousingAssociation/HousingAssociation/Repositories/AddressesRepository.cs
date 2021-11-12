using System;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HousingAssociation.Repositories
{
    public class AddressesRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<Address> _addresses;
        
        public AddressesRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _addresses = _dbContext.Addresses;
        }
        
        public async Task<Address> AddNewAddressOrReturnExisting(Address address)
        {

            var existingAddress = await _addresses.FirstOrDefaultAsync(a => a.City.Equals(address.City) 
                                                                && a.District.Equals(address.District) 
                                                                && a.Street.Equals(address.Street));
            
            if (existingAddress is null)
            {
                await _addresses.AddAsync(address);
                //await _dbContext.SaveChangesAsync();
                return address;
            }
            return existingAddress;
        }
    }
}