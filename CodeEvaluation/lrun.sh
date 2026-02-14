#!/bin/sh
rm -Rf coveragereport
docker build -t apptests .
cd ..
volume=$(pwd)
docker rm -f apptests 2>/dev/null || true
docker run -it -v $volume:/app --env-file Solutions/App/.env --name apptests apptests /app/CodeEvaluation/druntests.sh