using AutoMapper;


namespace Beca.PlaylistInfo.API.Profiles
{
    public class PlaylistProfile : Profile
    {
        public PlaylistProfile()
        {
            CreateMap<Entities.Playlist, Models.PlaylistWithoutSongsDto>();
            CreateMap<Entities.Playlist, Models.PlaylistDto>();
            CreateMap<Entities.Playlist, Models.PlaylistForUpdateDto>();
            CreateMap<Models.PlaylistForUpdateDto, Entities.Playlist>();
        }
    }
}
