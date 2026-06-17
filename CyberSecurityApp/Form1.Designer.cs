namespace CyberSecurityApp
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using MySql.Data.MySqlClient;

        public partial class Form2 : Form
        {
            private TextBox txtUserInput;
            private RichTextBox rtxtChatDisplay;
            private Button btnSend;
            private ListBox lstTasks;
            private Panel panelTasks;
            private Panel panelQuiz;
            private Button btnShowTasks;
            private Button btnShowQuiz;
            private Button btnShowChat;
            private Label lblQuizScore;
            private Label lblQuizQuestion;
            private RadioButton rbOption1;
            private RadioButton rbOption2;
            private RadioButton rbOption3;
            private RadioButton rbOption4;
            private Button btnSubmitAnswer;
            private Button btnStartQuiz;

            // Database connection
            private string connectionString = "server=localhost;database=cybersecurity_db;uid=root;pwd=@Labs2026!;";

            // Quiz variables
            private List<QuizQuestion> quizQuestions;
            private int currentQuestionIndex = 0;
            private int quizScore = 0;
            private bool quizActive = false;

            // Activity log
            private List<ActivityLogEntry> activityLog;

            // Current user
            private string currentUser = "User1";

            public Form2()
            {
                InitializeComponent();
                InitializeDatabase();
                LoadQuizQuestions();
                activityLog = new List<ActivityLogEntry>();
                AddToActivityLog("Chatbot started", "Application launched");
                DisplayBotMessage("Hello! I'm your Cybersecurity Awareness Assistant. I can help you with:\n" +
                                 "• Adding cybersecurity tasks\n" +
                                 "• Setting reminders\n" +
                                 "• Playing cybersecurity quiz games\n" +
                                 "• Showing activity logs\n\n" +
                                 "Try saying: 'Add task', 'Start quiz', or 'Show activity log'");
            }

            private void InitializeComponent()
            {
                this.txtUserInput = new TextBox();
                this.rtxtChatDisplay = new RichTextBox();
                this.btnSend = new Button();
                this.lstTasks = new ListBox();
                this.panelTasks = new Panel();
                this.btnShowTasks = new Button();
                this.btnShowQuiz = new Button();
                this.btnShowChat = new Button();
                this.panelQuiz = new Panel();
                this.lblQuizScore = new Label();
                this.lblQuizQuestion = new Label();
                this.rbOption1 = new RadioButton();
                this.rbOption2 = new RadioButton();
                this.rbOption3 = new RadioButton();
                this.rbOption4 = new RadioButton();
                this.btnSubmitAnswer = new Button();
                this.btnStartQuiz = new Button();
                this.panelTasks.SuspendLayout();
                this.panelQuiz.SuspendLayout();
                this.SuspendLayout();

                // Form properties
                this.Text = "Cybersecurity Awareness Chatbot";
                this.Size = new Size(1200, 700);
                this.BackColor = Color.FromArgb(30, 30, 45);

                // Chat Display
                this.rtxtChatDisplay.Location = new Point(12, 12);
                this.rtxtChatDisplay.Size = new Size(600, 500);
                this.rtxtChatDisplay.BackColor = Color.FromArgb(20, 20, 35);
                this.rtxtChatDisplay.ForeColor = Color.White;
                this.rtxtChatDisplay.Font = new Font("Segoe UI", 10);
                this.rtxtChatDisplay.ReadOnly = true;

                // User Input
                this.txtUserInput.Location = new Point(12, 520);
                this.txtUserInput.Size = new Size(500, 30);
                this.txtUserInput.BackColor = Color.FromArgb(40, 40, 55);
                this.txtUserInput.ForeColor = Color.White;
                this.txtUserInput.Font = new Font("Segoe UI", 10);
                this.txtUserInput.KeyPress += TxtUserInput_KeyPress;

                // Send Button
                this.btnSend.Location = new Point(520, 518);
                this.btnSend.Size = new Size(92, 35);
                this.btnSend.Text = "Send";
                this.btnSend.BackColor = Color.FromArgb(0, 120, 215);
                this.btnSend.ForeColor = Color.White;
                this.btnSend.FlatStyle = FlatStyle.Flat;
                this.btnSend.Click += BtnSend_Click;

                // Navigation Buttons
                this.btnShowChat = CreateNavButton("Chat", 630, 12);
                this.btnShowTasks = CreateNavButton("Tasks", 630, 52);
                this.btnShowQuiz = CreateNavButton("Quiz", 630, 92);

                // Tasks Panel
                this.panelTasks.Location = new Point(630, 140);
                this.panelTasks.Size = new Size(550, 520);
                this.panelTasks.BackColor = Color.FromArgb(40, 40, 55);
                this.panelTasks.Visible = false;

                Label lblTasksHeader = new Label()
                {
                    Text = "My Cybersecurity Tasks",
                    Location = new Point(10, 10),
                    Size = new Size(200, 30),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 14, FontStyle.Bold)
                };
                this.panelTasks.Controls.Add(lblTasksHeader);

                this.lstTasks.Location = new Point(10, 50);
                this.lstTasks.Size = new Size(530, 350);
                this.lstTasks.BackColor = Color.FromArgb(50, 50, 65);
                this.lstTasks.ForeColor = Color.White;
                this.lstTasks.Font = new Font("Segoe UI", 10);

                Button btnCompleteTask = new Button()
                {
                    Text = "Mark Completed",
                    Location = new Point(10, 410),
                    Size = new Size(150, 35),
                    BackColor = Color.FromArgb(40, 167, 69),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnCompleteTask.Click += BtnCompleteTask_Click;

                Button btnDeleteTask = new Button()
                {
                    Text = "Delete Task",
                    Location = new Point(170, 410),
                    Size = new Size(150, 35),
                    BackColor = Color.FromArgb(220, 53, 69),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnDeleteTask.Click += BtnDeleteTask_Click;

                Button btnRefreshTasks = new Button()
                {
                    Text = "Refresh",
                    Location = new Point(330, 410),
                    Size = new Size(100, 35),
                    BackColor = Color.FromArgb(108, 117, 125),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnRefreshTasks.Click += BtnRefreshTasks_Click;

                this.panelTasks.Controls.Add(this.lstTasks);
                this.panelTasks.Controls.Add(btnCompleteTask);
                this.panelTasks.Controls.Add(btnDeleteTask);
                this.panelTasks.Controls.Add(btnRefreshTasks);

                // Quiz Panel
                this.panelQuiz.Location = new Point(630, 140);
                this.panelQuiz.Size = new Size(550, 520);
                this.panelQuiz.BackColor = Color.FromArgb(40, 40, 55);
                this.panelQuiz.Visible = false;

                Label lblQuizHeader = new Label()
                {
                    Text = "Cybersecurity Quiz",
                    Location = new Point(10, 10),
                    Size = new Size(200, 30),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 14, FontStyle.Bold)
                };
                this.panelQuiz.Controls.Add(lblQuizHeader);

                this.lblQuizScore = new Label()
                {
                    Text = "Score: 0",
                    Location = new Point(400, 10),
                    Size = new Size(100, 30),
                    ForeColor = Color.Gold,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold)
                };
                this.panelQuiz.Controls.Add(this.lblQuizScore);

                this.lblQuizQuestion = new Label()
                {
                    Location = new Point(10, 60),
                    Size = new Size(530, 80),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 12),
                    BackColor = Color.FromArgb(60, 60, 80),
                    Padding = new Padding(10)
                };
                this.panelQuiz.Controls.Add(this.lblQuizQuestion);

                int radioY = 160;
                this.rbOption1 = CreateRadioButton(10, radioY);
                this.rbOption2 = CreateRadioButton(10, radioY + 40);
                this.rbOption3 = CreateRadioButton(10, radioY + 80);
                this.rbOption4 = CreateRadioButton(10, radioY + 120);

                this.panelQuiz.Controls.Add(this.rbOption1);
                this.panelQuiz.Controls.Add(this.rbOption2);
                this.panelQuiz.Controls.Add(this.rbOption3);
                this.panelQuiz.Controls.Add(this.rbOption4);

                this.btnSubmitAnswer = new Button()
                {
                    Text = "Submit Answer",
                    Location = new Point(10, 330),
                    Size = new Size(150, 40),
                    BackColor = Color.FromArgb(0, 120, 215),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Enabled = false
                };
                this.btnSubmitAnswer.Click += BtnSubmitAnswer_Click;

                this.btnStartQuiz = new Button()
                {
                    Text = "Start New Quiz",
                    Location = new Point(180, 330),
                    Size = new Size(150, 40),
                    BackColor = Color.FromArgb(40, 167, 69),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                this.btnStartQuiz.Click += BtnStartQuiz_Click;

                this.panelQuiz.Controls.Add(this.btnSubmitAnswer);
                this.panelQuiz.Controls.Add(this.btnStartQuiz);

                // Add controls to form
                this.Controls.Add(this.rtxtChatDisplay);
                this.Controls.Add(this.txtUserInput);
                this.Controls.Add(this.btnSend);
                this.Controls.Add(this.btnShowChat);
                this.Controls.Add(this.btnShowTasks);
                this.Controls.Add(this.btnShowQuiz);
                this.Controls.Add(this.panelTasks);
                this.Controls.Add(this.panelQuiz);

                this.panelTasks.ResumeLayout();
                this.panelQuiz.ResumeLayout();
                this.ResumeLayout();

                LoadTasks();
            }

            private Button CreateNavButton(string text, int x, int y)
            {
                Button btn = new Button()
                {
                    Text = text,
                    Location = new Point(x, y),
                    Size = new Size(120, 35),
                    BackColor = Color.FromArgb(0, 120, 215),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                };
                btn.Click += (s, e) => {
                    panelTasks.Visible = (text == "Tasks");
                    panelQuiz.Visible = (text == "Quiz");
                    if (text == "Tasks") LoadTasks();
                };
                return btn;
            }

            private RadioButton CreateRadioButton(int x, int y)
            {
                RadioButton rb = new RadioButton()
                {
                    Location = new Point(x, y),
                    Size = new Size(520, 30),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10)
                };
                return rb;
            }

            private void InitializeDatabase()
            {
                try
                {
                    string createDBQuery = "CREATE DATABASE IF NOT EXISTS cybersecurity_db";
                    using (MySqlConnection conn = new MySqlConnection("server=localhost;uid=root;pwd=@Labs2026!"))
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand(createDBQuery, conn);
                        cmd.ExecuteNonQuery();
                    }

                    string createTableQuery = @"CREATE TABLE IF NOT EXISTS tasks (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    username VARCHAR(50),
                    title VARCHAR(200),
                    description TEXT,
                    reminder_date DATETIME,
                    is_completed BOOLEAN DEFAULT FALSE,
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                )";

                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand(createTableQuery, conn);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Database error: {ex.Message}\n\nNote: Make sure MySQL is installed and running.",
                        "Database Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            private void LoadQuizQuestions()
            {
                quizQuestions = new List<QuizQuestion>
            {
                new QuizQuestion("What is phishing?", new[] { "A type of computer virus", "A fraudulent attempt to obtain sensitive information", "A fishing technique", "A security software" }, 1, "Phishing is a cyber attack where attackers trick users into revealing sensitive information."),
                new QuizQuestion("Which of the following is a strong password?", new[] { "123456", "password", "P@ssw0rd!2024", "qwerty" }, 2, "A strong password contains uppercase, lowercase, numbers, and special characters."),
                new QuizQuestion("True or False: You should use the same password for all your accounts.", new[] { "True", "False" }, 1, "Using the same password across accounts is dangerous. If one gets compromised, all are at risk."),
                new QuizQuestion("What does two-factor authentication (2FA) add to your security?", new[] { "A second password", "An extra layer of verification", "A fingerprint scanner only", "A backup email" }, 1, "2FA adds an extra verification step, making accounts much harder to compromise."),
                new QuizQuestion("What should you do if you receive a suspicious email attachment?", new[] { "Open it to check", "Forward it to friends", "Delete it and report it", "Save it to your computer" }, 2, "Suspicious attachments should never be opened. Delete and report them immediately."),
                new QuizQuestion("What is social engineering?", new[] { "Building social networks", "Manipulating people to reveal information", "Engineering social media", "A type of encryption" }, 1, "Social engineering tricks people into revealing confidential information."),
                new QuizQuestion("True or False: Public Wi-Fi is completely safe for online banking.", new[] { "True", "False" }, 1, "Public Wi-Fi is often unencrypted, making it risky for sensitive activities like banking."),
                new QuizQuestion("What is ransomware?", new[] { "Software that locks your files for ransom", "A type of antivirus", "A backup system", "A password manager" }, 0, "Ransomware encrypts files and demands payment for their release."),
                new QuizQuestion("How often should you update your software?", new[] { "Never", "Once a year", "When updates are available", "Only when forced" }, 2, "Regular updates patch security vulnerabilities and should be installed when available."),
                new QuizQuestion("What information is safe to share on social media?", new[] { "Your full address", "Your pet's name", "Your phone number", "General interests only" }, 3, "Personal identifiable information should be limited. Share only general, non-sensitive information."),
                new QuizQuestion("What does HTTPS indicate?", new[] { "Highly Tested Protocol", "Secure encrypted connection", "Hyper Transfer System", "Hack-proof website" }, 1, "HTTPS indicates the website uses encryption to protect data transmission."),
                new QuizQuestion("True or False: You should click 'Unsubscribe' in suspicious emails.", new[] { "True", "False" }, 1, "Clicking links in suspicious emails can confirm your email is active to attackers."),
            };
            }

            private void BtnSend_Click(object sender, EventArgs e)
            {
                ProcessUserInput();
            }

            private void TxtUserInput_KeyPress(object sender, KeyPressEventArgs e)
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    ProcessUserInput();
                }
            }

            private void ProcessUserInput()
            {
                string userInput = txtUserInput.Text.Trim();
                if (string.IsNullOrEmpty(userInput)) return;

                DisplayUserMessage(userInput);
                string response = ProcessNaturalLanguage(userInput);
                DisplayBotMessage(response);

                txtUserInput.Clear();
                AddToActivityLog("User interaction", $"User said: {userInput}");
            }

            private string ProcessNaturalLanguage(string input)
            {
                string lowerInput = input.ToLower();

                // Activity log request
                if (lowerInput.Contains("activity log") || lowerInput.Contains("what have you done") ||
                    lowerInput.Contains("show me what") || lowerInput.Contains("recent actions"))
                {
                    return GetActivityLog();
                }

                // Task addition
                if (lowerInput.Contains("add task") || lowerInput.Contains("new task") ||
                    lowerInput.Contains("create task") || lowerInput.Contains("add reminder"))
                {
                    return HandleAddTask(input);
                }

                // Show tasks
                if (lowerInput.Contains("show tasks") || lowerInput.Contains("list tasks") ||
                    lowerInput.Contains("my tasks") || lowerInput.Contains("view tasks"))
                {
                    panelTasks.Visible = true;
                    panelQuiz.Visible = false;
                    LoadTasks();
                    return "Here are your cybersecurity tasks. You can view them in the Tasks panel on the right.";
                }

                // Quiz commands
                if (lowerInput.Contains("start quiz") || lowerInput.Contains("play quiz") ||
                    lowerInput.Contains("take quiz") || lowerInput.Contains("begin quiz"))
                {
                    panelQuiz.Visible = true;
                    panelTasks.Visible = false;
                    StartQuiz();
                    return "Opening the cybersecurity quiz. Answer the questions to test your knowledge!";
                }

                // Delete task
                if (lowerInput.Contains("delete task") || lowerInput.Contains("remove task"))
                {
                    return HandleDeleteTask(input);
                }

                // Complete task
                if (lowerInput.Contains("complete task") || lowerInput.Contains("finish task"))
                {
                    return HandleCompleteTask(input);
                }

                // Help command
                if (lowerInput.Contains("help") || lowerInput.Contains("what can you do"))
                {
                    return GetHelpMessage();
                }

                // General cybersecurity questions
                return GetCybersecurityAdvice(lowerInput);
            }

            private string HandleAddTask(string input)
            {
                // Extract task title
                string taskTitle = ExtractTaskTitle(input);

                if (string.IsNullOrEmpty(taskTitle))
                {
                    return "What cybersecurity task would you like to add? (e.g., 'Enable two-factor authentication')";
                }

                // Check for reminder
                bool hasReminder = input.ToLower().Contains("remind me") || input.ToLower().Contains("reminder");
                DateTime reminderDate = DateTime.Now.AddDays(7);

                if (hasReminder)
                {
                    // Parse reminder date
                    if (input.ToLower().Contains("tomorrow"))
                        reminderDate = DateTime.Now.AddDays(1);
                    else if (input.ToLower().Contains("in 3 days"))
                        reminderDate = DateTime.Now.AddDays(3);
                    else if (input.ToLower().Contains("in 7 days"))
                        reminderDate = DateTime.Now.AddDays(7);
                    else if (input.ToLower().Contains("next week"))
                        reminderDate = DateTime.Now.AddDays(7);
                }

                AddTaskToDatabase(taskTitle, $"Task: {taskTitle}", hasReminder ? reminderDate : (DateTime?)null);

                if (hasReminder)
                    return $"✅ Task '{taskTitle}' added with reminder for {reminderDate:MMM dd, yyyy}. I'll remind you then!";
                else
                    return $"✅ Task '{taskTitle}' added successfully! Would you like to set a reminder? Just say 'Remind me'.";
            }

            private string ExtractTaskTitle(string input)
            {
                // Simple keyword extraction
                string[] prefixes = { "add task ", "new task ", "create task ", "add reminder ", "remind me to " };
                string lowerInput = input.ToLower();

                foreach (string prefix in prefixes)
                {
                    if (lowerInput.Contains(prefix))
                    {
                        int index = lowerInput.IndexOf(prefix) + prefix.Length;
                        if (index < input.Length)
                        {
                            string title = input.Substring(index).Trim();
                            // Remove trailing reminder phrases
                            string[] removePhrases = { " tomorrow", " in 3 days", " in 7 days", " next week" };
                            foreach (string phrase in removePhrases)
                            {
                                if (title.ToLower().Contains(phrase))
                                    title = title.Substring(0, title.ToLower().IndexOf(phrase)).Trim();
                            }
                            return title.Length > 0 ? title : "Cybersecurity task";
                        }
                    }
                }

                return "General cybersecurity task";
            }

            private void AddTaskToDatabase(string title, string description, DateTime? reminderDate)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = @"INSERT INTO tasks (username, title, description, reminder_date) 
                                    VALUES (@username, @title, @description, @reminderDate)";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@username", currentUser);
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@reminderDate", reminderDate ?? (object)DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                    AddToActivityLog("Task added", $"Task: {title}");
                    LoadTasks();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database error: {ex.Message}");
                }
            }

            private void LoadTasks()
            {
                try
                {
                    lstTasks.Items.Clear();
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "SELECT id, title, reminder_date, is_completed FROM tasks WHERE username = @username ORDER BY is_completed ASC, created_at DESC";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@username", currentUser);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string status = reader.GetBoolean("is_completed") ? "✓ " : "○ ";
                                string task = $"{status}{reader.GetString("title")}";
                                if (!reader.IsDBNull(reader.GetOrdinal("reminder_date")))
                                {
                                    task += $" (Reminder: {reader.GetDateTime("reminder_date"):MMM dd})";
                                }
                                lstTasks.Items.Add(new TaskItem { Id = reader.GetInt32("id"), DisplayText = task });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lstTasks.Items.Add($"Error loading tasks: {ex.Message}");
                }
            }

            private string HandleDeleteTask(string input)
            {
                if (lstTasks.SelectedItem == null)
                    return "Please select a task from the Tasks panel first, then say 'Delete task' again.";

                TaskItem selected = (TaskItem)lstTasks.SelectedItem;
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM tasks WHERE id = @id AND username = @username";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", selected.Id);
                        cmd.Parameters.AddWithValue("@username", currentUser);
                        cmd.ExecuteNonQuery();
                    }
                    AddToActivityLog("Task deleted", $"Task ID: {selected.Id}");
                    LoadTasks();
                    return "✅ Task deleted successfully!";
                }
                catch (Exception ex)
                {
                    return $"Error deleting task: {ex.Message}";
                }
            }

            private string HandleCompleteTask(string input)
            {
                if (lstTasks.SelectedItem == null)
                    return "Please select a task from the Tasks panel first, then say 'Complete task' again.";

                TaskItem selected = (TaskItem)lstTasks.SelectedItem;
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "UPDATE tasks SET is_completed = TRUE WHERE id = @id AND username = @username";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", selected.Id);
                        cmd.Parameters.AddWithValue("@username", currentUser);
                        cmd.ExecuteNonQuery();
                    }
                    AddToActivityLog("Task completed", $"Task ID: {selected.Id}");
                    LoadTasks();
                    return "✅ Great job! Task marked as completed!";
                }
                catch (Exception ex)
                {
                    return $"Error completing task: {ex.Message}";
                }
            }

            private void BtnCompleteTask_Click(object sender, EventArgs e)
            {
                if (lstTasks.SelectedItem != null)
                {
                    string response = HandleCompleteTask("");
                    DisplayBotMessage(response);
                }
                else
                {
                    DisplayBotMessage("Please select a task to mark as completed.");
                }
            }

            private void BtnDeleteTask_Click(object sender, EventArgs e)
            {
                if (lstTasks.SelectedItem != null)
                {
                    string response = HandleDeleteTask("");
                    DisplayBotMessage(response);
                }
                else
                {
                    DisplayBotMessage("Please select a task to delete.");
                }
            }

            private void BtnRefreshTasks_Click(object sender, EventArgs e)
            {
                LoadTasks();
                DisplayBotMessage("Tasks refreshed!");
            }

            private void StartQuiz()
            {
                quizActive = true;
                currentQuestionIndex = 0;
                quizScore = 0;
                UpdateQuizScore();
                DisplayNextQuestion();
                btnSubmitAnswer.Enabled = true;
                AddToActivityLog("Quiz started", "User began cybersecurity quiz");
            }

            private void BtnStartQuiz_Click(object sender, EventArgs e)
            {
                StartQuiz();
            }

            private void DisplayNextQuestion()
            {
                if (currentQuestionIndex < quizQuestions.Count)
                {
                    var question = quizQuestions[currentQuestionIndex];
                    lblQuizQuestion.Text = $"Q{currentQuestionIndex + 1}: {question.QuestionText}";

                    // Clear radio buttons
                    rbOption1.Checked = false;
                    rbOption2.Checked = false;
                    rbOption3.Checked = false;
                    rbOption4.Checked = false;

                    // Set radio button text
                    if (question.Options.Length >= 1) { rbOption1.Text = question.Options[0]; rbOption1.Visible = true; }
                    if (question.Options.Length >= 2) { rbOption2.Text = question.Options[1]; rbOption2.Visible = true; }
                    if (question.Options.Length >= 3) { rbOption3.Text = question.Options[2]; rbOption3.Visible = true; }
                    if (question.Options.Length >= 4) { rbOption4.Text = question.Options[3]; rbOption4.Visible = true; }

                    // Hide extra radio buttons
                    rbOption3.Visible = question.Options.Length > 2;
                    rbOption4.Visible = question.Options.Length > 3;
                }
                else
                {
                    EndQuiz();
                }
            }

            private void BtnSubmitAnswer_Click(object sender, EventArgs e)
            {
                if (!quizActive) return;

                int selectedOption = -1;
                if (rbOption1.Checked) selectedOption = 0;
                else if (rbOption2.Checked) selectedOption = 1;
                else if (rbOption3.Checked) selectedOption = 2;
                else if (rbOption4.Checked) selectedOption = 3;

                if (selectedOption == -1)
                {
                    DisplayBotMessage("Please select an answer first!");
                    return;
                }

                var question = quizQuestions[currentQuestionIndex];
                bool isCorrect = (selectedOption == question.CorrectAnswer);

                if (isCorrect)
                {
                    quizScore++;
                    DisplayBotMessage($"✅ Correct! {question.Explanation}");
                    AddToActivityLog("Quiz answer", $"Correct: {question.QuestionText}");
                }
                else
                {
                    string correctOption = question.Options[question.CorrectAnswer];
                    DisplayBotMessage($"❌ Incorrect. The correct answer is: {correctOption}\n{question.Explanation}");
                    AddToActivityLog("Quiz answer", $"Incorrect: {question.QuestionText}");
                }

                UpdateQuizScore();
                currentQuestionIndex++;
                DisplayNextQuestion();
            }

            private void UpdateQuizScore()
            {
                lblQuizScore.Text = $"Score: {quizScore}/{quizQuestions.Count}";
            }

            private void EndQuiz()
            {
                quizActive = false;
                btnSubmitAnswer.Enabled = false;

                string feedback;
                double percentage = (double)quizScore / quizQuestions.Count * 100;

                if (percentage >= 90)
                    feedback = "🏆 Excellent! You're a cybersecurity pro! 🏆";
                else if (percentage >= 70)
                    feedback = "🎉 Good job! You have solid cybersecurity knowledge! 🎉";
                else if (percentage >= 50)
                    feedback = "📚 Not bad! Keep learning to improve your cybersecurity awareness! 📚";
                else
                    feedback = "⚠️ Keep studying! Cybersecurity is important for everyone. Try the quiz again! ⚠️";

                DisplayBotMessage($"Quiz Complete!\nFinal Score: {quizScore}/{quizQuestions.Count} ({percentage:F1}%)\n{feedback}");
                AddToActivityLog("Quiz completed", $"Score: {quizScore}/{quizQuestions.Count}");
            }

            private string GetActivityLog()
            {
                if (activityLog.Count == 0)
                    return "No activities recorded yet. Start by adding tasks or playing the quiz!";

                string log = "📋 Recent Activity Log:\n\n";
                int count = Math.Min(10, activityLog.Count);

                for (int i = activityLog.Count - count; i < activityLog.Count; i++)
                {
                    log += $"{i + 1}. [{activityLog[i].Timestamp:HH:mm:ss}] {activityLog[i].Action}: {activityLog[i].Details}\n";
                }

                return log;
            }

            private void AddToActivityLog(string action, string details)
            {
                activityLog.Add(new ActivityLogEntry
                {
                    Timestamp = DateTime.Now,
                    Action = action,
                    Details = details
                });

                // Keep only last 50 entries
                while (activityLog.Count > 50)
                    activityLog.RemoveAt(0);
            }

            private string GetHelpMessage()
            {
                return "🤖 I can help you with:\n\n" +
                       "📝 TASKS:\n" +
                       "   • 'Add task to enable 2FA'\n" +
                       "   • 'Remind me to update my password tomorrow'\n" +
                       "   • 'Show my tasks'\n" +
                       "   • 'Complete task' / 'Delete task'\n\n" +
                       "🎮 QUIZ:\n" +
                       "   • 'Start quiz' - Test your cybersecurity knowledge!\n\n" +
                       "📋 OTHER:\n" +
                       "   • 'Show activity log' - See what I've done\n" +
                       "   • 'Help' - Show this message\n\n" +
                       "💡 Try asking cybersecurity questions too!";
            }

            private string GetCybersecurityAdvice(string input)
            {
                if (input.Contains("password"))
                    return "🔐 Use strong, unique passwords for each account. Consider using a password manager!";
                if (input.Contains("phishing"))
                    return "🎣 Never click suspicious links or download attachments from unknown senders. Always verify the sender's email address!";
                if (input.Contains("2fa") || input.Contains("two factor"))
                    return "📱 Two-factor authentication adds an extra layer of security. Always enable it when available!";
                if (input.Contains("update") || input.Contains("software"))
                    return "🔄 Keep all software updated! Security patches fix vulnerabilities that hackers could exploit.";
                if (input.Contains("wifi") || input.Contains("wi-fi"))
                    return "📡 Avoid sensitive activities (banking, shopping) on public Wi-Fi. Use a VPN for extra security!";
                if (input.Contains("backup"))
                    return "💾 Regular backups are crucial! Use the 3-2-1 rule: 3 copies, 2 different media, 1 off-site.";

                return "That's a great cybersecurity question! For specific advice, try asking about passwords, phishing, 2FA, or software updates.";
            }

            private void DisplayUserMessage(string message)
            {
                rtxtChatDisplay.SelectionColor = Color.LightGreen;
                rtxtChatDisplay.AppendText($"You: {message}\n\n");
                rtxtChatDisplay.ScrollToCaret();
            }

            private void DisplayBotMessage(string message)
            {
                rtxtChatDisplay.SelectionColor = Color.Cyan;
                rtxtChatDisplay.AppendText($"Bot: {message}\n\n");
                rtxtChatDisplay.ScrollToCaret();
            }

            private class TaskItem
            {
                public int Id { get; set; }
                public string DisplayText { get; set; }
                public override string ToString() => DisplayText;
            }

            private class QuizQuestion
            {
                public string QuestionText { get; set; }
                public string[] Options { get; set; }
                public int CorrectAnswer { get; set; }
                public string Explanation { get; set; }

                public QuizQuestion(string text, string[] options, int correct, string explanation)
                {
                    QuestionText = text;
                    Options = options;
                    CorrectAnswer = correct;
                    Explanation = explanation;
                }
            }

            private class ActivityLogEntry
            {
                public DateTime Timestamp { get; set; }
                public string Action { get; set; }
                public string Details { get; set; }
            }
        }
    }