@allowed(['dev','prod'])
param environment string

param domain string

param participant string

param location string

var baseResourceName = '${domain}-${participant}'

targetScope = 'resourceGroup'

module appService 'appservice.bicep' = {
  name: 'appService'
  params: {
    appName: '${baseResourceName}-${environment}'
    location: location
    environment: environment
  }
}
  
module keyvault 'keyvault.bicep' = {
      name: 'keyvault'
      params: {
        appId: appService.outputs.appServiceInfo.appId
        slotId: appService.outputs.appServiceInfo.slotId
        location: location
        appName: '${participant}-${environment}' // key vault has 24 char max so just doing your name, usually would do appname-env but that'll conflict for everyone
      }
    }
