using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Infrastructure.Data.DTO;

namespace BoardGame.Infrastructure.Data.EF.Contexts
{
    public class GameContext : DbContext
    {
        public GameContext()
            : base("name=GameContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<MoveDto> Moves { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MoveDto>().ToTable("Moves");
            base.OnModelCreating(modelBuilder);
        }
    }
}
