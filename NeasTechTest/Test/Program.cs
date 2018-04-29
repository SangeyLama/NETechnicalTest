using DAL;
using Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        //Quick Testing for methods
        static void Main(string[] args)
        {

            SalespersonDAO spDAL = new SalespersonDAO();
            DistrictDAO dDAL = new DistrictDAO();
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
            //var result = dDAL.UpdateDS(new District { Id = 1, Name = "North Denmark Update2", PrimarySalesperson = spDAL.GetById(5) });

            //Test District Delete
            //dDAL.Delete(dDAL.GetById(3));

            //Test Get District using a data set
            //var result = dDAL.GetByIdDataSet(3).ToString();

            //Test Store Insert
            //var result = sDAO.Insert(new Store { Name = "Fotex" });

            //Test Store GetByID
            //var result = sDAO.GetById(1);

            //Test Store GetAll
            //var result = sDAO.GetAll();
            //foreach (Store s in result)
            //{
            //    Console.WriteLine(s.ToString());
            //}

            // Test Store Update
            //var result = sDAO.Update(new Store { Id = 1, Name = "Rema 1000 Updated"});

            //Test Store Delete
            //var result = sDAO.Delete(sDAO.GetById(3));

            //District dist = dDAL.GetByIdDataSet(2);
            //var tempSalespersons = new List<Salesperson>();
            //tempSalespersons.Add(spDAL.GetById(1));
            //tempSalespersons.Add(spDAL.GetById(3));
            //tempSalespersons.Add(spDAL.GetById(5));
            //tempSalespersons.Add(spDAL.GetById(7));
            //dist.Salespersons = tempSalespersons;
            //var result = dDAL.UpdateSalespersonsList(dist);

            //Console.WriteLine(result.ToString());
            Console.ReadLine();
        }

    }
}
