using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Order
    {
        public int OrderId { get; set; }        
        public DateTime OrderDate { get; set; }
        public Person Person { get; set; }
        public string Comment { get; set; }
    }
}
