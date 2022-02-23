using System;

namespace DoctorSystem.Dtos
{
    public class MessageDto
    {
        public UserDto Sender { get; set; }
        public UserDto Reciever { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime DateSent { get; set; }
        public string Content { get; set; }
        public bool SenderDeleted { get; set; }
        public bool RecieverDeleted { get; set; }
    }
}
