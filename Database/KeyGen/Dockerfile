FROM alpine:3.19

RUN apk add --no-cache openssl && \
    openssl rand -base64 756 > /mongo-keyfile && \
    chmod 600 /mongo-keyfile

CMD ["cat", "/mongo-keyfile"]
