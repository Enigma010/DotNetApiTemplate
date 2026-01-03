#!/bin/sh
rm -Rf coveragereport
docker build -t apptests .
cd ..
volume=$(pwd)
docker run -it -v $volume:/app --env-file Api/.env apptests /app/App.UnitTests/druntests.sh