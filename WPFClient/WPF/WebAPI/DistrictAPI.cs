using RestSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.WebAPI
{
    public class DistrictAPI
    {
        public IEnumerable<District> GetAll()
        {
            var client = new RestClient("http://localhost:54048/api/District/");
            var request = new RestRequest(Method.GET);
            IRestResponse<List<District>> response = client.Execute<List<District>>(request);
            return response.Data;
        }
        public District GetById(District district)
        {
            var client = new RestClient("http://localhost:54048/");
            var request = new RestRequest("api/District/{id}", Method.GET);
            request.AddUrlSegment("id", district.Id);
            IRestResponse<District> response = client.Execute<District>(request);
            return response.Data;
        }

        public IRestResponse Update(District district)
        {
            string districtData = JsonConvert.SerializeObject(district);
            var client = new RestClient("http://localhost:54048/");
            var request = new RestRequest("api/District/", Method.PUT);
            request.AddParameter("application/json; charset=utf-8", districtData, ParameterType.RequestBody);
            IRestResponse response = client.Execute<District>(request);
            return response;
        }
    }
}
