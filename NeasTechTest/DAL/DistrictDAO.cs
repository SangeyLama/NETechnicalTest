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
                "Values (@name, @primarySalespersonId);" +
                "SELECT CAST(scope_identity() as int)";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = district.Name;
                        command.Parameters.Add("@primarySalespersonId", SqlDbType.Int).Value = district.PrimarySalesperson.Id;
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
                        command.Parameters.Add("@id", SqlDbType.Int).Value = district.Id;
                        command.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = district.Name;
                        command.Parameters.Add("@primarySalespersonId", SqlDbType.Int).Value = district.PrimarySalesperson.Id;
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

        public int UpdateDS(District district)
        {
            int rowsAffected = 0;
            string selectQuery =
                "SELECT * FROM Districts Where id = @id";
            string updateQuery =
                "UPDATE Districts SET name = @name, primary_salesperson_id = @primarySalespersonId WHERE id = @oldId AND name = @oldName AND primary_salesperson_id = @oldPrimarySalespersonId";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, connection))
                {
                    adapter.SelectCommand.Parameters.Add("@id", SqlDbType.Int).Value = district.Id;
                    adapter.UpdateCommand = new SqlCommand(updateQuery, connection);
                    adapter.UpdateCommand.Parameters.Add("@name", SqlDbType.NVarChar, 50, "name");
                    adapter.UpdateCommand.Parameters.Add("@primarySalespersonId", SqlDbType.Int).SourceColumn = "primary_salesperson_id";

                    SqlParameter parameter = adapter.UpdateCommand.Parameters.Add("@oldId", SqlDbType.Int);
                    parameter.SourceColumn = "id";
                    parameter.SourceVersion = DataRowVersion.Original;
                    parameter = adapter.UpdateCommand.Parameters.Add("@oldName", SqlDbType.NVarChar, 50, "name");
                    parameter.SourceVersion = DataRowVersion.Original;
                    parameter = adapter.UpdateCommand.Parameters.Add("@oldPrimarySalespersonId", SqlDbType.Int);
                    parameter.SourceColumn = "primary_salesperson_id";
                    parameter.SourceVersion = DataRowVersion.Original;

                    adapter.RowUpdated += new SqlRowUpdatedEventHandler(OnRowUpdated);

                    DataTable districtTable = new DataTable();
                    adapter.Fill(districtTable);

                    DataRow districtRow = districtTable.Rows[0];
                    districtRow["name"] = district.Name;
                    districtRow["primary_salesperson_id"] = district.PrimarySalesperson.Id;

                    rowsAffected = adapter.Update(districtTable);

                    if (districtRow.HasErrors)
                        Console.WriteLine("Update error:" + "\n" + districtRow.RowError);
                }
            }
            return rowsAffected;
        }

        public int UpdateSalespersonsList(District district)
        {
            int rowsAffected = 0;
            DataSet districtSalespersons = new DataSet("districtSalespersons");
            string selectQuery =
                "SELECT * FROM District_Salesperson_Junction WHERE district_id = @districtId";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, connection))
                {
                    adapter.SelectCommand.Parameters.Add("@districtId", SqlDbType.Int).Value = district.Id;
                    adapter.Fill(districtSalespersons, "District_Salesperson_Junction");

                    var PKdistSales = new DataColumn[2];
                    PKdistSales[0] = districtSalespersons.Tables["District_Salesperson_Junction"].Columns["district_id"];
                    PKdistSales[1] = districtSalespersons.Tables["District_Salesperson_Junction"].Columns["salesperson_id"];
                    districtSalespersons.Tables["District_Salesperson_Junction"].PrimaryKey = PKdistSales;

                    string salespersonQuery =
                       "SELECT * FROM Salespersons";
                    adapter.SelectCommand.CommandText = salespersonQuery;
                    adapter.Fill(districtSalespersons, "Salespersons");

                    districtSalespersons.Relations.Add("SalespersonJunc", districtSalespersons.Tables["Salespersons"].Columns["id"],
                        districtSalespersons.Tables["District_Salesperson_Junction"].Columns["salesperson_id"]);

                    string insertQuery =
                        "INSERT INTO District_Salesperson_Junction(district_id, salesperson_id)" +
                        "Values (@districtId, @salespersonId);";
                    adapter.InsertCommand = new SqlCommand(insertQuery, connection);
                    adapter.InsertCommand.Parameters.Add("@districtId", SqlDbType.Int).SourceColumn = "district_id";
                    adapter.InsertCommand.Parameters.Add("@salespersonId", SqlDbType.Int).SourceColumn = "salesperson_id";

                    string deleteQuery =
                        "DELETE FROM District_Salesperson_Junction WHERE district_id = @districtId AND salesperson_id = @salespersonId";
                    adapter.DeleteCommand = new SqlCommand(deleteQuery, connection);
                    adapter.DeleteCommand.Parameters.Add("@districtId", SqlDbType.Int).SourceColumn = "district_id";
                    adapter.DeleteCommand.Parameters.Add("@salespersonId", SqlDbType.Int).SourceColumn = "salesperson_id";

                    DataTable table = districtSalespersons.Tables["District_Salesperson_Junction"];
                    foreach (DataRow row in table.Rows)
                    {
                        row.Delete();
                    }
                    if (district.Salespersons != null || district.Salespersons.Count() != 0)
                    {
                        foreach (Salesperson sp in district.Salespersons)
                        {
                            DataRow row = table.NewRow();
                            row["district_id"] = district.Id;
                            row["salesperson_id"] = sp.Id;
                            table.Rows.Add(row);
                        }
                        Object[] keys = new Object[2];
                        keys[0] = district.Id;
                        keys[1] = district.PrimarySalesperson.Id;
                        var found = table.Rows.Find(keys);
                        if (found == null)
                        {
                            DataRow row = table.NewRow();
                            row["district_id"] = district.Id;
                            row["salesperson_id"] = district.PrimarySalesperson.Id;
                            table.Rows.Add(row);
                        }
                    }
                    else
                    {
                        DataRow row = table.NewRow();
                        row["district_id"] = district.Id;
                        row["salesperson_id"] = district.PrimarySalesperson.Id;
                        table.Rows.Add(row);
                    }

                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();
                    adapter.InsertCommand.Transaction = transaction;
                    adapter.DeleteCommand.Transaction = transaction;
                    try
                    {
                        rowsAffected = adapter.Update(table);
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return rowsAffected;
        }

        protected static void OnRowUpdated(object sender, SqlRowUpdatedEventArgs args)
        {
            if (args.RecordsAffected == 0)
            {
                args.Row.RowError = "Optimistic Concurrency Violation Encountered";
                args.Status = UpdateStatus.SkipCurrentRow;
            }
        }

        public int Delete(District district)
        {
            int rowsAffected = 0;
            string query =
                "DELETE FROM Districts WHERE id = @id";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@id", SqlDbType.Int).Value = district.Id;
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

        public District GetByIdDataSet(int id)
        {
            DataSet districtSalesperson = new DataSet("districtSalesperson");
            string districtsQuery =
                "SELECT * FROM Districts WHERE id = @districtId";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(districtsQuery, connection))
                {
                    adapter.SelectCommand.Parameters.Add("@districtId", SqlDbType.Int).Value = id;

                    adapter.Fill(districtSalesperson, "Districts");

                    string salespersonQuery =
                        "SELECT * FROM Salespersons";
                    adapter.SelectCommand.CommandText = salespersonQuery;
                    adapter.Fill(districtSalesperson, "Salespersons");

                    string districtSalespersonQuery =
                        "SELECT * FROM District_Salesperson_Junction WHERE district_id = @districtId1";
                    adapter.SelectCommand.CommandText = districtSalespersonQuery;
                    adapter.SelectCommand.Parameters.Add("@districtId1", SqlDbType.Int).Value = id;
                    adapter.Fill(districtSalesperson, "District_Salesperson_Junction");

                    string storeQuery =
                        "SELECT * FROM Stores where district_id = @districtId2";
                    adapter.SelectCommand.CommandText = storeQuery;
                    adapter.SelectCommand.Parameters.Add("@districtId2", SqlDbType.Int).Value = id;
                    adapter.Fill(districtSalesperson, "Stores");

                    //Not sure if necessary
                    //var PKdistSales = new DataColumn[2];
                    //PKdistSales[0] = districtSalesperson.Tables["District_Salesperson_Junction"].Columns["district_id"];
                    //PKdistSales[1] = districtSalesperson.Tables["District_Salesperson_Junction"].Columns["salesperson_id"];
                    //districtSalesperson.Tables["District_Salesperson_Junction"].PrimaryKey = PKdistSales;

                    districtSalesperson.Relations.Add("DistrictJunc", districtSalesperson.Tables["Districts"].Columns["id"],
                        districtSalesperson.Tables["District_Salesperson_Junction"].Columns["district_id"]);

                    districtSalesperson.Relations.Add("SalespersonJunc", districtSalesperson.Tables["Salespersons"].Columns["id"],
                        districtSalesperson.Tables["District_Salesperson_Junction"].Columns["salesperson_id"]);

                    districtSalesperson.Relations.Add("DistStore", districtSalesperson.Tables["Districts"].Columns["id"],
                        districtSalesperson.Tables["Stores"].Columns["district_id"]);
                }
            }
            return BuildDistrict(districtSalesperson);
        }

        public District BuildDistrict(DataSet dataSet)
        {
            District districtObject = null;
            foreach (DataRow distRow in dataSet.Tables["Districts"].Rows)
            {
                int id = (int)distRow["id"];
                string name = (string)distRow["name"];
                districtObject = new District { Id = id, Name = name };

                var primarySP = new Salesperson();
                var primarySPRowData = dataSet.Tables["Salespersons"].Select("id = " + (int)distRow["primary_salesperson_id"]);
                primarySP.Id = (int)primarySPRowData.FirstOrDefault()["id"];
                primarySP.Name = (string)primarySPRowData.FirstOrDefault()["name"];
                districtObject.PrimarySalesperson = primarySP;

                var tempStoreList = new List<Store>();
                foreach (DataRow distStoreRow in distRow.GetChildRows(dataSet.Relations["DistStore"]))
                {
                    int storeId = (int)distStoreRow["id"];
                    string storeName = (string)distStoreRow["name"];
                    var tempStore = new Store { Id = storeId, Name = storeName };
                    tempStoreList.Add(tempStore);
                }
                districtObject.Stores = tempStoreList;

                var tempSPList = new List<Salesperson>();
                foreach (DataRow distSalesRow in distRow.GetChildRows(dataSet.Relations["DistrictJunc"]))
                {
                    int spId = (int)distSalesRow.GetParentRow(dataSet.Relations["SalespersonJunc"])["id"];
                    string spName = (string)distSalesRow.GetParentRow(dataSet.Relations["SalespersonJunc"])["name"];
                    var tempSalesperson = new Salesperson { Id = spId, Name = spName };
                    tempSPList.Add(tempSalesperson);
                }
                districtObject.Salespersons = tempSPList;
            }
            return districtObject;
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
                    //DistrictSalespersonJunctionDAO DsjDAL = new DistrictSalespersonJunctionDAO();
                    //district.Salespersons = DsjDAL.GetSalespersonsById(district.Id);
                }
                else
                {
                    return null;
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

