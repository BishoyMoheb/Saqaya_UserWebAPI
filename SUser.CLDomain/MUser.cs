using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;//To use Key
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUser.CLDomain
{
    public class MUser
    {
        [Key]
        public string UId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsMarketingConsent { get; set; }
    }
}
