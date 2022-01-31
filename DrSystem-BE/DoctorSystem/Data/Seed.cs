using DoctorSystem.Dtos;
using DoctorSystem.Entities;
using DoctorSystem.Entities.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DoctorSystem.Data
{
    public class AddressSeedModel
    {
        public string IrSzam { get; set; }
        public string City { get; set; }
        public string County { get; set; }
    }


    public class DoctorSeedModel
    {
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string SealNumber { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Postcode { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }



    }

    public class Seed
    {
        public static async Task SeedAddress(BaseDbContext _context)
        {
            if (await _context._county.AnyAsync()) return;

            var AddressData = await System.IO.File.ReadAllTextAsync("Data/AddressSeedData.json", Encoding.UTF8);
            var addressModels = JsonSerializer.Deserialize<List<AddressSeedModel>>(AddressData);
            
            foreach (var addressModel in addressModels)
            {

                var county = await _context._county.SingleOrDefaultAsync(x => x.Name == addressModel.County);
                if (county == null)
                {
                    county = new County();
                    county.Name = addressModel.County;
                }

                var city = await _context._city.SingleOrDefaultAsync(x => x.Name == addressModel.City);
                if (city == null)
                {
                    city = new City();
                    city.Name = addressModel.City;
                }

                var place = new Place();
                place.PostCode = int.Parse(addressModel.IrSzam);
                place.City = city;
                place.City.County = county;

                _context._place.Add(place);
                await _context.SaveChangesAsync();
            }

        }

        public static async Task SeedDoctors(BaseDbContext _context)
        {
            if (await _context._doctors.AnyAsync()) return;

            var docData = await System.IO.File.ReadAllTextAsync("Data/DoctorSeedData.json", Encoding.UTF8);
            var docModels = JsonSerializer.Deserialize<List<DoctorSeedModel>>(docData);

          

        }

        
    }
}
