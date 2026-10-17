#!/bin/sh
docker build -t generate-cert .
docker run --rm -v "$(pwd)/../../Output/Certs":/certs generate-cert
