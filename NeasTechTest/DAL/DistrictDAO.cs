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
    public class DistrictDAO
    {
        private string ConnectionString { get; set; }

        public DistrictDAO()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["NeasDBConnection"].ToString();
        }

        public int Insert(District district)
        {
            int lastId = 0;
            string query =
                "INSERT INTO Districts(name,primary_salesperson_id)" +
                "Values (@name, @salespersonId);" +
                "SELECT CAST(scope_identity() as int)";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", district.Name);
                        command.Parameters.AddWithValue("@salespersonId", district.PrimarySalesperson.Id);
                        Object newId = command.ExecuteScalar();
                        lastId = Convert.ToInt32(newId);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
            }
            return lastId;
        }

        public District GetById(int id)
        {
            string query =
                "SELECT * FROM Districts WHERE id = @id";
            District found = new District();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            found = BuildDistrict(reader);
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

        public IEnumerable<District> GetAll()
        {
            string query =
                "SELECT * FROM Districts";
            List<District> found = new List<District>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            District district = BuildDistrict(reader);
                            found.Add(district);
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

        public int Update(District district)
        {
            int rowsAffected = 0;
            string query =
                "UPDATE Districts SET name = @name, primary_salesperson_id = @primarySalespersonId WHERE id = @id";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", district.Name);
                        command.Parameters.AddWithValue("@primarySalespersonId", district.PrimarySalesperson.Id);
                        command.Parameters.AddWithValue("@id", district.Id);
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

        public int Delete(District district)
        {
            int rowsAffected = 0;
            string query =
                "DELETE FROM Districts WHERE id = @id AND name = @name AND primary_salesperson_id = @primarySalespersonId";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", district.Id);
                        command.Parameters.AddWithValue("@name", district.Name);
                        command.Parameters.AddWithValue("@primarySalespersonId", district.PrimarySalesperson.Id);
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

        public District BuildDistrict(SqlDataReader reader)
        {
            District district = new District();
            int idOrdinal = reader.GetOrdinal("id");
            int nameOrdinal = reader.GetOrdinal("name");
            int primarySalespersonIdOrdinal = reader.GetOrdinal("primary_salesperson_id");
            try
            {
                if (!reader.IsDBNull(idOrdinal) && !reader.IsDBNull(nameOrdinal) && !reader.IsDBNull(primarySalespersonIdOrdinal))
                {
                    district.Id = reader.GetInt32(idOrdinal);
                    district.Name = reader.GetString(nameOrdinal);
                    SalespersonDAO salespersonDal = new SalespersonDAO();
                    district.PrimarySalesperson = salespersonDal.GetById(reader.GetInt32(primarySalespersonIdOrdinal));
                    DistrictSalespersonJunctionDAO DsjDAL = new DistrictSalespersonJunctionDAO();
                    district.Salespersons = DsjDAL.GetSalespersonsById(district.Id);
                }
                else
                {
                    district.Name = "District not found";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in BuildDistrict: " + e.Message);
            }
            return district;
        }
    }
}

