using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public District District { get; set; }

        public override string ToString()
        {
            return String.Format("Id: {0} \t Name: {1}", Id, Name);
        }
    }
}
