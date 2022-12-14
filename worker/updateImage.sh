docker build -t worker-image -f Dockerfile .
docker tag worker-image marththecreater/auktionshuset:worker
docker push marththecreater/auktionshuset:worker