# Deployment

## resursegruppe
RESGROUP=AuktionsHusetG6RG
az group create --name $RESGROUP --location eastus

## vnet
NETWORKNAME=auktionshusetG6VNET
GATEWAYSUBNET=auktionshusetG6_gateway_subnet
PUBLICIPNAME=auktionshusetG6_ip

az network vnet create \
 --name $NETWORKNAME \
 --resource-group $RESGROUP \
 --location eastus \
 --address-prefix 10.0.0.0/16 \
 --subnet-name $GATEWAYSUBNET \
 --subnet-prefix 10.0.0.0/24

az network public-ip create \
 --resource-group $RESGROUP \
 --name $PUBLICIPNAME \
 --dns-name auktionshusetg6 \
 --allocation-method Dynamic \
 --sku Basic

## gateway
GATEWAYNAME=auktionshusetG6AG

az network application-gateway create \
 --name $GATEWAYNAME \
 --location eastus \
 --resource-group $RESGROUP \
 --capacity 2 \
 --sku Standard_small \
 --http-settings-protocol http \
 --public-ip-address $PUBLICIPNAME \
 --vnet-name $NETWORKNAME \
 --subnet $GATEWAYSUBNET

## dns zone

DNSZONENAME=auktionshusetg6.dk

az network private-dns zone create -g $RESGROUP -n $DNSZONENAME

az network private-dns link vnet create \
 -g $RESGROUP \
 -n auktionshusetDNSLink \
 -z $DNSZONENAME \
 -v $NETWORKNAME \
 -e false

## storage account

STORAGEACCOUNTNAME=auktionstoreg6

az storage account create -n $STORAGEACCOUNTNAME -g $RESGROUP -l eastus --sku Standard_LRS
az storage share create --account-name $STORAGEACCOUNTNAME --name config

## backend - zone

BACKENDSUBNET=auktionshusetg6_backend_subnet

az network vnet subnet create \
 --name $BACKENDSUBNET \
 --resource-group $RESGROUP \
 --vnet-name $NETWORKNAME \
 --address-prefix 10.0.1.0/24

az network vnet subnet update \
 --resource-group $RESGROUP \
 --name $BACKENDSUBNET \
 --vnet-name $NETWORKNAME \
 --delegations Microsoft.ContainerInstance/containerGroups

 az container create \
 --resource-group $RESGROUP \
 --file deploy-backend-aci.yaml


BACKEND_SERVER_IP=$(az container show --resource-group $RESGROUP --name auktionsHusetg6BackendGroup --output tsv --query ipAddress.ip)

echo $BACKEND_SERVER_IP

BACKENDPOOL=$(az network application-gateway address-pool list -g $RESGROUP --gateway-name $GATEWAYNAME --output tsv --query [].name)

## tilføj ip til dns zone

az network private-dns record-set a add-record -g $RESGROUP --zone-name $DNSZONENAME -n BACKEND -a $BACKEND_SERVER_IP

az network application-gateway address-pool update -g $RESGROUP --gateway-name $GATEWAYNAME -n $BACKENDPOOL --add backendAddresses ipAddress=$BACKEND_SERVER_IP

## brug httplistener rabbitmq
MQFRONTENDPORTNAME=rabbitmqPort
RABBITMQLISTENERNAME=RabbitMQHttpListener
RABBITMQSETTING=rabbitMQHttpSettings

az network application-gateway frontend-port create -g $RESGROUP --gateway-name $GATEWAYNAME -n $MQFRONTENDPORTNAME --port 15672

az network application-gateway http-listener create -g $RESGROUP --gateway-name $GATEWAYNAME --frontend-port $MQFRONTENDPORTNAME -n $RABBITMQLISTENERNAME

az network application-gateway settings create -g $RESGROUP --gateway-name $GATEWAYNAME \
 -n $RABBITMQSETTING --port 15672 --protocol Http --timeout 30

az network application-gateway rule create -g $RESGROUP --gateway-name $GATEWAYNAME \
 -n RabbitMqRule --http-listener $RABBITMQLISTENERNAME --rule-type Basic \
 --address-pool $BACKENDPOOL --http-settings $RABBITMQSETTING

 # Deploy services

 ## Opret subnet

 BACKENDSUBNET=auktionshuset_services_subnet

 az network vnet subnet create \
 --name $BACKENDSUBNET \
 --resource-group $RESGROUP \
 --vnet-name $NETWORKNAME \
 --address-prefix 10.0.2.0/24

az network vnet subnet update \
 --resource-group $RESGROUP \
 --name $BACKENDSUBNET \
 --vnet-name $NETWORKNAME \
 --delegations Microsoft.ContainerInstance/containerGroups

## create container
 az container create \
 --resource-group $RESGROUP \
 --file deploy-service-aci.yaml

 ## find ip

SERVICE_SERVER_IP=$(az container show --resource-group $RESGROUP --name auktionsHusetServicesGroup --output tsv --query ipAddress.ip)

## tilføj til dnszone
az network private-dns record-set a add-record -g $RESGROUP --zone-name $DNSZONENAME -n DEVOPS -a $SERVICE_SERVER_IP

az network application-gateway address-pool update -g $RESGROUP --gateway-name $GATEWAYNAME -n $BACKENDPOOL --add backendAddresses ipAddress=$SERVICE_SERVER_IP

## sluk aplication gateway

az network application-gateway stop --name $GATEWAYNAME
                                    --resource-group $RESGROUP

## start aplication gateway

az network application-gateway start --name $GATEWAYNAME
                                    --resource-group $RESGROUP
## husk at start containers først


