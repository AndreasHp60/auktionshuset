docker build -t auktionsapi-image -f Dockerfile .
docker tag auktionssapi-image marththecreater/auktionshuset:auktionapi
docker push marththecreater/auktionshuset:auktionapi