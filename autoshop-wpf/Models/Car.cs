using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoshop.Models
{
    public class Car
    {
        public int ID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }        
        public string EngineType { get; set; }    // Тип двигателя (бензин, дизель, электро и т.д.)
        public int Mileage { get; set; }
        public int Price { get; set; }
        public Byte[] Image { get; set; }
        public List<User> User { get; set; } = new List<User>();
    }
}
