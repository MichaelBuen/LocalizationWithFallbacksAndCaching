using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Order
    {
        public virtual int OrderId { get; set; }        
        public virtual DateTime OrderDate { get; set; }
        public virtual Person Person { get; set; }
        public virtual string Comment { get; set; }
    }
}
