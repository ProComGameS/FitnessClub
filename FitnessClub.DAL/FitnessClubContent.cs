using System.Collections.Generic;
using System.Data.Entity;
using FitnessClub.DAL.Models;

namespace FitnessClub.DAL
{
    /// <summary>
    /// Контекст бази даних для Фітнес клубу із використанням EF Code First.
    /// </summary>
    public class FitnessClubContext : DbContext
    {
        public FitnessClubContext() : base("name=FitnessClubDB")
        {
            // Включення lazy loading
            this.Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<Club> Clubs { get; set; }
        public DbSet<ClassSession> ClassSessions { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MembershipCard> MembershipCards { get; set; }
        public DbSet<Visit> Visits { get; set; }

        // Налаштування зв’язків за допомогою Fluent API
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Налаштовуємо зв’язок 1:1 між Member та MembershipCard
            modelBuilder.Entity<Member>()
                .HasOptional(m => m.MembershipCard)
                .WithRequired(mc => mc.Member);
            base.OnModelCreating(modelBuilder);
        }
    }
}
