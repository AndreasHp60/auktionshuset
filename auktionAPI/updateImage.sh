docker build -t auktionsapi-image -f Dockerfile .
docker tag auktionsapi-image marththecreater/auktionshuset:auktionapi
docker push marththecreater/auktionshuset:auktionapi