# Facebook-clone

### Tech Stack:
- **Docker**
- **Redis (cache)**
- **Postgres**
- **Mongodb**

- **Redis CLI:** ```docker exec -it cache redis-cli```
- **Mongodb CLI:** ```docker exec -it profile-database mongo```
- **Postgres CLI:** ```docker exec -it feed-database psql -U admin postgres```

### Remove all images and containers
- **Remove all images:** ```docker image rm $(docker images -aq)```
- **Remove all containers:** ```docker rm $(docker ps -aq)```

**Does not work with CMD**