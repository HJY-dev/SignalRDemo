using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DbContexts
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContext<AppDbContext> options)
            : base(options)
        {

        }
    }
}
