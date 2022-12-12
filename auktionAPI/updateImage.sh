docker build -t auktionsapi-image -f Dockerfile .
docker tag auktionapi-image andreashp60/auktionshus:auktionapi
docker push andreashp60/auktionshus:auktionapi