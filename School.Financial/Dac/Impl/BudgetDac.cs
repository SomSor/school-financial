using MySql.Data.MySqlClient;
using School.Financial.Models;
using System;
using System.Collections.Generic;

namespace School.Financial.Dac.Impl
{
    public class BudgetDac : IBudgetDac
    {
        private readonly SchoolFinancialContext context;

        public BudgetDac(SchoolFinancialContext context)
        {
            this.context = context;
        }

        public IEnumerable<Budget> Get()
        {
            var list = new List<Budget>();
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Budget", conn);

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

        public Budget Get(int id)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Budget where id = @id", conn);
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

        public int Insert(Budget data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `Budget` VALUES (0,@Name,@BankAccountId,@SchoolId,@CreatedDate)", conn);
                cmd.Parameters.AddWithValue("@Name", data.Name);
                cmd.Parameters.AddWithValue("@BankAccountId", data.BankAccountId);
                cmd.Parameters.AddWithValue("@SchoolId", data.SchoolId);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.UtcNow);

                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
        }

        public void Update(Budget data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE `Budget` SET `Name`=@Name WHERE `Id`=@Id", conn);
                cmd.Parameters.AddWithValue("@Name", data.Name);
                cmd.Parameters.AddWithValue("@Id", data.Id);

                cmd.ExecuteNonQuery();
            }
        }

        public int Upsert(Budget data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `Budget` VALUES (@Id,@Name,@BankAccountId,@SchoolId,@CreatedDate)" +
                    "ON DUPLICATE KEY UPDATE `Name`=@Name", conn);
                cmd.Parameters.AddWithValue("@Id", data.Id);
                cmd.Parameters.AddWithValue("@Name", data.Name);
                cmd.Parameters.AddWithValue("@BankAccountId", data.BankAccountId);
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
                MySqlCommand cmd = new MySqlCommand("DELETE FROM `Budget` WHERE `id`=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }

        private Budget ReadData(MySqlDataReader reader)
        {
            return new Budget()
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = reader["Name"].ToString(),
                BankAccountId = Convert.ToInt32(reader["BankAccountId"]),
                SchoolId = Convert.ToInt32(reader["SchoolId"]),
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
            };
        }
    }
}
