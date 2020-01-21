using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Order
    {
        public long OrderKey { set; get; }
        public long HostingUnitKey { set; get; }
        public long GuestRequestKey { set; get; }
        public statusOrder Status { set; get; }
        public DateTime CreateDate { set; get; }
        public DateTime OrderDate { set; get; }
        public override string ToString()
        {
            return string.Format("Order Key: {0}\n", OrderKey) + 
                string.Format("Hosting Unit Key: {0}\n", HostingUnitKey) +
                string.Format("Guest Request Key: {0}\n", GuestRequestKey) +              
                string.Format("Status: {0}\n", Status) +
                string.Format("Create Date: {0}\n", CreateDate) +
                string.Format("OrderDate: {0}\n", OrderDate);
        }
    }
}
