targetScope = 'subscription'

@description('Location for all resources.')
param location string

@description('Base Name of Resources')
param commonResourceName string

resource ResourceGroup 'Microsoft.Resources/resourceGroups@2019-05-01' = {
  name: commonResourceName
  location: location
}

module Resources './resources.bicep' = {
  name: '${commonResourceName}-ProvisionResources'
  params: {
    location: location
    commonResourceName: commonResourceName
  }
  scope: ResourceGroup
}
