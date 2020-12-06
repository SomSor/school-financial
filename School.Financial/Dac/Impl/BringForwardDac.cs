using MySql.Data.MySqlClient;
using School.Financial.Models;
using System;
using System.Collections.Generic;

namespace School.Financial.Dac.Impl
{
    public class BringForwardDac : IBringForwardDac
    {
        private readonly SchoolFinancialContext context;

        public BringForwardDac(SchoolFinancialContext context)
        {
            this.context = context;
        }

        public IEnumerable<BringForward> Get()
        {
            var list = new List<BringForward>();
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from BringForward", conn);

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

        public BringForward Get(int id)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from BringForward where id = @id", conn);
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

        public IEnumerable<BringForward> Get(DateTime month)
        {
            var list = new List<BringForward>();
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from BringForward where month(Month) = month(@month) and year(Month) = year(@month)", conn);
                cmd.Parameters.AddWithValue("@month", month);

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

        public BringForward Get(DateTime month, int budgetId)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from BringForward where month(Month) = month(@month) and year(Month) = year(@month) and budgetId = @budgetId", conn);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@budgetId", budgetId);

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

        public int Insert(BringForward data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `BringForward` VALUES (0,@Amount,@Month,@BudgetId,@CreatedDate)", conn);
                cmd.Parameters.AddWithValue("@Amount", data.Amount);
                cmd.Parameters.AddWithValue("@Month", data.Month);
                cmd.Parameters.AddWithValue("@BudgetId", data.BudgetId);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.UtcNow);

                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
        }

        public void Update(BringForward data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE `BringForward` SET `Amount`=@Amount,`Month`=@Month,`BudgetId`=@BudgetId WHERE `Id`=@Id", conn);
                cmd.Parameters.AddWithValue("@Amount", data.Amount);
                cmd.Parameters.AddWithValue("@Month", data.Month);
                cmd.Parameters.AddWithValue("@BudgetId", data.BudgetId);
                cmd.Parameters.AddWithValue("@Id", data.Id);

                cmd.ExecuteNonQuery();
            }
        }

        public int Upsert(BringForward data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `BringForward` VALUES (@Id,@Amount,@Month,@BudgetId,@CreatedDate)" +
                    "ON DUPLICATE KEY UPDATE `Amount`=@Amount,`Month`=@Month,`BudgetId`=@BudgetId", conn);
                cmd.Parameters.AddWithValue("@Id", data.Id);
                cmd.Parameters.AddWithValue("@Amount", data.Amount);
                cmd.Parameters.AddWithValue("@Month", data.Month);
                cmd.Parameters.AddWithValue("@BudgetId", data.BudgetId);
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
                MySqlCommand cmd = new MySqlCommand("DELETE FROM `BringForward` WHERE `id`=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }

        private BringForward ReadData(MySqlDataReader reader)
        {
            return new BringForward()
            {
                Id = Convert.ToInt32(reader["Id"]),
                Amount = Convert.ToDecimal(reader["Amount"]),
                Month = Convert.ToDateTime(reader["Month"]),
                BudgetId = Convert.ToInt32(reader["BudgetId"]),
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
            };
        }
    }
}
