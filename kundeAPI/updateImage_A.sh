docker build -t customer-image -f Dockerfile .
docker tag customer-image andreashp60/auktionshus:customer
docker push andreashp60/auktionshus:customer