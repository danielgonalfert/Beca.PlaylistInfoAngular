using Beca.PlaylistInfo.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Beca.PlaylistInfo.API.Repositories
{
    public interface IPlaylistRepository
    {
        Task<IEnumerable<Playlist>> GetPlaylistsAsync();

        Task<(IEnumerable<Playlist>,PaginationMetadata)> GetPlaylistsAsync(string? name, string? searchQuery, int pageNumber, int pageSize);

        Task<Playlist> GetPlaylistByNameAsync(string title);

        Task<Playlist> GetPlaylistByIdAsync(int id, bool withSongs);

        Task<IEnumerable<Song>> GetSongsAsync();

        Task<IEnumerable<Song>> GetSongsByPlaylistIdAsync(int Id);

        Task<Song> GetSongsByIdAsync(int Id);

        Task<Song> GetSongFromPlaylistByIdAsync(int playlistId, int songId);

        Task AddSongToPlaylistAsync(int playlistId,Song song);
 
        void DeleteSong(Song song);

        void DeletePlaylist(Playlist playlist);

        Task AddPlaylistAsync(Playlist playlist);

        Task<bool> SaveChangesAsync();
    }
}
