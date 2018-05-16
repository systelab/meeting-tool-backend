namespace Main.Models
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    public class MeetingsContext : DbContext
    {
        private readonly IConfigurationRoot config;

        public MeetingsContext(IConfigurationRoot config, DbContextOptions options)
            : base(options)
        {
            this.config = config;
        }

        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<CheckUpdate> CheckUpdates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlite(this.config["ConnectionStrings:meetingsContextConnection"]);
        }
    }
}