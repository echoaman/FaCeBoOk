# Facebook-clone

### Tech Stack:
- **Docker**
- **Redis (cache)**
- **Postgres**
- **Mongodb**


# CLI commands
- **Redis:** ```docker exec -it cache redis-cli```
- **Mongodb:** ```docker exec -it profile-database mongo```
- **Postgres:** ```docker exec -it feed-database psql -U admin postgres```

### Remove all images and containers
- **Remove all images:** ```docker image rm $(docker images -aq)```
- **Remove all containers:** ```docker rm $(docker ps -aq)```

**Does not work with CMD**