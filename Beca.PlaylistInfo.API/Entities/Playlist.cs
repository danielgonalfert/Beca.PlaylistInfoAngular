using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beca.PlaylistInfo.API.Entities
{
    public class Playlist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [Required]
        [MaxLength(25)]
        public string Title { get; set; }

        [MaxLength(200)]
        public string? Description  { get; set; }

        public ICollection<Song> Songs { get; set; } = new List<Song>();

        public Playlist(string title)
        {
            Title = title;
        }
    }
}
