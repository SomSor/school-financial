using MySql.Data.MySqlClient;
using School.Financial.Models;
using System;
using System.Collections.Generic;

namespace School.Financial.Dac.Impl
{
    public class TransactionDac : ITransactionDac
    {
        private readonly SchoolFinancialContext context;

        public TransactionDac(SchoolFinancialContext context)
        {
            this.context = context;
        }

        public IEnumerable<Transaction> Get()
        {
            var list = new List<Transaction>();
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Transaction", conn);

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

        public Transaction Get(int id)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Transaction where id = @id", conn);
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

        public IEnumerable<Transaction> Get(DateTime month)
        {
            var list = new List<Transaction>();
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Transaction where month(IssueDate) = month(@month) and year(IssueDate) = year(@month)", conn);
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

        public IEnumerable<Transaction> Get(DateTime month, int budgetId)
        {
            var list = new List<Transaction>();
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Transaction where month(IssueDate) = month(@month) and year(IssueDate) = year(@month) and budgetId = @budgetId", conn);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@budgetId", budgetId);

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

        public IEnumerable<Transaction> GetWithVat(DateTime month)
        {
            var list = new List<Transaction>();
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Transaction where month(IssueDate) = month(@month) and year(IssueDate) = year(@month) and IsTrackVat", conn);
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

        public void Insert(Transaction data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `Transaction` VALUES (0,@IssueDate,@PayEvidence,@Title,@Remark,@PartnerId,@Amount,@IsTrackVat,@VatInclude,@Remain,@Cash,@Deposit,@BudgetId,@CreatedDate)", conn);
                cmd.Parameters.AddWithValue("@IssueDate", data.IssueDate);
                cmd.Parameters.AddWithValue("@PayEvidence", data.PayEvidence);
                cmd.Parameters.AddWithValue("@Title", data.Title);
                cmd.Parameters.AddWithValue("@Remark", data.Remark);
                cmd.Parameters.AddWithValue("@PartnerId", data.PartnerId);
                cmd.Parameters.AddWithValue("@Amount", data.Amount);
                cmd.Parameters.AddWithValue("@IsTrackVat", data.IsTrackVat);
                cmd.Parameters.AddWithValue("@VatInclude", data.VatInclude);
                cmd.Parameters.AddWithValue("@Remain", data.Remain);
                cmd.Parameters.AddWithValue("@Cash", data.Cash);
                cmd.Parameters.AddWithValue("@Deposit", data.Deposit);
                cmd.Parameters.AddWithValue("@BudgetId", data.BudgetId);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.UtcNow);

                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Transaction data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE `Transaction` SET `IssueDate`=@IssueDate,`Title`=@Title,`Amount`=@Amount,`Remain`=@Remain,`Cash`=@Cash,`Deposit`=@Deposit,`Remark`=@Remark WHERE `Id`=@Id", conn);
                cmd.Parameters.AddWithValue("@IssueDate", data.IssueDate);
                cmd.Parameters.AddWithValue("@Title", data.Title);
                cmd.Parameters.AddWithValue("@Amount", data.Amount);
                cmd.Parameters.AddWithValue("@Remain", data.Remain);
                cmd.Parameters.AddWithValue("@Cash", data.Cash);
                cmd.Parameters.AddWithValue("@Deposit", data.Deposit);
                cmd.Parameters.AddWithValue("@Remark", data.Remark);
                cmd.Parameters.AddWithValue("@Id", data.Id);

                cmd.ExecuteNonQuery();
            }
        }

        public void Upsert(Transaction data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `Transaction` VALUES (@Id,@IssueDate,@PayEvidence,@Title,@Remark,@PartnerId,@Amount,@IsTrackVat,@VatInclude,@Remain,@Cash,@Deposit,@budgetId,@CreatedDate)" +
                    "ON DUPLICATE KEY UPDATE `IssueDate`=@IssueDate,`Title`=@Title,`Amount`=@Amount,`Remain`=@Remain,`Cash`=@Cash,`Deposit`=@Deposit,`Remark`=@Remark", conn);
                cmd.Parameters.AddWithValue("@Id", data.Id);
                cmd.Parameters.AddWithValue("@IssueDate", data.IssueDate);
                cmd.Parameters.AddWithValue("@PayEvidence", data.PayEvidence);
                cmd.Parameters.AddWithValue("@Title", data.Title);
                cmd.Parameters.AddWithValue("@Remark", data.Remark);
                cmd.Parameters.AddWithValue("@PartnerId", data.PartnerId);
                cmd.Parameters.AddWithValue("@Amount", data.Amount);
                cmd.Parameters.AddWithValue("@IsTrackVat", data.IsTrackVat);
                cmd.Parameters.AddWithValue("@VatInclude", data.VatInclude);
                cmd.Parameters.AddWithValue("@Remain", data.Remain);
                cmd.Parameters.AddWithValue("@Cash", data.Cash);
                cmd.Parameters.AddWithValue("@Deposit", data.Deposit);
                cmd.Parameters.AddWithValue("@budgetId", data.BudgetId);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.UtcNow);

                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM `Transaction` WHERE `id`=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }

        private Transaction ReadData(MySqlDataReader reader)
        {
            return new Transaction()
            {
                Id = Convert.ToInt32(reader["Id"]),
                IssueDate = Convert.ToDateTime(reader["IssueDate"]),
                PayEvidence = reader["PayEvidence"] == DBNull.Value ? null : reader["PayEvidence"].ToString(),
                Title = reader["Title"].ToString(),
                Remark = reader["Remark"] == DBNull.Value ? null : reader["Remark"].ToString(),
                PartnerId = reader["PartnerId"] == DBNull.Value ? default : Convert.ToInt32(reader["PartnerId"]),
                Amount = Convert.ToDecimal(reader["Amount"]),
                IsTrackVat = reader["IsTrackVat"] == DBNull.Value ? null : (bool?)reader["IsTrackVat"],
                VatInclude = reader["VatInclude"] == DBNull.Value ? null : (decimal?)reader["VatInclude"],
                Remain = Convert.ToDecimal(reader["Remain"]),
                Cash = Convert.ToDecimal(reader["Cash"]),
                Deposit = Convert.ToDecimal(reader["Deposit"]),
                BudgetId = Convert.ToInt32(reader["budgetId"]),
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
            };
        }
    }
}
