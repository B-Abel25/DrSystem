using System;

namespace DoctorSystem.Entities
{
    public class Appointment : AbstractAuditable
    {
        public Client Client { get; set; }
        public Doctor Doctor { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

    }
}
