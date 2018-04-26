using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class SalespersonDAO
    {
        private string ConnectionString { get; set; }

        public SalespersonDAO()
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
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = salesperson.Name;
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

        public Salesperson GetById(int id)
        {
            string query =
                "SELECT * FROM Salespersons WHERE id = @id";
            Salesperson found = new Salesperson();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            found = BuildSalesperson(reader);
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

        public IEnumerable<Salesperson> GetAll()
        {
            string query =
                "SELECT * FROM Salespersons";
            List<Salesperson> found = new List<Salesperson>();
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
                            Salesperson salesperson = BuildSalesperson(reader);
                            found.Add(salesperson);
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

        public int Update(Salesperson salesperson)
        {
            int rowsAffected = 0;
            string query =
                "UPDATE Salespersons SET name = @name WHERE id = @id";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = salesperson.Name;
                        command.Parameters.Add("@id", SqlDbType.Int).Value = salesperson.Id;
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

        public int Delete(Salesperson salesperson)
        {
            int rowsAffected = 0;
            string query =
                "DELETE FROM Salespersons WHERE id = @id AND name = @name";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = salesperson.Name;
                        command.Parameters.Add("@id", SqlDbType.Int).Value = salesperson.Id;
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

        private Salesperson BuildSalesperson(SqlDataReader reader)
        {
            Salesperson salesperson = new Salesperson();
            int idOrdinal = reader.GetOrdinal("id");
            int nameOrdinal = reader.GetOrdinal("name");
            try
            {
                if (!reader.IsDBNull(idOrdinal) && !reader.IsDBNull(nameOrdinal))
                {
                    salesperson.Id = reader.GetInt32(idOrdinal);
                    salesperson.Name = reader.GetString(nameOrdinal);
                }
                else
                {
                    return null;
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
