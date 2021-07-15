using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoxingStore.Data
{
    public class BoxingStoreDbContext : IdentityDbContext
    {
        public BoxingStoreDbContext(DbContextOptions<BoxingStoreDbContext> options)
            : base(options)
        {
        }
    }
}
