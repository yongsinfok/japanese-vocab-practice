# Data Model

This document defines the JSON structure for the data used in the Japanese Practice App. All data will be stored in local JSON files.

## 1. Question Bank (`questions.json`)

This file will store the list of generated grammar questions.

### Schema
```json
[
  {
    "id": "string (UUID)",
    "questionText": "string",
    "correctAnswer": "string",
    "options": ["string"],
    "difficulty": "string (e.g., N5, N4)",
    "createdAt": "string (ISO 8601 datetime)"
  }
]
```

### Example
```json
[
  {
    "id": "a1b2c3d4-e5f6-7890-1234-567890abcdef",
    "questionText": "猫はテーブルの＿＿にいます。",
    "correctAnswer": "下",
    "options": ["上", "中", "外"],
    "difficulty": "N5",
    "createdAt": "2025-10-21T10:00:00Z"
  }
]
```

## 2. User Settings (`settings.json`)

This file will store user preferences and application settings.

### Schema
```json
{
  "selectedDifficulty": "string (e.g., N5, All)",
  "numberOfQuestions": "integer",
  "theme": "string (e.g., Light, Dark)"
}
```

### Example
```json
{
  "selectedDifficulty": "N5",
  "numberOfQuestions": 20,
  "theme": "Light"
}
```
