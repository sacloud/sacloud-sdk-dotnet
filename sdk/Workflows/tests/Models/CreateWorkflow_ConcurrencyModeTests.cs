using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// CreateWorkflow_ConcurrencyMode Enumのテスト
/// </summary>
public class CreateWorkflow_ConcurrencyModeTests
{
    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void CreateWorkflow_ConcurrencyMode_AllValuesAreDefined()
    {
        var values = Enum.GetValues(typeof(CreateWorkflow_ConcurrencyMode));

        Assert.Equal(3, values.Length);
        Assert.Contains(CreateWorkflow_ConcurrencyMode.Parallel, values.Cast<CreateWorkflow_ConcurrencyMode>());
        Assert.Contains(CreateWorkflow_ConcurrencyMode.Lock, values.Cast<CreateWorkflow_ConcurrencyMode>());
        Assert.Contains(CreateWorkflow_ConcurrencyMode.Queue, values.Cast<CreateWorkflow_ConcurrencyMode>());
    }

    /// <summary>
    /// Enum値が正しい文字列に変換されることを確認
    /// </summary>
    [Fact]
    public void CreateWorkflow_ConcurrencyMode_ToString_ReturnsCorrectName()
    {
        Assert.Equal("Parallel", CreateWorkflow_ConcurrencyMode.Parallel.ToString());
        Assert.Equal("Lock", CreateWorkflow_ConcurrencyMode.Lock.ToString());
        Assert.Equal("Queue", CreateWorkflow_ConcurrencyMode.Queue.ToString());
    }
}
