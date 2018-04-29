using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.WebAPI
{
    public static class SalespersonAPI
    {
        public static IEnumerable<Salesperson> GetAll()
        {
            var client = new RestClient("http://localhost:54048/api/Salesperson/");
            var request = new RestRequest(Method.GET);
            IRestResponse<List<Salesperson>> response = client.Execute<List<Salesperson>>(request);
            return response.Data;
        }
    }
}
