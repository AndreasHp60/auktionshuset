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
 --priority 1001 \
 --sku Standard_small \
 --http-settings-protocol http \
 --public-ip-address $PUBLICIPNAME \
 --vnet-name $NETWORKNAME \
 --subnet $GATEWAYSUBNET

