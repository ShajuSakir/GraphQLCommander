using System.Threading.Tasks;
using CommanderGQL.Data;
using CommanderGQL.GraphQL.Platforms;
using CommanderGQL.GraphQL.Commands;
using CommanderGQL.Models;
using HotChocolate;
using HotChocolate.Data;
using System.Threading;
using HotChocolate.Subscriptions;

namespace CommanderGQL.GraphQL
{
    public class Mutation
    {
        //this is to make use of multi threaded DB context
        [UseDbContext(typeof(AppDbContext))]
        public async Task<AddPlatformPayload> AddPlatformAsync(
            AddPlatformInput input, 
            [ScopedService] AppDbContext context,
            [Service] ITopicEventSender eventSender,//this is the event sender for subscription
            CancellationToken cancellationToken) //allows to cancel async method calls incase it hangs
            {
                var platform = new Platform{
                    Name = input.Name
                };

                context.Platforms.Add(platform);
                await context.SaveChangesAsync(cancellationToken);

                //send off event message
                await eventSender.SendAsync(nameof(Subscription.OnPlatformAdded), platform, cancellationToken);//pass the method we hook into, pass over the platform object, also pass the cancellation  token
            

                return new AddPlatformPayload(platform);
            }

        [UseDbContext(typeof(AppDbContext))]
        public async Task<AddCommandPayload> AddCommandAsync(AddCommandInput input, 
            [ScopedService] AppDbContext context)
            {
                var command = new Command{
                    HowTo = input.HowTo,
                    CommandLine = input.CommandLine,
                    PlatformId = input.PlatformId
                };

                context.Commands.Add(command);
                await context.SaveChangesAsync();

                return new AddCommandPayload(command);
            }
    }
    
}