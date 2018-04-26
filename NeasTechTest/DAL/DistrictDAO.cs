﻿using Model;
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
                "Values (@name, @salespersonId);" +
                "SELECT CAST(scope_identity() as int)";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = district.Id;
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
                        command.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = district.Id;
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
                        command.Parameters.Add("@id", SqlDbType.Int).Value = district.Id;
                        command.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = district.Id;
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

        public District GetByIdDataSet(int id)
        {
            DataSet districtSalesperson = new DataSet("districtSalesperson");
            string districtsQuery =
                "SELECT * FROM Districts WHERE id = @districtId";
            SqlDataAdapter adapter = new SqlDataAdapter(districtsQuery, ConnectionString);
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
                foreach(DataRow distStoreRow in distRow.GetChildRows(dataSet.Relations["DistStore"]))
                {
                    int storeId = (int)distStoreRow["id"];
                    string storeName = (string)distStoreRow["name"];
                    var tempStore = new Store { Id = storeId, Name = storeName };
                    tempStoreList.Add(tempStore);
                }
                districtObject.Stores = tempStoreList;

                var tempSPList = new List<Salesperson>();
                foreach(DataRow distSalesRow in distRow.GetChildRows(dataSet.Relations["DistrictJunc"]))
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
                    DistrictSalespersonJunctionDAO DsjDAL = new DistrictSalespersonJunctionDAO();
                    district.Salespersons = DsjDAL.GetSalespersonsById(district.Id);
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

