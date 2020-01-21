using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace BL
{
    public delegate bool GuestReqDelegate(GuestRequest gs);
    public interface IBL//16
    {
        #region logic func
        bool isDateValid(DateTime start, DateTime end);//(1)
        bool allowedSendReq(Order order);//(2)
        bool checkIfOk(long gsKey, long huKey);//(3)
        bool CanChangeStatus(Order order);//(4)
        int calculateAmla(Order order);//(5)
        void updateDiary(Order order);//(6)
        void changeStatusOfOtherThings(Order order);//(7)
        bool cantDel(HostingUnit hu);//(8)
        bool canCencelAllowens(Host host);//(9)
        void sendEmailAfterStatusChange(Order order);//(10)
        List<HostingUnit> availableUnits(DateTime d, int numOfDays);//(11)
        int numOfDaysPassed(DateTime d1, DateTime? d2 = null);//(12)
        List<Order> ordersOverTime(int numOfDays);//(13)
        List<GuestRequest> guestWithCondition(GuestReqDelegate d);//(14)
        int numOfOrders(GuestRequest gs);//(15)
        int numOfClosedOrSentOrders(HostingUnit hostU);//(16
        #endregion
        #region func by grouping
        List<IGrouping<area, GuestRequest>> groupGuestReqByArea();//(17)
        List<IGrouping<int, GuestRequest>> groupByNumOfPeople();//(18)
        List<IGrouping<int, Host>> groupByNumOfHostingUnits();//(19)
        List<IGrouping<area, HostingUnit>> groupHostUnitByArea();//(20)
        #endregion



        string statisticsForHostUn(HostingUnit hostUn);//(21)
        int calculateTotalPriceForVication(Order order);//(22)
        List<IGrouping<int, HostingUnit>> groupByNumOfRooms();//(23)
        List<HostingUnit> availableForTomorrow();//(24)
        List<int> numOfDaysBooked(HostingUnit hostingU);//(25) 
        int calculatProfitOfHostUn(HostingUnit hostingU);//(26)
         bool LegalId(string s);
         bool IsHebrew(string word);
         bool IsTelephone(string tel);
         bool IsCellPhone(string tel);
         int GetAge(DateTime d);
         bool CheackMail(string t);
         bool IsNumber(string num);
         bool IsDate(string date);



        #region fuc of dal
        void addCustomerReq(GuestRequest gs);//()
        void updateCustomerReq(long gsKey, statusGusReq stat);
        List<GuestRequest> getGuestRequestList();
        void addHostingUnit(HostingUnit hostunit);
        void deleteHostingUnit(HostingUnit HstUnt);
        void UpdateHostingUnit(HostingUnit HstUnt);
        List<HostingUnit> getHostingUnitList();
        void addOrder(Order ord);
        void updateOrder(Order order);
        void updateOrder2(Order order, statusOrder status);

        List<Order> getOrderList();
        List<BankBranch> getBankBranches();
        #endregion
    }
}
