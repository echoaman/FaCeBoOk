# Facebook-clone

## Tech Stack:
![Spring Boot](https://img.shields.io/badge/-SPRING%20BOOT-6DB33F?logo=spring&logoColor=white&style=for-the-badge)
![ASP.NET Core](https://img.shields.io/badge/-ASP.NET%20CORE-512BD4?logo=.net&logoColor=white&style=for-the-badge)
![Redis](https://img.shields.io/badge/-REDIS-FF0000?logo=redis&logoColor=white&style=for-the-badge)
![PostgreSQL](https://img.shields.io/badge/-POSTGRESQL-336791?logo=postgresql&logoColor=white&style=for-the-badge)
![MongoDB](https://img.shields.io/badge/-MONGODB-47A248?logo=mongodb&logoColor=white&style=for-the-badge)
![Docker](https://img.shields.io/badge/-DOCKER-2496ED?logo=docker&logoColor=white&style=for-the-badge)


### CLI commands
- **Redis:** ```docker exec -it cache redis-cli```
- **Mongodb:** ```docker exec -it profile-database mongo -u admin -p admin --authenticationDatabase admin```
- **Postgres:** ```docker exec -it feed-database psql -U admin postgres```

### Remove all images and containers
- **Remove all images:** ```docker image rm $(docker images -aq)```
- **Remove all containers:** ```docker rm $(docker ps -aq)```

**Above does not work with CMD**

## Models
**User**
```
uid : string
name : string
email : string
password : string
friends : List<string>
```

## Profile Service APIs

### `GET /users`

**Request**
```
null
```

**Responses**
```
Status code 200 - List<User>
Status code 404 - null
```

### `GET /users/{uid}`

**Request**
```
uid : string
```

**Responses**
```
Status code 200 - User
Status code 404 - null
```

### `PUT /updateUser`

**Request**
```json
{
    "uid" : "string",
    "name" : "string",
    "password" : "string"
}
```

**Responses**
```
Status code 204
Status code 400
```

### `GET /search?[name]`

**Request - Query string**
```
name : string
```

**Responses**
```
Status code 200 - List<User>
Status code 404 - null
```

### `GET /friends/{uid}`

**Request**
```
uid : string
```

**Responses**
```
Status code 200 - List<string>
Status code 404 - null
```

### `PUT /addfriend?[uid]&[newFriendId]`

**Request - Query strings**
```
uid : string
newFriendId : string
```

**Responses**
```
Status code 204
Status code 400
```

### `GET /login?[email]&[password]`

**Request - Query strings**
```
email : string
password : string
```

**Responses**
```
Status code 200 - User
Status code 404 - null
```

### `POST /signup`

**Request**
```json
{
    "name" : "string",
    "email" : "string",
    "password" : "string"
}
```

**Responses**
```
Status code 201
Status code 400
```