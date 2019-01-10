using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Models;
using Moq;
using Repositories.Interfaces;
using Repositories.Repositories;
using Services.Interfaces;
using Services.Services;

namespace Services.Tests.Services
{
    [TestClass]
    public class MovieServicesTester
    {
        private IGenericRepository _movieRepository;
        private IMovieService _movieService;

        public MovieServicesTester()
        {
            _movieRepository = new GenericRepository(null);
            _movieService = new MovieService(_movieRepository);
        }

        [TestMethod]
        public void GivenNoMovieWithSameTitle_WhenAddingNewMovie_ThenMovieIsReturnedFromService()
        {
            var mock = new Mock<IGenericRepository>();
            var movie = new Movie() {Title = "Ok", Description ="Test"};

            mock.Setup(p => p.GetByTitle(It.IsAny<string>()))
                .Returns<Movie>(null);
            mock.Setup(p => p.Add(It.IsAny<Movie>()))
                .Returns(movie);

            var service = new MovieService(mock.Object);
            var dbMovie = service.Add(movie);
            Assert.IsNotNull(dbMovie);
        }

        [TestMethod]
        public void GivenMovieWithSameTitle_WhenAddingNewMovie_ThenNullIsReturnedFromService()
        {
            var mock = new Mock<IGenericRepository>();
            var movie = new Movie() { Title = "Ok", Description = "Test" };

            mock.Setup(p => p.GetByTitle(It.IsAny<string>()))
                .Returns(It.IsAny<Movie>());

            var service = new MovieService(mock.Object);
            var dbMovie = service.Add(movie);
            Assert.IsNull(dbMovie);
        }

    }
}