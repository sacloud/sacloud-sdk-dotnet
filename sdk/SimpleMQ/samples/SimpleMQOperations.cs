using Sacloud.Sdk.SimpleMQ.Models;

namespace Sacloud.Sdk.SimpleMQ.Samples;

/// <summary>
/// シンプルMQの各種操作を実装するクラス
/// </summary>
public class SimpleMQOperations
{
    private readonly SimpleMQClient _client;
    private readonly string _queueName;

    public SimpleMQOperations(SimpleMQClient client)
    {
        _client = client;
        _queueName = "<YOUR_QUEUE_NAME>";
    }

    /// <summary>
    /// ユーザーに操作メニューを表示し、選択された操作を実行する
    /// </summary>
    public async Task RunInteractiveMenuAsync()
    {
        Console.WriteLine("\n実行する操作を選択してください:");
        Console.WriteLine("1. メッセージの送信");
        Console.WriteLine("2. メッセージの受信");
        Console.WriteLine("3. メッセージのタイムアウト延長");
        Console.WriteLine("4. メッセージの削除");
        Console.Write("選択 (1 ~ 4 から入力): ");
        var choice = Console.ReadLine();

        await ExecuteOperationAsync(choice);
    }

    /// <summary>
    /// 選択された操作を実行する
    /// </summary>
    private async Task ExecuteOperationAsync(string? choice)
    {
        switch (choice)
        {
            case "1":
                await SendMessageAsync();
                break;
            case "2":
                await ReceiveMessageAsync();
                break;
            case "3":
                await ExtendMessageTimeoutAsync();
                break;
            case "4":
                await DeleteMessageAsync();
                break;
            default:
                Console.WriteLine("無効な番号が選択されました。");
                break;
        }
    }

    private async Task SendMessageAsync()
    {
        var body = new SendRequest
        {
            Content = "SampleContent"
        };
        var result = await _client.Queues[_queueName].Messages.PostAsync(body);
        Console.WriteLine("送信されたメッセージ:");
        Console.WriteLine($"ID: {result.Message.Id}, Content: {result.Message.Content}");
    }

    private async Task ReceiveMessageAsync()
    {
        var result = await _client.Queues[_queueName].Messages.GetAsync();
        Console.WriteLine("受信したメッセージ:");
        foreach (var message in result.Messages)
        {
            Console.WriteLine($"ID: {message.Id}, Content: {message.Content}, VisibilityTimeoutAt: {message.VisibilityTimeoutAt}");
        }
    }

    private async Task ExtendMessageTimeoutAsync()
    {
        Console.Write("タイムアウトを延長するメッセージIDを入力してください: ");
        var messageId = Console.ReadLine();
        var result = await _client.Queues[_queueName].Messages[messageId].PutAsync();
        Console.WriteLine("タイムアウトを延長したメッセージ:");
        Console.WriteLine($"ID: {result.Message.Id}, Content: {result.Message.Content}, VisibilityTimeoutAt: {result.Message.VisibilityTimeoutAt}");
    }

    private async Task DeleteMessageAsync()
    {
        Console.Write("削除するメッセージIDを入力してください: ");
        var messageId = Console.ReadLine();
        var result = await _client.Queues[_queueName].Messages[messageId].DeleteAsync();
        Console.WriteLine($"削除結果: {result.Result}");
    }
}
