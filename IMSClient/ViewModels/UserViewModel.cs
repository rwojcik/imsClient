using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSPrototyper.ViewModels
{
    public class Token
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string userName { get; set; }

        public DateTime expires { get; set; } = DateTime.Now.AddDays(14);
    }
}
