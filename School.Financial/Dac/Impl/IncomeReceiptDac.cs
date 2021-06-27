using MySql.Data.MySqlClient;
using School.Financial.Models;
using System;
using System.Collections.Generic;

namespace School.Financial.Dac.Impl
{
    public class IncomeReceiptDac : IIncomeReceiptDac
    {
        private readonly SchoolFinancialContext context;

        public IncomeReceiptDac(SchoolFinancialContext context)
        {
            this.context = context;
        }

        public IEnumerable<IncomeReceipt> Get()
        {
            var list = new List<IncomeReceipt>();
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from IncomeReceipt", conn);

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

        public IncomeReceipt Get(int id)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from IncomeReceipt where id = @id", conn);
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

        public int Insert(IncomeReceipt data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `IncomeReceipt` VALUES (0,@IssueDate,@ReceiveFrom,@Remark,@Amount,@CreatedDate)", conn);
                cmd.Parameters.AddWithValue("@IssueDate", data.IssueDate);
                cmd.Parameters.AddWithValue("@ReceiveFrom", data.ReceiveFrom);
                cmd.Parameters.AddWithValue("@Remark", data.Remark);
                cmd.Parameters.AddWithValue("@Amount", data.Amount);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.UtcNow);

                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
        }

        public void Update(IncomeReceipt data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE `IncomeReceipt` SET `IssueDate`=@IssueDate,`ReceiveFrom`=@ReceiveFrom,`Remark`=@Remark,`Amount`=@Amount WHERE `Id`=@Id", conn);
                cmd.Parameters.AddWithValue("@IssueDate", data.IssueDate);
                cmd.Parameters.AddWithValue("@ReceiveFrom", data.ReceiveFrom);
                cmd.Parameters.AddWithValue("@Remark", data.Remark);
                cmd.Parameters.AddWithValue("@Amount", data.Amount);
                cmd.Parameters.AddWithValue("@Id", data.Id);

                cmd.ExecuteNonQuery();
            }
        }

        public int Upsert(IncomeReceipt data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `IncomeReceipt` VALUES (@Id,@IssueDate,@ReceiveFrom,@Remark,@Amount,@CreatedDate)" +
                    "ON DUPLICATE KEY UPDATE `IssueDate`=@IssueDate,`ReceiveFrom`=@ReceiveFrom,`Remark`=@Remark,`Amount`=@Amount", conn);
                cmd.Parameters.AddWithValue("@Id", data.Id);
                cmd.Parameters.AddWithValue("@IssueDate", data.IssueDate);
                cmd.Parameters.AddWithValue("@ReceiveFrom", data.ReceiveFrom);
                cmd.Parameters.AddWithValue("@Remark", data.Remark);
                cmd.Parameters.AddWithValue("@Amount", data.Amount);
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
                MySqlCommand cmd = new MySqlCommand("DELETE FROM `IncomeReceipt` WHERE `id`=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }

        private IncomeReceipt ReadData(MySqlDataReader reader)
        {
            return new IncomeReceipt()
            {
                Id = Convert.ToInt32(reader["Id"]),
                IssueDate = Convert.ToDateTime(reader["IssueDate"]),
                ReceiveFrom = reader["ReceiveFrom"].ToString(),
                Remark = reader["Remark"].ToString(),
                Amount = Convert.ToDecimal(reader["Amount"]),
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
            };
        }
    }
}
