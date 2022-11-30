namespace Beca.PlaylistInfo.API.Models
{
    public class PlaylistDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public ICollection<SongDto> Songs { get; set; } = new List<SongDto>();
    }
}
