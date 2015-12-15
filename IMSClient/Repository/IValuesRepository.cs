using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMSClient.Repository
{
    public interface IValuesRepository
    {
        IEnumerable<string> GetValues();

        Task<IEnumerable<string>> GetRestValuesAsync();
    }
}