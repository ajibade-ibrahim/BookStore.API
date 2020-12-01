using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.BlazorServer.Data.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<bool> Create(string url, T obj);
        Task<bool> Delete(string url, int id);
        Task<T> Get(string url, int id);
        Task<IList<T>> Get(string url);
        Task<bool> Update(string url, T obj, int id);
    }
}