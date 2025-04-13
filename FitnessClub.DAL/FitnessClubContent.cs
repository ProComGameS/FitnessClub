using Microsoft.EntityFrameworkCore;
using FitnessClub.DAL.Models;

namespace FitnessClub.DAL
{
    public class FitnessClubContext : DbContext
    {
        public FitnessClubContext(DbContextOptions<FitnessClubContext> options) : base(options) {}

        public FitnessClubContext() : base(new DbContextOptionsBuilder<FitnessClubContext>()
            .UseSqlite("Data Source=fitnessclub.db")
            .Options) {}

        public DbSet<Club> Clubs { get; set; }
        public DbSet<ClassSession> ClassSessions { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MembershipCard> MembershipCards { get; set; }
        public DbSet<Visit> Visits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>()
                .HasOne(m => m.MembershipCard)
                .WithOne(mc => mc.Member)
                .HasForeignKey<MembershipCard>(mc => mc.MemberId);
        }
    }
}