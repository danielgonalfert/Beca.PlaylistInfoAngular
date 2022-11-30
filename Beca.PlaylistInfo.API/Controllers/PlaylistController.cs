using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Beca.PlaylistInfo.API.Models;
using Beca.PlaylistInfo.API.Repositories;
using Beca.PlaylistInfo.API.Entities;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.AspNetCore.JsonPatch;
using System.Xml.XPath;

namespace Beca.PlaylistInfo.API.Controllers
{
    [ApiController]
    [Route("api/playlists")]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PlaylistController> _logger;
        const int maxPlaylistsPageSize = 20;


        public PlaylistController(ILogger<PlaylistController> logger,IPlaylistRepository playlistRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _playlistRepository = playlistRepository ?? throw new ArgumentNullException(nameof(playlistRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet()]

        public async Task<ActionResult<IEnumerable<Playlist>>> GetPlaylists()
        {
            IEnumerable<Playlist> playlistEntities = await _playlistRepository.GetPlaylistsAsync();
            return Ok(_mapper.Map<IEnumerable<PlaylistWithoutSongsDto>>(playlistEntities));
        }

        [HttpGet("id/{id}", Name = "GetPlaylistById")]
        public async Task<ActionResult<Playlist>> GetPlaylistByIdAsync(int id, bool withSongs = false)
        {
            Playlist playlistEntity = await _playlistRepository.GetPlaylistByIdAsync(id, withSongs);

            if (playlistEntity == null)
            {
                _logger.LogInformation(
              $"No playlist with id {id} was found.");
                return NotFound();
            }

            if (withSongs)
            {
                return Ok(_mapper.Map<PlaylistDto>(playlistEntity));
            }
            return Ok(_mapper.Map<PlaylistWithoutSongsDto>(playlistEntity));
        }

        [HttpGet("title/{title}")]
        public async Task<ActionResult<Playlist>> GetPlaylistByNameAsync(string title)
        {
            var playlistEntity = await _playlistRepository.GetPlaylistByNameAsync(title);

            if(playlistEntity == null)
            {
                _logger.LogInformation(
               $"No playlist with title {title} was found.");
                return NotFound();
            }
            var result = _mapper.Map<PlaylistDto>(playlistEntity);
            return Ok(result);
        }

        
        [HttpGet("paginated")]

        public async Task<ActionResult<IEnumerable<PlaylistDto>>> GetPlaylistsPaginated(string? title, string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxPlaylistsPageSize)
            {
                pageSize = maxPlaylistsPageSize;
            }

            var (playlistEntities, paginationMetadata) = await _playlistRepository.GetPlaylistsAsync(title, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            return Ok(_mapper.Map<IEnumerable<PlaylistWithoutSongsDto>>(playlistEntities));
        }

        //Acciones que modifican la base de datos.

        [HttpPost("addplaylist")]
        public async Task<ActionResult> AddPlaylist(string title,string description)
        {
            Playlist playlist = new Playlist(title);
            playlist.Description = description;

            var createdPlaylistEntity = _mapper.Map<Entities.Playlist>(playlist);

            await _playlistRepository.AddPlaylistAsync(playlist);
            await _playlistRepository.SaveChangesAsync();

            var createdPlaylistToReturn = _mapper.Map<PlaylistDto>(createdPlaylistEntity);

            _logger.LogInformation($"Playlist with id {createdPlaylistToReturn.Id} was created.");

            return CreatedAtRoute("GetPlaylistById", new { id = createdPlaylistToReturn.Id }, createdPlaylistToReturn);
        }

        [HttpPost("update/{playlistid}")]
        public async Task<ActionResult> UpdatePlaylist(int playlistid,PlaylistForUpdateDto playlist)
        {
            Playlist selectedPlaylist = await _playlistRepository.GetPlaylistByIdAsync(playlistid, false);
            if(selectedPlaylist == null)
            {
                _logger.LogInformation(
               $"No playlist with id {playlistid} was found.");

                return NotFound();
            }

            _mapper.Map(playlist, selectedPlaylist);

            await _playlistRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("update/{playlistid}")]
        public async Task<ActionResult> PartiallyUpdatePlaylist(int playlistid, JsonPatchDocument<PlaylistForUpdateDto> patchDocument)
        {
            Playlist selectedPlaylist = await _playlistRepository.GetPlaylistByIdAsync(playlistid, false);
            if (selectedPlaylist == null)
            {
                _logger.LogInformation(
               $"Playlist with id {playlistid} can not be updated because it doesn't exist.");

                return NotFound();
            }

            var playlistToPatch = _mapper.Map<PlaylistForUpdateDto>(selectedPlaylist);

            patchDocument.ApplyTo(playlistToPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!TryValidateModel(playlistToPatch))
            {
                return BadRequest(ModelState);
            }


            _mapper.Map(playlistToPatch, selectedPlaylist);

            await _playlistRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("delete/{playlistid}")]
        public async Task<ActionResult> DeletePlaylistWithSongs(int playlistid)
        {
            Playlist playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistid, true);

            if(playlist == null)
            {
                _logger.LogInformation(
               $"Playlist with id {playlistid} can not be deleted because it doesn't exist.");
                return NotFound();
            }
            ICollection < Song > songs = playlist.Songs;

            foreach(Song song in songs)
            {
                _playlistRepository.DeleteSong(song);
            }

            _playlistRepository.DeletePlaylist(playlist);
            await _playlistRepository.SaveChangesAsync();

            return NoContent();
        }
    };
}