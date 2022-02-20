using DoctorSystem.Entities;

namespace DoctorSystem.Dtos
{
    public class PlaceDto
    {
        public int PostCode { get; set; }
        public string City { get; set; }
        public string County { get; set; }

        public PlaceDto() { }

        public PlaceDto(Place p)
        {
            this.PostCode = p.PostCode;
            this.City = p.City.Name;
            this.County = p.City.County.Name;
        }
    }
}
