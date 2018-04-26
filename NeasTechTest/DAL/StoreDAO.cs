using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class StoreDAO
    {
        private string ConnectionString { get; set; }
        public StoreDAO()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["NeasDBConnection"].ToString();
        }

        public int Insert(Store store)
        {
            int lastId = 0;
            string query =
                "INSERT INTO Stores(name)" +
                "Values (@name);" +
                "SELECT CAST(scope_identity() as int)";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = store.Name;
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

        public Store GetById(int id)
        {
            string query =
                "SELECT * FROM Stores WHERE id = @id";
            Store found = new Store();
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
                            found = BuildStore(reader);
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

        public IEnumerable<Store> GetAll()
        {
            string query =
                "SELECT * FROM Stores";
            List<Store> found = new List<Store>();
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
                            Store store = BuildStore(reader);
                            found.Add(store);
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

        public int Update(Store store)
        {
            int rowsAffected = 0;
            string query =
                "UPDATE Stores SET name = @name, district_id = @districtId WHERE id = @id";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = store.Name;
                        if (store.District != null)
                        {
                            command.Parameters.Add("@districtId", SqlDbType.Int).Value = store.District?.Id;
                        }
                        else
                        {
                            command.Parameters.Add("@districtId", SqlDbType.Int).Value = DBNull.Value;
                        }
                        command.Parameters.Add("@id", SqlDbType.Int).Value = store.Id;
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

        public int Delete(Store store)
        {
            int rowsAffected = 0;
            string query =
                "DELETE FROM Stores WHERE id = @id";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@id", SqlDbType.Int).Value = store.Id;
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

        private Store BuildStore(SqlDataReader reader)
        {
            Store store = new Store();
            int idOrdinal = reader.GetOrdinal("id");
            int nameOrdinal = reader.GetOrdinal("name");
            int districtIdOrdinal = reader.GetOrdinal("district_id");
            try
            {
                if (!reader.IsDBNull(idOrdinal) && !reader.IsDBNull(nameOrdinal))
                {
                    store.Id = reader.GetInt32(idOrdinal);
                    store.Name = reader.GetString(nameOrdinal);
                    if (!reader.IsDBNull(districtIdOrdinal))
                    {
                        store.District = new DistrictDAO().GetById(reader.GetInt32(districtIdOrdinal));
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in BuildStore: " + e.Message);
            }
            return store;
        }
    }
}
