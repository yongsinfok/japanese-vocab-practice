# Japanese Practice App

A WPF desktop application for practicing Japanese vocabulary with an interactive study interface.

## Features

- **Vocabulary Practice**: Study Japanese vocabulary with Chinese prompts
- **Multiple Weeks Support**: Select different weeks of vocabulary to practice
- **Hint System**: Get helpful hints for difficult vocabulary items
- **Alternative Answers**: Supports multiple correct answer variations
- **MVVM Architecture**: Clean separation of concerns with proper testability
- **Local Data Storage**: Vocabulary data stored in JSON files by week

## Technology Stack

- **.NET 8** with C#
- **WPF** for the user interface
- **System.Text.Json** for data persistence
- **MSTest + Moq** for unit testing

## Quick Start

### Prerequisites

- .NET 8 SDK
- Windows OS (WPF specific)

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/yongsinfok/japanese-vocab-practice.git
   cd japanese-practice-js
   ```

2. **Prepare vocabulary data**
   - Create a `Data` directory under src\JapanesePracticeApp.UI\
   - Add vocabulary JSON files named `week1.json`, `week2.json`, etc.
   - Each file should contain vocabulary items in the format:
     ```json
     [
       {
         "ChineseText": "你好",
         "JapaneseAnswer": "こんにちは",
         "AlternativeAnswers": [],
         "Hint": "Common greeting"
       }
     ]
     ```

3. **Build and run**
   ```bash
   dotnet build
   dotnet run --project src/JapanesePracticeApp.UI
   ```

## Architecture

The application follows a layered MVVM architecture:

```
JapanesePracticeApp.Core/
├── Models/              # Data models (VocabularyItem)
└── Services/            # Core business logic interfaces

JapanesePracticeApp.Infrastructure/
└── Services/            # Implementation details (VocabularyService)

JapanesePracticeApp.UI/
├── Views/               # WPF windows and controls
├── ViewModels/          # View models and MVVM infrastructure
└── Converters/          # Data binding converters
```

## Usage

1. **Launch the application** - The vocabulary data will load automatically
2. **Select a week** - Choose which week's vocabulary to practice
3. **Type your answer** - Enter the Japanese translation
4. **Click Enter** - Check your answer and see the correct response
5. **Repeat** - Continue practicing with new vocabulary items

## Development

### Running Tests

```bash
dotnet test
```

### Project Structure

- **Core**: Contains business logic, models, and service interfaces
- **Infrastructure**: Contains concrete implementations (VocabularyService)
- **UI**: Contains WPF views, view models, and UI-specific code

### Key Components

- **`VocabularyService`**: Loads and manages vocabulary data from JSON files
- **`MainViewModel`**: Main window's view model with command bindings
- **`VocabularyItem`**: Model representing a single vocabulary entry

## License

This project is provided as a learning example for Japanese language practice applications.