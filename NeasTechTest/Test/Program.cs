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

            SalespersonDAO spDAL = new SalespersonDAO();
            DistrictDAO dDAL = new DistrictDAO();
            DistrictSalespersonJunctionDAO DsjDAL = new DistrictSalespersonJunctionDAO();
            StoreDAO sDAO = new StoreDAO();

            //Test Insert
            //var result = spDAL.Insert(new Salesperson { Name = "Sangey" });

            //Test get single
            //var result = spDAL.GetById(1).ToString();

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
            //var result = dDAL.GetById(1).ToString();

            //Test District GetAll
            //var result = dDAL.GetAll();
            //foreach (District d in result)
            //{
            //    Console.WriteLine(d.ToString());
            //}

            //Test District Update
            //var result = dDAL.Update(new District { Id = 1, Name = "North Denmark Updated", PrimarySalesperson = spDAL.GetById(4) });

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

            //Test Get District using a data set
            //var result = dDAL.GetByIdDataSet(3).ToString();

            //Test Store Insert
            //var result = sDAO.Insert(new Store { Name = "Fotex" });

            //Test Store GetByID
            //var result = sDAO.GetById(1);

            //Test Store GetAll
            var result = sDAO.GetAll();
            foreach (Store s in result)
            {
                Console.WriteLine(s.ToString());
            }

            // Test Store Update
            //var result = sDAO.Update(new Store { Id = 1, Name = "Rema 1000 Updated"});

            //Test Store Delete
            //var result = sDAO.Delete(sDAO.GetById(3));

            //Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
