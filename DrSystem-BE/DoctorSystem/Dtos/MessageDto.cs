using DoctorSystem.Entities;
using System;

namespace DoctorSystem.Dtos
{
    public class MessageDto
    {
        public string Id { get; set; }
        public UserDto Sender { get; set; }
        public UserDto Reciever { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime DateSent { get; set; }
        public string Content { get; set; }
        public bool SenderDeleted { get; set; }
        public bool RecieverDeleted { get; set; }
        public MessageDto(Message m)
        {
            //TODO itt megkéne nézni ezt a Generic type-ot, nem biztos, hogy ez a jó megoldás
            Sender = new DoctorDto(m.Sender);
            Reciever = new ClientDto(m.Reciever);
            DateRead = m.DateRead;
            DateSent = m.DateSent;
            Content = m.Content;
            SenderDeleted = m.SenderDeleted;
            RecieverDeleted = m.RecieverDeleted;
        }

        public MessageDto()
        {
           
        }
    }
}
