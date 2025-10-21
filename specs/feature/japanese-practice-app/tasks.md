# Tasks: Japanese Practice App

This document outlines the implementation tasks for the Japanese Practice App.

## Implementation Strategy

The implementation will follow an incremental approach, starting with a Minimum Viable Product (MVP) that focuses on User Story 1. The initial setup will establish the project structure, followed by the implementation of the core question generation and display feature.

## Phase 1: Setup

- [x] T001 Initialize C# WPF project `JapanesePracticeApp.csproj`
- [x] T002 Create the MVVM directory structure: `Models/`, `Views/`, `ViewModels/`, `Services/`
- [x] T003 Create the main application entry point in `App.xaml` and `App.xaml.cs`
- [x] T004 Create the main window view in `Views/MainWindow.xaml`

## Phase 2: Foundational

- [x] T005 Define the data model for a grammar question in `Models/GrammarQuestion.cs`
- [x] T006 Define the data model for user progress in `Models/UserProgress.cs`
- [x] T007 Create an interface for the LLM service `Services/ILlmService.cs` to decouple the core logic from a specific LLM implementation.
- [x] T008 Implement a service for persisting data to JSON files in `Services/PersistenceService.cs`

## Phase 3: User Story 1 - Auto-generate and display grammar questions

**Goal**: The application generates a fill-in-the-blank grammar question using a local LLM and displays it to the user.
**Independent Test Criteria**: The main window launches and displays a question.

- [x] T009 [US1] Create the main window's view model in `ViewModels/MainViewModel.cs`
- [x] T010 [P] [US1] Design the UI in `Views/MainWindow.xaml` to display a question, an input field, and a submit button.
- [x] T011 [US1] Implement the question generation logic in `Services/QuestionGenerationService.cs`, using the `ILlmService` interface.
- [x] T012 [US1] Implement logic in `ViewModels/MainViewModel.cs` to use the `QuestionGenerationService` to fetch and display a question.
- [x] T013 [US1] Bind the `Views/MainWindow.xaml` to the `ViewModels/MainViewModel.cs`.
- [x] T014 [US1] Write unit tests for `ViewModels/MainViewModel.cs` to verify question loading and display logic.
- [x] T015 [US1] Write unit tests for `Services/QuestionGenerationService.cs` to verify its interaction with the LLM service interface.

## Phase 4: Polish & Cross-Cutting Concerns

- [x] T016 Add basic error handling for file I/O and service calls.
- [x] T017 Add initial comments and XML documentation to public methods and classes.

## Dependencies

- **US1** depends on the completion of the **Foundational** phase.

## Parallel Execution Opportunities

- **T010 (UI Design)** can be done in parallel with **T011 (Service Logic)** once the foundational data models and interfaces are in place.
