using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// Execution_Status Enumのテスト
/// </summary>
public class Execution_StatusTests
{
    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void Execution_Status_AllValuesAreDefined()
    {
        var values = Enum.GetValues(typeof(Execution_Status));

        Assert.Equal(6, values.Length);
        Assert.Contains(Execution_Status.Queued, values.Cast<Execution_Status>());
        Assert.Contains(Execution_Status.Running, values.Cast<Execution_Status>());
        Assert.Contains(Execution_Status.Succeeded, values.Cast<Execution_Status>());
        Assert.Contains(Execution_Status.Failed, values.Cast<Execution_Status>());
        Assert.Contains(Execution_Status.Canceling, values.Cast<Execution_Status>());
        Assert.Contains(Execution_Status.Canceled, values.Cast<Execution_Status>());
    }

    /// <summary>
    /// Enum値が正しい文字列に変換されることを確認
    /// </summary>
    [Fact]
    public void Execution_Status_ToString_ReturnsCorrectName()
    {
        Assert.Equal("Queued", Execution_Status.Queued.ToString());
        Assert.Equal("Running", Execution_Status.Running.ToString());
        Assert.Equal("Succeeded", Execution_Status.Succeeded.ToString());
        Assert.Equal("Failed", Execution_Status.Failed.ToString());
        Assert.Equal("Canceling", Execution_Status.Canceling.ToString());
        Assert.Equal("Canceled", Execution_Status.Canceled.ToString());
    }
}
