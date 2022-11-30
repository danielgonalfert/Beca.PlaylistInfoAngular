using Beca.PlaylistInfo.API.Controllers;
using Beca.PlaylistInfo.API.Repositories;
using Beca.PlaylistInfo.API.Entities;
using Beca.PlaylistInfo.API.Models;
using Xunit;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;

namespace Beca.PlaylistInfo.API.Test
{
    public class PlaylistControllerTests
    {

        [Fact]
        public async Task CreatePlaylistController_GetAllPlaylists_ReturnIsTypeTaskOfActionResultOfIEnumerableOfPlaylist()
        {
            //Arrange
            PlaylistController playlistController = new PlaylistController(
                new Mock<Microsoft.Extensions.Logging.ILogger<PlaylistController>>().Object,
                new Mock<IPlaylistRepository>().Object,
                new Mock<IMapper>().Object
            );

            //Act
            var result = await playlistController.GetPlaylists();

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreatePlaylistAndController_FetchPlaylistByID_ReturnIsTypeOkObjectResult()
        {
            //Arrange 
            int consultId = 1;
            Playlist playlist = new Playlist("The best of Taylor Swift");
            playlist.Id = consultId;
            playlist.Description = "A great list of the best hits of our beloved singer";

            var repository = new Mock<IPlaylistRepository>();
            
            repository.Setup(m => m.GetPlaylistByIdAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(playlist);

            PlaylistController playlistController = new PlaylistController(
                new Mock<Microsoft.Extensions.Logging.ILogger<PlaylistController>>().Object,
                repository.Object,
                new Mock<IMapper>().Object
            );

            var result = await playlistController.GetPlaylistByIdAsync(consultId,false);

            Assert.IsType<OkObjectResult>(result.Result);

        }

        [Fact]
        public async Task CreatePlaylistAndController_FetchPlaylistByName_ReturnExpectedData()
        {
            //Arrange 
            int expectedId = 1;
            string consultTitle = "The best of Taylor Swift";
            Playlist playlist = new Playlist(consultTitle);
            playlist.Id = expectedId;
            playlist.Description = "A great list of the best hits of our beloved singer";
            playlist.Songs = new List<Song>();

            var repository  = new Mock<IPlaylistRepository>();
            
            repository.Setup(m => m.GetPlaylistByNameAsync(It.IsAny<string>())).ReturnsAsync(playlist);

            var mapperConfiguration = new MapperConfiguration(
                cfg => cfg.AddProfile<Profiles.PlaylistProfile>());
            var mapper = new Mapper(mapperConfiguration);


            PlaylistController playlistController = new PlaylistController(
                new Mock<Microsoft.Extensions.Logging.ILogger<PlaylistController>>().Object,
                repository.Object,
                mapper
            ) ;

            //Act
            var result = await playlistController.GetPlaylistByNameAsync(consultTitle);


            //Assert
            var resultObject =(OkObjectResult) result.Result;
            var resultPlaylistModel =(PlaylistDto) resultObject.Value;
            var resultId = resultPlaylistModel.Id;

     
            Assert.Equal(expectedId, resultId);

        }



        [Fact]
        public async Task CreatePlaylistAndController_FetchPlaylistPaginated_ReturnExpectedNumberOfItems()
        {
            //Arrange
          
            List<Playlist> playlistList = new List<Playlist>();
            for(int i = 0; i< 20; i++)
            {
                Playlist playlist = new Playlist(i.ToString());
                playlist.Id = i;
                playlistList.Add(playlist);
            }

            IEnumerable<Playlist> playlists = playlistList;

            int totalItemcount = 20;
            int pageSize = 2;
            int currentPage = 1;


            var metadata = new PaginationMetadata(totalItemcount,pageSize,currentPage);
            var data = (playlists, metadata);

            var repository = new Mock<IPlaylistRepository>();
            repository.Setup(m => m.GetPlaylistsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(data);
            var mapperConfiguration = new MapperConfiguration(
                cfg => cfg.AddProfile<Profiles.PlaylistProfile>());
            var mapper = new Mapper(mapperConfiguration);


            PlaylistController playlistController = new PlaylistController(
                new Mock<Microsoft.Extensions.Logging.ILogger<PlaylistController>>().Object,
                repository.Object,
                mapper
            );

            playlistController.ControllerContext = new ControllerContext() { HttpContext = new DefaultHttpContext() };

            //Act

            var result = await playlistController.GetPlaylistsPaginated(null, null, currentPage, pageSize);


            //Assert
            var resultObject = (OkObjectResult)result.Result;
            var resultPlaylistModel =(List<PlaylistWithoutSongsDto>)resultObject.Value;
            var resultItemCount = resultPlaylistModel.Count();

            Assert.Equal(totalItemcount, resultItemCount);


        }

        [Fact]
        public async Task CreatePlaylistControllerAndPlaylist_AddPlaylist_ReturnExpectedPlaylist()
        {
            var mapperConfiguration = new MapperConfiguration(
               cfg => cfg.AddProfile<Profiles.PlaylistProfile>());
            var mapper = new Mapper(mapperConfiguration);
            var repository = new Mock<IPlaylistRepository>();

            PlaylistController playlistController = new PlaylistController(
                new Mock<Microsoft.Extensions.Logging.ILogger<PlaylistController>>().Object,
                repository.Object,
                mapper
            );

            var title = "Title";
            var description = "Description";

            var resultAsync = await playlistController.AddPlaylist(title, description);

            var result = resultAsync.ExecuteResult;
            CreatedAtRouteResult actionResult = (CreatedAtRouteResult)result.Target;
            PlaylistDto value = (PlaylistDto)actionResult.Value;


            Assert.True((value.Title == title) && (value.Description == description));
        }

    }
}
