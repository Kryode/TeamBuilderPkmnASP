using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public ViewResult Index()
        {
            return View(_context.Pokemon.OrderBy(pokemon => pokemon.Order).ToList());
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
