docker rm -f DotNetEventBus || true
docker run -d --name DotNetEventBus -p 5672:5672 -p 15672:15672 rabbitmq:3-management
