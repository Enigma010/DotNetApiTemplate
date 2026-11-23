#!/bin/sh
cd Solutions/Ddd.App
docker compose -f docker-compose.test.yml build
docker compose -f docker-compose.test.yml run --rm api-unit-test
cd ../..
./lCodeEvaluationDisp.sh