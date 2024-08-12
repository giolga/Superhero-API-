using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperHeroApi_DotNet8.Data;
using SuperHeroApi_DotNet8.Entities;

namespace SuperHeroApi_DotNet8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly DataContext _context;

        public SuperHeroController(DataContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SuperHero>>> GetAllHeros()
        {
            var heros = await _context.SuperHeros.ToListAsync();
            return Ok(heros);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHero>> GetHero(int id)
        {
            var hero = _context.SuperHeros.FirstOrDefault(h => h.Id == id);

            if (hero == null)
            {
                return NotFound();
            }

            return Ok(hero);
        }

        [HttpPost]
        public async Task<ActionResult<SuperHero>> PostHero(SuperHero hero)
        {
            _context.SuperHeros.Add(hero);
            await _context.SaveChangesAsync();
            return Ok(await GetAllHeros());
        }

        [HttpPut]
        public async Task<ActionResult<SuperHero>> UpdateHero(SuperHero superHero)
        {
            var dbHero = await _context.SuperHeros.FirstOrDefaultAsync(h => h.Id == superHero.Id);

            if (dbHero is null)
                return NotFound("Hero not found!");

            dbHero.Name = superHero.Name;
            dbHero.FirstName = superHero.FirstName;
            dbHero.LastName = superHero.LastName;
            dbHero.Place = superHero.Place;

            _context.Entry(dbHero).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(dbHero);
        }

        [HttpDelete]
        public async Task<ActionResult<SuperHero>> DeleteHero(int id)
        {
            var hero = await _context.SuperHeros.FirstOrDefaultAsync(hero => hero.Id == id);

            if (hero is null)
                return NotFound("Hero not found!");

            _context.SuperHeros.Remove(hero);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
