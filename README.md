# Facebook-clone

### Tech Stack
![Next.js](https://img.shields.io/badge/-NEXT.JS-000000?logo=next-dot-js&logoColor=white&style=for-the-badge)
![ASP.NET Core](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=dot-net&logoColor=white)
![Redis](https://img.shields.io/badge/redis-CC0000.svg?&style=for-the-badge&logo=redis&logoColor=white)
![Postgres](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![Mongodb](https://img.shields.io/badge/MongoDB-4EA94B?style=for-the-badge&logo=mongodb&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2CA5E0?style=for-the-badge&logo=docker&logoColor=white)


### CLI commands
- **Redis:** ```docker exec -it cache redis-cli```
- **Mongodb:** ```docker exec -it profile-database mongo -u admin -p admin --authenticationDatabase admin```
- **Postgres:** ```docker exec -it feed-database psql -U admin postgres```

### Remove all images and containers
- **Remove all images:** ```docker image rm $(docker images -aq)```
- **Remove all containers:** ```docker rm $(docker ps -aq)```

**Does not work with CMD**