# Japanese Practice App

A WPF desktop application for practicing Japanese grammar with AI-generated questions using a local language model.

## Features

- **AI-Powered Question Generation**: Uses local LLM (via LLamaSharp) to generate contextual grammar questions
- **Fill-in-the-Blank Format**: Focuses on Japanese particles and grammar patterns
- **MVVM Architecture**: Clean separation of concerns with proper testability
- **Local Data Storage**: Saves progress and questions to JSON files
- **Unit Testing**: Comprehensive test coverage for core functionality

## Technology Stack

- **.NET 8** with C#
- **WPF** for the user interface
- **LLamaSharp** for LLM integration
- **System.Text.Json** for data persistence
- **MSTest + Moq** for unit testing

## Quick Start

### Prerequisites

- .NET 8 SDK
- Windows OS (WPF specific)
- A GGUF-compatible language model (e.g., Mistral 7B)

### Setup

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd japanese-practice-js
   ```

2. **Download a language model**
   - Download a GGUF model (recommended: Mistral 7B)
   - Create a `models` directory at the root of the project
   - Place the `.gguf` file in the `models` directory

3. **Build and run**
   ```bash
   dotnet build
   dotnet run --project src/JapanesePracticeApp.UI
   ```

## Architecture

The application follows a layered MVVM architecture:

```
JapanesePracticeApp.Core/
├── Models/              # Data models (GrammarQuestion, UserProgress)
└── Services/            # Core business logic interfaces

JapanesePracticeApp.Infrastructure/
└── Services/            # Implementation details (LLM, persistence)

JapanesePracticeApp.UI/
├── Views/               # WPF windows and controls
├── ViewModels/          # View models and MVVM infrastructure
└── Converters/          # Data binding converters
```

## Usage

1. **Launch the application** - The AI model will load automatically when the window opens
2. **Click "Load Question"** - Generate a new grammar question
3. **Type your answer** - Enter the particle or word that should fill the blank
4. **Click "Submit Answer"** - Check your answer and see the explanation
5. **Repeat** - Continue practicing with new questions

## Development

### Running Tests

```bash
dotnet test
```

### Project Structure

- **Core**: Contains business logic, models, and service interfaces
- **Infrastructure**: Contains concrete implementations (LLM service, file persistence)
- **UI**: Contains WPF views, view models, and UI-specific code

### Key Components

- **`QuestionGenerationService`**: Coordinates LLM interaction to create questions
- **`PersistenceService`**: Handles saving/loading progress and questions
- **`MainViewModel`**: Main window's view model with command bindings
- **`LlamaSharpService`**: Concrete LLM implementation using LLamaSharp

## License

This project is provided as a learning example for Japanese language practice applications.