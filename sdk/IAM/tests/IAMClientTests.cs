using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.IAM.Auth;
using Sacloud.Sdk.IAM.Compat;
using Sacloud.Sdk.IAM.DisableServicePolicy;
using Sacloud.Sdk.IAM.EnableServicePolicy;
using Sacloud.Sdk.IAM.Folders;
using Sacloud.Sdk.IAM.Groups;
using Sacloud.Sdk.IAM.IamRoles;
using Sacloud.Sdk.IAM.IdRoles;
using Sacloud.Sdk.IAM.MoveFolders;
using Sacloud.Sdk.IAM.MoveProjects;
using Sacloud.Sdk.IAM.Organization;
using Sacloud.Sdk.IAM.OrganizationAuthConditions;
using Sacloud.Sdk.IAM.OrganizationIamPolicy;
using Sacloud.Sdk.IAM.OrganizationIdPolicy;
using Sacloud.Sdk.IAM.OrganizationPasswordPolicy;
using Sacloud.Sdk.IAM.OrganizationServicePolicy;
using Sacloud.Sdk.IAM.Projects;
using Sacloud.Sdk.IAM.ScimConfigurations;
using Sacloud.Sdk.IAM.ServicePolicyRuleTemplates;
using Sacloud.Sdk.IAM.ServicePolicyStatus;
using Sacloud.Sdk.IAM.ServicePrincipals;
using Sacloud.Sdk.IAM.SsoProfiles;

namespace Sacloud.Sdk.IAM.Tests;

/// <summary>
/// IAMClientのテスト
/// </summary>
public class IAMClientTests
{
    /// <summary>
    /// コンストラクタが正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = string.Empty;

        var client = new IAMClient(requestAdapter);

        Assert.NotNull(client);
    }

    /// <summary>
    /// コンストラクタがBaseUrlが空の場合にデフォルトURLを設定することを確認
    /// </summary>
    [Fact]
    public void Constructor_SetsBaseUrl_WhenBaseUrlIsEmpty()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = string.Empty;

        var client = new IAMClient(requestAdapter);

        Assert.Equal("https://secure.sakura.ad.jp/cloud/api/iam/1.0", requestAdapter.BaseUrl);
    }

    /// <summary>
    /// コンストラクタがBaseUrlが既に設定されている場合に変更しないことを確認
    /// </summary>
    [Fact]
    public void Constructor_DoesNotOverrideBaseUrl_WhenBaseUrlIsSet()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://custom.api.example.com";

        var client = new IAMClient(requestAdapter);

        Assert.Equal("https://custom.api.example.com", requestAdapter.BaseUrl);
    }

    /// <summary>
    /// コンストラクタがPathParametersにbaseurlを追加することを確認
    /// </summary>
    [Fact]
    public void Constructor_AddsBaseurlToPathParameters()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new IAMClient(requestAdapter);

        Assert.NotNull(client);
    }

    /// <summary>
    /// AuthプロパティがAuthRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Auth_ReturnsRequestBuilderInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result = client.Auth;

        Assert.NotNull(result);
        Assert.IsType<AuthRequestBuilder>(result);
    }

    /// <summary>
    /// CompatプロパティがCompatRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Compat_ReturnsRequestBuilderInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result = client.Compat;

        Assert.NotNull(result);
        Assert.IsType<CompatRequestBuilder>(result);
    }

    /// <summary>
    /// DisableServicePolicyプロパティがDisableServicePolicyRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void DisableServicePolicy_ReturnsRequestBuilderInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result = client.DisableServicePolicy;

        Assert.NotNull(result);
        Assert.IsType<DisableServicePolicyRequestBuilder>(result);
    }

    /// <summary>
    /// EnableServicePolicyプロパティがEnableServicePolicyRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void EnableServicePolicy_ReturnsRequestBuilderInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result = client.EnableServicePolicy;

        Assert.NotNull(result);
        Assert.IsType<EnableServicePolicyRequestBuilder>(result);
    }

    /// <summary>
    /// FoldersプロパティがFoldersRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Folders_ReturnsRequestBuilderInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result = client.Folders;

        Assert.NotNull(result);
        Assert.IsType<FoldersRequestBuilder>(result);
    }

    /// <summary>
    /// GroupsプロパティがGroupsRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Groups_ReturnsRequestBuilderInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result = client.Groups;

        Assert.NotNull(result);
        Assert.IsType<GroupsRequestBuilder>(result);
    }

    /// <summary>
    /// IamRolesプロパティがIamRolesRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void IamRoles_ReturnsRequestBuilderInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result = client.IamRoles;

        Assert.NotNull(result);
        Assert.IsType<IamRolesRequestBuilder>(result);
    }

    /// <summary>
    /// IdRolesプロパティがIdRolesRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void IdRoles_ReturnsRequestBuilderInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result = client.IdRoles;

        Assert.NotNull(result);
        Assert.IsType<IdRolesRequestBuilder>(result);
    }

    /// <summary>
    /// MoveFoldersプロパティがMoveFoldersRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void MoveFolders_ReturnsRequestBuilderInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result = client.MoveFolders;

        Assert.NotNull(result);
        Assert.IsType<MoveFoldersRequestBuilder>(result);
    }

    /// <summary>
    /// MoveProjectsプロパティがMoveProjectsRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void MoveProjects_ReturnsRequestBuilderInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result = client.MoveProjects;

        Assert.NotNull(result);
        Assert.IsType<MoveProjectsRequestBuilder>(result);
    }

    /// <summary>
    /// OrganizationプロパティがOrganizationRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Organization_ReturnsRequestBuilderInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result = client.Organization;

        Assert.NotNull(result);
        Assert.IsType<OrganizationRequestBuilder>(result);
    }

    /// <summary>
    /// OrganizationAuthConditionsプロパティがOrganizationAuthConditionsRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void OrganizationAuthConditions_ReturnsRequestBuilderInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result = client.OrganizationAuthConditions;

        Assert.NotNull(result);
        Assert.IsType<OrganizationAuthConditionsRequestBuilder>(result);
    }

    /// <summary>
    /// OrganizationIamPolicyプロパティがOrganizationIamPolicyRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void OrganizationIamPolicy_ReturnsRequestBuilderInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result = client.OrganizationIamPolicy;

        Assert.NotNull(result);
        Assert.IsType<OrganizationIamPolicyRequestBuilder>(result);
    }

    /// <summary>
    /// OrganizationIdPolicyプロパティがOrganizationIdPolicyRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void OrganizationIdPolicy_ReturnsRequestBuilderInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result = client.OrganizationIdPolicy;

        Assert.NotNull(result);
        Assert.IsType<OrganizationIdPolicyRequestBuilder>(result);
    }

    /// <summary>
    /// OrganizationPasswordPolicyプロパティがOrganizationPasswordPolicyRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void OrganizationPasswordPolicy_ReturnsRequestBuilderInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result = client.OrganizationPasswordPolicy;

        Assert.NotNull(result);
        Assert.IsType<OrganizationPasswordPolicyRequestBuilder>(result);
    }

    /// <summary>
    /// OrganizationServicePolicyプロパティがOrganizationServicePolicyRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void OrganizationServicePolicy_ReturnsRequestBuilderInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result = client.OrganizationServicePolicy;

        Assert.NotNull(result);
        Assert.IsType<OrganizationServicePolicyRequestBuilder>(result);
    }

    /// <summary>
    /// ProjectsプロパティがProjectsRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Projects_ReturnsRequestBuilderInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result = client.Projects;

        Assert.NotNull(result);
        Assert.IsType<ProjectsRequestBuilder>(result);
    }

    /// <summary>
    /// ServicePolicyRuleTemplatesプロパティがServicePolicyRuleTemplatesRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void ServicePolicyRuleTemplates_ReturnsRequestBuilderInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result = client.ServicePolicyRuleTemplates;

        Assert.NotNull(result);
        Assert.IsType<ServicePolicyRuleTemplatesRequestBuilder>(result);
    }

    /// <summary>
    /// ServicePolicyStatusプロパティがServicePolicyStatusRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void ServicePolicyStatus_ReturnsRequestBuilderInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result = client.ServicePolicyStatus;

        Assert.NotNull(result);
        Assert.IsType<ServicePolicyStatusRequestBuilder>(result);
    }

    /// <summary>
    /// ServicePrincipalsプロパティがServicePrincipalsRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void ServicePrincipals_ReturnsRequestBuilderInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result = client.ServicePrincipals;

        Assert.NotNull(result);
        Assert.IsType<ServicePrincipalsRequestBuilder>(result);
    }

    /// <summary>
    /// SsoProfilesプロパティがSsoProfilesRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void SsoProfiles_ReturnsRequestBuilderInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result = client.SsoProfiles;

        Assert.NotNull(result);
        Assert.IsType<SsoProfilesRequestBuilder>(result);
    }

    /// <summary>
    /// ScimConfigurationsプロパティがScimConfigurationsRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void ScimConfigurations_ReturnsRequestBuilderInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result = client.ScimConfigurations;

        Assert.NotNull(result);
        Assert.IsType<ScimConfigurationsRequestBuilder>(result);
    }

    /// <summary>
    /// Compatプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Compat_ReturnsNewInstanceOnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result1 = client.Compat;
        var result2 = client.Compat;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// DisableServicePolicyプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void DisableServicePolicy_ReturnsNewInstanceOnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result1 = client.DisableServicePolicy;
        var result2 = client.DisableServicePolicy;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// EnableServicePolicyプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void EnableServicePolicy_ReturnsNewInstanceOnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result1 = client.EnableServicePolicy;
        var result2 = client.EnableServicePolicy;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// Foldersプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Folders_ReturnsNewInstanceOnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result1 = client.Folders;
        var result2 = client.Folders;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// Groupsプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Groups_ReturnsNewInstanceOnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result1 = client.Groups;
        var result2 = client.Groups;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// IamRolesプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void IamRoles_ReturnsNewInstanceOnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result1 = client.IamRoles;
        var result2 = client.IamRoles;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// IdRolesプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void IdRoles_ReturnsNewInstanceOnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result1 = client.IdRoles;
        var result2 = client.IdRoles;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// MoveFoldersプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void MoveFolders_ReturnsNewInstanceOnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result1 = client.MoveFolders;
        var result2 = client.MoveFolders;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// MoveProjectsプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void MoveProjects_ReturnsNewInstanceOnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result1 = client.MoveProjects;
        var result2 = client.MoveProjects;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// Organizationプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Organization_ReturnsNewInstanceOnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result1 = client.Organization;
        var result2 = client.Organization;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// OrganizationAuthConditionsプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void OrganizationAuthConditions_ReturnsNewInstanceOnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result1 = client.OrganizationAuthConditions;
        var result2 = client.OrganizationAuthConditions;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// OrganizationIamPolicyプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void OrganizationIamPolicy_ReturnsNewInstanceOnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result1 = client.OrganizationIamPolicy;
        var result2 = client.OrganizationIamPolicy;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// OrganizationIdPolicyプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void OrganizationIdPolicy_ReturnsNewInstanceOnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result1 = client.OrganizationIdPolicy;
        var result2 = client.OrganizationIdPolicy;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// OrganizationPasswordPolicyプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void OrganizationPasswordPolicy_ReturnsNewInstanceOnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result1 = client.OrganizationPasswordPolicy;
        var result2 = client.OrganizationPasswordPolicy;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// OrganizationServicePolicyプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void OrganizationServicePolicy_ReturnsNewInstanceOnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result1 = client.OrganizationServicePolicy;
        var result2 = client.OrganizationServicePolicy;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// Projectsプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Projects_ReturnsNewInstanceOnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result1 = client.Projects;
        var result2 = client.Projects;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// ServicePolicyRuleTemplatesプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void ServicePolicyRuleTemplates_ReturnsNewInstanceOnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result1 = client.ServicePolicyRuleTemplates;
        var result2 = client.ServicePolicyRuleTemplates;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// ServicePolicyStatusプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void ServicePolicyStatus_ReturnsNewInstanceOnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result1 = client.ServicePolicyStatus;
        var result2 = client.ServicePolicyStatus;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// ServicePrincipalsプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void ServicePrincipals_ReturnsNewInstanceOnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result1 = client.ServicePrincipals;
        var result2 = client.ServicePrincipals;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// SsoProfilesプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void SsoProfiles_ReturnsNewInstanceOnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result1 = client.SsoProfiles;
        var result2 = client.SsoProfiles;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// ScimConfigurationsプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void ScimConfigurations_ReturnsNewInstanceOnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result1 = client.ScimConfigurations;
        var result2 = client.ScimConfigurations;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// Authプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Auth_ReturnsNewInstanceOnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new IAMClient(requestAdapter);

        var result1 = client.Auth;
        var result2 = client.Auth;

        Assert.NotSame(result1, result2);
    }
}
