# API Status Codes Reference

This document describes all HTTP status codes used in the Contest Management Portal API.

## Success Status Codes (2xx)

### 200 OK
- **Usage**: Successful GET, PUT requests
- **Response**: Returns the requested resource or updated resource
- **Endpoints**: 
  - `GET /api/{controller}/{id}` - Get by ID
  - `GET /api/{controller}` - Get all
  - `PUT /api/{controller}/{id}` - Update

**Example Response:**
```json
{
  "id": 1,
  "name": "Event Name",
  "description": "Event Description"
}
```

### 201 Created
- **Usage**: Successful POST requests (resource creation)
- **Response**: Returns the newly created resource
- **Location Header**: Contains the URI of the created resource
- **Endpoints**: 
  - `POST /api/{controller}` - Create new resource

**Example Response:**
```json
{
  "id": 5,
  "name": "New Event",
  "description": "New Description"
}
```

### 204 No Content
- **Usage**: Successful DELETE requests
- **Response**: No response body
- **Endpoints**: 
  - `DELETE /api/{controller}/{id}` - Delete resource

## Client Error Status Codes (4xx)

### 400 Bad Request
- **Usage**: Invalid request data, validation errors, or ID mismatch
- **Response**: Error details or validation errors
- **Common Scenarios**:
  - Validation errors (missing required fields, invalid format)
  - ID mismatch between URL and request body
  - Invalid data types

**Example Response:**
```json
{
  "errors": [
    {
      "propertyName": "Name",
      "errorMessage": "Name is required."
    }
  ]
}
```

### 404 Not Found
- **Usage**: Resource not found
- **Response**: Error message indicating the resource was not found
- **Common Scenarios**:
  - Requested ID does not exist
  - Resource was deleted
  - Invalid endpoint path

**Example Response:**
```
"Event with ID 999 not found."
```

## Server Error Status Codes (5xx)

### 500 Internal Server Error
- **Usage**: Unexpected server errors
- **Response**: Generic error message
- **Note**: Should not occur in normal operation. If it does, check server logs.

## Status Code Summary by Operation

| Operation | Success Code | Error Codes |
|-----------|-------------|-------------|
| GET (by ID) | 200 OK | 404 Not Found |
| GET (all) | 200 OK | - |
| POST (create) | 201 Created | 400 Bad Request |
| PUT (update) | 200 OK | 400 Bad Request, 404 Not Found |
| DELETE | 204 No Content | 404 Not Found, 400 Bad Request* |

*400 Bad Request for DELETE: Used when trying to delete a Submission that has an associated Appeal (SubmissionService)

## Common Error Scenarios

### Validation Errors (400 Bad Request)
- Missing required fields
- Field length violations (e.g., Name exceeds 255 characters)
- Invalid data types
- Business rule violations (e.g., DateEnd before DateStart)

### Not Found Errors (404 Not Found)
- Attempting to GET/UPDATE/DELETE a non-existent resource
- Referenced foreign key does not exist

### ID Mismatch (400 Bad Request)
- URL parameter `id` does not match `id` in request body during UPDATE operations

### SubmissionService Specific Errors

#### Cannot Delete Submission with Appeal (400 Bad Request)
- Attempting to delete a Submission that has an associated Appeal
- Must delete the Appeal first, then the Submission

#### Appeal Already Exists (400 Bad Request)
- Attempting to create a second Appeal for a Submission (1:1 relationship enforced)

## Best Practices

1. **Always check the status code** before processing the response body
2. **Handle 400 errors** by displaying validation messages to the user
3. **Handle 404 errors** by informing the user the resource doesn't exist
4. **Log 5xx errors** for debugging and monitoring
5. **Use appropriate HTTP methods** for operations (GET for read, POST for create, PUT for update, DELETE for delete)

