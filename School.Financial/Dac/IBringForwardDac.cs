using School.Financial.Models;
using System;
using System.Collections.Generic;

namespace School.Financial.Dac
{
    public interface IBringForwardDac : IDataDac<BringForward>
    {
        IEnumerable<BringForward> Get(DateTime month);
        BringForward Get(DateTime month, int budgetId);
    }
}
