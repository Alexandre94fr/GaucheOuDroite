using Microsoft.EntityFrameworkCore;

namespace GaucheOuDroiteBackEnd.Data
{
    public class DataBaseContext(DbContextOptions<DataBaseContext> p_options) : DbContext(p_options)
    {
        public DbSet<Models.User> User { get; set; } = default!;

        public DbSet<Models.UserProgression> UserProgression { get; set; } = default!;

        public DbSet<Models.Level> Level { get; set; } = default!;

        public DbSet<Models.LevelResponseTimeStep> LevelResponseTimeStep { get; set; } = default!;
    }
}