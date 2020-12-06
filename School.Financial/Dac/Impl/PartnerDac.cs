using MySql.Data.MySqlClient;
using School.Financial.Models;
using System;
using System.Collections.Generic;

namespace School.Financial.Dac.Impl
{
    public class PartnerDac : IPartnerDac
    {
        private readonly SchoolFinancialContext context;

        public PartnerDac(SchoolFinancialContext context)
        {
            this.context = context;
        }

        public IEnumerable<Partner> Get()
        {
            var list = new List<Partner>();
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Partner", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(ReadData(reader));
                    }
                }
            }
            return list;
        }

        public Partner Get(int id)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Partner where id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return ReadData(reader);
                    }
                }
            }
            return null;
        }

        public int Insert(Partner data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `Partner` VALUES (0,@Name,@VatNumber,@Address,@PartnerType,@CreatedDate)", conn);
                cmd.Parameters.AddWithValue("@Name", data.Name);
                cmd.Parameters.AddWithValue("@VatNumber", data.VatNumber);
                cmd.Parameters.AddWithValue("@Address", data.Address);
                cmd.Parameters.AddWithValue("@PartnerType", data.PartnerType);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.UtcNow);

                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
        }

        public void Update(Partner data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE `Partner` SET `Name`=@Name,`VatNumber`=@VatNumber,`Address`=@Address,`PartnerType`=@PartnerType WHERE `Id`=@Id", conn);
                cmd.Parameters.AddWithValue("@Name", data.Name);
                cmd.Parameters.AddWithValue("@VatNumber", data.VatNumber);
                cmd.Parameters.AddWithValue("@Address", data.Address);
                cmd.Parameters.AddWithValue("@PartnerType", data.PartnerType);
                cmd.Parameters.AddWithValue("@Id", data.Id);

                cmd.ExecuteNonQuery();
            }
        }

        public int Upsert(Partner data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `Partner` VALUES (@Id,@Name,@VatNumber,@Address,@PartnerType,@CreatedDate)" +
                    "ON DUPLICATE KEY UPDATE `Name`=@Name,`VatNumber`=@VatNumber,`Address`=@Address,`PartnerType`=@PartnerType", conn);
                cmd.Parameters.AddWithValue("@Id", data.Id);
                cmd.Parameters.AddWithValue("@Name", data.Name);
                cmd.Parameters.AddWithValue("@VatNumber", data.VatNumber);
                cmd.Parameters.AddWithValue("@Address", data.Address);
                cmd.Parameters.AddWithValue("@PartnerType", data.PartnerType);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.UtcNow);

                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM `Partner` WHERE `id`=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }

        private Partner ReadData(MySqlDataReader reader)
        {
            return new Partner()
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = reader["Name"] == DBNull.Value ? null : reader["Name"].ToString(),
                VatNumber = reader["VatNumber"] == DBNull.Value ? null : reader["VatNumber"].ToString(),
                Address = reader["Address"] == DBNull.Value ? null : reader["Address"].ToString(),
                PartnerType = reader["PartnerType"] == DBNull.Value ? null : reader["PartnerType"].ToString(),
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
            };
        }
    }
}
