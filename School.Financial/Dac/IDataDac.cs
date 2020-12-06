using System.Collections.Generic;

namespace School.Financial.Dac
{
    public interface IDataDac<T>
    {
        IEnumerable<T> Get();
        T Get(int id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Budget"></param>
        /// <returns>Last inserted id</returns>
        int Insert(T Budget);
        void Update(T Budget);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Budget"></param>
        /// <returns>Last inserted id</returns>
        int Upsert(T Budget);
        void Delete(int id);
    }
}
