using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Data.SqlClient;

namespace DAL
{
    public class SalespersonDAL
    {
        private string ConnectionString { get; set; }

        public SalespersonDAL()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["NeasDBConnection"].ToString();
        }

        public int Insert(Salesperson salesperson)
        {
            int lastId = 0;
            string query =
                "INSERT INTO Salespersons(name)" +
                "Values (@name);" +
                "SELECT CAST(scope_identity() as int)";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", salesperson.Name);
                    Object newId = command.ExecuteScalar();
                    lastId = Convert.ToInt32(newId);
                }
            }
            return lastId;
        }

        public Salesperson GetSalespersonById(int id)
        {
            string query = "SELECT * FROM Salespersons WHERE id = @id";
            Salesperson found = new Salesperson();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        found = BuildSalesperson(reader);
                    }
                }
            }
            return found;
        }

        public IEnumerable<Salesperson> GetAllSalespersons()
        {
            string query = "SELECT * FROM Salespersons";
            List<Salesperson> found = new List<Salesperson>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Salesperson salesperson = BuildSalesperson(reader);
                        found.Add(salesperson);
                    }
                }
            }
            return found;
        }

        public void Update(Salesperson salesperson)
        {
            string query = "UPDATE salespersons SET name = @name WHERE id = @id";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", salesperson.Name);
                    command.Parameters.AddWithValue("@id", salesperson.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(Salesperson salesperson)
        {
            string query = "DELETE FROM Salespersons WHERE id = @id AND name = @name";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", salesperson.Name);
                    command.Parameters.AddWithValue("@id", salesperson.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        private Salesperson BuildSalesperson(SqlDataReader reader)
        {
            Salesperson salesperson = new Salesperson();
            try
            {
                if (!reader.IsDBNull(0) && !reader.IsDBNull(1))
                {
                    salesperson.Id = reader.GetInt32(0);
                    salesperson.Name = reader.GetString(1);
                }
                else
                {
                    salesperson.Name = "Salesperson not found";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in BuildSalesperson: " + e.Message);
            }

            return salesperson;
        }


    }
}
