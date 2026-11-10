using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using Sacloud.Sdk.KMS.Models;
using System.Text;

namespace Sacloud.Sdk.KMS.Tests.Models;
using System.Text.Json;

/// <summary>
/// WrappedKeyScheduledDestruction responseモデルのテスト
/// </summary>
public class WrappedKeyScheduledDestructionTests
{
    /// <summary>
    /// 削除予約APIのJSONレスポンスをデシリアライズできることを確認
    /// </summary>
    [Fact]
    public async Task CanDeserializeResponseJson()
    {
        const string json = """
            {
              "is_ok": true,
              "Key": {
                "DeletionScheduledAfter": "2026-10-15T12:30:00Z",
                "Status": "pending_destruction"
              }
            }
            """;
        var expectedDeletionScheduledAfter = new DateTimeOffset(2026, 10, 15, 12, 30, 0, TimeSpan.Zero);
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", stream);
        var response = parseNode.GetObjectValue(WrappedKeyScheduledDestruction.CreateFromDiscriminatorValue);

        Assert.NotNull(response);
        Assert.Equal(true, response.IsOk);
        Assert.NotNull(response.Key);
        Assert.Equal(expectedDeletionScheduledAfter, response.Key.DeletionScheduledAfter);
        Assert.Equal(KeyScheduledDestruction_Status.Pending_destruction, response.Key.Status);
    }

      [Fact]
      public async Task CanSerializeResponseJson()
      {
        var expectedDeletionScheduledAfter = new DateTimeOffset(2026, 10, 15, 12, 30, 0, TimeSpan.Zero);
        var model = new WrappedKeyScheduledDestruction
        {
          IsOk = true,
          Key = new KeyScheduledDestruction
          {
            DeletionScheduledAfter = expectedDeletionScheduledAfter,
            Status = KeyScheduledDestruction_Status.Pending_destruction
          }
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
        var key = root.GetProperty("Key");
        Assert.Equal(expectedDeletionScheduledAfter, key.GetProperty("DeletionScheduledAfter").GetDateTimeOffset());
        Assert.Equal("pending_destruction", key.GetProperty("Status").GetString());
        Assert.Equal("custom_value", root.GetProperty("custom_field").GetString());
      }
}