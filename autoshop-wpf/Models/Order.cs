using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoshop.Models
{
    public class Order
    {
        public int ID { get; set; }
        public string Comment { get; set; }
        public string Adress { get; set; }
        public string Status { get; set; }

        public int CarId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string EngineType { get; set; }    // Тип двигателя (бензин, дизель, электро и т.д.)
        public int Mileage { get; set; }
        public int Price { get; set; }
        public Byte[] Image { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
