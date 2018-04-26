using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DistrictSalespersonJunctionDAO
    {
        private string ConnectionString { get; set; }

        public DistrictSalespersonJunctionDAO()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["NeasDBConnection"].ToString();
        }

        public int Insert(District district, Salesperson salesperson)
        {
            int rowsAffected = 0;
            string query =
                "INSERT INTO District_Salesperson_Junction(district_id, salesperson_id)" +
                "Values (@districtId, @salespersonId);";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@districtId", district.Id);
                        command.Parameters.AddWithValue("@salespersonId", salesperson.Id);
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
            }
            return rowsAffected;
        }

        public int Delete(District district, Salesperson salesperson)
        {
            int rowsAffected = 0;
            string query =
                "DELETE FROM District_Salesperson_Junction WHERE district_id = @districtId AND salespersonid = @salespersonId";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@districtId", district.Id);
                        command.Parameters.AddWithValue("@salespersonId", salesperson.Id);
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
            }
            return rowsAffected;
        }

        public IEnumerable<District> GetDistrictsById(int salespersonId)
        {
            string query =
                "SELECT * FROM District_Salesperson_Junction WHERE salesperson_id = @salespersonId";
            List<District> found = new List<District>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@salespersonId", salespersonId);
                        SqlDataReader reader = command.ExecuteReader();
                        int districtIdOrdinal = reader.GetOrdinal("district_id");
                        DistrictDAO dDAL = new DistrictDAO();
                        while (reader.Read())
                        {
                            var foundDistrict = dDAL.GetById(reader.GetInt32(districtIdOrdinal));
                            found.Add(foundDistrict);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
            }
            return found;
        }

        public IEnumerable<Salesperson> GetSalespersonsById(int districtId)
        {
            string query =
                "SELECT * FROM District_Salesperson_Junction WHERE district_id = @districtId";
            List<Salesperson> found = new List<Salesperson>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@districtId", districtId);
                        SqlDataReader reader = command.ExecuteReader();
                        int salespersonIdOrdinal = reader.GetOrdinal("salesperson_id");
                        SalespersonDAO spDAL = new SalespersonDAO();
                        while (reader.Read())
                        {
                            var foundSalesperson = spDAL.GetById(reader.GetInt32(salespersonIdOrdinal));
                            found.Add(foundSalesperson);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
            }
            return found;
        }
    }
}
