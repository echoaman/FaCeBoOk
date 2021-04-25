# Facebook-clone

### Tech Stack:
![Spring-Boot](https://img.shields.io/static/v1?label&message=spring-boot&color=6DB33F&style=for-the-badge)
![ASP.NET](https://img.shields.io/static/v1?label&message=asp.net&color=5C2D91&style=for-the-badge)
![Redis](https://img.shields.io/static/v1?label&message=redis&color=ff0000&style=for-the-badge)
![PostgreSQL](https://img.shields.io/static/v1?label&message=postgresql&color=316192&style=for-the-badge)
![MongoDB](https://img.shields.io/static/v1?label&message=mongodb&color=4EA94B&style=for-the-badge)
![Docker](https://img.shields.io/static/v1?label&message=docker&color=2CA5E0&style=for-the-badge)


### CLI commands
- **Redis:** ```docker exec -it cache redis-cli```
- **Mongodb:** ```docker exec -it profile-database mongo -u admin -p admin --authenticationDatabase admin```
- **Postgres:** ```docker exec -it feed-database psql -U admin postgres```

### Remove all images and containers
- **Remove all images:** ```docker image rm $(docker images -aq)```
- **Remove all containers:** ```docker rm $(docker ps -aq)```

**Does not work with CMD**