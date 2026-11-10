using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using Sacloud.Sdk.KMS.Models;
using System.Text;

namespace Sacloud.Sdk.KMS.Tests.Models;
using System.Text.Json;

/// <summary>
/// WrappedChangeKeyState responseモデルのテスト
/// </summary>
public class WrappedChangeKeyStateTests
{
    /// <summary>
    /// ステータス変更APIのJSONレスポンスをデシリアライズできることを確認
    /// </summary>
    [Fact]
    public async Task CanDeserializeResponseJson()
    {
        const string json = """
            {
              "is_ok": true,
              "Key": {
                "Status": "suspended"
              }
            }
            """;
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", stream);
        var response = parseNode.GetObjectValue(WrappedChangeKeyState.CreateFromDiscriminatorValue);

        Assert.NotNull(response);
        Assert.Equal(true, response.IsOk);
        Assert.NotNull(response.Key);
        Assert.Equal(ChangeKeyState_Status.Suspended, response.Key.Status);
    }

      [Fact]
      public async Task CanSerializeResponseJson()
      {
        var model = new WrappedChangeKeyState
        {
          IsOk = true,
          Key = new ChangeKeyState { Status = ChangeKeyState_Status.Suspended }
        };
        model.AdditionalData.Add("custom_field", "custom_value");
        using var stream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, model);
        await writer.GetSerializedContent().CopyToAsync(stream);

        stream.Position = 0;
        using var jsonDocument = await JsonDocument.ParseAsync(stream);
        var root = jsonDocument.RootElement;
        Assert.True(root.GetProperty("is_ok").GetBoolean());
        Assert.Equal("suspended", root.GetProperty("Key").GetProperty("Status").GetString());
        Assert.Equal("custom_value", root.GetProperty("custom_field").GetString());
      }
}