using System;
using System.Collections.Generic;

namespace School.Financial.Dac
{
    public interface IBringForwardDac : IDataDac<Models.BringForward>
    {
        IEnumerable<Models.BringForward> Get(DateTime month);
        Models.BringForward Get(DateTime month, int budgetId);
    }
}
