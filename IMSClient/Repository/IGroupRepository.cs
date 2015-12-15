using System.Collections.Generic;
using System.Threading.Tasks;
using IMSPrototyper.ViewModels;

namespace IMSClient.Repository
{
    public interface IGroupRepository
    {
        Task<IList<GroupViewModel>> GetGroupsAsync();

    }
}
