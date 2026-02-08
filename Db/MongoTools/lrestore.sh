#!/bin/bash
set -e
if [[ -z "$1" ]]; then
  echo "Usage: $0 directory to restore from" >&2
  exit 1
fi
source ../../Api/.env
sourceDir=$(pwd)/../../Output/Db/backup
docker rm -f mongo-tools-exec || true
retoreDir=/backup/$1
if [[ ! -d "$sourceDir/$1" ]]; then
  echo "Error: directory does not exist: $DIR" >&2
  exit 1
fi
docker run -v ${sourceDir}:/backup --name mongo-tools-exec mongo-tools:latest mongorestore --host=host.docker.internal --port=10260 --username=${APP_DB_USERNAME} --password=${APP_DB_PASSWORD} --authenticationDatabase=admin --ssl --tlsInsecure ${retoreDir}