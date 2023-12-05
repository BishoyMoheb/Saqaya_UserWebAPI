using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;//To use Key
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUser.CLDomain
{
    public class MJwtSTokens
    {
        [Key]
        public string UserId { get; set; }
        public string JwtSToken { get; set; }
    }
}
