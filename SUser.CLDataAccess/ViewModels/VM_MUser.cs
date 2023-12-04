using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUser.CLDataAccess.ViewModels
{
    public class VM_MUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsMarketingConsent { get; set; }
    }
}
