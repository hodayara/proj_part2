using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public static class Cloning
    {
        public static BankBranch Clone(this BankBranch original)
        {
            BankBranch target = new BankBranch();
            target.BranchNmber = original.BranchNmber;
            target.BranchCity = original.BranchCity;
            target.BankName = original.BankName;
            target.BankNumber = original.BankNumber;
            target.BranchAdress = original.BranchAdress;

            return target;

        }
        public static Host Clone(this Host original)
        {
            Host target = new Host();
            target.BankAccountNumber = original.BankAccountNumber;
            target.BankBranchDetails = original.BankBranchDetails;
            target.CollectionClearance = original.CollectionClearance;
            target.FamilyName = original.FamilyName;
            target.HostKey = original.HostKey;
            target.MailAdress = original.MailAdress;
            target.NumOfHostingUnit = original.NumOfHostingUnit;
            target.PhoneNumber = original.PhoneNumber;
            target.PrivateName = original.PrivateName;
            return target;
        }
        public static HostingUnit Clone(this HostingUnit original)
        {
            HostingUnit target = new HostingUnit();
            target.areaOfUnit = original.areaOfUnit;
            target.HostingUnitKey = original.HostingUnitKey;
            target.HostingUnitName = original.HostingUnitName;
            target.numOfRoom = original.numOfRoom;
            target.Owner = original.Owner;
            target.price = original.price;
            target.diary = original.diary;
            return target;
        }
        public static GuestRequest Clone(this GuestRequest original)
        {
            GuestRequest target = new GuestRequest();
            target.Adults = original.Adults;
            target.Area = original.Area;
            target.Children = original.Children;
            target.ChildrensAttractions = original.ChildrensAttractions;
            target.EntryDate = original.EntryDate;
            target.FamilyName = original.FamilyName;
            target.Garden = original.Garden;
            target.GuestRequestKey = original.GuestRequestKey;
            target.Jacuzzi = original.Jacuzzi;
            target.MailAddress = original.MailAddress;
            target.Pool = original.Pool;
            target.PrivateName = original.PrivateName;
            target.RegistrationDate = original.RegistrationDate;
            target.ReleaseDate = original.ReleaseDate;
            target.Status = original.Status;
            target.Id = original.Id;
            target.SubArea = original.SubArea;
            target.Type = original.Type;
            target.NumOfPeople = original.NumOfPeople;
            return target;
        }
        public static Order Clone(this Order original)
        {
            Order target = new Order();
            target.CreateDate = original.CreateDate;
            target.GuestRequestKey = original.GuestRequestKey;
            target.HostingUnitKey = original.HostingUnitKey;
            target.OrderDate = original.OrderDate;
            target.OrderKey = original.OrderKey;
            target.Status = original.Status;
            return target;
        }
    }
}
