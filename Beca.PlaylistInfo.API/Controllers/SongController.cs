using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Beca.PlaylistInfo.API.Models;
using Beca.PlaylistInfo.API.Repositories;
using Beca.PlaylistInfo.API.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace Beca.PlaylistInfo.API.Controllers
{
    [ApiController]
    [Route("api/songs")]
    public class SongController : ControllerBase
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SongController> _logger;

        public SongController(ILogger<SongController> logger,IPlaylistRepository playlistRepository, IMapper mapper)
        {
            _logger = logger                            ?? throw new ArgumentNullException(nameof(logger));
            _playlistRepository = playlistRepository    ?? throw new ArgumentNullException(nameof(playlistRepository));
            _mapper = mapper                            ?? throw new ArgumentNullException(nameof(mapper));

        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<SongDto>>> GetSongsAsync()
        {
            var songEntitties = await _playlistRepository.GetSongsAsync();
            return Ok(_mapper.Map<IEnumerable<Song>>(songEntitties));
        }

        [HttpGet("bysongid/{id}", Name = "GetSongsById" )]
        public async Task<ActionResult<SongDto>> GetSongsById(int id)
        {
            Song songEntities = await _playlistRepository.GetSongsByIdAsync(id);
            if(songEntities == null)
            {
                _logger.LogInformation(
                   $"Song with id {id} wasn't found when accessing songs.");
                return NotFound();
            }
            var result = _mapper.Map<SongDto>(songEntities);
            return Ok(result);
        }

        [HttpGet("byplaylistid/{id}")]
        public async Task<ActionResult<IEnumerable<SongDto>>> GetSongsByPlaylistIdAsync(int id)
        {
            IEnumerable<Song> songEntities = await _playlistRepository.GetSongsByPlaylistIdAsync(id);
            if (songEntities == null)
            {
                _logger.LogInformation(
                   $"Playlist with id {id} wasn't found when accessing playlists.");
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<SongDto>>(songEntities));
        }

        [HttpGet("fromplaylistbyid/{playlistId}/{songId}")]
        public async Task<ActionResult<SongDto>> GetSongFromPlaylistByIdAsync(int playlistId, int songId)
        {
            var song = await _playlistRepository.GetSongFromPlaylistByIdAsync(playlistId, songId);

            if (song == null)
            {
                _logger.LogInformation(
                 $"Song with id {songId} wasn't found when accessing playlist with id {playlistId}.");
                return NotFound();
            }
            return Ok(_mapper.Map<SongDto>(song));
        }


        //Acciones que modifican la base de datos

        [HttpPost("addtoplaylist/{playlistId}")]
        public async Task<ActionResult> AddSongToPlaylist(int playlistId,string title, string description)
        {
            var targetPlaylist = await _playlistRepository.GetPlaylistByIdAsync(playlistId, true);

            if(targetPlaylist == null)
            {
                _logger.LogInformation(
                 $"Song could not be added to playlist with id {playlistId}.");
                return BadRequest();
            }

            Song createdSong = new Song(title);
            createdSong.Description = description;

            var createdSongEntity = _mapper.Map<Entities.Song>(createdSong);

            await _playlistRepository.AddSongToPlaylistAsync(playlistId, createdSong);

            await _playlistRepository.SaveChangesAsync();

            var createdSongToReturn = _mapper.Map<Models.SongDto>(createdSongEntity);

       

            return CreatedAtRoute("GetSongsById", new { id = createdSongToReturn.Id }, createdSongToReturn) ;
        }


        [HttpPut("update/{songid}")]
        public async Task<ActionResult> UpdateSong(int songid, SongForUpdateDto songForUpdate)
        {
            var songs = await _playlistRepository.GetSongsAsync();

            var selectedSong = songs.Where(c => c.Id == songid).FirstOrDefault();
            if(selectedSong == null)
            {
                _logger.LogInformation(
                $"Song with {songid} could not be updated.");
                return NotFound();
            }
            
            _mapper.Map(songForUpdate, selectedSong);
            await _playlistRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("update/{songid}")]
        public async Task<ActionResult> PartiallyUpdateSong(int songid, JsonPatchDocument<SongForUpdateDto> patchDocument)
        {
            var songs = await _playlistRepository.GetSongsAsync();

            var selectedSong = songs.Where(c => c.Id == songid).FirstOrDefault();

            if (selectedSong == null)
            {
                _logger.LogInformation(
                $"Song with {songid} could not be partially updated because it doesn't exist.");
                return NotFound();
            }

            var songToPatch = _mapper.Map<SongForUpdateDto>(selectedSong);

            patchDocument.ApplyTo(songToPatch,ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(!TryValidateModel(songToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(songToPatch,selectedSong);
            await _playlistRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("delete/{songId}")]

        public async Task<ActionResult> DeleteSong(int songId)
        {
            Song song = await _playlistRepository.GetSongsByIdAsync(songId);
            if(song == null)
            {
                _logger.LogInformation(
                $"Song with {songId} could not be partially deleted because it doesn't exist.");
                return NotFound();
            }

            _playlistRepository.DeleteSong(song);
            await _playlistRepository.SaveChangesAsync();

            return NoContent();

        }
       
    }
}
