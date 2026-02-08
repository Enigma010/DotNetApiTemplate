#!/bin/bash
set -e
source ../../Api/.env
sourceDir=$(pwd)/../../Output/Db/backup
docker rm -f mongo-tools-exec || true
timestamp=$(date +"%Y%m%d_%H%M%S")
backupDir=/backup/${timestamp}
echo "Backing up MongoDB to ${sourceDir}${timestamp}"
docker run -v ${sourceDir}:/backup --name mongo-tools-exec mongo-tools:latest  mongodump --host=host.docker.internal --port=10260 --db=${APP_DOMAIN}-${APP_SUBDOMAIN} --username=${APP_DB_USERNAME} --password=${APP_DB_PASSWORD} --authenticationDatabase=admin --ssl --tlsInsecure --readPreference=primary --out=${backupDir}