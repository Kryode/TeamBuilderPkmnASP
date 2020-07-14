using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

        public PokemonsController(PokemonContext context)
        {
            _context = AddTypesToPokemon(context);
        }

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
