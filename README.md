# Facebook-clone

### Tech Stack:
![Spring-Boot](https://img.shields.io/badge/spring--boot-00ff80?style=for-the-badge)
![.NET](https://img.shields.io/badge/asp.net-8000ff?style=for-the-badge)
![Redis](https://img.shields.io/badge/redis-ff0000?style=for-the-badge)
![Postgres](https://img.shields.io/badge/postgres-0080ff?style=for-the-badge)
![Mongodb](https://img.shields.io/badge/mongodb-00ff00?style=for-the-badge)
![Docker](https://img.shields.io/badge/docker-00bfff?style=for-the-badge)


### CLI commands
- **Redis:** ```docker exec -it cache redis-cli```
- **Mongodb:** ```docker exec -it profile-database mongo -u admin -p admin --authenticationDatabase admin```
- **Postgres:** ```docker exec -it feed-database psql -U admin postgres```

### Remove all images and containers
- **Remove all images:** ```docker image rm $(docker images -aq)```
- **Remove all containers:** ```docker rm $(docker ps -aq)```

**Does not work with CMD**