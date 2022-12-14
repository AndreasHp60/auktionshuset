docker build -t worker-image -f Dockerfile .
docker tag worker-image andreashp60/auktionshus:worker
docker push andreashp60/auktionshus:worker