using System.Linq;
using CommanderGQL.Models;
using HotChocolate;
using HotChocolate.Data;
using CommanderGQL.Data;

namespace CommanderGQL.GraphQL
{
    public class Query
    {
        //DI with hotchocolate framework ,method level injecting
        [UseDbContext(typeof(AppDbContext))] //tells the method has to get a dbcontext from pool, execute the query and return the context back to the pool when it finished.
        //[UseProjection]//walk the graph to pull back any child objects
        //we have explicitly provided resolver hence we dont need to use projection
        [UseFiltering]
        [UseSorting]
        public IQueryable<Platform> GetPlatform([ScopedService] AppDbContext context)// ScopedService refers to service lifetime
        {
            return context.Platforms;
        }

        [UseDbContext(typeof(AppDbContext))]
        //[UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Command> GetCommand([ScopedService] AppDbContext context)
        {
            return context.Commands;
        }
    }
}   