param(
      [Parameter(Mandatory=$false, ParameterSetName="cleanup")]
      [switch] $Cleanup,

      [Parameter(Mandatory=$true, ParameterSetName="create")]
      [Parameter(Mandatory=$true, ParameterSetName="cleanup")]
      [string] $SubscriptionName,

      [Parameter(Mandatory=$true, ParameterSetName="create")]
      [Parameter(Mandatory=$true, ParameterSetName="cleanup")]
      [string] $ResourceGroupName,

      [Parameter(Mandatory=$true, ParameterSetName="create")]
      [string] $ResourceGroupLocation,

      [Parameter(Mandatory=$true, ParameterSetName="create")]
      [string] $EventHubNamespaceName,

      [Parameter(Mandatory=$true, ParameterSetName="create")]
      [string] $EventHubName,

      [Parameter(Mandatory=$true, ParameterSetName="create")]
      [Parameter(Mandatory=$true, ParameterSetName="cleanup")]
      [string] $AdxClusterName,

      [Parameter(Mandatory=$true, ParameterSetName="create")]
      [string] $AdxDatabaseName
)

Write-Host "Please login to Azure..."
az login

az account set --subscription $SubscriptionName
Write-Host "Using subscription '$SubscriptionName'."

if ($Cleanup)
{
    Write-Host "Removing service principal"
    $appId = az ad sp list `
        --display-name $AdxClusterName `
        --query '[].{appId:appId}' `
        -o tsv

    az ad sp delete --id $appId

    Write-Host "Removing resource group..."
    az group delete --name $ResourceGroupName --yes

    return
}

Write-Host "Creating resource group..."

az group create `
    --name $ResourceGroupName `
    --location $ResourceGroupLocation

Write-Host "Creating event hub namespace and event hub..."

az eventhubs namespace create `
    --name $EventHubNamespaceName `
    --resource-group $ResourceGroupName `
    --location $ResourceGroupLocation

az eventhubs eventhub create `
    --name $EventHubName `
    --resource-group $ResourceGroupName `
    --namespace-name $EventHubNamespaceName

Write-Host "Creating event hub namespace access policies..."

az eventhubs eventhub authorization-rule create `
    --resource-group $ResourceGroupName `
    --namespace-name $EventHubNamespaceName `
    --eventhub-name $EventHubName `
    --name listen-policy `
    --rights Listen

az eventhubs eventhub authorization-rule create `
    --resource-group $ResourceGroupName `
    --namespace-name $EventHubNamespaceName `
    --eventhub-name $EventHubName `
    --name send-policy `
    --rights Send

Write-Host "Creating Kusto cluster and database. This is a good time to enjoy a cup or two of coffee..."
az kusto cluster create `
    --name $AdxClusterName `
    --sku D11_v2 `
    --resource-group $ResourceGroupName

az kusto database create `
    --cluster-name $AdxClusterName `
    --name $AdxDatabaseName `
    --resource-group $ResourceGroupName `
    --soft-delete-period P7D `
    --hot-cache-period P7D

Write-Warning "Copy the output below. It is needed to run the demo."
Write-Warning "BEGIN COPY here"

Write-Host "Event hub namespace connection strings:"

$ruleNames = az eventhubs eventhub authorization-rule list `
    --resource-group $ResourceGroupName `
    --namespace-name $EventHubNamespaceName `
    --eventhub-name $EventHubName `
    --query '[].{name:name}' `
    -o tsv

ForEach ($ruleName in $ruleNames)
{
    az eventhubs eventhub authorization-rule keys list `
        --resource-group $ResourceGroupName `
        --namespace-name $EventHubNamespaceName `
        --eventhub-name $EventHubName `
        --name $ruleName `
        --query '[keyName, primaryConnectionString]' `
        -o json
}


Write-Host "Kusto cluster data ingestion URI:"

az kusto cluster show `
    --name $AdxClusterName `
    --resource-group $ResourceGroupName `
    --query dataIngestionUri

Write-Host "Creating service principal"

$subscriptionId = az account show --subscription $SubscriptionName --query id

az ad sp create-for-rbac `
    --name $AdxClusterName `
    --role contributor `
    --scopes /subscriptions/$subscriptionId/resourceGroups/$ResourceGroupName

Write-Warning "END COPY here"