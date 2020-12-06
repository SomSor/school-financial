using School.Financial.Models;
using System;
using System.Collections.Generic;

namespace School.Financial.Dac
{
    public interface ITransactionDac : IDataDac<Transaction>
    {
        IEnumerable<Transaction> Get(DateTime month);
        IEnumerable<Transaction> Get(DateTime month, int budgetId);
        IEnumerable<Transaction> GetTeackVat(DateTime month);
        IEnumerable<Transaction> GetDuplicatePayment();

        IEnumerable<TransactionWithPartner> GetWithVat(DateTime month);
        IEnumerable<TransactionWithPartner> GetWithPartner();
    }
}
