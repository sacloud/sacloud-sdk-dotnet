using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// Workflow_ConcurrencyMode Enumのテスト
/// </summary>
public class Workflow_ConcurrencyModeTests
{
    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void Workflow_ConcurrencyMode_AllValuesAreDefined()
    {
        var values = Enum.GetValues(typeof(Workflow_ConcurrencyMode));

        Assert.Equal(3, values.Length);
        Assert.Contains(Workflow_ConcurrencyMode.Parallel, values.Cast<Workflow_ConcurrencyMode>());
        Assert.Contains(Workflow_ConcurrencyMode.Lock, values.Cast<Workflow_ConcurrencyMode>());
        Assert.Contains(Workflow_ConcurrencyMode.Queue, values.Cast<Workflow_ConcurrencyMode>());
    }

    /// <summary>
    /// Enum値が正しい文字列に変換されることを確認
    /// </summary>
    [Fact]
    public void Workflow_ConcurrencyMode_ToString_ReturnsCorrectName()
    {
        Assert.Equal("Parallel", Workflow_ConcurrencyMode.Parallel.ToString());
        Assert.Equal("Lock", Workflow_ConcurrencyMode.Lock.ToString());
        Assert.Equal("Queue", Workflow_ConcurrencyMode.Queue.ToString());
    }
}
