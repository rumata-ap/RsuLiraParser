using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsing
{
    public class RsuDBContext : DbContext
    {
        public RsuDBContext() : base("DBLiteConnection")
        { }

        public DbSet<RsuBar> RsuBars { get; set; }
        public DbSet<RsuLBar> RsuLBars { get; set; }
        public DbSet<RsuNBar> RsuNBars { get; set; }
        public DbSet<RsuNLBar> RsuNLBars { get; set; }
        public DbSet<RsuShell> RsuShells { get; set; }
        public DbSet<RsuLShell> RsuLShells { get; set; }
        public DbSet<RsuNShell> RsuNShells { get; set; }
        public DbSet<RsuNLShell> RsuNLShells { get; set; }

    }
    public class RsuBDBContext : DbContext
    {
        public RsuBDBContext(): base("DBLiteConnection")
        { }

        public DbSet<RsuBar> RsuBars { get; set; }
    }
    public class RsuLBDBContext : DbContext
    {
        public RsuLBDBContext() : base("DBLiteConnection")
        { }

        public DbSet<RsuLBar> RsuLBars { get; set; }
    }
    public class RsuNBDBContext : DbContext
    {
        public RsuNBDBContext() : base("DBLiteConnection")
        { }

        public DbSet<RsuNBar> RsuNBars { get; set; }
    }
    public class RsuNLBDBContext : DbContext
    {
        public RsuNLBDBContext() : base("DBLiteConnection")
        { }

        public DbSet<RsuNLBar> RsuNLBars { get; set; }
    }
    public class RsuSDBContext : DbContext
    {
        public RsuSDBContext() : base("DBLiteConnection")
        { }
        public DbSet<RsuShell> RsuShells { get; set; }
    }
    public class RsuLSDBContext : DbContext
    {
        public RsuLSDBContext() : base("DBLiteConnection")
        { }
        public DbSet<RsuLShell> RsuLShells { get; set; }
    }
    public class RsuNSDBContext : DbContext
    {
        public RsuNSDBContext() : base("DBLiteConnection")
        { }
        public DbSet<RsuNShell> RsuNShells { get; set; }
    }
    public class RsuNLSDBContext : DbContext
    {
        public RsuNLSDBContext() : base("DBLiteConnection")
        { }
        public DbSet<RsuNLShell> RsuNLShells { get; set; }
    }
}
