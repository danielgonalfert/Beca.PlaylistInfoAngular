using Beca.PlaylistInfo.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace Beca.PlaylistInfo.API.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly PlaylistInfoContext _context;

        public PlaylistRepository(PlaylistInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Playlist>> GetPlaylistsAsync()  
        {
            return await _context.Playlists.OrderBy(c => c.Title).ToListAsync();
        }

        public async Task<Playlist> GetPlaylistByNameAsync(string title)
        {
            return await _context.Playlists.Where(c => c.Title == title).FirstOrDefaultAsync();
        }

        public async Task<Playlist> GetPlaylistByIdAsync(int id, bool withSongs)  
        {
            if (withSongs)
            {
                return await _context.Playlists.Include(c => c.Songs).Where(c => c.Id == id).FirstOrDefaultAsync();
            }

            return await _context.Playlists.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Song>> GetSongsAsync()
        {
            return await _context.Songs.OrderBy(c => c.Title).ToListAsync();
        }

        public async Task<IEnumerable<Song>> GetSongsByPlaylistIdAsync(int Id)
        {
            return await _context.Songs.Where(c => c.PlaylistId == Id).ToListAsync();
        }

        public async Task<Song> GetSongFromPlaylistByIdAsync(int playlistId, int songId)
        {
            return await _context.Songs.Where(c => c.PlaylistId == playlistId && c.Id == songId).FirstOrDefaultAsync();
        }

        public async Task<Song> GetSongsByIdAsync(int Id)
        {
            return await _context.Songs.Where(c => c.Id == Id).FirstOrDefaultAsync();
        }


        public async Task<(IEnumerable<Playlist>,PaginationMetadata)> GetPlaylistsAsync(string? name, string? searchQuery, int pageNumber, int pageSize)
        {

            var collection = _context.Playlists as IQueryable<Playlist>;

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                collection = collection.Where(c => c.Title == name);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(a => a.Title.Contains(searchQuery)
                    || (a.Description != null && a.Description.Contains(searchQuery)));
            }

            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection.OrderBy(c => c.Title)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (collectionToReturn, paginationMetadata);
        }

        //MÉTODOS DE MODIFICACIÓN DE LA BASE DE DATOS

        public async Task AddSongToPlaylistAsync(int playlistId, Song song)
        {
            Playlist playlist = await GetPlaylistByIdAsync(playlistId,false);
            if( playlist!= null)
            {
                playlist.Songs.Add(song);
            }
        }

        public void DeleteSong(Song song)
        {
            _context.Songs.Remove(song);
        }

        public void DeletePlaylist(Playlist playlist)
        {
            _context.Playlists.Remove(playlist);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task AddPlaylistAsync(Playlist playlist)
        {
            _context.Playlists.Add(playlist);
        }
    }

   
}
