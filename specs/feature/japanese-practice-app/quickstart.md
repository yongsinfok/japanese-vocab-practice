# Quickstart Guide

This guide provides instructions for setting up and running the Japanese Practice App development environment.

## Prerequisites

- **.NET 8 SDK (or later):** [Download .NET](https://dotnet.microsoft.com/download)
- **Visual Studio 2022 (or later):** Required for WPF development. Make sure to install the ".NET desktop development" workload.
- **Git:** For cloning the repository.

## Setup

### 1. Clone the Repository
```bash
git clone <repository-url>
cd japanese-practice-js
```

### 2. Download the LLM Model
The application requires a local language model to function.

- **Model:** Download a GGUF-compatible model. We recommend starting with **Mistral 7B**. You can find it on [Hugging Face](https://huggingface.co/TheBloke/Mistral-7B-Instruct-v0.2-GGUF).
- **Location:** Create a `models` directory in the root of the cloned repository.
- **Save:** Place the downloaded `.gguf` file inside the `models` directory.

The final structure should look like this:
```
/
|-- models/
|   |-- mistral-7b-instruct-v0.2.Q4_K_M.gguf
|-- src/
|-- japanese-practice-js.sln
|-- ...
```

### 3. Restore Dependencies
Open a terminal in the root of the repository and run the following command to restore the required NuGet packages:
```bash
dotnet restore
```

## Running the Application

### Using Visual Studio
1.  Open the `.sln` file in Visual Studio.
2.  Set the `.UI` project as the startup project.
3.  Press the "Start" button (or F5) to build and run the application.

### Using the Command Line
```bash
dotnet run --project src/JapanesePracticeApp.UI
```
*(Note: The project path may vary based on the final solution structure.)*
