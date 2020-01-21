using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Host
    {
        public int HostKey { get; /*private*/ set; }
        public string PrivateName { set; get; }
        public string FamilyName { set; get; }
        public int PhoneNumber { set; get; }
        public string MailAdress { set; get; }
        public BankBranch BankBranchDetails { set; get; }
        public int BankAccountNumber { set; get; }
        public int NumOfHostingUnit { set; get; }
        public bool CollectionClearance { set; get; }
        public override string ToString()
        {
            return string.Format("Host Key: {0}\n", HostKey) +
                string.Format("Private Name: {0}\n", PrivateName) +
                string.Format("Family Name: {0}\n", FamilyName) +
                string.Format("Phone Number: {0}\n", PhoneNumber) +
                string.Format("Mail Adress: {0}\n", MailAdress) +
                string.Format("Bank Account: {0}\n", BankBranchDetails) +
                string.Format("Num Of Hosting Unit: {0}\n", NumOfHostingUnit) +
                string.Format("Collection Clearance: {0}\n", CollectionClearance);
        }
       
    }
}
