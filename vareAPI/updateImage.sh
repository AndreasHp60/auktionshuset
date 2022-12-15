docker build -t product-image -f Dockerfile .
docker tag product-image marththecreater/auktionshuset:product
docker push marththecreater/auktionshuset:product