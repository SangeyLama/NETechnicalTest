using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF
{
    public class District
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Salesperson PrimarySalesperson { get; set; }
        public IEnumerable<Store> Stores { get; set; }
        public IEnumerable<Salesperson> Salespersons { get; set; }

        public override string ToString()
        {
            string line = String.Format("Id: {0} \nName: {1} ", Id, Name);
            if(PrimarySalesperson != null)
            {
                line += String.Format("\nPrimary Salesperson: {0}", PrimarySalesperson?.Name);
            }
            if (Salespersons?.Count() != 0 && Salespersons != null)
            {
                line += "\nSecondary Salespersons:";
                foreach (Salesperson sp in Salespersons)
                {
                    line += String.Format("\n {0}", sp.ToString());
                }
            }
            if (Stores?.Count() != 0 && Stores != null)
            {
                line += "\nStores in district:";
                foreach (Store s in Stores)
                {
                    line += String.Format("\n {0}", s.ToString());
                }
            }
            return line;
        }
    }
}
