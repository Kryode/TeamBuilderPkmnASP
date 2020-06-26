using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using TeamBuilderPkmnASP.Data;
using TeamBuilderPkmnASP.Models;

namespace TeamBuilderPkmnASP.Controllers
{
    public class PokemonsController : Controller
    {
        private readonly PokemonContext _context;

        public PokemonsController(PokemonContext context)
        {
            _context = context;
        }

        // GET: Pokemons
        //public ViewResult Index()
        //{
          //  return View(_context.Pokemon.OrderBy(pokemon => pokemon.Order).ToList());
        //}

        public ViewResult Index()
        {
            StringValues search;
            var isSearched = Url.ActionContext.HttpContext.Request.Query.TryGetValue("search", out search);
            if(!isSearched)
            {
                return View(_context.Pokemon.OrderBy(pokemon => pokemon.Order).ToList());
            }
            else
            {
                return View(_context.Pokemon.Where(pokemon => pokemon.Identifier.Contains(search)).OrderBy(pokemon => pokemon.Order));
            }
        }

        // GET: Pokemons/Details/5
        public ViewResult Details(int? id)
        {
            if (id == null)
            {
                return View(null);
            }

            var pokemon =  _context.Pokemon
                .FirstOrDefault(m => m.Id == id);
            if (pokemon == null)
            {
                return View(null);
            }

            return View(pokemon);
        }
    }
}
