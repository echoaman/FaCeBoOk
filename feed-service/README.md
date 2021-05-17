# Feed Service

## Tech Stack:
![Spring Boot](https://img.shields.io/badge/-SPRING%20BOOT-6DB33F?logo=spring&logoColor=white&style=for-the-badge)
![Redis](https://img.shields.io/badge/-REDIS-FF0000?logo=redis&logoColor=white&style=for-the-badge)
![PostgreSQL](https://img.shields.io/badge/-POSTGRESQL-336791?logo=postgresql&logoColor=white&style=for-the-badge)

### Run in local environment
```./mvnw spring-boot:run -Dspring-boot.run.arguments="--spring.profiles.active=dev"```

## Models

**Post**
```
postid : integer
uid : string
content : string
postedOn : string (dd-MM-yyyy HH:mm:ss)
```

## Feed Service APIs

### `GET /posts`

**Request**
```
null
```

**Responses**
```
Status code 200 - List<Post>
Status code 404 - null
```

### `GET /posts/user/{uid}`

**Request**
```
uid : string
```

**Responses**
```
Status code 200 - List<Post>
Status code 404 - null
```

### `POST /posts`

**Request**
```json
{
    "uid" : "string",
    "content" : "string"
}
```

**Responses**
```
Status code - 201
Status code - 400
```

### `GET /feed/{uid}`

**Request**
```
uid : string
```

**Responses**
```
Status code 200 - List<Post>
Status code 404 - null
```