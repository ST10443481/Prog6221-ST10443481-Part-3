# Prog6221-ST10443481-Part-3

# 🛡️ Cybersecurity Awareness Chatbot

A comprehensive Windows Forms application that serves as an interactive cybersecurity awareness tool with task management, educational quizzes, and natural language processing capabilities.

## 📋 Table of Contents
- [Overview](#overview)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Database Setup](#database-setup)
- [Usage Guide](#usage-guide)
- [Project Structure](#project-structure)
- [Features in Detail](#features-in-detail)
- [Screenshots](#screenshots)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## 🎯 Overview

The Cybersecurity Awareness Chatbot is a GUI-based desktop application designed to educate users about cybersecurity best practices through interactive features. It combines a conversational interface with practical tools for managing security tasks, testing knowledge through quizzes, and tracking user activities.

**Key Objectives:**
- Enhance cybersecurity awareness through interactive learning
- Help users manage cybersecurity-related tasks with reminders
- Test and reinforce security knowledge through quizzes
- Provide a responsive chatbot experience with NLP simulation
- Track and log user interactions and activities

## ✨ Features

### 🤖 Intelligent Chatbot
- Natural Language Processing simulation using keyword detection
- Context-aware responses to user queries
- Support for multiple command variations
- Cybersecurity advice and tips on demand

### 📝 Task Assistant
- Add cybersecurity tasks with descriptions
- Set optional reminders with specific dates
- View, complete, and delete tasks
- MySQL database integration for persistent storage
- Mark tasks as completed or pending

### 🎮 Cybersecurity Quiz Game
- 12+ cybersecurity questions covering various topics
- Multiple-choice and true/false question formats
- Immediate feedback with explanations
- Score tracking and personalized feedback
- Progressive learning experience

### 📋 Activity Log
- Track all significant actions (tasks, quiz attempts, interactions)
- Timestamped entries for each activity
- View last 10 actions on demand
- Maintains up-to-date user interaction history

### 💬 NLP Simulation
- Keyword-based intent recognition
- Supports variations in user phrasing
- Handles commands like:
  - Task management ("add task", "show tasks", "complete task")
  - Quiz control ("start quiz", "play quiz")
  - Activity viewing ("show activity log", "recent actions")
  - Help requests ("help", "what can you do")

## 🛠️ Technologies Used

### Frontend
- **Windows Forms (.NET Framework 4.8)** - GUI framework
- **C#** - Programming language
- **RichTextBox, ListBox, Panel** - UI Controls

### Backend
- **MySQL** - Database for task storage
- **MySql.Data** - Database connector
- **.NET Framework** - Application framework

### Development Tools
- **Visual Studio 2022/2019** - IDE
- **NuGet Package Manager** - Dependency management
- **Git** - Version control

## 📦 Prerequisites

Before you begin, ensure you have the following installed:

### Required Software
- **Visual Studio** (2019 or 2022 Community Edition or higher)
  - With ".NET desktop development" workload
- **MySQL Server** (5.7 or higher)
  - Or XAMPP/WAMP with MySQL
- **.NET Framework 4.8** or higher

### Required NuGet Packages
- `MySql.Data` (version 8.0.33 or higher)

MIT License

Copyright (c) 2024 Thando F

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
