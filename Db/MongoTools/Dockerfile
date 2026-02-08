FROM ubuntu:22.04

# Avoid prompts during install
ENV DEBIAN_FRONTEND=noninteractive

# Install dependencies
RUN apt-get update && apt-get install -y \
    curl \
    gnupg \
    ca-certificates \
    && rm -rf /var/lib/apt/lists/*

# Add MongoDB public GPG key
RUN curl -fsSL https://pgp.mongodb.com/server-6.0.asc | \
    gpg --dearmor -o /usr/share/keyrings/mongodb-server.gpg

# Add MongoDB repository
RUN echo "deb [signed-by=/usr/share/keyrings/mongodb-server.gpg] \
    https://repo.mongodb.org/apt/ubuntu jammy/mongodb-org/6.0 multiverse" \
    > /etc/apt/sources.list.d/mongodb-org.list

# Install MongoDB Database Tools (includes mongodump)
RUN apt-get update && apt-get install -y \
    mongodb-database-tools \
    && rm -rf /var/lib/apt/lists/*

# Verify install (optional but nice)
RUN mongodump --version

VOLUME [ "/backup" ]

CMD ["mongodump"]
