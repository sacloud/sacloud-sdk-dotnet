// さくらのクラウドに作成したシンプルMQのキューに対してSDKを介してリクエストを送信する
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Sacloud.Sdk.SimpleMQ;
using Sacloud.Sdk.SimpleMQ.Samples;

// ロガーファクトリーの設定
using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole().SetMinimumLevel(LogLevel.Information);
});

// シンプルMQのキュー作成時に発行されるAPIキー
var key = "<YOUR_QUEUE_API_KEY>";
var authHeaderValue = $"Bearer {key}";

// 認証プロバイダーの設定
var authProvider = new ApiKeyAuthenticationProvider(authHeaderValue, "Authorization", ApiKeyAuthenticationProvider.KeyLocation.Header);

// HTTPクライアントの設定
using var adapter = new HttpClientRequestAdapter(authProvider);

// SimpleMQクライアントの初期化
var simpleMQClient = new SimpleMQClient(adapter, loggerFactory);

// 操作を実行
var operations = new SimpleMQOperations(simpleMQClient);
await operations.RunInteractiveMenuAsync();
