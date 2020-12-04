using HotChocolate;
using HotChocolate.Resolvers;

namespace repro_fields_delegate
{
    public class Query
    {
        public NetworkAvailabilityCounterType GetTestCounter([Service] IResolverContext context)
        {
            context.ScopedContextData = context.ScopedContextData.SetItem("customer", "TST");
            return new NetworkAvailabilityCounterType { Threshold = 20, Hostname = "test", Location = "caews", ServiceName = "service" };
        }
    }

    public class NetworkAvailabilityCounterType
    {
        public string? ServiceName { set; get; }
        public string? Location { set; get; }
        public string? Hostname { set; get; }
        public double? Threshold { get; set; }
    }
}
