using System;

namespace RobsonDev.Domain.Entities
{
    public class People
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public bool Active { get; set; }
    }
}
