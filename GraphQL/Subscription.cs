using CommanderGQL.Models;
using HotChocolate;
using HotChocolate.Types;

namespace CommanderGQL.GraphQL
{
    public class Subscription
    {
        //eventmessage is Platform , and this return platform
        //public Platform OnPlatformAdded([EventMessage] Platform platform) => platform;  //OR
        [Subscribe]
        [Topic]//is the type of subscription
        public Platform OnPlatformAdded([EventMessage] Platform platform)
        {
            return platform;
        } 
    }    
}