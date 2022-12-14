docker build -t auktionsapi-image -f Dockerfile .
docker tag auktionsapi-image andreashp60/auktionshus:auktionsapi
docker push andreashp60/auktionshus:auktionsapi