# Facebook-clone

## Tech Stack
![Next.js](https://img.shields.io/badge/-NEXT.JS-000000?logo=next-dot-js&logoColor=white&style=for-the-badge)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-5C2D91?style=for-the-badge&logo=dot-net&logoColor=white)
![Spring Boot](https://img.shields.io/badge/-SPRING%20BOOT-6DB33F?logo=spring&logoColor=white&style=for-the-badge)
![Redis](https://img.shields.io/badge/-REDIS-DC382D?logo=redis&logoColor=white&style=for-the-badge)
![PostgreSQL](https://img.shields.io/badge/-POSTGRESQL-336791?logo=postgresql&logoColor=white&style=for-the-badge)
![MongoDB](https://img.shields.io/badge/-MONGODB-47A248?logo=mongodb&logoColor=white&style=for-the-badge)
![Docker](https://img.shields.io/badge/-DOCKER-2496ED?logo=docker&logoColor=white&style=for-the-badge)

### CLI commands
- **Redis:** ```docker exec -it cache redis-cli -a <redis-password>```
- **Mongodb:** ```docker exec -it profile-database mongo -u <mongo-username> -p <mongo-password> --authenticationDatabase admin```
- **Postgres:** ```docker exec -it feed-database psql -U <postgres-username> postgres```

### Docker Commands
- **Remove all images:** ```docker image rm $(docker images -aq)```
- **Remove all containers:** ```docker rm $(docker ps -aq)```
- **Run all containers in dev:** ```docker compose --env-file .env.dev up```
- **Run all containers in prod:** ```docker compose -f docker-compose.yml -f docker-compose.prod.yml --env-file .env.prod up```

## Environment Variables and Configuration Settings
### .env
- Has configurations for ```docker-compose.yml```
- Create ```.env.dev```, ```.env.prod```, etc based on ```.env.example```
- Configure all properties

### appsettings.json
- Has configurations for ```profile-service (ASP.NET Core Webapi)```
- Create ```appsettings.Development.json```, ```appsettings.Production.json```, etc based on ```appsettings.example.json```
- Configure **CacheSettings:ConnectionString**, **DatabaseSettings:CollectionName**, **DatabaseSettings:ConnectionString** and **DatabaseSettings:DatabaseName**

### application.properties
- Has configurations for ```feed-service (Spring Boot)```
- Create ```application-dev.properties```, ```application-prod.properties```, etc based on ```application-example.properties```
- Configure **spring.datasource.url**, **spring.datasource.username**, **spring.datasource.password**, **spring.redis.host** and **spring.redis.password**

**All files must be created in the same directory as their example file**