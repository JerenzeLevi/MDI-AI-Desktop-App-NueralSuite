# 🧠 MDI AI Desktop Application

> A modular AI-powered desktop application combining chatbot and computer vision technologies.

A **C# Windows Forms (MDI-based)** desktop application that integrates:
- 🤖 AI Chatbot (Gemini API + Firebase)
- 👤 Face Detection (EmguCV Haar Cascade)
- 🧩 Modular UI with multiple feature forms

> ⚠️ Note: Object recognition and detection features are **not highly accurate** and are intended for demonstration and learning purposes only.

---

## 📦 Project Overview

This project follows an **MDI (Multiple Document Interface)** architecture where:

- `Form1` acts as the main container
- Features are loaded as **child forms**
- Only one feature is visible at a time

### 🧩 Modules Included

| Module | Description |
|--------|------------|
| 💬 ChatBotForm | AI chatbot using Gemini API with Firebase chat history |
| 👁️ FaceRecognitionForm | Real-time face detection using webcam |
| 📐 ObjectOrientationForm | Placeholder module (no active logic yet) |

---

## ⚙️ Requirements

Before running the project, make sure yAou have:

### 🖥️ Software
- Visual Studio (2019 or later recommended)
- .NET Framework **4.6.1**
- Windows OS

### 📚 Required NuGet Packages

Install these via NuGet Package Manager:

```powershell
Install-Package Emgu.CV -Version 4.1.1
Install-Package Guna.UI2.WinForms -Version 2.0.4.7
Install-Package System.Text.Json -Version 10.0.8
Install-Package ZedGraph -Version 5.1.7
```

## 📥 Additional Required Files

### 🔍 Haar Cascade File (IMPORTANT)

Download:
- `haarcascade_frontalface_default.xml`

📌 Place inside:
/bin/x64/Debug/
or
/bin/Debug/

> ❗ Required for face detection to function.

## 🔑 API Keys Setup (IMPORTANT)

This project uses:
- Google Gemini API
- Firebase Realtime Database

### ⚠️ Important Notes
- API keys are currently **hardcoded**
- Do **NOT** expose real keys in public repositories

### 📌 Recommended Setup
- Replace API keys in `ChatBotForm`
- Use environment variables if possible:

```csharp
string apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
```

---

### 🚀 Build & Run

```markdown
## 🚀 Build & Run

### Using Visual Studio
1. Open `MDI.sln`
2. Set platform:
   - ✅ **x64 (REQUIRED for EmguCV)**
3. Run the project

### Using MSBuild

```powershell
# Debug Build
msbuild MDI.sln /p:Configuration=Debug /p:Platform="Any CPU"

# x64 Build (Recommended)
msbuild MDI.sln /p:Configuration=Debug /p:Platform=x64
```


---

### 🛠️ Troubleshooting

```markdown
## 🛠️ Troubleshooting

### ❌ Face Detection Not Working
- Ensure `haarcascade_frontalface_default.xml` is placed correctly:
  - `/bin/x64/Debug/`
- Confirm you are running **x64 build**

### ❌ Application Crashes on Startup
- Switch build configuration to:
  - ✅ `x64`
- Rebuild the solution

### 🔄 Build Errors / Issues
Try:
1. Clean Solution
2. Rebuild Solution
3. Restart Visual Studio
4. Restore NuGet Packages

### ❌ Missing DLL Errors
- Reinstall NuGet packages

### ❌ API Not Responding
- Check API key
- Verify Firebase URL
- Check internet connection
```

## 🧠 Key Features

### 🤖 Chatbot
- Gemini API integration
- Firebase chat storage
- Dynamic UI messaging

### 👤 Face Detection
- EmguCV (OpenCV)
- Haar Cascade detection
- Real-time webcam

> ⚠️ Accuracy depends on lighting and camera quality.

---

## ⚠️ Known Limitations

- Face detection is not highly accurate
- Object module is incomplete
- API keys are hardcoded
- No automated tests

---

## 👨‍💻 Author

**Jerenze Levi T. Omandam**

---

## ⭐ Support

If you found this project helpful:
- ⭐ Star the repository  
- 🍴 Fork it  
- 📢 Share it  
