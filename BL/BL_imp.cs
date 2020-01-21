
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;
using System.Text.RegularExpressions;

namespace BL
{

    class BL_imp : IBL
    {
        public static Idal dal = FactoryDal.getDal();
        #region logic func
        /// <summary>
        /// a func to check if number of days between dates ate available
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="hu"></param>
        /// <returns></returns>
        public bool dateOk(DateTime start, DateTime end, HostingUnit hu)
        {
            int i = start.Month - 1, k = end.Month - 1;
            int j = start.Day - 1, s = end.Day - 1;
            while (i != k || j != s)
            {
                if (hu.diary[i, j] == true)
                    return false;
                j++;
                if (j == 31)
                {
                    j = 0;
                    i++;
                    if (i == 12)
                        i = 0;
                }
            }
            return true;
        }
        /// <summary>
        /// func to check if date is valid
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public bool isDateValid(DateTime start, DateTime end)//(1)
        {
            return start < end;
        }
        /// <summary>
        /// a func to check if host can send an email to a client
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public bool allowedSendReq(Order order)//(2)//לממש
        {
            var hostingU = (from hosUn in dal.getHostingUnitList()
                            where (order.HostingUnitKey == hosUn.HostingUnitKey)
                            select hosUn).FirstOrDefault();
            if (hostingU.Owner.CollectionClearance)
                return true;
            return false;
        }
        /// <summary>
        /// a func to check that all the dates are available from guest request
        /// </summary>
        /// <param name="gs"></param>
        /// <param name="hu"></param>
        /// <returns></returns>
        public bool checkIfOk(long gsKey, long huKey)//(3)
        {
            var gs = (from g in dal.getGuestRequestList()
                      where g.GuestRequestKey == gsKey
                      select g).FirstOrDefault();
            var hu = (from h in dal.getHostingUnitList()
                      where h.HostingUnitKey == huKey
                      select h).FirstOrDefault();
            if (gs == null || hu == null)
                throw new System.ArgumentException("host unit or gues request does not exist ");
            return dateOk(gs.EntryDate, gs.ReleaseDate, hu);

        }
        /// <summary>
        /// a func to check the status of an order and if it close returns false-you cant change the status anymore
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool CanChangeStatus(Order order)//(4)
        {
            return !(order.Status == statusOrder.נסגר_מחוסר_הענות_של_הלקוח || order.Status == statusOrder.נסגר_בהיענות_של_לקוח);
        }
        /// <summary>
        /// a func to calculate the amla that needs to be paid to the host
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int calculateAmla(Order order)//(5)
        {
            GuestRequest gs = (from g in dal.getGuestRequestList()
                               where (g.GuestRequestKey == order.GuestRequestKey)
                               select g).FirstOrDefault();
            return (gs.ReleaseDate - gs.EntryDate).Days * Configurations.amla;
        }
        /// <summary>
        /// func to update diary with a given order
        /// </summary>
        /// <param name="order"></param>
        public void updateDiary(Order order)//(6)
        {
            GuestRequest gs = dal.getGuestRequestList().Find(g => g.GuestRequestKey == order.GuestRequestKey);
            HostingUnit hu = dal.getHostingUnitList().Find(hosu => hosu.HostingUnitKey == order.HostingUnitKey);
            int i = gs.EntryDate.Month - 1, k = gs.ReleaseDate.Month - 1;
            int j = gs.EntryDate.Day - 1, s = gs.ReleaseDate.Day - 1;
            while (i != k || j != s)
            {
                hu.diary[i, j] = true;
                j++;
                if (j == 31)
                {
                    j = 0;
                    i++;
                    if (i == 12)
                        i = 0;
                }
            }

        }
        /// <summary>
        /// a func that change status of other orders from the same client and his guest request
        /// </summary>
        /// <param name="order"></param>
        public void changeStatusOfOtherThings(Order order)//(7)
        {
            var guestReq = (from gs in dal.getGuestRequestList()
                            where (order.GuestRequestKey == gs.GuestRequestKey)
                            select gs).FirstOrDefault();
            dal.updateCustomerReq(guestReq.GuestRequestKey, statusGusReq.נסגר_על_ידי_האתר);
            //guestReq.Status = statusGusReq.נסגר_על_ידי_האתר;
            var listOrd = (from ord in dal.getOrderList()
                           where (ord.GuestRequestKey == order.GuestRequestKey)
                           select ord).ToList();

            foreach (Order ord in listOrd)//האם מותר לעבור כאן עם FOREACH 
                dal.updateOrder(ord.OrderKey, order.Status);
                //ord.Status = order.Status;
        }
        /// <summary>
        /// a func to check if there is an open order for a hosting unit if there is- the host cant delete it
        /// </summary>
        /// <param name="hu"></param>
        /// <returns></returns>
        public bool cantDel(HostingUnit hu)//(8)
        {
            List<Order> lorder = dal.getOrderList();
            var isThere = (from order in lorder
                           where (hu.HostingUnitKey == order.HostingUnitKey)
                           select new { order }).FirstOrDefault();
            return isThere == null;
        }
        /// <summary>
        /// if there is an open 
        /// </summary>
        /// <param name="gs"></param>
        public bool canCencelAllowens(Host host)//(9)
        {
            var ordList = (from order in dal.getOrderList()
                           from hu in dal.getHostingUnitList()
                           where order.HostingUnitKey == hu.HostingUnitKey && hu.Owner.HostKey == host.HostKey && order.Status == statusOrder.טרם_טופל
                           select order).ToList();
            return !ordList.Any();
        }

        /// <summary>
        /// a func to send an enail after status change to נשלח_מייל
        /// </summary>
        /// <param name="order"></param>
        public void sendEmailAfterStatusChange(Order order)//(10)
        {
            if (order.Status == statusOrder.נשלח_מייל)
                Console.WriteLine(order);
        }
        /// <summary>
        /// a func that retuns all the available units in a particular dates
        /// </summary>
        /// <param name="d"></param>
        /// <param name="numOfDays"></param>
        /// <returns></returns>
        public List<HostingUnit> availableUnits(DateTime d, int numOfDays)//(11)
        {
            DateTime end = d.AddDays(numOfDays);
            List<HostingUnit> hostUnit = dal.getHostingUnitList().FindAll(hu => dateOk(d, end, hu));
            return hostUnit;
        }
        /// <summary>
        /// a func that returns the number of days between two date or between a date until now
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public int numOfDaysPassed(DateTime d1, DateTime? d2 = null)//(12)
        {
            if (d2 == null)
            {
                DateTime today = new DateTime(2020, (DateTime.Today).Month, (DateTime.Today).Day);
                return (today-d1).Days+1;
            }
            DateTime d3 = (DateTime)d2;
            return (d3-d1).Days+1;
        }
        /// <summary>
        /// a func that returns a list of orders who created before a given number of days
        /// </summary>
        /// <param name="numOfDays"></param>
        /// <returns></returns>
        public List<Order> ordersOverTime(int numOfDays)//(13)
        {
            var order = (from ord in dal.getOrderList()
                         where (DateTime.Today - ord.CreateDate).Days >= numOfDays 
                         select ord).ToList();
            return order;
        }
        /// <summary>
        /// abstract func that returns all the guest request how answer to a sertain condition
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public List<GuestRequest> guestWithCondition(GuestReqDelegate d)//(14)
        {
            var guesReq = (from gs in dal.getGuestRequestList()
                           where d(gs)
                           select gs).ToList();
            return guesReq;
        }
        /// <summary>
        /// a func that returns the number of all the orders that abstract guest request has
        /// </summary>
        /// <param name="gs"></param>
        /// <returns></returns>
        public int numOfOrders(GuestRequest gs)//(15)
        {
            List<Order> listOrd = dal.getOrderList().FindAll(o => o.GuestRequestKey == gs.GuestRequestKey);
            return listOrd.Count();
        }
        /// <summary>
        /// a func that returns the number of orders who werte sent or closed successfuly
        /// </summary>
        /// <param name="hostU"></param>
        /// <returns></returns>
        public int numOfClosedOrSentOrders(HostingUnit hostU)//(16)
        {
            List<Order> listOrd = dal.getOrderList().FindAll(o => o.HostingUnitKey == hostU.HostingUnitKey && (o.Status == statusOrder.נסגר_בהיענות_של_לקוח || o.Status == statusOrder.נשלח_מייל));
            return listOrd.Count();
        }
        #endregion
        #region func by grouping
        /// <summary>
        /// a func that returns a list of groups of guest request by sertain area
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public List<IGrouping<area, GuestRequest>> groupGuestReqByArea()//(17)
        {
            var listByArea = (from gs in dal.getGuestRequestList()
                              group gs by gs.Area).ToList();
            return listByArea;
        }
        /// <summary>
        /// a func that returns a list of groups of guest request by number of peaople in vication
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public List<IGrouping<int, GuestRequest>> groupByNumOfPeople()//(18)
        {
            var listByNum = (from gs in dal.getGuestRequestList()
                             group gs by gs.Adults + gs.Children).ToList();
            return listByNum;
        }
        /// <summary>
        /// a func that return a list of groups host unit by number of hosting units 
        /// </summary>
        /// <param name="numOfHostingUnits"></param>
        /// <returns></returns>
        public List<IGrouping<int, Host>> groupByNumOfHostingUnits()//(19)
        {
            //var groupedList1 = from hu in dal.getHostingUnitList()
            //                   from hu1 in dal.getHostingUnitList()
            //                   group Host hu.HostingUnitKey == hu1.HostingUnitKey

            var groupedList = (from hu in dal.getHostingUnitList()
                               select hu.Owner).ToList();
           var groupedList3 = (groupedList.GroupBy(x => x.HostKey).Select(y => y.First())).ToList();
            var listByNumOfUnit = (from host in groupedList3
                                   group host by host.NumOfHostingUnit).ToList();
            return listByNumOfUnit;
        }
        /// <summary>
        /// a func that returns a list of groups of hosting unit by area
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public List<IGrouping<area, HostingUnit>> groupHostUnitByArea()//(20)
        {
            var listByArea = (from gs in dal.getHostingUnitList()
                              group gs by gs.areaOfUnit).ToList();
            return listByArea;
        }
        #endregion
        #region fuc of dal
        /// <summary>
        /// a func that add guest request after checking the edges
        /// </summary>
        /// <param name="gs"></param>
        public void addCustomerReq(GuestRequest gs)//מה צריך לבדןק? איך בודקים מייל ואיך בודקים אם ההמפתח קיים אם הוא עוד לא מאותחל
        {
            if (isDateValid(gs.Clone().EntryDate, gs.Clone().ReleaseDate))
            {
                gs.RegistrationDate = new DateTime(2020, (DateTime.Today).Month, (DateTime.Today).Day);
                dal.addCustomerReq(gs);
            }
        }
        /// <summary>
        /// a func to update guest request
        /// </summary>
        /// <param name="gsKey"></param>
        /// <param name="stat"></param>
        public void updateCustomerReq(long gsKey, statusGusReq stat)
        {
            if (!(dal.getGuestRequestList().Any(gs1 => (gs1.GuestRequestKey == gsKey))))
                throw new System.ArgumentException("request dont exist!");
            if (stat != statusGusReq.נסגר_כי_פג_תוקף && stat != statusGusReq.נסגר_על_ידי_האתר && stat != statusGusReq.פתוח)
                throw new System.ArgumentException("dont have a status");
            dal.updateCustomerReq(gsKey, stat);
        }
        /// <summary>
        /// a func that returns the guest request list
        /// </summary>
        /// <returns></returns>
        public List<GuestRequest> getGuestRequestList()
        {
            return dal.getGuestRequestList();
        }
        /// <summary>
        /// a func that adds a new host unit to the list
        /// </summary>
        /// <param name="hostunit"></param>
        public void addHostingUnit(HostingUnit hostunit)
        {
            dal.addHostingUnit(hostunit);
        }
        /// <summary>
        /// a func that deletes a host unit from the list
        /// </summary>
        /// <param name="HstUnt"></param>
        public void deleteHostingUnit(HostingUnit HstUnt)
        {
            if (!(dal.getHostingUnitList().Any(hu => (hu.HostingUnitKey == HstUnt.HostingUnitKey))))
                throw new System.ArgumentException("hosting unit dont exist!");
            if (cantDel(HstUnt))
                dal.deleteHostingUnit(HstUnt);
            else
                throw new System.ArgumentException("can not delete hosting unit becase there is an open order!");
        }
        /// <summary>
        /// a func that updates hosting unit
        /// </summary>
        /// <param name="HstUnt"></param>
        public void UpdateHostingUnit(HostingUnit HstUnt)
        {
            if (!(dal.getHostingUnitList().Any(hu => (hu.HostingUnitKey == HstUnt.HostingUnitKey))))
                throw new System.ArgumentException("hosting unit dont exist!");
            dal.UpdateHostingUnit(HstUnt);
        }
        /// <summary>
        /// a func that returns the hosting unit list
        /// </summary>
        /// <returns></returns>
        public List<HostingUnit> getHostingUnitList()
        {
            return dal.getHostingUnitList();
        }
        /// <summary>
        /// a func to add an order to the list
        /// </summary>
        /// <param name="ord"></param>
        public void addOrder(Order ord)
        {
            if (dal.getOrderList().Any(ord1 => (ord1.OrderKey == ord.OrderKey)))
                throw new System.ArgumentException("order is exist");
            if(!checkIfOk(ord.GuestRequestKey, ord.HostingUnitKey))
                throw new System.ArgumentException("the hosting unit is not available in these dates!");
            ord.CreateDate = new DateTime(2020, (DateTime.Today).Month, (DateTime.Today).Day);
            dal.addOrder(ord);
        }
        /// <summary>
        /// a func to update order status
        /// </summary>
        /// <param name="orKey"></param>
        /// <param name="statO"></param>
        public void updateOrder(Order order)
        {
            if (!dal.getOrderList().Any(ord1 => (ord1.OrderKey ==order.OrderKey)))
                throw new System.ArgumentException("order is not exist");
            if (order.Status != statusOrder.טרם_טופל && order.Status != statusOrder.נסגר_בהיענות_של_לקוח && order.Status != statusOrder.נסגר_מחוסר_הענות_של_הלקוח && order.Status != statusOrder.נשלח_מייל)
                throw new System.ArgumentException("dont have a status");
            if (!CanChangeStatus(order))
                throw new System.ArgumentNullException("cant change status - satus is close");
            if (order.Status == statusOrder.נסגר_בהיענות_של_לקוח)
            {
                updateDiary(order);
                changeStatusOfOtherThings(order);
                order.OrderDate = new DateTime(2020, (DateTime.Today).Month, (DateTime.Today).Day);
            }
            if (order.Status == statusOrder.נשלח_מייל)
                if (allowedSendReq(order))
                    {
                    dal.updateOrder(order.OrderKey, order.Status);
                    sendEmailAfterStatusChange(order);
                    order.OrderDate = new DateTime(2020, (DateTime.Today).Month, (DateTime.Today).Day);
                    }
                else
                    throw new System.ArgumentException("cant change status to 'נשלח_מיל '");
            else
                dal.updateOrder(order.OrderKey, order.Status);
            
        }
        public void updateOrder2(Order order, statusOrder status)
        {
            if (!dal.getOrderList().Any(ord1 => (ord1.OrderKey == order.OrderKey)))
                throw new System.ArgumentException("order does not exist");
            if (status != statusOrder.טרם_טופל && status != statusOrder.נסגר_בהיענות_של_לקוח && status != statusOrder.נסגר_מחוסר_הענות_של_הלקוח && status != statusOrder.נשלח_מייל)
                throw new System.ArgumentException("dont have a status");
            if (!CanChangeStatus(order))
                throw new System.ArgumentNullException("cant change status - satus is close");
            if (status == statusOrder.נסגר_בהיענות_של_לקוח)
                if(order.Status==statusOrder.נשלח_מייל)
                {
                    updateDiary(order);
                    changeStatusOfOtherThings(order);
                    dal.updateOrder(order.OrderKey, status);
                    return;
                }
                else
                {
                    throw new System.ArgumentNullException("cant close order without sending e-mail to the client");

                }

            if (status == statusOrder.נשלח_מייל)
                if (allowedSendReq(order))
                {
                    dal.updateOrder(order.OrderKey, status);
                    sendEmailAfterStatusChange(order);
                    order.OrderDate = new DateTime(2020, (DateTime.Today).Month, (DateTime.Today).Day);
                }
                else
                    throw new System.ArgumentException("cant change status to 'נשלח_מיל '");
            else
                dal.updateOrder(order.OrderKey, status);

        }
        /// <summary>
        /// a func that returns the order list
        /// </summary>
        /// <returns></returns>
        public List<Order> getOrderList()
        {
            return dal.getOrderList();
        }
        /// <summary>
        /// a func that returns the bank branch list
        /// </summary>
        /// <returns></returns>
        public List<BankBranch> getBankBranches()
        {
            return dal.getBankBranches();
        }
        #endregion
        #region our functions
        /// <summary>
        /// a func that returns the statistics of the presantage os a hosting unit usage
        /// </summary>
        /// <param name="hostUn"></param>
        /// <returns></returns>
        public string statisticsForHostUn ( HostingUnit hostUn)//(21)
        {
            return string.Format("{0}% booked" ,(float)numOfDaysBooked(hostUn)[12]*100 / 372);
        }
        /// <summary>
        /// a func that calculate the total price for a vacation with host unit cost for one night times the number of days
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int calculateTotalPriceForVication(Order order)//(22)
        {
            //order.HostingUnitKey
            var hostingU = (from hosUn in dal.getHostingUnitList()
                            where (order.HostingUnitKey == hosUn.HostingUnitKey)
                            select hosUn).FirstOrDefault();
            var gs = (from g in dal.getGuestRequestList()
                      where g.GuestRequestKey == order.GuestRequestKey
                      select g).FirstOrDefault();
            return numOfDaysPassed(gs.EntryDate, gs.ReleaseDate) * hostingU.price;
        }
        /// <summary>
        /// a func that return a list of hosting units grouped by number of rooms
        /// </summary>
        /// <returns></returns>
        public List<IGrouping<int, HostingUnit>> groupByNumOfRooms()//(23)
        {
            var listBynumOfRooms = (from gs in dal.getHostingUnitList()
                                group gs by gs.numOfRoom).ToList();
            return listBynumOfRooms;
        }
        /// <summary>
        /// a func that returns a list of all the hosting units available for tomorrow
        /// </summary>
        /// <returns></returns>
        public List<HostingUnit> availableForTomorrow()//(24)
        {
            var hostingUList = (from hosUn in dal.getHostingUnitList()
                            where ( hosUn.diary[DateTime.Today.Month-1,DateTime.Today.Day-1]==false)
                            select hosUn).ToList();
            return hostingUList;
        }
        /// <summary>
        /// a function that returns the total number of days booked in a year
        /// </summary>
        /// <param name="hostingU"></param>
        /// <returns></returns>
        public List<int> numOfDaysBooked(HostingUnit hostingU)//(25)
        {
            List<int> everyMonth = new List<int>();
            int sum = 0;
            int sum2 = 0;
            int i = 0, k = 11;
            int j = 0, s = 30;
            while (i != k || j != s)
            {
                if (hostingU.diary[i, j] == true)
                {
                    sum++;
                    sum2++;
                }
                j++;
                if(j==30)
                {
                    everyMonth.Add(sum2);
                    sum2 = 0;
                }
                if (j == 31)
                {
                    
                    j = 0;
                    i++;
                    if (i == 12)
                        i = 0;
                }
            }
            everyMonth.Add(sum);
            return everyMonth;
        }
        /// <summary>
        /// a func that returns the profit from one hosting unit throgh the year
        /// </summary>
        /// <param name="hostingU"></param>
        /// <returns></returns>
        public int calculatProfitOfHostUn(HostingUnit hostingU)//(26)
        {
            return hostingU.price * numOfDaysBooked(hostingU)[12];
        }

            //בדיקת תעודת זהות 
            public  bool LegalId(string s)
            {
                int x;
                if (!int.TryParse(s, out x))
                    return false;
                if (s.Length < 5 || s.Length > 9)
                    return false;
                for (int i = s.Length; i < 9; i++)
                    s = "0" + s;
                int sum = 0;
                for (int i = 0; i < 9; i++)
                {
                    int k = ((i % 2) + 1) * (Convert.ToInt32(s[i]) - '0');
                    if (k > 9)
                        k -= 9;
                    sum += k;

                }
                return sum % 10 == 0;
            }

            //אותיות בלבד

            public  bool IsHebrew(string word)
            {
                string pattern = @"\b[א-ת-\s ]+$";
                Regex reg = new Regex(pattern);
                return reg.IsMatch(word);

            }
            //טלפון
            public  bool IsTelephone(string tel)
            {
                string pattern = @"\b0[ 2 4 7 8 3 77 72 73 79]-[2-9]\d{6}$";
                Regex reg = new Regex(pattern);
                return reg.IsMatch(tel);
            }

            //פלאפון
            public  bool IsCellPhone(string tel)
            {
                string pattern = @"\b05[0 2 4 6 7 8 3]-[2-9]\d{6}$";
                Regex reg = new Regex(pattern);
                return reg.IsMatch(tel);
            }


            //חישוב גיל לפי תאריך לידה
            public  int GetAge(DateTime d)
            {
                DateTime t = DateTime.Today;
                int age = t.Year - d.Year;
                if (t < d.AddYears(age)) age--;
                return age;
            }

            //בדיקה שהטקסט בפורמט של 
            public bool CheackMail(string t)
            {
                //דוא"ל
                if (t.Length == 0)
                    return true;
                else //בדיקה שהטקסט מכיל את הסימנים '.' ו-'@'.
                    if ((t.IndexOf("@") == -1) || (t.IndexOf(".") == -1))
                        return false;
                else //אם הכתובת נכונה
                    return true;

            }
            //   מספרים בלבד
            public  bool IsNumber(string num)
            {
                string pattern = @"\b[0-9-\s]+$";
                Regex reg = new Regex(pattern);
                return reg.IsMatch(num);
            }

        //תקינות תאריך
        public  bool IsDate(string date)
        {
            DateTime d;
            bool b = DateTime.TryParse(date, out d);
            return b;
        }

    #endregion


}
}
