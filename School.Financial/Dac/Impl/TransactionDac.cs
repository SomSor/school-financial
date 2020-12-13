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
                MySqlCommand cmd = new MySqlCommand("select * from Transaction where day(IssueDate) <= day(@month) and month(IssueDate) = month(@month) and year(IssueDate) = year(@month)", conn);
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
                MySqlCommand cmd = new MySqlCommand("select * from Transaction where month(IssueDate) = month(@month) and year(IssueDate) = year(@month) and BudgetId = @BudgetId", conn);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@BudgetId", budgetId);

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

        public IEnumerable<Transaction> GetTeackVat(DateTime month)
        {
            var list = new List<Transaction>();
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Transaction where month(IssueDate) = month(@month) and year(IssueDate) = year(@month) and VatInclude > 0", conn);
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

        public IEnumerable<Transaction> GetDuplicatePayment()
        {
            var list = new List<Transaction>();
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Transaction where PaymentType is not null and PaymentType <> ''", conn);

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

        public IEnumerable<TransactionWithPartner> GetWithVat(DateTime month)
        {
            var list = new List<TransactionWithPartner>();
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select tx.*," +
                    "pn.Id as pn_Id,pn.Name as pn_Name,pn.VatNumber as pn_VatNumber,pn.Address as pn_Address,pn.PartnerType as pn_PartnerType,pn.CreatedDate as pn_CreatedDate " +
                    "from Transaction as tx " +
                    "left join Partner as pn on tx.PartnerId = pn.Id " +
                    "where month(IssueDate) = month(@month) and year(IssueDate) = year(@month) and VatInclude > 0", conn);
                cmd.Parameters.AddWithValue("@month", month);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(ReadDataWithPartner(reader));
                    }
                }
            }
            return list;
        }

        public IEnumerable<TransactionWithPartner> GetWithPartner()
        {
            var list = new List<TransactionWithPartner>();
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select tx.*," +
                    "pn.Id as pn_Id,pn.Name as pn_Name,pn.VatNumber as pn_VatNumber,pn.Address as pn_Address,pn.PartnerType as pn_PartnerType,pn.CreatedDate as pn_CreatedDate " +
                    "from Transaction as tx " +
                    "left join Partner as pn on tx.PartnerId = pn.Id " +
                    "where tx.PartnerId is not null or tx.PartnerId<>''", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(ReadDataWithPartner(reader));
                    }
                }
            }
            return list;
        }

        public int Insert(Transaction data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `Transaction` VALUES (" +
                    "0,@IssueDate,@DuplicatePaymentType,@DuplicatePaymentNumber,@DuplicatePaymentYear," +
                    "@Title,@Remark,@PartnerId,@Amount,@PaymentType," +
                    "@VatInclude,@ProductType,@BudgetId,@SchoolId,@CreatedDate)", conn);
                cmd.Parameters.AddWithValue("@IssueDate", data.IssueDate);
                cmd.Parameters.AddWithValue("@DuplicatePaymentType", data.DuplicatePaymentType);
                cmd.Parameters.AddWithValue("@DuplicatePaymentNumber", data.DuplicatePaymentNumber);
                cmd.Parameters.AddWithValue("@DuplicatePaymentYear", data.DuplicatePaymentYear);
                cmd.Parameters.AddWithValue("@Title", data.Title);
                cmd.Parameters.AddWithValue("@Remark", data.Remark);
                cmd.Parameters.AddWithValue("@PartnerId", data.PartnerId);
                cmd.Parameters.AddWithValue("@Amount", data.Amount);
                cmd.Parameters.AddWithValue("@PaymentType", data.PaymentType);
                cmd.Parameters.AddWithValue("@VatInclude", data.VatInclude);
                cmd.Parameters.AddWithValue("@ProductType", data.ProductType);
                cmd.Parameters.AddWithValue("@BudgetId", data.BudgetId);
                cmd.Parameters.AddWithValue("@SchoolId", data.SchoolId);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.UtcNow);

                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
        }

        public int InsertPayment(Transaction data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `Transaction` VALUES (" +
                    "0,@IssueDate,@DuplicatePaymentType,@DuplicatePaymentNumber,@DuplicatePaymentYear," +
                    "@Title,@Remark,@PartnerId,@Amount,@PaymentType," +
                    "@VatInclude,@ProductType,@BudgetId,@SchoolId,@CreatedDate)", conn);
                cmd.Parameters.AddWithValue("@IssueDate", data.IssueDate);
                //TODO: auto running DuplicatePaymentNumber
                cmd.Parameters.AddWithValue("@DuplicatePaymentType", data.DuplicatePaymentType);
                cmd.Parameters.AddWithValue("@DuplicatePaymentNumber", data.DuplicatePaymentNumber);
                cmd.Parameters.AddWithValue("@DuplicatePaymentYear", data.DuplicatePaymentYear);
                cmd.Parameters.AddWithValue("@Title", data.Title);
                cmd.Parameters.AddWithValue("@Remark", data.Remark);
                cmd.Parameters.AddWithValue("@PartnerId", data.PartnerId);
                cmd.Parameters.AddWithValue("@Amount", data.Amount);
                cmd.Parameters.AddWithValue("@PaymentType", data.PaymentType);
                cmd.Parameters.AddWithValue("@VatInclude", data.VatInclude);
                cmd.Parameters.AddWithValue("@ProductType", data.ProductType);
                cmd.Parameters.AddWithValue("@BudgetId", data.BudgetId);
                cmd.Parameters.AddWithValue("@SchoolId", data.SchoolId);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.UtcNow);

                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
        }

        public void Update(Transaction data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE `Transaction` SET " +
                    "`IssueDate`=@IssueDate,`DuplicatePaymentType`=@DuplicatePaymentType,`DuplicatePaymentNumber`=@DuplicatePaymentNumber,`DuplicatePaymentYear`=@DuplicatePaymentYear,`Title`=@Title," +
                    "`Remark`=@Remark,`PartnerId`=@PartnerId,`Amount`=@Amount,`PaymentType`=@PaymentType,`VatInclude`=@VatInclude," +
                    "`ProductType`=@ProductType WHERE `Id`=@Id", conn);
                cmd.Parameters.AddWithValue("@IssueDate", data.IssueDate);
                cmd.Parameters.AddWithValue("@DuplicatePaymentType", data.DuplicatePaymentType);
                cmd.Parameters.AddWithValue("@DuplicatePaymentNumber", data.DuplicatePaymentNumber);
                cmd.Parameters.AddWithValue("@DuplicatePaymentYear", data.DuplicatePaymentYear);
                cmd.Parameters.AddWithValue("@Title", data.Title);
                cmd.Parameters.AddWithValue("@Remark", data.Remark);
                cmd.Parameters.AddWithValue("@PartnerId", data.PartnerId);
                cmd.Parameters.AddWithValue("@Amount", data.Amount);
                cmd.Parameters.AddWithValue("@PaymentType", data.PaymentType);
                cmd.Parameters.AddWithValue("@VatInclude", data.VatInclude);
                cmd.Parameters.AddWithValue("@ProductType", data.ProductType);
                cmd.Parameters.AddWithValue("@Id", data.Id);

                cmd.ExecuteNonQuery();
            }
        }

        public int Upsert(Transaction data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `Transaction` VALUES (" +
                    "@Id,@IssueDate,@DuplicatePaymentType,@DuplicatePaymentNumber,@DuplicatePaymentYear," +
                    "@Title,@Remark,@PartnerId,@Amount,@PaymentType," +
                    "@VatInclude,@ProductType,@BudgetId,@SchoolId,@CreatedDate)" +
                    "ON DUPLICATE KEY UPDATE " +
                    "`IssueDate`=@IssueDate,`DuplicatePaymentType`=@DuplicatePaymentType,`DuplicatePaymentNumber`=@DuplicatePaymentNumber,`DuplicatePaymentYear`=@DuplicatePaymentYear,`Title`=@Title," +
                    "`Remark`=@Remark,`PartnerId`=@PartnerId,`Amount`=@Amount,`PaymentType`=@PaymentType,`VatInclude`=@VatInclude," +
                    "`ProductType`=@ProductType", conn);
                cmd.Parameters.AddWithValue("@Id", data.Id);
                cmd.Parameters.AddWithValue("@IssueDate", data.IssueDate);
                cmd.Parameters.AddWithValue("@DuplicatePaymentType", data.DuplicatePaymentType);
                cmd.Parameters.AddWithValue("@DuplicatePaymentNumber", data.DuplicatePaymentNumber);
                cmd.Parameters.AddWithValue("@DuplicatePaymentYear", data.DuplicatePaymentYear);
                cmd.Parameters.AddWithValue("@Title", data.Title);
                cmd.Parameters.AddWithValue("@Remark", data.Remark);
                cmd.Parameters.AddWithValue("@PartnerId", data.PartnerId);
                cmd.Parameters.AddWithValue("@Amount", data.Amount);
                cmd.Parameters.AddWithValue("@PaymentType", data.PaymentType);
                cmd.Parameters.AddWithValue("@VatInclude", data.VatInclude);
                cmd.Parameters.AddWithValue("@ProductType", data.ProductType);
                cmd.Parameters.AddWithValue("@BudgetId", data.BudgetId);
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
                DuplicatePaymentType = reader["DuplicatePaymentType"] == DBNull.Value ? null : reader["DuplicatePaymentType"].ToString(),
                DuplicatePaymentNumber = reader["DuplicatePaymentNumber"] == DBNull.Value ? null : reader["DuplicatePaymentNumber"].ToString(),
                DuplicatePaymentYear = reader["DuplicatePaymentYear"] == DBNull.Value ? null : reader["DuplicatePaymentYear"].ToString(),
                Title = reader["Title"].ToString(),
                Remark = reader["Remark"] == DBNull.Value ? null : reader["Remark"].ToString(),
                PartnerId = reader["PartnerId"] == DBNull.Value ? default : Convert.ToInt32(reader["PartnerId"]),
                Amount = Convert.ToDecimal(reader["Amount"]),
                PaymentType = reader["PaymentType"] == DBNull.Value ? null : reader["PaymentType"].ToString(),
                VatInclude = reader["VatInclude"] == DBNull.Value ? null : (decimal?)reader["VatInclude"],
                ProductType = reader["ProductType"] == DBNull.Value ? null : reader["ProductType"].ToString(),
                BudgetId = Convert.ToInt32(reader["BudgetId"]),
                SchoolId = Convert.ToInt32(reader["SchoolId"]),
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
            };
        }

        private TransactionWithPartner ReadDataWithPartner(MySqlDataReader reader)
        {
            return new TransactionWithPartner()
            {
                Id = Convert.ToInt32(reader["Id"]),
                IssueDate = Convert.ToDateTime(reader["IssueDate"]),
                DuplicatePaymentType = reader["DuplicatePaymentType"] == DBNull.Value ? null : reader["DuplicatePaymentType"].ToString(),
                DuplicatePaymentNumber = reader["DuplicatePaymentNumber"] == DBNull.Value ? null : reader["DuplicatePaymentNumber"].ToString(),
                DuplicatePaymentYear = reader["DuplicatePaymentYear"] == DBNull.Value ? null : reader["DuplicatePaymentYear"].ToString(),
                Title = reader["Title"].ToString(),
                Remark = reader["Remark"] == DBNull.Value ? null : reader["Remark"].ToString(),
                PartnerId = reader["PartnerId"] == DBNull.Value ? default : Convert.ToInt32(reader["PartnerId"]),
                Amount = Convert.ToDecimal(reader["Amount"]),
                PaymentType = reader["PaymentType"] == DBNull.Value ? null : reader["PaymentType"].ToString(),
                VatInclude = reader["VatInclude"] == DBNull.Value ? null : (decimal?)reader["VatInclude"],
                ProductType = reader["ProductType"] == DBNull.Value ? null : reader["ProductType"].ToString(),
                BudgetId = Convert.ToInt32(reader["BudgetId"]),
                SchoolId = Convert.ToInt32(reader["SchoolId"]),
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                Partner = new Partner
                {
                    Id = Convert.ToInt32(reader["pn_Id"]),
                    Name = reader["pn_Name"] == DBNull.Value ? null : reader["pn_Name"].ToString(),
                    VatNumber = reader["pn_VatNumber"] == DBNull.Value ? null : reader["pn_VatNumber"].ToString(),
                    Address = reader["pn_Address"] == DBNull.Value ? null : reader["pn_Address"].ToString(),
                    PartnerType = reader["pn_PartnerType"] == DBNull.Value ? null : reader["pn_PartnerType"].ToString(),
                    CreatedDate = Convert.ToDateTime(reader["pn_CreatedDate"]),
                },
            };
        }
    }
}
