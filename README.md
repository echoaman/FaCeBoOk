# Facebook-clone

### Tech Stack:
![Spring-Boot](https://img.shields.io/static/v1?label&message=spring-boot&color=2eb82e&style=for-the-badge)
![.NET](https://img.shields.io/static/v1?label&message=asp.net&color=8000ff&style=for-the-badge)
![Redis](https://img.shields.io/static/v1?label&message=redis&color=ff0000&style=for-the-badge)
![Postgres](https://img.shields.io/static/v1?label&message=postgres&color=0080ff&style=for-the-badge)
![Mongodb](https://img.shields.io/static/v1?label&message=mongodb&color=success&style=for-the-badge)
![Docker](https://img.shields.io/static/v1?label&message=docker&color=00bfff&style=for-the-badge)


### CLI commands
- **Redis:** ```docker exec -it cache redis-cli```
- **Mongodb:** ```docker exec -it profile-database mongo -u admin -p admin --authenticationDatabase admin```
- **Postgres:** ```docker exec -it feed-database psql -U admin postgres```

### Remove all images and containers
- **Remove all images:** ```docker image rm $(docker images -aq)```
- **Remove all containers:** ```docker rm $(docker ps -aq)```

**Does not work with CMD**