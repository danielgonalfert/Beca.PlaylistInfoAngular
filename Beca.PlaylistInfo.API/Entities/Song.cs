using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Beca.PlaylistInfo.API.Entities
{
    public class Song
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [Required]
        [MaxLength(25)]
        public string Title { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        [ForeignKey("PlaylistId")]
        public Playlist? Playlist {get; set; }

        public int PlaylistId {get; set; }

        public Song(string title)
        {
            Title = title;
        }
    }
}
