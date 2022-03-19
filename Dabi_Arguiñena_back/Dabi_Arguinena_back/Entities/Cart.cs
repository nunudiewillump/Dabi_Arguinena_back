using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dabi_Arguinena_back.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string email { get; set; }
        public int? ProductId { get; set; }
        public string productName { get; set; }
        public double price { get; set; }
        public double total { get; set; }

        


        
    }
}
