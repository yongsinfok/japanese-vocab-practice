# Research Findings

This document summarizes the research conducted to resolve the "NEEDS CLARIFICATION" items from the `plan.md`.

## 1. Local LLM Selection

### Decision
We will use a model from the **Mistral** family (e.g., Mistral 7B in GGUF format).

### Rationale
- **Performance and Size:** Mistral models, particularly the 7B version, offer a strong balance of performance and resource requirements, making them suitable for running locally on user machines.
- **Availability:** They are widely available on platforms like Hugging Face in the GGUF format, which is directly compatible with our chosen integration library.
- **Flexibility:** While we will start with a base model, the option to fine-tune it for more specific question-generation tasks in the future remains available.

### Alternatives Considered
- **Llama 3:** A powerful model, but even its smaller versions can be more resource-intensive than Mistral 7B.
- **Phi-3:** A capable small language model, but often requires using the ONNX Runtime, adding a potential model conversion step.

## 2. LLM Integration with C#

### Decision
We will use the **LLamaSharp** library for integrating the local LLM into the C# WPF application.

### Rationale
- **Direct Integration:** LLamaSharp is a .NET wrapper for the highly optimized `llama.cpp` library, allowing for direct, in-process execution of the LLM. This avoids the complexity of managing a separate server process.
- **Cross-Platform:** It is cross-platform and supports both CPU and GPU inference, providing flexibility for different user hardware.
- **Community and Support:** LLamaSharp is a popular and actively maintained open-source project with a strong community.

### Alternatives Considered
- **Ollama:** Requires users to run a separate Ollama server, adding an external dependency and setup step for the end-user.
- **ONNX Runtime:** A valid approach but requires models to be in the ONNX format. Sticking with LLamaSharp simplifies the model pipeline by using the common GGUF format directly.

## 3. WPF Project Structure

### Decision
We will adopt a layered **Model-View-ViewModel (MVVM)** architecture separated into three distinct projects:
1.  `.Core`: A .NET class library for Models and core business logic (LLM interaction logic).
2.  `.Infrastructure`: A .NET class library for services (e.g., file persistence, settings management).
3.  `.UI`: The main WPF project containing Views, ViewModels, and other UI-specific components.

### Rationale
- **Separation of Concerns:** This structure strictly separates business logic from the user interface, which is a core principle of clean software architecture.
- **Testability:** Decoupling the core logic into its own project makes it significantly easier to write unit tests without any UI dependencies.
- **Scalability and Maintainability:** This layered approach is highly scalable and makes the codebase easier to navigate and maintain as the application grows.

### Alternatives Considered
- **Single Project Structure:** While simpler for very small applications, it quickly becomes unmanageable and leads to tightly coupled code.
- **Feature-Based Structure:** A good pattern for very large applications, but the layered approach provides a better foundational separation for a new project of this scope.
