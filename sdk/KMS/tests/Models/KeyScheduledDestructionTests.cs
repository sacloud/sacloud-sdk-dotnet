using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// KeyScheduledDestructionモデルのテスト
/// </summary>
public class KeyScheduledDestructionTests
{
    [Fact]
    public async Task CanSerializeAndDeserializeProperties()
    {
        var expectedDate = new DateTimeOffset(2026, 10, 15, 12, 30, 0, TimeSpan.Zero);
        var originalModel = new KeyScheduledDestruction
        {
            DeletionScheduledAfter = expectedDate,
            Status = KeyScheduledDestruction_Status.Pending_destruction
        };
        using var stream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        await writer.GetSerializedContent().CopyToAsync(stream);

        stream.Position = 0;
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var result = parseNode.GetObjectValue(KeyScheduledDestruction.CreateFromDiscriminatorValue);

        Assert.NotNull(result);
        Assert.Equal(expectedDate, result.DeletionScheduledAfter);
        Assert.Equal(KeyScheduledDestruction_Status.Pending_destruction, result.Status);
    }
}
