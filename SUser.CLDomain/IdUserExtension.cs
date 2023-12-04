using Microsoft.AspNetCore.Identity;
using System;

namespace SUser.CLDomain
{
    public class IdUserExtension : IdentityUser
    {
        public new Guid Id { get; set; }
        public bool IsMarketingConsent { get; set; }
    }
}
