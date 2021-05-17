# Profile Service

## Tech Stack
![ASP.NET Core](https://img.shields.io/badge/-ASP.NET%20CORE-512BD4?logo=.net&logoColor=white&style=for-the-badge)
![Redis](https://img.shields.io/badge/-REDIS-DC382D?logo=redis&logoColor=white&style=for-the-badge)
![MongoDB](https://img.shields.io/badge/-MONGODB-47A248?logo=mongodb&logoColor=white&style=for-the-badge)

### Run in local environment
```dotnet run```

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

**Info**
```
Returns a list of all users
```

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

**Info**
```
Returns a single user
```

**Request**
```
uid : string
```

**Responses**
```
Status code 200 - User
Status code 404 - null
```

### `PUT /users`

**Info**
```
Updates a user's name and password
```

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

**Info**
```
Returns a list of users based on name searched
```

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

**Info**
```
Returns all friends (IDs) of a user
```

**Request**
```
uid : string
```

**Responses**
```
Status code 200 - List<string>
Status code 404 - null
```

### `PUT /friends?[uid]&[newFriendId]`

**Info**
```
Adds a new friend to a user
```

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

**Info**
```
Login a user
```

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

**Info**
```
Create new account
```

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