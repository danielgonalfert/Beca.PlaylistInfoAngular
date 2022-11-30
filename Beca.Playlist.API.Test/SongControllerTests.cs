using Beca.PlaylistInfo.API.Controllers;
using Beca.PlaylistInfo.API.Repositories;
using Beca.PlaylistInfo.API.Entities;
using Beca.PlaylistInfo.API.Models;
using Xunit;
using Moq;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;

namespace Beca.PlaylistInfo.API.Test
{
    public class SongControllerTests
    {
        [Fact]
        public async Task CreateControllers_GetAllSongs_ReturnOkObjectResult()
        {
            SongController songController = new SongController(
                new Mock<Microsoft.Extensions.Logging.ILogger<SongController>>().Object,
                new Mock<IPlaylistRepository>().Object,
                new Mock<IMapper>().Object
            );

            //Act
            var result = await songController.GetSongsAsync();

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateSongAndController_GetSongByID_ReturnExpectedObject()
        {
            Song song = new Song("Bad guy");
            song.Id = 0;
            song.Description = "Super nice song";

            var repository = new Mock<IPlaylistRepository>();
            repository.Setup(m => m.GetSongsByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(song));

            var mapperConfiguration = new MapperConfiguration(
               cfg => cfg.AddProfile<Profiles.SongProfile>());
            var mapper = new Mapper(mapperConfiguration);

            SongController songController = new SongController(
                new Mock<Microsoft.Extensions.Logging.ILogger<SongController>>().Object,
                repository.Object,
                mapper
            );

            var result = await songController.GetSongsById(0);
            var songResult =(SongDto) ((OkObjectResult)result.Result).Value;


            Assert.True(songResult.Title ==song.Title && songResult.Description == song.Description);
        }

        [Fact]
        public async Task CreateSongController_GetSongsByPlaylistId_ReturnExpectedType()
        {

            Song song = new Song("Bad guy");

            var repository = new Mock<IPlaylistRepository>();
            repository.Setup(m => m.GetSongFromPlaylistByIdAsync(It.IsAny<int>(),It.IsAny<int>())).ReturnsAsync(song);

            var mapperConfiguration = new MapperConfiguration(
               cfg => cfg.AddProfile<Profiles.SongProfile>());
            var mapper = new Mapper(mapperConfiguration);

            SongController songController = new SongController(
                new Mock<Microsoft.Extensions.Logging.ILogger<SongController>>().Object,
                repository.Object,
                mapper
            );

            var result = await songController.GetSongFromPlaylistByIdAsync(0, 0);
            var songResult = (SongDto)((OkObjectResult)result.Result).Value;

            Assert.IsType<SongDto>(songResult);

        }
    }
}
