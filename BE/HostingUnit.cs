using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class HostingUnit
    {
        public long HostingUnitKey { set; get; }
        public Host Owner { set; get; }
        public string HostingUnitName { set; get; }
        public bool[,] diary { set; get; }
        public area areaOfUnit { set; get; }
        public int price { set; get; }
        public int numOfRoom { set; get; }

        public override string ToString()
        {
            string s = "";
            s += string.Format("Hosting UnitKey: {0}\n", HostingUnitKey)
                + string.Format("Owner: {0}\n", Owner)
                + string.Format("Hosting Unit Name: {0}\n", HostingUnitName)
                + string.Format("price: {0}\n", price)
                + string.Format("num of room: {0}\n", numOfRoom);
            bool start = false;
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 31; j++)
                {
                    if (start == false && diary[i, j] == true)
                    {
                        s += string.Format("start period: {0}/{1}\n", j + 1, i + 1);
                        start = true;
                    }
                    if (start == true && diary[i, j] == false)
                    {
                        s += string.Format("end period: {0}/{1}\n", j + 1, i + 1);
                        start = false;
                    }

                }
            }
            return s;
        }
    }
}
