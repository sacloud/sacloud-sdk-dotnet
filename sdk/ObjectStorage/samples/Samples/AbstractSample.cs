namespace Sacloud.Sdk.ObjectStorage.Samples.Samples;

public abstract class AbstractSample(ObjectStorageClient client)
{
    protected readonly ObjectStorageClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    protected const float DELAY_SECONDS = 0.5f;    

    protected static string? ReadRequiredValue(string variableName)
    {
        Console.Write($"{variableName}を入力してください: ");
        var value = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(value))
        {
            Console.WriteLine($"{variableName}が未入力です。");
            return null;
        }
        return value;
    }
}
