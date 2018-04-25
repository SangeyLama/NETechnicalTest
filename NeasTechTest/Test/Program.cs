using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            SalespersonDAL spDAL = new SalespersonDAL();
            DistrictDAL dDAL = new DistrictDAL();
            DistrictSalespersonJunctionDAL DsjDAL = new DistrictSalespersonJunctionDAL();

            //Test Insert
            //var result = spDAL.Insert(new Salesperson { Name = "Sangey" });

            //Test get single
            //var result = spDAL.GetSalespersonById(1).ToString();

            //Test get all
            //var result = spDAL.GetAll();
            //foreach (Salesperson sp in result)
            //{
            //    Console.WriteLine(sp.ToString());
            //}

            //Test Update
            //spDAL.Update(new Salesperson { Id = 6, Name = "Updated" });
            //var result = spDAL.GetById(6).ToString();

            //Test Delete
            //spDAL.Delete(spDAL.GetSalespersonById(9));

            //Test District Insert
            //var result = dDAL.Insert(new District { Name = "East Denmark", PrimarySalesperson = spDAL.GetById(5) });

            //Test District Get
            var result = dDAL.GetById(1).ToString();

            //Test District GetAll
            //var result = dDAL.GetAll();
            //foreach (District d in result)
            //{
            //    Console.WriteLine(d.ToString());
            //}

            //Test District Update
            //dDAL.Update(new District { Id = 1, Name = "North Denmark Updated", PrimarySalesperson = spDAL.GetById(4) });

            //Test District Delete
            //dDAL.Delete(dDAL.GetById(3));

            //Test DistrictSalespersonJunction Insert
            //var result = DsjDAL.Insert(dDAL.GetById(2), spDAL.GetById(6));

            //Test DistrictSalespersonJunction Delete
            //var result = DsjDAL.Delete(dDAL.GetById(1), spDAL.GetById(4));

            //Test DistrictSalespersonJunction Get Districts
            //var result = DsjDAL.GetDistrictsById(6);
            //foreach (District d in result)
            //{
            //    Console.WriteLine(d.ToString());
            //}

            //Test DistrictSalespersonJunction Get Salespersons
            //var result = DsjDAL.GetSalespersonsById(1);
            //foreach (Salesperson sp in result)
            //{
            //    Console.WriteLine(sp.ToString());
            //}

            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
