param(
      [Parameter(Mandatory=$true)]
      [string] $SubscriptionName,

      [Parameter(Mandatory=$true)]
      [string] $ResourceGroupName,

      [Parameter(Mandatory=$true)]
      [string] $ResourceGroupLocation,

      [Parameter(Mandatory=$true)]
      [string] $EventHubNamespaceName,

      [Parameter(Mandatory=$true)]
      [string] $EventHubName,

      [Parameter(Mandatory=$true)]
      [string] $AdxClusterName,

      [Parameter(Mandatory=$true)]
      [string] $AdxDatabaseName
)

Write-Host "Please login to Azure..."
az login

az account set --subscription $SubscriptionName
Write-Host "Using subscription '$SubscriptionName'."

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
