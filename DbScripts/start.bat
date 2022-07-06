docker container rm -f database
docker build -t database:latest .
docker run --env-file env.list -p 3306:3306 --name database database 