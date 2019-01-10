using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;

namespace NFE104._1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService ms)
        {
            this._movieService = ms;
        }

        [HttpGet]
        public List<Movie> GetAllMovies()
        {
            return this._movieService.GetAllMovies();
        }

        [HttpGet("{title}")]
        public List<Movie> GetAllMoviesByTitleThatContains(String title)
        {
            return this._movieService.GetAllMoviesByTitleThatContains(title);
        }

        [HttpPost]
        public IActionResult PostMovie([FromBody] Movie movie)
        {
            Movie movieTmp = this._movieService.Add(movie);
            if(movieTmp != null )
                return Ok(movieTmp);
            return BadRequest("Titre existant");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMovie(Guid id)
        {
            if (this._movieService.Delete(id) == true)
                return Ok("Film : " + id + " supprimé.");
            return BadRequest("Id inexsitant");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMovie(Guid id,[FromBody] Movie movie)
        {
            Movie movieTmp = this._movieService.Update(id, movie);
            if(movieTmp != null)
                return Ok(movieTmp);
            return BadRequest("Aucun changement repéré");
        }

    }
}
