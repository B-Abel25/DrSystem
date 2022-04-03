using DoctorSystem.Entities;
using System;

namespace DoctorSystem.Dtos
{
    public class MessageDto
    {
        public UserDto Sender { get; set; }
        public UserDto Reciever { get; set; }
        public string DateRead { get; set; }
        public string DateSent { get; set; }
        public string Content { get; set; }
        public MessageDto(Message m)
        {
            Sender = new UserDto(m.Sender);
            Reciever = new UserDto(m.Reciever);
            DateRead = m.DateRead?.ToString("yyyy.MM.dd HH:mm:ss");
            DateSent = m.DateSent.ToString("yyyy.MM.dd HH:mm:ss");
            Content = m.Content;
        }

        public MessageDto()
        {
           
        }
    }
}
