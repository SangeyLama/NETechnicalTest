using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
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
            string line = String.Format("Id: {0} \t Name: {1} \nPrimary Salesperson: {2} \nSecondary Salespersons:", Id, Name, PrimarySalesperson.ToString());
            foreach (Salesperson sp in Salespersons)
            {
                line += String.Format("\n {0}", sp.ToString());
            }
            return line;
        }
    }
}
