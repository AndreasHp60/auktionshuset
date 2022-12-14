docker build -t product-image -f Dockerfile .
docker tag product-image andreashp60/auktionshus:product
docker push andreashp60/auktionshus:product