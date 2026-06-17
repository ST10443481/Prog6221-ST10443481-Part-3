-- Create database
CREATE DATABASE IF NOT EXISTS cybersecurity_db;
USE cybersecurity_db;

-- Create tasks table
CREATE TABLE IF NOT EXISTS tasks (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) NOT NULL,
    title VARCHAR(200) NOT NULL,
    description TEXT,
    reminder_date DATETIME,
    is_completed BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Sample data (optional)
INSERT INTO tasks (username, title, description, reminder_date) VALUES 
('User1', 'Enable two-factor authentication', 'Set up 2FA on email and social media accounts', DATE_ADD(NOW(), INTERVAL 7 DAY)),
('User1', 'Review privacy settings', 'Check privacy settings on social media platforms', DATE_ADD(NOW(), INTERVAL 3 DAY)),
('User1', 'Update antivirus software', 'Ensure antivirus is updated and run a full scan', DATE_ADD(NOW(), INTERVAL 1 DAY));
