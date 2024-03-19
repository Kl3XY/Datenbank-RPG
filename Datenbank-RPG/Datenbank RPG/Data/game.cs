using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using sql;

    public class game : DbContext
    {
        public game (DbContextOptions<game> options)
            : base(options)
        {
        }

        public DbSet<sql.Player> Player { get; set; } = default!;
    }
