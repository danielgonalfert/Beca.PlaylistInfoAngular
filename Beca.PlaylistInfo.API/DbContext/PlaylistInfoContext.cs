using Microsoft.EntityFrameworkCore;
using Beca.PlaylistInfo.API.Entities;

namespace Beca.PlaylistInfo.API
{
    public class PlaylistInfoContext : DbContext
    {
        public virtual DbSet<Playlist> Playlists { get; set; } = null!;

        public virtual DbSet<Song> Songs { get; set; } = null!;

        public PlaylistInfoContext(DbContextOptions<PlaylistInfoContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Playlist>()
                .HasData(
               new Playlist("Playlist 1")
               {
                   Id = 1,
                   Description = "The one with that big park."
               },
               new Playlist("Playlist 2")
               {
                   Id = 2,
                   Description = "The one with the cathedral that was never really finished."
               },
               new Playlist("Playlist 3")
               {
                   Id = 3,
                   Description = "The one with that big tower."
               });

            modelBuilder.Entity<Song>()
             .HasData(
               new Song("Song 1")
               {
                   Id = 1,
                   PlaylistId = 1,
                   Description = "The most visited urban park in the United States."
               },
               new Song("Song 3")
               {
                   Id = 2,
                   PlaylistId = 1,
                   Description = "A 102-story skyscraper located in Midtown Manhattan."
               },
               new Song("Song 4")
               {
                   Id = 3,
                   PlaylistId = 2,
                   Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans."
               },
               new Song("Song 5")
               {
                   Id = 4,
                   PlaylistId = 2,
                   Description = "The the finest example of railway architecture in Belgium."
               },
               new Song("Song 6")
               {
                   Id = 5,
                   PlaylistId = 3,
                   Description = "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel."
               },
               new Song("Song 7")
               {
                   Id = 6,
                   PlaylistId = 3,
                   Description = "The world's largest museum."
               }
               );
            base.OnModelCreating(modelBuilder);
        }

    }
}

