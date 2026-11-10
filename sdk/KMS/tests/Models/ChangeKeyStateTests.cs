using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// ChangeKeyStateモデルのテスト
/// </summary>
public class ChangeKeyStateTests
{
    [Fact]
    public async Task CanSerializeAndDeserializeStatus()
    {
        var originalModel = new ChangeKeyState { Status = ChangeKeyState_Status.Restricted };
        using var stream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        await writer.GetSerializedContent().CopyToAsync(stream);

        stream.Position = 0;
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var result = parseNode.GetObjectValue(ChangeKeyState.CreateFromDiscriminatorValue);

        Assert.NotNull(result);
        Assert.Equal(ChangeKeyState_Status.Restricted, result.Status);
    }
}
