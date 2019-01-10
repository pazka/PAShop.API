using System;
using System.Collections.Generic;
using System.Text;
using Model.Models;

namespace Tests.Interfaces
{
    public interface IMovieService
    {
        List<Movie> GetAllMovies();
        Movie Add(Movie movie);
        bool Delete(Guid id);
        Movie Update(Guid id, Movie movie);
    }
}
