using MySql.Data.MySqlClient;
using School.Financial.Models;
using System;
using System.Collections.Generic;

namespace School.Financial.Dac.Impl
{
    public class SchoolYearDac : ISchoolYearDac
    {
        private readonly SchoolFinancialContext context;

        public SchoolYearDac(SchoolFinancialContext context)
        {
            this.context = context;
        }

        public IEnumerable<SchoolYear> Get()
        {
            var list = new List<SchoolYear>();
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from SchoolYear", conn);

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

        public SchoolYear Get(int id)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from SchoolYear where id = @id", conn);
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

        public int Insert(SchoolYear data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `SchoolYear` VALUES (0,@Year,@StartDate,@SchoolId,@CreatedDate)", conn);
                cmd.Parameters.AddWithValue("@Year", data.Year);
                cmd.Parameters.AddWithValue("@StartDate", data.StartDate);
                cmd.Parameters.AddWithValue("@SchoolId", data.SchoolId);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.UtcNow);

                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
        }

        public void Update(SchoolYear data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE `SchoolYear` SET `Year`=@Year,`StartDate`=@StartDate WHERE `Id`=@Id", conn);
                cmd.Parameters.AddWithValue("@Year", data.Year);
                cmd.Parameters.AddWithValue("@StartDate", data.StartDate);
                cmd.Parameters.AddWithValue("@Id", data.Id);

                cmd.ExecuteNonQuery();
            }
        }

        public int Upsert(SchoolYear data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `SchoolYear` VALUES (@Id,@Year,@StartDate,@SchoolId,@CreatedDate)" +
                    "ON DUPLICATE KEY UPDATE `Year`=@Year,`StartDate`=@StartDate", conn);
                cmd.Parameters.AddWithValue("@Id", data.Id);
                cmd.Parameters.AddWithValue("@Year", data.Year);
                cmd.Parameters.AddWithValue("@StartDate", data.StartDate);
                cmd.Parameters.AddWithValue("@SchoolId", data.SchoolId);
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
                MySqlCommand cmd = new MySqlCommand("DELETE FROM `SchoolYear` WHERE `id`=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }

        private SchoolYear ReadData(MySqlDataReader reader)
        {
            return new SchoolYear()
            {
                Id = Convert.ToInt32(reader["Id"]),
                Year = reader["Year"].ToString(),
                StartDate = Convert.ToDateTime(reader["StartDate"]),
                SchoolId = Convert.ToInt32(reader["SchoolId"]),
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
            };
        }
    }
}
