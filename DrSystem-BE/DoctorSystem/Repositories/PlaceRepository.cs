using DoctorSystem.Entities;
using DoctorSystem.Entities.Contexts;
using DoctorSystem.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorSystem.Repositories
{
    public class PlaceRepository : IPlaceRepository
    {
        private readonly BaseDbContext _context;

        public PlaceRepository(BaseDbContext context)
        {
            _context = context;
        }

        public async Task<Place> GetPlaceByIdAsync(string id)
        {
            return await _context._places.Include(x => x.City.County).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Place> GetPlaceByPostCodeAndCityAsync(int postCode, string city)
        {
            return await _context._places.Include(x => x.City.County).SingleOrDefaultAsync(x => x.City.Name == city && x.PostCode == postCode);
        }

        public async Task<City> GetCityByNameAsync(string city)
        {
            return await _context._city.Include(x => x.County).SingleOrDefaultAsync(x => x.Name == city);
        }

        public async Task<List<Place>> GetPlacesAsync()
        {
            return await _context._places.Include(x => x.City.County).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Place place)
        {
            _context._places.Update(place);
        }
    }
}
