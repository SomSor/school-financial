using System;
using System.Collections.Generic;

namespace School.Financial.Dac
{
    public interface ITransactionDac : IDataDac<Models.Transaction>
    {
        IEnumerable<Models.Transaction> Get(string ids);
        IEnumerable<Models.Transaction> Get(DateTime month);
        IEnumerable<Models.Transaction> Get(DateTime month, int budgetId);
        IEnumerable<Models.Transaction> GetTeackVat(DateTime month);
        IEnumerable<Models.Transaction> GetDuplicatePayment();

        IEnumerable<Models.TransactionWithPartner> GetWithVat(DateTime month);
        IEnumerable<Models.TransactionWithPartner> GetWithPartner();
        Models.TransactionWithPartner GetWithPartner(int id);

        Models.Transaction GetLastDuplicatePaymentNumber(string year);

        int InsertPayment(Models.Transaction data);
    }
}
