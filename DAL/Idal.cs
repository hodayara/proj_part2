using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public interface Idal
    {
        #region customer reques func
         void addCustomerReq(GuestRequest gs);
         void updateCustomerReq(long gsKey, statusGusReq stat);
        List<GuestRequest> getGuestRequestList();
        #endregion

        #region hosting unit func
        void addHostingUnit(HostingUnit hostunit);
        void deleteHostingUnit(HostingUnit HstUnt);
        void UpdateHostingUnit(HostingUnit HstUnt);
        List<HostingUnit> getHostingUnitList();
        #endregion

        #region order func
        void addOrder(Order ord);
        void updateOrder(long orKey,statusOrder statO);
        List<Order> getOrderList();
        #endregion


        List<BankBranch> getBankBranches();
    }
}
