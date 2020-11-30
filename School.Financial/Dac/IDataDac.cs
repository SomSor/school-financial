using System.Collections.Generic;

namespace School.Financial.Dac
{
    public interface IDataDac<T>
    {
        IEnumerable<T> Get();
        T Get(int id);
        void Insert(T Budget);
        void Update(T Budget);
        void Upsert(T Budget);
        void Delete(int id);
    }
}
