using Model.Models;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Services.Services
{
    public class MovieService : IMovieService
    {
        private readonly IGenericRepository _movieRepository;

        public MovieService(IGenericRepository mr)
        {
            this._movieRepository = mr;
        }

        public List<Movie> GetAllMovies()
        {
            return _movieRepository.GetAllMovies();
        }

        public List<Movie> GetAllMoviesByTitleThatContains(String title)
        {
            return _movieRepository.GetAllMoviesByTitleThatContains(title);
        }

        public Movie Add(Movie movie)
        {
            Movie researchedMovie = _movieRepository.GetByTitle(movie.Title);

            if (researchedMovie != null)
            {
                return null;
            }
            return _movieRepository.Add(movie);

        }

        public bool Delete(Guid id)
        {
            Movie researchedMovie = _movieRepository.GetById(id);
            if (researchedMovie != null)
            {
                return false;
            }
            return _movieRepository.Delete(id);
        }

        public Movie Update(Guid id, Movie movie)
        {
            Movie researchedMovie = _movieRepository.GetByTitle(movie.Title);
            if (researchedMovie != null)
            {
                return null;
            }
            return _movieRepository.Update(id, movie);
        }

        //public bool isValidDateTime(Movie movie)
        //{
        //    if(movie.ReleaseDate > movie.)
        //}
        
    }
}
