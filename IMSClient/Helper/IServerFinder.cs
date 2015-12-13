using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSClient.Helper
{
    public interface IServerFinder
    {
        Task<string> GetServerAddressAsync();

        bool ServerFound();
    }
}
