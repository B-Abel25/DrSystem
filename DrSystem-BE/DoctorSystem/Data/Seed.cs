using DoctorSystem.Entities;
using DoctorSystem.Entities.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DoctorSystem.Data
{
    public class Seed
    {
        public static async Task SeedCounties(BaseDbContext _context)
        {
            if (await _context._county.AnyAsync()) return;

            var CountyData = await System.IO.File.ReadAllTextAsync("Data/CountySeedData.json", Encoding.UTF8);
            var counties = JsonSerializer.Deserialize<List<County>>(CountyData);
            _context._county.AddRange(counties);
            await _context.SaveChangesAsync();
            
           
           
        }

        public static async Task SeedCities(BaseDbContext _context)
        {
            if (await _context._county.AnyAsync()) return;

            var CountyData = await System.IO.File.ReadAllTextAsync("Data/CountySeedData.json", Encoding.UTF8);
            var counties = JsonSerializer.Deserialize<List<County>>(CountyData);
            _context._county.AddRange(counties);
            await _context.SaveChangesAsync();



        }

        public static async Task SeedPublicAreaTypes(BaseDbContext _context)
        {
            if (await _context._county.AnyAsync()) return;

            var PATData = await System.IO.File.ReadAllTextAsync("Data/CountySeedData.json", Encoding.UTF8);
            var Types = JsonSerializer.Deserialize<List<County>>(PATData);
            _context._county.AddRange(Types);
            await _context.SaveChangesAsync();



        }
    }
}
