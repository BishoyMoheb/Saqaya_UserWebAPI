using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SUser.CLDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUser.CLDataAccess.EFContext
{
    public class MUserDbContext : IdentityDbContext<IdUserExtension>
    {
        public MUserDbContext(DbContextOptions<MUserDbContext> DBConOptions)
            : base(DBConOptions)
        {

        }

        public DbSet<MUser> DbS_Users { get; set; }
    }
}
