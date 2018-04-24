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

            //Test Insert
            //var result = spDAL.Insert(new Salesperson { Name = "Sangey" });

            //Test get single
            //var result = spDAL.GetSalespersonById(1).ToString();

            //Test get all
            //var result = spDAL.GetAllSalespersons();
            //foreach(Salesperson sp in result)
            //{
            //    Console.WriteLine(sp.ToString());
            //}

            //Test Update
            //spDAL.Update(new Salesperson { Id = 6, Name = "Updated" });
            //var result = spDAL.GetSalespersonById(6).ToString();

            //Test Delete
            //spDAL.Delete(spDAL.GetSalespersonById(9));

            //Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
