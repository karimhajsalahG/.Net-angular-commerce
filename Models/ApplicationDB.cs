using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularToApiw.Models
{
    public class ApplicationDB : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
       public ApplicationDB(DbContextOptions<ApplicationDB> options):base(options)
        {

        }
    }
}
