# Project: japanese-practice-js

This document provides context for the AI assistant working on this project.

**Last updated**: 2025-11-10

## Active Technologies

This is the technology stack for the project.

- C# + WPF (.NET 8.0)
- LLamaSharp
- Unit Testing (xUnit)

## Project Structure

A high-level overview of the project's directory structure.

```
src/
├── JapanesePracticeApp.Core/          # Core domain models and interfaces
├── JapanesePracticeApp.Infrastructure/ # Data access and service implementations
└── JapanesePracticeApp.UI/             # WPF user interface
tests/
└── JapanesePracticeApp.UnitTests/      # Unit tests
```

## Common Commands

Key commands for testing and linting.

```bash
# Build the solution
dotnet build

# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --verbosity normal

# Clean and rebuild
dotnet clean && dotnet build
```

## Coding Conventions

Follow these language-specific conventions.

- C#: Follow standard conventions
- Use async/await for I/O operations
- Constructor injection for dependencies
- Interfaces in Core project, implementations in Infrastructure
- ViewModels follow MVVM pattern with INotifyPropertyChanged

## Recent Changes

A log of recent features and the technologies they introduced.

- 2025-01-10: Fixed VocabularyItem model properties to match existing usage (ChineseText, JapaneseAnswer, AlternativeAnswers, Hint)
- 2025-01-10: Added missing VocabularyItem.cs model to Core project
- 2025-01-10: Resolved build errors by updating property references throughout codebase
- previous: Added C# + WPF application structure
