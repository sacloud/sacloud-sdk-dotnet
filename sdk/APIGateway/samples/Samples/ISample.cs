
namespace Sacloud.Sdk.APIGateway.Samples.Samples;

public interface ISample
{
    string Description { get; }
    Task RunAsync();
}
