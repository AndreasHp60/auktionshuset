docker build -t auktionsapi-image -f Dockerfile .
docker tag auktionsapi-image marththecreater/auktionshuset:auktionsapi
docker push marththecreater/auktionshuset:auktionsapi