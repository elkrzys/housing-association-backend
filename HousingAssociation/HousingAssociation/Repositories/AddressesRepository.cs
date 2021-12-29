using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HousingAssociation.Repositories
{
    public class AddressesRepository
    {
        private readonly DbSet<Address> _addresses;
        
        public AddressesRepository(AppDbContext dbContext)
        {
            _addresses = dbContext.Addresses;
        }
        
        public async Task<Address> AddNewAddressOrReturnExisting(Address address)
        {

            var existingAddress = await _addresses
                .FirstOrDefaultAsync(a => a.City.Equals(address.City) 
                                          && (string.IsNullOrEmpty(address.District) || a.District.Equals(address.District))
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

        public async Task<List<string>> FindAllCitiesAsync() 
            => await _addresses
                .Select(a => a.City)
                .Distinct()
                .ToListAsync();

        public async Task<List<string>> FindAllDistrictsByCityAsync(string city)
            => await _addresses
                .Where(a => a.City.Equals(city))
                .Where(a => !string.IsNullOrEmpty(a.District))
                .Select(a => a.District)
                .Distinct()
                .ToListAsync();

        public async Task<List<string>> FindAllStreetsByCityAndDistrictAsync(string city,
            string district)
            => await _addresses
                .Where(a => a.City.Equals(city))
                .Where(a => string.IsNullOrEmpty(district) || string.IsNullOrWhiteSpace(district) || a.District.Equals(district))
                .Select(a => a.Street)
                .Distinct()
                .ToListAsync();

        public void Delete(Address address)
        {
            _addresses.Remove(address);
        }
    }
}