using DoctorSystem.Data.SeedModels;
using DoctorSystem.Dtos;
using DoctorSystem.Entities;
using DoctorSystem.Entities.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DoctorSystem.Data
{

    public class Seed
    {
        private static readonly Random random = new Random();

        public static async Task SeedDatas(BaseDbContext _context)
        {
            await SeedAddress(_context);
            await SeedDoctors(_context);
        }

        private static async Task SeedAddress(BaseDbContext _context)
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

        private static async Task SeedDoctors(BaseDbContext _context)
        {
            if (await _context._doctors.AnyAsync()) return;

            var docData = await System.IO.File.ReadAllTextAsync("Data/DoctorSeedData.json", Encoding.UTF8);
            var docModels = JsonSerializer.Deserialize<List<DoctorSeedModel>>(docData);

            foreach (var doctorModel in docModels)
            {
                Doctor doc = new Doctor();

                

                doc.Name = doctorModel.Name;
                doc.DateOfBirth = DateTime.Parse(doctorModel.DateOfBirth);
                do
                {
                    doc.SealNumber = random.Next(10000, 100000).ToString();
                } while (await _context._doctors.SingleOrDefaultAsync(x => x.SealNumber == doc.SealNumber) != null);
                doc.Email = doctorModel.Email;
                doc.PhoneNumber = doctorModel.PhoneNumber;
                string PC = doctorModel.PostCode.Split(' ')[0];
                string CT = doctorModel.PostCode.Split(' ')[1];
                doc.Place = await _context._place.SingleOrDefaultAsync(x => x.PostCode == int.Parse(PC) && x.City.Name == CT);
                doc.Street = doctorModel.Street;
                doc.HouseNumber = doctorModel.HouseNumber;
                doc.Token = GenerateToken(10);
                var hmac = new HMACSHA512();
                doc.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(doctorModel.Password));
                doc.PasswordSalt = hmac.Key;

                _context._doctors.Add(doc);
                await _context.SaveChangesAsync();
            }

        }

        private static async Task SeedUsers(BaseDbContext _context)
        {
            if (await _context._users.AnyAsync()) return;

            var docData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json", Encoding.UTF8);
            var docModels = JsonSerializer.Deserialize<List<DoctorSeedModel>>(docData);

            foreach (var doctorModel in docModels)
            {
                Doctor doc = new Doctor();



                doc.Name = doctorModel.Name;
                doc.DateOfBirth = DateTime.Parse(doctorModel.DateOfBirth);
                do
                {
                    doc.SealNumber = random.Next(10000, 100000).ToString();
                } while (await _context._doctors.SingleOrDefaultAsync(x => x.SealNumber == doc.SealNumber) != null);
                doc.Email = doctorModel.Email;
                doc.PhoneNumber = doctorModel.PhoneNumber;
                string PC = doctorModel.PostCode.Split(' ')[0];
                string CT = doctorModel.PostCode.Split(' ')[1];
                doc.Place = await _context._place.SingleOrDefaultAsync(x => x.PostCode == int.Parse(PC) && x.City.Name == CT);
                doc.Street = doctorModel.Street;
                doc.HouseNumber = doctorModel.HouseNumber;
                doc.Token = GenerateToken(10);
                var hmac = new HMACSHA512();
                doc.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(doctorModel.Password));
                doc.PasswordSalt = hmac.Key;

                _context._doctors.Add(doc);
                await _context.SaveChangesAsync();
            }

        }

        private static string GenerateToken(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789#&@";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
