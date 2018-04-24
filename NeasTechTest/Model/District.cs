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
        public ICollection<Store> Stores { get; set; }
        public ICollection<Salesperson> Salespersons { get; set; }

        public override string ToString()
        {
            return String.Format("Id: {0} \t Name: {1} \nPrimary Salesperson: {2}", Id, Name, PrimarySalesperson.ToString());
        }
    }
}
