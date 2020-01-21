using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class GuestRequest
    {
        public long GuestRequestKey { get; set; }
        public string PrivateName { set; get; }
        public string FamilyName { set; get; }
        public int Id { set; get; }
        public string MailAddress { set; get; }
        public statusGusReq Status { set; get; }
        public DateTime RegistrationDate { get; set; }
        public DateTime EntryDate { set; get; }
        public DateTime ReleaseDate { get; set; }
        public area Area { set; get; }
        public subArea SubArea { set; get; }
        public type Type { set; get; }
        public int Adults { set; get; }
        public int Children { set; get; }

        public int NumOfPeople { set; get; }
        public isNecessary Pool { set; get; }
        public isNecessary Jacuzzi { set; get; }
        public isNecessary Garden { set; get; }
        public isNecessary ChildrensAttractions { set; get; }
       

        public override string ToString()
        {
            return string.Format("Guest Request Key: {0}\n", GuestRequestKey)
                + string.Format("name: {0}\n", PrivateName)
                + string.Format("Family Name: {0}\n", FamilyName)
                + string.Format("Mail Address: {0}\n", MailAddress)
                + string.Format("Status: {0}\n", Status)
                + string.Format("Registration Date: {0}\n", RegistrationDate)
                + string.Format("Entry Date: {0}\n", EntryDate)
                + string.Format("Release Date: {0}\n", ReleaseDate)
                + string.Format("Area: {0}\n", Area)
                + string.Format("SubArea: {0}\n", SubArea)
                + string.Format("Type: {0}\n", Type)
                + string.Format("Number of Adults: {0}\n", Adults)
                + string.Format("Number of Children: {0}\n", Children)
                + string.Format("pool: {0}\n", Pool)
                + string.Format("Jacuzzi: {0}\n", Jacuzzi)
                + string.Format("Garden: {0}\n", Garden)
                + string.Format("ChildrensAttractions: {0}\n", ChildrensAttractions);
        }

    }
}
