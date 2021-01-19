using MySql.Data.MySqlClient;
using School.Financial.Models;
using System;
using System.Collections.Generic;

namespace School.Financial.Dac.Impl
{
    public class EducationAreaDac : IEducationAreaDac
    {
        private readonly SchoolFinancialContext context;

        public EducationAreaDac(SchoolFinancialContext context)
        {
            this.context = context;
        }

        public IEnumerable<EducationArea> Get()
        {
            var list = new List<EducationArea>();
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from sao", conn);

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

        public EducationArea Get(int id)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from sao where sao_id = @id", conn);
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

        public EducationArea Get(string id)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from sao where sao_id = @id", conn);
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

        public int Insert(EducationArea data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `sao`(`sao_id`, `sao_type`, `sao_name`) VALUES (@Id,=@Type,@Name)", conn);
                cmd.Parameters.AddWithValue("@Type", data.sao_type);
                cmd.Parameters.AddWithValue("@Name", data.sao_name);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.UtcNow);

                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
        }

        public void Update(EducationArea data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE `sao` SET `sao_type`=@Type,`sao_name`=@Name WHERE `sao_id`=@Id", conn);
                cmd.Parameters.AddWithValue("@Type", data.sao_type);
                cmd.Parameters.AddWithValue("@Name", data.sao_name);
                cmd.Parameters.AddWithValue("@Id", data.sao_id);

                cmd.ExecuteNonQuery();
            }
        }

        public int Upsert(EducationArea data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `sao`(`sao_id`, `sao_type`, `sao_name`) VALUES (@Id,=@Type,@Name)" +
                    "ON DUPLICATE KEY UPDATE `sao_type`=@Type,`sao_name`=@Name", conn);
                cmd.Parameters.AddWithValue("@Id", data.sao_id);
                cmd.Parameters.AddWithValue("@Type", data.sao_type);
                cmd.Parameters.AddWithValue("@Name", data.sao_name);

                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM `sao` WHERE `sao_id`=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }

        private EducationArea ReadData(MySqlDataReader reader)
        {
            return new EducationArea()
            {
                sao_id = reader["sao_id"].ToString(),
                sao_type = reader["sao_type"].ToString(),
                sao_name = reader["sao_name"].ToString(),
            };
        }
    }
}
