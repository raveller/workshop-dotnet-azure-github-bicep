@allowed(['dev','prod'])
param environment string

param domain string

param participant string

var baseResourceName = '${domain}-${participant}'

targetScope = 'resourceGroup'

module appService 'appservice.bicep' = {
  name: 'appService'
  params: {
    appName: '${baseResourceName}-${environment}'
    location: 'centralus'
    environment: environment
  }
}
  