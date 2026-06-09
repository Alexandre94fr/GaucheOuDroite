using Microsoft.EntityFrameworkCore;

namespace GaucheOuDroiteBackEnd.Data
{
    public class DataBaseContext(DbContextOptions<DataBaseContext> p_options) : DbContext(p_options)
    {
        public DbSet<Models.User> Users { get; set; } = default!;

        public DbSet<Models.UserProgression> UserProgressions { get; set; } = default!;

        public DbSet<Models.Level> Levels { get; set; } = default!;

        public DbSet<Models.LevelResponseTimeStep> LevelResponseTimeSteps { get; set; } = default!;
    }
}