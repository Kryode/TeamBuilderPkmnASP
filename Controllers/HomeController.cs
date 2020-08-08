using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using TeamBuilderPkmnASP.Data;
using TeamBuilderPkmnASP.Models;

namespace TeamBuilderPkmnASP.Controllers
{
    public class HomeController : Controller
    {
        private readonly PokemonContext _pokemonContext;
        private readonly UserContext _userContext;
        private readonly ILogger<HomeController> _logger;

        private PokemonContext AddTypesToPokemon(PokemonContext context)
        {
            foreach (var pokemon in context.Pokemon)
            {
                IQueryable<PokemonType> pokemonTypesQuery = from pokeType in context.PokemonType
                                                            where pokeType.PokemonId == pokemon.Id
                                                            select pokeType;

                List<PokemonType> pokemonTypes = pokemonTypesQuery.ToList();
                foreach (PokemonType pokemonType in pokemonTypes)
                {
                    pokemonType.Type = (from type in context.Type
                                        where type.Id == pokemonType.TypeId
                                        select type).Single();
                    if (null == pokemon.Types)
                    {
                        pokemon.Types = new List<Models.Type>();
                    }
                    pokemon.Types.Add(pokemonType.Type);
                }
            }
            return context;
        }

        public HomeController(PokemonContext context, UserContext userContext, ILogger<HomeController> logger)
        {
            _pokemonContext = AddTypesToPokemon(context);
            _userContext = userContext;
            _logger = logger;
        }

        public ViewResult Index()
        {
            ISession userSession = HttpContext.Session;
            string userMail = userSession.GetString("User");
            if (userMail != null)
            {
                User user = _userContext.User.Where(user => user.Mail == userMail).FirstOrDefault();
                ViewData["user"] = user;
            }

            StringValues search;
            var isSearched = Url.ActionContext.HttpContext.Request.Query.TryGetValue("search", out search);
            if (!isSearched)
            {
                return View(_pokemonContext.Pokemon.OrderBy(pokemon => pokemon.Order));
            }
            else
            {
                return View(_pokemonContext.Pokemon.Where(pokemon => pokemon.Identifier.Contains(search)).OrderBy(pokemon => pokemon.Order));
            }
        }

        public ViewResult Details(int? id)
        {
            if (id == null)
            {
                return View(null);
            }
            var pokemon = _pokemonContext.Pokemon
                .FirstOrDefault(m => m.Id == id);
            if (pokemon == null)
            {
                return View(null);
            }
            return View(pokemon);
        }

        ///TODO: Change Log&Sign to LoginController
        public ActionResult Login()
        {
            ViewData["Error"] = TempData["Error"];
            ViewData["Usermail"] = TempData["Usermail"];
            ViewData["Token"] = TempData["Token"];
            return View("Login");
        }
        public ActionResult Signon()
        {
            ViewData["Error"] = TempData["Error"];
            return View("Signon");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
