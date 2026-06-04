using System;
using System.Collections.Generic;

namespace Backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public string Address { get; set; } = string.Empty;
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
    }

    public class Document
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = "รออนุมัติ";
        public string? Reason { get; set; }
    }

    public class UserProfile
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime BirthDay { get; set; }
        public string Occupation { get; set; } = string.Empty;
        public string ProfileImageBase64 { get; set; } = string.Empty;
    }

    public class QueueEntry
    {
        public int Id { get; set; }
        public string QueueNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class Product16
    {
        public int Id { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class Product36
    {
        public int Id { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class Exam
    {
        public int Id { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string OptionsJson { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
        public int Sequence { get; set; }
    }

    public class ExamResult
    {
        public int Id { get; set; }
        public string ExamineeName { get; set; } = string.Empty;
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public DateTime ExamDate { get; set; }
    }

    public class UserComment
    {
        public int Id { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    // Auth Models
    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
