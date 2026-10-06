using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.SimpleNotification.Models;
using Sacloud.Sdk.Shared.Validation;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.SimpleNotification.Tests.Models.Model;

/// <summary>
/// CommonServiceItemSettingsモデルのテスト
/// </summary>
public class CommonServiceItemSettingsTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = CommonServiceItemSettings.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<CommonServiceItemSettings>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => CommonServiceItemSettings.CreateFromDiscriminatorValue(null!));
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがDestinationSettingsを認識することを確認（TypeとValueフィールドあり）
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithTypeAndValue_CreatesDestinationSettings()
    {
        var parseNode = Substitute.For<IParseNode>();
        var typeNode = Substitute.For<IParseNode>();
        var destinationSettings = new DestinationSettings
        {
            Type = DestinationSettings_Type.Email,
            Value = "test@example.com"
        };

        parseNode.GetChildNode("Type").Returns(typeNode);
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<DestinationSettings>>()).Returns(destinationSettings);

        var result = CommonServiceItemSettings.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.NotNull(result.DestinationSettings);
        Assert.Null(result.GroupSettings);
        Assert.Null(result.RoutingSettings);
        Assert.Equal(DestinationSettings_Type.Email, result.DestinationSettings.Type);
        Assert.Equal("test@example.com", result.DestinationSettings.Value);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがGroupSettingsを認識することを確認（Destinationsフィールドあり）
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithDestinations_CreatesGroupSettings()
    {
        var parseNode = Substitute.For<IParseNode>();
        var destinationsNode = Substitute.For<IParseNode>();
        var groupSettings = new GroupSettings
        {
            Destinations = new List<string> { "123456789012" }
        };

        parseNode.GetChildNode("Type").Returns((IParseNode?)null);
        parseNode.GetChildNode("Destinations").Returns(destinationsNode);
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<GroupSettings>>()).Returns(groupSettings);

        var result = CommonServiceItemSettings.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.Null(result.DestinationSettings);
        Assert.NotNull(result.GroupSettings);
        Assert.Null(result.RoutingSettings);
        Assert.Single(result.GroupSettings.Destinations);
        Assert.Equal("123456789012", result.GroupSettings.Destinations[0]);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがRoutingSettingsを認識することを確認（SourceIDとTargetGroupIDフィールドあり）
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithSourceIdAndTargetGroupId_CreatesRoutingSettings()
    {
        var parseNode = Substitute.For<IParseNode>();
        var sourceIdNode = Substitute.For<IParseNode>();
        var routingSettings = new RoutingSettings
        {
            SourceID = "123456789012",
            TargetGroupID = "098765432109"
        };

        parseNode.GetChildNode("Type").Returns((IParseNode?)null);
        parseNode.GetChildNode("Destinations").Returns((IParseNode?)null);
        parseNode.GetChildNode("SourceID").Returns(sourceIdNode);
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<RoutingSettings>>()).Returns(routingSettings);

        var result = CommonServiceItemSettings.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.Null(result.DestinationSettings);
        Assert.Null(result.GroupSettings);
        Assert.NotNull(result.RoutingSettings);
        Assert.Equal("123456789012", result.RoutingSettings.SourceID);
        Assert.Equal("098765432109", result.RoutingSettings.TargetGroupID);
    }

    /// <summary>
    /// GetFieldDeserializersが空のモデルで空の辞書を返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_WithEmptyModel_ReturnsEmptyDictionary()
    {
        var model = new CommonServiceItemSettings();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Empty(deserializers);
    }

    /// <summary>
    /// GetFieldDeserializersがDestinationSettingsの場合に正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_WithDestinationSettings_ReturnsCorrectMappings()
    {
        var model = new CommonServiceItemSettings
        {
            DestinationSettings = new DestinationSettings()
        };
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("Disabled"));
        Assert.True(deserializers.ContainsKey("Type"));
        Assert.True(deserializers.ContainsKey("Value"));
    }

    /// <summary>
    /// GetFieldDeserializersがGroupSettingsの場合に正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_WithGroupSettings_ReturnsCorrectMappings()
    {
        var model = new CommonServiceItemSettings
        {
            GroupSettings = new GroupSettings()
        };
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("Disabled"));
        Assert.True(deserializers.ContainsKey("Destinations"));
    }

    /// <summary>
    /// GetFieldDeserializersがRoutingSettingsの場合に正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_WithRoutingSettings_ReturnsCorrectMappings()
    {
        var model = new CommonServiceItemSettings
        {
            RoutingSettings = new RoutingSettings()
        };
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("SourceID"));
        Assert.True(deserializers.ContainsKey("TargetGroupID"));
        Assert.True(deserializers.ContainsKey("MatchLabels"));
        Assert.True(deserializers.ContainsKey("PriorityRank"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CommonServiceItemSettings();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// DestinationSettingsでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserialize_WithDestinationSettings()
    {
        var originalModel = new CommonServiceItemSettings
        {
            DestinationSettings = new DestinationSettings
            {
                Type = DestinationSettings_Type.Email,
                Value = "user@example.com",
                Disabled = false,
            }
        };

        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        using var serializeStream = new MemoryStream();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CommonServiceItemSettings.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.DestinationSettings);
        Assert.Equal(originalModel.DestinationSettings.Type, deserializedModel.DestinationSettings.Type);
        Assert.Equal(originalModel.DestinationSettings.Value, deserializedModel.DestinationSettings.Value);
        Assert.Equal(originalModel.DestinationSettings.Disabled, deserializedModel.DestinationSettings.Disabled);
    }

    /// <summary>
    /// GroupSettingsでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserialize_WithGroupSettings()
    {
        var originalModel = new CommonServiceItemSettings
        {
            GroupSettings = new GroupSettings
            {
                Destinations = new List<string> { "123456789012", "098765432109" },
                Disabled = true,
            }
        };

        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        using var serializeStream = new MemoryStream();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CommonServiceItemSettings.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.GroupSettings);
        Assert.Equal(originalModel.GroupSettings.Destinations.Count, deserializedModel.GroupSettings.Destinations.Count);
        Assert.Equal(originalModel.GroupSettings.Disabled, deserializedModel.GroupSettings.Disabled);
    }

    /// <summary>
    /// RoutingSettingsでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserialize_WithRoutingSettings()
    {
        var originalModel = new CommonServiceItemSettings
        {
            RoutingSettings = new RoutingSettings
            {
                SourceID = "123456789012",
                TargetGroupID = "098765432109",
                MatchLabels = new List<RoutingSettings_MatchLabels>
                {
                    new RoutingSettings_MatchLabels { Name = "env", Value = "prod" }
                }
            }
        };

        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        using var serializeStream = new MemoryStream();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CommonServiceItemSettings.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.RoutingSettings);
        Assert.Equal(originalModel.RoutingSettings.SourceID, deserializedModel.RoutingSettings.SourceID);
        Assert.Equal(originalModel.RoutingSettings.TargetGroupID, deserializedModel.RoutingSettings.TargetGroupID);
    }
}
