# Japanese Practice App

A WPF desktop application for practicing Japanese vocabulary with an interactive study interface.

## Features

- **Vocabulary Practice**: Study Japanese vocabulary with Chinese prompts
- **Multiple Weeks Support**: Select different weeks of vocabulary to practice
- **Difficulty Level Filtering**: Filter questions by Beginner, Intermediate, or Advanced levels
- **Dataset Manager GUI**: Add, edit, and delete vocabulary items without editing JSON files directly
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

   **Option A: Use the Dataset Manager GUI (Recommended)**
   - Run the application and navigate to **Tools > Manage Datasets**
   - Create new weeks and add vocabulary items through the intuitive interface
   - No need to manually edit JSON files!

   **Option B: Manual JSON Setup**
   - Create a `Data` directory under src\JapanesePracticeApp.UI\
   - Add vocabulary JSON files named `week1.json`, `week2.json`, etc.
   - Each file should contain vocabulary items in the format:
     ```json
     [
       {
         "ChineseText": "你好",
         "JapaneseAnswer": "こんにちは",
         "AlternativeAnswers": [],
         "DifficultyLevel": "Beginner",
         "Hint": "Common greeting",
         "WeekNumber": 1
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

### Practice Mode

1. **Launch the application** - The vocabulary data will load automatically
2. **Select a week** - Choose which week's vocabulary to practice
3. **Select difficulty** - Filter by Beginner, Intermediate, Advanced, or All
4. **Type your answer** - Enter the Japanese translation
5. **Press Enter** - Check your answer and see the correct response
6. **Repeat** - Continue practicing with new vocabulary items

### Managing Vocabulary Data

1. **Open Dataset Manager** - Go to Tools > Manage Datasets
2. **Select or create a week** - Choose an existing week or create a new one
3. **Add vocabulary items** - Fill in the form with:
   - Chinese Text (required)
   - Japanese Answer (required)
   - Alternative Answers (optional, comma-separated)
   - Difficulty Level (Beginner/Intermediate/Advanced)
   - Hint (optional)
4. **Edit items** - Click on any item in the list to load it into the editor
5. **Delete items** - Select an item and click "Delete Selected Item"
6. **Save changes** - All changes are saved automatically

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

- **`VocabularyService`**: Loads and manages vocabulary data from JSON files with filtering support
- **`MainViewModel`**: Main window's view model with command bindings and difficulty filtering
- **`DatasetManagerViewModel`**: Dataset manager's view model for CRUD operations
- **`VocabularyItem`**: Model representing a single vocabulary entry with difficulty levels and week numbers

## License

This project is provided as a learning example for Japanese language practice applications.