using CurrencyFetcher.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CurrencyFetcher.Core.Core
{
    public class CurrencyDbContext: IdentityDbContext<IdentityUser>
    {
        /// <summary>
        /// Reference with Logs table in database
        /// </summary>
        public DbSet<Log> Logs { get; set; }

        /// <summary>
        /// Reference with Currencies table in database
        /// </summary>
        public DbSet<Currency> Currencies { get; set; }

        /// <summary>
        /// Reference with CurrencyValues table in database
        /// </summary>
        public DbSet<CurrencyValue> CurrencyValues { get; set; }

        /// <summary>
        /// Creates instance of class
        /// </summary>
        public CurrencyDbContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// Add extended alters for migration
        /// </summary>
        /// <param name="builder"><see cref="ModelBuilder"/></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Currency>()
                .HasIndex(c => new {c.CurrencyBeingMeasured, c.CurrencyMatched})
                .IsUnique();

            builder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = "cf7ad5b0-9cf9-4cb9-8f29-0e8297f0ee37",
                    UserName = "currency",
                    NormalizedUserName = "CURRENCY",
                    Email = "kmaraszkiewicz86@gmail.com",
                    NormalizedEmail = "KMARASZKIEWICZ86@GMAIL.COM",
                    PasswordHash = "AQAAAAEAACcQAAAAEHEwRsH9SjgjRSbVOJo2jrDJSU5DkSiun6I3SYr/8A5jMp0uAmq4jr4+7hBO9pnumQ==",
                    SecurityStamp = "QVKRSIRR6C6GQ3VO2J2VQGRPQZJVR2HM",
                    ConcurrencyStamp = "d411e5fc-efd9-4f61-9fdd-bb9858f9b43e"
                });

            base.OnModelCreating(builder);
        }
    }
}
