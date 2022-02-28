using DoctorSystem.Entities;
using DoctorSystem.Entities.Contexts;
using DoctorSystem.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorSystem.Services
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
            return await _context._place.Include(x => x.City.County).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Place> GetPlaceByPostCodeAndCityAsync(int postCode, string city)
        {
            return await _context._place.Include(x => x.City.County).SingleOrDefaultAsync(x => x.City.Name == city && x.PostCode == postCode);
        }

        public async Task<List<Place>> GetPlacesAsync()
        {
            return await _context._place.Include(x => x.City.County).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Place place)
        {
            _context.Entry(place).State = EntityState.Modified;
        }
    }
}
