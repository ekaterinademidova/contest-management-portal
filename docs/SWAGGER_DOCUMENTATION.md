# Swagger API Documentation Guide

## Overview

The Contest Management Portal API includes comprehensive Swagger/OpenAPI documentation with examples, request/response schemas, and status code descriptions.

## Accessing Swagger UI

Once the application is running, access Swagger UI at:
- **Root URL**: `http://localhost:5000` or `https://localhost:5001`
- **Swagger JSON**: `http://localhost:5000/swagger/v1/swagger.json`

## Features

### 1. Complete API Documentation
- All endpoints are documented with descriptions
- Request/response examples for each endpoint
- Parameter descriptions and constraints
- Status code documentation

### 2. Interactive Testing
- Try out endpoints directly from Swagger UI
- View request/response examples
- Test with different parameters

### 3. Schema Documentation
- All DTOs are documented with examples
- Property descriptions and constraints
- Data type information

## API Endpoints

### Events Controller
- `GET /api/Events` - Get all events (with optional filtering)
- `GET /api/Events/{id}` - Get event by ID
- `POST /api/Events` - Create new event
- `PUT /api/Events/{id}` - Update event
- `DELETE /api/Events/{id}` - Delete event

### Event Stages Controller
- `GET /api/EventStages` - Get all event stages (with optional filtering)
- `GET /api/EventStages/{id}` - Get event stage by ID
- `POST /api/EventStages` - Create new event stage
- `PUT /api/EventStages/{id}` - Update event stage
- `DELETE /api/EventStages/{id}` - Delete event stage

### Event Stage Criterias Controller
- `GET /api/EventStageCriterias` - Get all criteria
- `GET /api/EventStageCriterias/{id}` - Get criteria by ID
- `GET /api/EventStageCriterias/event-stage/{eventStageId}` - Get criteria by event stage
- `POST /api/EventStageCriterias` - Create new criteria
- `PUT /api/EventStageCriterias/{id}` - Update criteria
- `DELETE /api/EventStageCriterias/{id}` - Delete criteria

### Contest Notices Controller
- `GET /api/ContestNotices` - Get all contest notices (with optional filtering)
- `GET /api/ContestNotices/{id}` - Get contest notice by ID
- `POST /api/ContestNotices` - Create new contest notice
- `PUT /api/ContestNotices/{id}` - Update contest notice
- `DELETE /api/ContestNotices/{id}` - Delete contest notice

### Attachments Controller
- `GET /api/Attachments` - Get all attachments
- `GET /api/Attachments/{id}` - Get attachment by ID
- `POST /api/Attachments` - Create new attachment
- `PUT /api/Attachments/{id}` - Update attachment
- `DELETE /api/Attachments/{id}` - Delete attachment

### Contest Docs Packages Controller
- `GET /api/ContestDocsPackages` - Get all packages
- `GET /api/ContestDocsPackages/{id}` - Get package by ID
- `GET /api/ContestDocsPackages/contest-notice/{contestNoticeId}` - Get packages by contest notice
- `POST /api/ContestDocsPackages` - Create new package
- `PUT /api/ContestDocsPackages/{id}` - Update package
- `DELETE /api/ContestDocsPackages/{id}` - Delete package

## Request Examples

### Create Event
```json
POST /api/Events
{
  "name": "New Programming Contest",
  "description": "A new exciting programming competition"
}
```

### Update Event
```json
PUT /api/Events/1
{
  "id": 1,
  "name": "Updated Event Name",
  "description": "Updated description"
}
```

### Filter Events
```
GET /api/Events?name=Programming
GET /api/Events?description=competition
```

## Response Examples

### Success Response (200 OK)
```json
{
  "id": 1,
  "name": "Annual Programming Championship 2024",
  "description": "A prestigious programming competition..."
}
```

### Created Response (201 Created)
```json
{
  "id": 4,
  "name": "New Programming Contest",
  "description": "A new exciting programming competition"
}
```

### Error Response (400 Bad Request)
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

### Error Response (404 Not Found)
```
"Event with ID 999 not found."
```

## Status Codes

See [API_STATUS_CODES.md](./API_STATUS_CODES.md) for detailed status code documentation.

## Testing in Swagger UI

1. **Navigate to Swagger UI** at the root URL
2. **Expand an endpoint** to see details
3. **Click "Try it out"** to enable editing
4. **Fill in parameters** (if any)
5. **Click "Execute"** to send the request
6. **View response** with status code and body

## Tips

- Use the filter/search box to find specific endpoints
- Check the "Model" section to see request/response schemas
- Review examples in the documentation for proper request formats
- All endpoints support cancellation tokens (optional)

