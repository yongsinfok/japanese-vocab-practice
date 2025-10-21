# Tasks: Japanese Practice App

This document outlines the implementation tasks for the Japanese Practice App.

## Implementation Strategy

The implementation will follow an incremental approach, starting with a Minimum Viable Product (MVP) that focuses on User Story 1. The initial setup will establish the project structure, followed by the implementation of the core question generation and display feature.

## Phase 1: Setup

- [ ] T001 Initialize C# WPF project `JapanesePracticeApp.csproj`
- [ ] T002 Create the MVVM directory structure: `Models/`, `Views/`, `ViewModels/`, `Services/`
- [ ] T003 Create the main application entry point in `App.xaml` and `App.xaml.cs`
- [ ] T004 Create the main window view in `Views/MainWindow.xaml`

## Phase 2: Foundational

- [ ] T005 Define the data model for a grammar question in `Models/GrammarQuestion.cs`
- [ ] T006 Define the data model for user progress in `Models/UserProgress.cs`
- [ ] T007 Create an interface for the LLM service `Services/ILlmService.cs` to decouple the core logic from a specific LLM implementation.
- [ ] T008 Implement a service for persisting data to JSON files in `Services/PersistenceService.cs`

## Phase 3: User Story 1 - Auto-generate and display grammar questions

**Goal**: The application generates a fill-in-the-blank grammar question using a local LLM and displays it to the user.
**Independent Test Criteria**: The main window launches and displays a question.

- [ ] T009 [US1] Create the main window's view model in `ViewModels/MainViewModel.cs`
- [ ] T010 [P] [US1] Design the UI in `Views/MainWindow.xaml` to display a question, an input field, and a submit button.
- [ ] T011 [US1] Implement the question generation logic in `Services/QuestionGenerationService.cs`, using the `ILlmService` interface.
- [ ] T012 [US1] Implement logic in `ViewModels/MainViewModel.cs` to use the `QuestionGenerationService` to fetch and display a question.
- [ ] T013 [US1] Bind the `Views/MainWindow.xaml` to the `ViewModels/MainViewModel.cs`.

## Phase 4: Polish & Cross-Cutting Concerns

- [ ] T014 Add basic error handling for file I/O and service calls.
- [ ] T015 Add initial comments and XML documentation to public methods and classes.

## Dependencies

- **US1** depends on the completion of the **Foundational** phase.

## Parallel Execution Opportunities

- **T010 (UI Design)** can be done in parallel with **T011 (Service Logic)** once the foundational data models and interfaces are in place.
