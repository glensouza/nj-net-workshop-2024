<#
.SYNOPSIS
This script adds necessary Microsoft Entry App Registration and displays variables needed for GitHub Action to run.

.DESCRIPTION
This script adds necessary Microsoft Entry App Registration and displays variables needed for GitHub Action to run.

.PARAMETER tenantId
Azure Tenant Id.

.PARAMETER subscriptionId
Azure Subscription Id.

.PARAMETER tenantId
Azure Tenant Id.

.PARAMETER applicationName
Application Name.

.PARAMETER repoName
Name of this GitHub repository.

.EXAMPLE
.\rbac.ps1 -tenantId '00000000-0000-0000-0000-000000000000' -subscriptionId '00000000-0000-0000-0000-000000000000' -applicationName 'njcsc' -repoName 'nj-net-workshop-2024'
#>

param(
    [Parameter(Mandatory=$true)]
    [string]$tenantId,

    [Parameter(Mandatory=$true)]
    [string]$subscriptionId,

    [Parameter(Mandatory=$true)]
    [string]$applicationName,

    [Parameter(Mandatory=$true)]
    [string]$repoName
)

$context = Get-AzContext  
if (!$context)   
{  
    Login-AzAccount -TenantId $tenantId -SubscriptionId $subscriptionId
}
else
{
    Set-AzContext -TenantId $tenantId -SubscriptionId $subscriptionId
}

New-AzADApplication -DisplayName $applicationName-github-deployer
$clientId = (Get-AzADApplication -DisplayName $applicationName-github-deployer).AppId
$appObjectId = (Get-AzADApplication -DisplayName $applicationName-github-deployer).Id

New-AzADServicePrincipal -ApplicationId $clientId
$objectId = (Get-AzADServicePrincipal -DisplayName $applicationName-github-deployer).Id

New-AzRoleAssignment -RoleDefinitionName Owner -ObjectId $objectId
$subscriptionId = (Get-AzContext).Subscription.Id
$tenantId = (Get-AzContext).Subscription.TenantId

$githubOrg = 'glensouza'

$subject = 'repo:' + $githubOrg + '/' + $repoName + ':ref:refs/heads/main'
New-AzADAppFederatedCredential -ApplicationObjectId $appObjectId -Audience 'api://AzureADTokenExchange' -Issuer 'https://token.actions.githubusercontent.com' -Name 'GitHub-Actions-MainBranch' -Subject $subject

$subject = 'repo:' + $githubOrg + '/' + $repoName + ':environment:CleanupResources'
New-AzADAppFederatedCredential -ApplicationObjectId $appObjectId -Audience 'api://AzureADTokenExchange' -Issuer 'https://token.actions.githubusercontent.com' -Name 'GitHub-Actions-CleanupResources' -Subject $subject

$subject = 'repo:' + $githubOrg + '/' + $repoName + ':environment:IaCDeloy'
New-AzADAppFederatedCredential -ApplicationObjectId $appObjectId -Audience 'api://AzureADTokenExchange' -Issuer 'https://token.actions.githubusercontent.com' -Name 'GitHub-Actions-IaCDeloy' -Subject $subject

$subject = 'repo:' + $githubOrg + '/' + $repoName + ':environment:AppDeploy'
New-AzADAppFederatedCredential -ApplicationObjectId $appObjectId -Audience 'api://AzureADTokenExchange' -Issuer 'https://token.actions.githubusercontent.com' -Name 'GitHub-Actions-AppDeploy' -Subject $subject

Write-Host "AZURE_TENANT_ID: $tenantId"
Write-Host "AZURE_SUBSCRIPTION_ID: $subscriptionId"
Write-Host "AZURE_CLIENT_ID: $clientId"
