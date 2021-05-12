using MySql.Data.MySqlClient;
using School.Financial.Models;
using System;
using System.Collections.Generic;

namespace School.Financial.Dac.Impl
{
    public class IncomeDetailDac : IIncomeDetailDac
    {
        private readonly SchoolFinancialContext context;

        public IncomeDetailDac(SchoolFinancialContext context)
        {
            this.context = context;
        }

        public IEnumerable<IncomeDetail> Get()
        {
            var list = new List<IncomeDetail>();
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from IncomeDetail", conn);

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

        public IncomeDetail Get(int id)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from IncomeDetail where id = @id", conn);
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

        public int Insert(IncomeDetail data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `IncomeDetail` VALUES (0,@Title,@Amount,@TransactionId,@CreatedDate)", conn);
                cmd.Parameters.AddWithValue("@Title", data.Title);
                cmd.Parameters.AddWithValue("@Amount", data.Amount);
                cmd.Parameters.AddWithValue("@TransactionId", data.TransactionId);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.UtcNow);

                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
        }

        public void Update(IncomeDetail data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE `IncomeDetail` SET `Title`=@Title,`Amount`=@Amount WHERE `Id`=@Id", conn);
                cmd.Parameters.AddWithValue("@Title", data.Title);
                cmd.Parameters.AddWithValue("@Amount", data.Amount);
                cmd.Parameters.AddWithValue("@Id", data.Id);

                cmd.ExecuteNonQuery();
            }
        }

        public int Upsert(IncomeDetail data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `IncomeDetail` VALUES (@Id,@Title,@Amount,@TransactionId,@CreatedDate)" +
                    "ON DUPLICATE KEY UPDATE `Title`=@Title,`Amount`=@Amount", conn);
                cmd.Parameters.AddWithValue("@Id", data.Id);
                cmd.Parameters.AddWithValue("@Title", data.Title);
                cmd.Parameters.AddWithValue("@Amount", data.Amount);
                cmd.Parameters.AddWithValue("@TransactionId", data.TransactionId);
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
                MySqlCommand cmd = new MySqlCommand("DELETE FROM `IncomeDetail` WHERE `id`=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }

        private IncomeDetail ReadData(MySqlDataReader reader)
        {
            return new IncomeDetail()
            {
                Id = Convert.ToInt32(reader["Id"]),
                Title = reader["Title"].ToString(),
                Amount = Convert.ToDecimal(reader["Amount"]),
                TransactionId = Convert.ToInt32(reader["TransactionId"]),
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
            };
        }
    }
}
