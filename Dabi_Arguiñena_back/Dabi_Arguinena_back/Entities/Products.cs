using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dabi_Arguinena_back.Entities
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }

        public int CategoryId { get; set; }
    }
}
