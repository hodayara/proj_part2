using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DS1;
using BE;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace DAL
{
    
    class Dal_imp : Idal
    {

        #region func GuestReq
        /// <summary>
        /// פונקצית עזר
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckGuestReq(long id)
        {

            return DS1.DataSource.GuestRequestList.Any(gs1 => (gs1.Clone()).GuestRequestKey == id);
        }
        /// <summary>
        /// a func that adds a new guest request to the list
        /// </summary>
        /// <param name="gs"></param>
        public void addCustomerReq(GuestRequest gs)
        {
            
            if (CheckGuestReq(gs.GuestRequestKey))
                throw new System.ArgumentException("request already exist!");
            gs.NumOfPeople = gs.Adults + gs.Children;
            BE.Configurations.guestReqID++;
            gs.GuestRequestKey = BE.Configurations.guestReqID;
            DS1.DataSource.GuestRequestList.Add(gs.Clone());
        }
        /// <summary>
        /// a func that updates a status of a guest request
        /// </summary>
        /// <param name="gsKey"></param>
        /// <param name="stat"></param>
        public void updateCustomerReq(long gsKey, statusGusReq stat)
        {
            if (!CheckGuestReq(gsKey))
                throw new System.ArgumentException("request dont exist!");
            var new_gs = (from item in DS1.DataSource.GuestRequestList
                                               where gsKey == item.GuestRequestKey
                                             select item).FirstOrDefault();
            if(stat != statusGusReq.נסגר_כי_פג_תוקף&& stat != statusGusReq.נסגר_על_ידי_האתר&& stat != statusGusReq.פתוח)
                throw new System.ArgumentException("dont have a status");
            new_gs.Status = stat;

        }
        /// <summary>
        /// a func that returns the guest request list
        /// </summary>
        /// <returns></returns>
        ///public IEnumerable<Student> Student GetStudents()
        public List<GuestRequest>  getGuestRequestList()
        {
            return (from gs in DataSource.GuestRequestList
                   select gs.Clone()).ToList();
            //return DS1.DataSource.GuestRequestList;
        }
        #endregion

        #region func HostUnit
        /// <summary>
        /// פונקצית עזר
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool checkHostingUnit(long id)
        {
            return DS1.DataSource.HostingUnitList.Any(hu => hu.Clone().HostingUnitKey == id);
        }
        /// <summary>
        /// a func that adds a new host unit to the list
        /// </summary>
        /// <param name="hostunit"></param>
        public void addHostingUnit(HostingUnit hostunit)
        {
            if (checkHostingUnit(hostunit.HostingUnitKey))
                throw new System.ArgumentException("host unit already exist!");
            var y = (from hu in DS1.DataSource.HostingUnitList
                     where hu.Owner.HostKey == hostunit.Owner.HostKey
                     select hu).ToList();
            y.ForEach(hu => hu.Owner.NumOfHostingUnit++);
            y.ForEach(hu => hostunit.Owner.NumOfHostingUnit++);
            BE.Configurations.hostUnitKey++;
            hostunit.HostingUnitKey = BE.Configurations.hostUnitKey;
            
            DS1.DataSource.HostingUnitList.Add(hostunit.Clone());
        }
        /// <summary>
        /// a func that deletes a host unit from the list
        /// </summary>
        /// <param name="HstUnt"></param>
        public void deleteHostingUnit(HostingUnit HstUnt)
        {
            if (!checkHostingUnit(HstUnt.HostingUnitKey))
                throw new System.ArgumentException("host unit does not exist!");
            var new_hu = (from item in DS1.DataSource.HostingUnitList
                          where item.HostingUnitKey == HstUnt.HostingUnitKey
                          select item).FirstOrDefault();
            DS1.DataSource.HostingUnitList.Remove(new_hu);
        }
        /// <summary>
        /// a func that updates hosting unit
        /// </summary>
        /// <param name="HstUnt"></param>
        public void UpdateHostingUnit(HostingUnit HstUnt)
        {
            var new_hu = (from item in DS1.DataSource.HostingUnitList
                                           where item.HostingUnitKey == HstUnt.HostingUnitKey
                          select item).FirstOrDefault();
            if(HstUnt.HostingUnitName!=null)
                new_hu.HostingUnitName = HstUnt.HostingUnitName;
            new_hu.Owner = HstUnt.Owner;
            if(HstUnt.diary != null)
                for (int i = 0; i < 12; i++)
                    for (int j = 0; j < 31; j++)
                         new_hu.diary[i, j] = HstUnt.diary[i, j];
            
        }
        /// <summary>
        /// a func that returns the hosting unit list
        /// </summary>
        /// <returns></returns>
        public List<HostingUnit> getHostingUnitList()
        {
            return (from hu in DataSource.HostingUnitList
                    select hu.Clone()).ToList();
        }
        #endregion

        #region func order
        /// <summary>
        /// פונקצית עזר
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool checkOrder(long id)
        {
            return DS1.DataSource.OrderList.Any(or => or.Clone().OrderKey == id);
        }
        /// <summary>
        /// a func to add an order to the list
        /// </summary>
        /// <param name="ord"></param>
        public void addOrder(Order ord)
        {
            if (checkOrder(ord.OrderKey))
                throw new System.ArgumentException("order already exist!");

            BE.Configurations.orderKey++;
            ord.OrderKey = BE.Configurations.orderKey;
            DS1.DataSource.OrderList.Add(ord.Clone());
        }
        /// <summary>
        /// a func to update order status
        /// </summary>
        /// <param name="orKey"></param>
        /// <param name="statO"></param>
        public void updateOrder(long orKey, statusOrder statO)
        {
            var new_ord = (from item in DS1.DataSource.OrderList
                                         where item.OrderKey == orKey
                           select item).FirstOrDefault();
            if(statO!=statusOrder.טרם_טופל && statO != statusOrder.נסגר_בהיענות_של_לקוח && statO != statusOrder.נסגר_מחוסר_הענות_של_הלקוח && statO != statusOrder.נשלח_מייל )
                throw new System.ArgumentException("dont have a status");
            new_ord.Status = statO;
            if(statO==statusOrder.נסגר_בהיענות_של_לקוח)
                new_ord.OrderDate = new DateTime(2020, (DateTime.Today).Month, (DateTime.Today).Day);

        }
        /// <summary>
        /// a func that returns the order list
        /// </summary>
        /// <returns></returns>
        public List<Order> getOrderList()
        {
            return (from ord in DataSource.OrderList
                    select ord.Clone()).ToList();
        }
        #endregion
        /// <summary>
        /// a func that returns the bank branch list
        /// </summary>
        /// <returns></returns>
        public List<BankBranch> getBankBranches()
        {
            DS1.DataSource.BankAccountList = new List<BankBranch> {
                new BankBranch{ BankNumber=11,BankName="discont"},
                new BankBranch{BankNumber=20,BankName="mizrachi"},
                new BankBranch{BankNumber=12,BankName="hapohalim"},
                new BankBranch{BankNumber=17,BankName="marcil discont"},
                new BankBranch{BankNumber=10,BankName="leomi"}
            };
            return (from ba in DataSource.BankAccountList
                    select ba.Clone()).ToList();
        }
    }
}

