using DoctorSystem.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorSystem.Interfaces
{
    public interface IPlaceRepository
    {
        Task<Place> GetPlaceByPostCodeAndCityAsync(int postCode, string city);
        Task<Place> GetPlaceByIdAsync(string id);
        Task<List<Place>> GetPlacesAsync();
        Task<bool> SaveAllAsync();
        void Update(Place place);
        Task<City> GetCityByNameAsync(string city);
    }
}
