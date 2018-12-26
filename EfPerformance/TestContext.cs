using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace EfPerformance
{
    public class TestContext : DbContext
    {
        public static readonly LoggerFactory MyLoggerFactory = new LoggerFactory(new[] { new ConsoleLoggerProvider((category, level)
                                                                                                => level >= LogLevel.Debug, true) });

        public DbSet<Person> Person { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(true);

            optionsBuilder
                .UseLoggerFactory(MyLoggerFactory)
                .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=EfPerformance;Integrated Security=True;MultipleActiveResultSets=true");
        }
    }
}
