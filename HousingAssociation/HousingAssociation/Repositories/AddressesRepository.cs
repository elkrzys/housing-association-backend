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
                return address;
            }
            return existingAddress;
        }

        public async Task Add(Address address) =>  await _addresses.AddAsync(address);

        public async Task<Address> FindByIdAsync(int id) => await _addresses.FindAsync(id);
        
        public async Task<Address> FindAddressAsync(Address address) => await _addresses.FirstOrDefaultAsync(a => a.City.Equals(address.City) 
                                                                            && a.District.Equals(address.District) 
                                                                            && a.Street.Equals(address.Street));
        
        public void Delete(Address address)
        {
            _addresses.Remove(address);
        }
    }
}