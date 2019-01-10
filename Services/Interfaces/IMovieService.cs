using Model.Models;
using System;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IMovieService
    {
        List<Movie> GetAllMovies();
        List<Movie> GetAllMoviesByTitleThatContains(String title);
        Movie Add(Movie movie);
        bool Delete(Guid id);
        Movie Update(Guid id, Movie movie);
    }
}
