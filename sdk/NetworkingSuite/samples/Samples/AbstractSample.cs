namespace Sacloud.Sdk.NetworkingSuite.Samples.Samples;

public abstract class AbstractSample(NetworkingSuiteClient client)
{
    protected readonly NetworkingSuiteClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    protected const float DELAY_SECONDS = 0.5f;

    protected static string? ReadRequiredValue(string variableName)
    {
        Console.Write($"{variableName}: ");
        string? value = Console.ReadLine()?.Trim();
        return value;
    }
}
