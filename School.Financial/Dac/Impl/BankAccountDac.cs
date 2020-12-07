using MySql.Data.MySqlClient;
using School.Financial.Models;
using System;
using System.Collections.Generic;

namespace School.Financial.Dac.Impl
{
    public class BankAccountDac : IBankAccountDac
    {
        private readonly SchoolFinancialContext context;

        public BankAccountDac(SchoolFinancialContext context)
        {
            this.context = context;
        }

        public IEnumerable<BankAccount> Get()
        {
            var list = new List<BankAccount>();
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from BankAccount", conn);

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

        public BankAccount Get(int id)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from BankAccount where id = @id", conn);
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

        public int Insert(BankAccount data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `BankAccount` VALUES (0,@BankName,@AccountName,@AccountNumber,@CreatedDate)", conn);
                cmd.Parameters.AddWithValue("@BankName", data.BankName);
                cmd.Parameters.AddWithValue("@AccountName", data.AccountName);
                cmd.Parameters.AddWithValue("@AccountNumber", data.AccountNumber);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.UtcNow);

                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
        }

        public void Update(BankAccount data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE `BankAccount` SET `BankName`=@BankName,`AccountName`=@AccountName,`AccountNumber`=@AccountNumber WHERE `Id`=@Id", conn);
                cmd.Parameters.AddWithValue("@BankName", data.BankName);
                cmd.Parameters.AddWithValue("@AccountName", data.AccountName);
                cmd.Parameters.AddWithValue("@AccountNumber", data.AccountNumber);
                cmd.Parameters.AddWithValue("@Id", data.Id);

                cmd.ExecuteNonQuery();
            }
        }

        public int Upsert(BankAccount data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `BankAccount` VALUES (@Id,@BankName,@AccountName,@AccountNumber,@CreatedDate)" +
                    "ON DUPLICATE KEY UPDATE `BankName`=@BankName,`AccountName`=@AccountName,`AccountNumber`=@AccountNumber", conn);
                cmd.Parameters.AddWithValue("@Id", data.Id);
                cmd.Parameters.AddWithValue("@BankName", data.BankName);
                cmd.Parameters.AddWithValue("@AccountName", data.AccountName);
                cmd.Parameters.AddWithValue("@AccountNumber", data.AccountNumber);
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
                MySqlCommand cmd = new MySqlCommand("DELETE FROM `BankAccount` WHERE `id`=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }

        private BankAccount ReadData(MySqlDataReader reader)
        {
            return new BankAccount()
            {
                Id = Convert.ToInt32(reader["Id"]),
                BankName = reader["BankName"].ToString(),
                AccountName = reader["AccountName"].ToString(),
                AccountNumber = reader["AccountNumber"].ToString(),
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
            };
        }
    }
}
