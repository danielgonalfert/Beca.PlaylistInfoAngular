using System.ComponentModel.DataAnnotations;

namespace Beca.PlaylistInfo.API.Models
{
	public class SongForUpdateDto
	{
			[Required(ErrorMessage = "Se requiere un título")]
			[MaxLength(50)]
			public string Title { get; set; } = string.Empty;

			[MaxLength(200)]
			public string? Description { get; set; }
		
	}
}
