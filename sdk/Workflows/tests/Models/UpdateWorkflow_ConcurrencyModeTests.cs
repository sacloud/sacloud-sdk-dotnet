using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// UpdateWorkflow_ConcurrencyMode Enumのテスト
/// </summary>
public class UpdateWorkflow_ConcurrencyModeTests
{
    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void UpdateWorkflow_ConcurrencyMode_AllValuesAreDefined()
    {
        var values = Enum.GetValues(typeof(UpdateWorkflow_ConcurrencyMode));

        Assert.Equal(3, values.Length);
        Assert.Contains(UpdateWorkflow_ConcurrencyMode.Parallel, values.Cast<UpdateWorkflow_ConcurrencyMode>());
        Assert.Contains(UpdateWorkflow_ConcurrencyMode.Lock, values.Cast<UpdateWorkflow_ConcurrencyMode>());
        Assert.Contains(UpdateWorkflow_ConcurrencyMode.Queue, values.Cast<UpdateWorkflow_ConcurrencyMode>());
    }

    /// <summary>
    /// Enum値が正しい文字列に変換されることを確認
    /// </summary>
    [Fact]
    public void UpdateWorkflow_ConcurrencyMode_ToString_ReturnsCorrectName()
    {
        Assert.Equal("Parallel", UpdateWorkflow_ConcurrencyMode.Parallel.ToString());
        Assert.Equal("Lock", UpdateWorkflow_ConcurrencyMode.Lock.ToString());
        Assert.Equal("Queue", UpdateWorkflow_ConcurrencyMode.Queue.ToString());
    }
}
