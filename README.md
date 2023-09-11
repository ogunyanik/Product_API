# Product Api

Project Description:  

## Table of Contents

- [Technologies Used](#technologies-used)
- [API Documentation](#api-documentation)
- [Features](#features) 
- [Usage](#usage)
- [Testing](#testing) 

## Technologies Used

This project leverages the following technologies and tools:

- ASP.NET Core 7
- FluentValidation for Validation
- Ratelimitter
- Entity Framework Core (Code-First Approach)
- Polly for external API calls
- Exception Handling Middleware
- Logging Middleware
- API Versioning
- (Kibana & ElasticSearch) for Logging and Serilog 
- In-Memory Cache for API calls
- JWT Authentication (Basic Implementation)
- Unit Testing (Partial Coverage)
- Integration Testing (Partial Coverage)
- Dockerization (Docker Compose)
- AutoMapper
- SwaggerUI
- PostgreSQL

## API Documentation

Explore the API using SwaggerUI: [API Documentation](api/documents/index.html)


## Features

- Provides CRUD operations for managing products.


## Usage

To run the project using Docker Compose, use the following command:

**docker-compose up -d --build**

## Testing

**To get Jwt Token**
```
curl -X POST -H "Content-Type: application/json" -d '{
  "Username": "demo",
  "Password": "password"
}' http://localhost:5000/api/auth/GetToken
```  
**Sample Request**
```
curl --location --request GET 'http://localhost:8080/api/v1/products/1' \
--header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c'
```  

**Healt Check endpoint**
http://localhost:8080/api/base/hc
