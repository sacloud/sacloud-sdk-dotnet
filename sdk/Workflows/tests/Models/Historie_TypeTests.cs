using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// Historie_Type Enumのテスト
/// </summary>
public class Historie_TypeTests
{
    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void Historie_Type_AllValuesAreDefined()
    {
        var values = Enum.GetValues(typeof(Historie_Type));

        Assert.Equal(12, values.Length);
        Assert.Contains(Historie_Type.WorkflowWillStart, values.Cast<Historie_Type>());
        Assert.Contains(Historie_Type.WorkflowDidSuspended, values.Cast<Historie_Type>());
        Assert.Contains(Historie_Type.WorkflowWillResume, values.Cast<Historie_Type>());
        Assert.Contains(Historie_Type.WorkflowDidCompleted, values.Cast<Historie_Type>());
        Assert.Contains(Historie_Type.WorkflowDidFailed, values.Cast<Historie_Type>());
        Assert.Contains(Historie_Type.WorkflowDidCanceled, values.Cast<Historie_Type>());
        Assert.Contains(Historie_Type.StepWillExecute, values.Cast<Historie_Type>());
        Assert.Contains(Historie_Type.StepDidExecuted, values.Cast<Historie_Type>());
        Assert.Contains(Historie_Type.FunctionWillCall, values.Cast<Historie_Type>());
        Assert.Contains(Historie_Type.FunctionWillRun, values.Cast<Historie_Type>());
        Assert.Contains(Historie_Type.FunctionDidRun, values.Cast<Historie_Type>());
        Assert.Contains(Historie_Type.FunctionDidFailed, values.Cast<Historie_Type>());
    }

    /// <summary>
    /// Enum値が正しい文字列に変換されることを確認
    /// </summary>
    [Fact]
    public void Historie_Type_ToString_ReturnsCorrectName()
    {
        Assert.Equal("WorkflowWillStart", Historie_Type.WorkflowWillStart.ToString());
        Assert.Equal("WorkflowDidSuspended", Historie_Type.WorkflowDidSuspended.ToString());
        Assert.Equal("WorkflowWillResume", Historie_Type.WorkflowWillResume.ToString());
        Assert.Equal("WorkflowDidCompleted", Historie_Type.WorkflowDidCompleted.ToString());
        Assert.Equal("WorkflowDidFailed", Historie_Type.WorkflowDidFailed.ToString());
    }
}
