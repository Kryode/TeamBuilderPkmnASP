/*using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using TeamBuilderPkmnASP.Data;
using TeamBuilderPkmnASP.Models;

namespace TeamBuilderPkmnASP.Controllers
{
    public class PokemonsController : Controller
    {
        private readonly PokemonContext _pokemonContext;
        private readonly UserContext _userContext;
        private PokemonContext AddTypesToPokemon (PokemonContext context)
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

        public PokemonsController(PokemonContext context, UserContext userContext)
        {
            _pokemonContext = AddTypesToPokemon(context);
            _userContext = userContext;
        }

        public ViewResult Index()
        {
            ISession userSession = HttpContext.Session;
            string userMail = userSession.GetString("user");
            if(userMail != null)
            {
                User user = _userContext.User.Where(user => user.Mail == userMail).FirstOrDefault();
                ViewData["user"] = user;
            }
            StringValues search;
            var isSearched = Url.ActionContext.HttpContext.Request.Query.TryGetValue("search", out search);
            if(!isSearched)
            {
                return View(_pokemonContext.Pokemon.OrderBy(pokemon => pokemon.Order).ToList());
            }
            else
            {
                var debug = _pokemonContext.Pokemon.Where(pokemon => pokemon.Identifier.Contains(search));
                return View(_pokemonContext.Pokemon.Where(pokemon => pokemon.Identifier.Contains(search)).OrderBy(pokemon => pokemon.Order));
            }
        }

        public ViewResult Details(int? id)
        {
            if (id == null)
            {
                return View(null);
            }
            var pokemon =  _pokemonContext.Pokemon
                .FirstOrDefault(m => m.Id == id);
            if (pokemon == null)
            {
                return View(null);
            }
            return View(pokemon);
        }

        public ActionResult Login()
        {
            ViewData["Error"] = TempData["Error"];
            ViewData["Title"] = "Login";
            return View("../Login/UserForm");
        }
        public ActionResult Signin()
        {
            ViewData["Error"] = TempData["Error"];
            ViewData["Title"] = "Signin";
            return View("../Login/UserForm");
        }
    }
}
*/