docker build -t customer-image -f Dockerfile .
docker tag customer-image marththecreater/auktionshuset:customer
docker push marththecreater/auktionshuset:customer