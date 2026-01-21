using LearningPlatform.Data.Interfaces;
using Microsoft.Extensions.Logging;

public class QuizService : IQuizService
{
    private readonly IQuizRepository _quizRepo;
    private readonly IQuestionRepository _questionRepo;
    private readonly IQuestionOptionRepository _optionRepo;
    private readonly IQuizAttemptRepository _attemptRepo;
    private readonly IUserRepository _userRepo;
    private readonly ICourseRepository _courseRepo;
    private readonly ILogger<QuizService> _logger;
    public QuizService(
        IQuizRepository quizRepo,
        IQuestionRepository questionRepo,
        IQuestionOptionRepository optionRepo,
        IQuizAttemptRepository attemptRepo,
        IUserRepository userRepo,
        ICourseRepository courseRepo,
        ILogger<QuizService> logger)
    {
        _quizRepo = quizRepo;
        _questionRepo = questionRepo;
        _optionRepo = optionRepo;
        _attemptRepo = attemptRepo;
        _userRepo = userRepo;
        _courseRepo = courseRepo;
        _logger = logger;
    }
    public Guid QuestionId { get; set; }

    public async Task<QuestionOptionDto> AddOptionToQuestionAsync(AddOptionDto addOptionDto)
    {
        var question = await _questionRepo.GetByIdAsync(addOptionDto.QuestionId);
        if (question == null)
        {
            throw new ArgumentException("Question not found", nameof(addOptionDto.QuestionId));
        }
        var option = QuestionOption.Create(addOptionDto.QuestionId, addOptionDto.OptionText, addOptionDto.IsCorrect, addOptionDto.OrderIndex);
        await _optionRepo.AddAsync(option);
        return new QuestionOptionDto
        {
            Id = option.Id,
            QuestionId = option.QuestionId,
            OptionText = option.OptionText,
            IsCorrect = option.IsCorrect,
            OrderIndex = option.OrderIndex,
            CreatedAt = option.CreatedAt,
            UpdatedAt = option.UpdatedAt
        };
    }

    public async Task<QuestionDto> AddQuestionToQuizAsync(AddQuestionToQuizDto addQuestionToQuizDto)
    {
        var quiz = await _quizRepo.GetByIdAsync(addQuestionToQuizDto.QuizId);
        if (quiz == null)
        {
            throw new ArgumentException("Quiz not found", nameof(addQuestionToQuizDto.QuizId));
        }
        var question = Question.Create(addQuestionToQuizDto.QuizId, addQuestionToQuizDto.QuestionText, addQuestionToQuizDto.QuestionType, addQuestionToQuizDto.Points, addQuestionToQuizDto.OrderIndex);
        await _questionRepo.AddAsync(question);
        return new QuestionDto
        {
            Id = question.Id,
            QuizId = question.QuizId,
            QuestionText = question.QuestionText,
            QuestionType = question.QuestionType,
            Points = question.Points,
            OrderIndex = question.OrderIndex,
            CreatedAt = question.CreatedAt,
            UpdatedAt = question.UpdatedAt
        };
    }

    public async Task<QuizDto> CreateQuizAsync(CreateQuizDto createQuizDto)
    {
        var course = await _courseRepo.GetByIdAsync(createQuizDto.CourseId);
        if (course == null)
        {
            throw new ArgumentException("Course not found", nameof(createQuizDto.CourseId));
        }
        var quiz = Quiz.Create(createQuizDto.CourseId, createQuizDto.ModuleId, createQuizDto.Title, createQuizDto.Description, createQuizDto.PassingScore, createQuizDto.TimeLimitMinutes, createQuizDto.MaxAttempts, createQuizDto.IsActive);
        await _quizRepo.AddAsync(quiz);
        return new QuizDto
        {
            Id = quiz.Id,
            CourseId = quiz.CourseId,
            ModuleId = quiz.ModuleId,
            Title = quiz.Title,
            Description = quiz.Description,
            PassingScore = quiz.PassingScore,
            TimeLimitMinutes = quiz.TimeLimitMinutes,
            MaxAttempts = quiz.MaxAttempts,
            IsActive = quiz.IsActive,
            CreatedAt = quiz.CreatedAt,
            UpdatedAt = quiz.UpdatedAt,
            Questions = new List<QuestionDto>()
        };
    }

    public async Task DeleteOptionAsync(Guid optionId)
    {
        var option = await _optionRepo.GetByIdAsync(optionId);
        if (option == null)
        {
            throw new ArgumentException("Option not found", nameof(optionId));
        }
        await _optionRepo.DeleteAsync(option);
    }

    public async Task DeleteQuestionAsync(Guid questionId)
    {
        var question = await _questionRepo.GetByIdAsync(questionId);
        if (question == null)
        {
            throw new ArgumentException("Question not found", nameof(questionId));
        }
        await _questionRepo.DeleteAsync(question);
    }

    public async Task DeleteQuizAsync(Guid quizId)
    {
        var quiz = await _quizRepo.GetByIdAsync(quizId);
        if (quiz == null)
        {
            throw new ArgumentException("Quiz not found", nameof(quizId));
        }
        await _quizRepo.DeleteAsync(quiz);
    }

    public async Task<QuizAttemptResponse> GetQuizAttemptByIdAsync(Guid attemptId)
    {
        var attempt = await _attemptRepo.GetByIdAsync(attemptId);
        if (attempt == null)
        {
            throw new ArgumentException("Quiz attempt not found", nameof(attemptId));
        }
        return new QuizAttemptResponse
        {
            Id = attempt.Id,
            UserId = attempt.UserId,
            QuizId = attempt.QuizId,
            StartedAt = attempt.StartedAt,
            SubmittedAt = attempt.SubmittedAt,
            CorrectAnswers = attempt.CorrectAnswers,
            ScorePercentage = attempt.ScorePercentage,
            IsPassed = attempt.IsPassed,
            CreatedAt = attempt.CreatedAt,
            UpdatedAt = attempt.UpdatedAt
        };
    }

    public async Task<IEnumerable<QuizAttemptResponse>> GetQuizAttemptsByUserAsync(Guid userId)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException("User not found", nameof(userId));
        }
        var attempts = await _attemptRepo.GetQuizAttemptsByUserAsync(userId);
        return attempts.Select(attempt => new QuizAttemptResponse
        {
            Id = attempt.Id,
            UserId = attempt.UserId,
            QuizId = attempt.QuizId,
            StartedAt = attempt.StartedAt,
            SubmittedAt = attempt.SubmittedAt,
            CorrectAnswers = attempt.CorrectAnswers,
            ScorePercentage = attempt.ScorePercentage,
            IsPassed = attempt.IsPassed,
            CreatedAt = attempt.CreatedAt,
            UpdatedAt = attempt.UpdatedAt
        });
    }

    public async Task<QuizDto> GetQuizByIdAsync(Guid quizId)
    {
        var quiz = await _quizRepo.GetByIdAsync(quizId);
        if (quiz == null)
        {
            throw new ArgumentException("Quiz not found", nameof(quizId));
        }
        return new QuizDto
        {
            Id = quiz.Id,
            CourseId = quiz.CourseId,
            ModuleId = quiz.ModuleId,
            Title = quiz.Title,
            Description = quiz.Description,
            PassingScore = quiz.PassingScore,
            TimeLimitMinutes = quiz.TimeLimitMinutes,
            MaxAttempts = quiz.MaxAttempts,
            IsActive = quiz.IsActive,
            CreatedAt = quiz.CreatedAt,
            UpdatedAt = quiz.UpdatedAt,
            Questions = new List<QuestionDto>()
        };
    }

    public async Task<IEnumerable<QuizDto>> GetQuizzesByCourseAsync(Guid courseId)
    {
        var course = await _courseRepo.GetByIdAsync(courseId);
        if (course == null)
        {
            throw new ArgumentException("Course not found", nameof(courseId));
        }
        var quizzes = await _quizRepo.GetQuizzesByCourseAsync(courseId);
        return quizzes.Select(quiz => new QuizDto
        {
            Id = quiz.Id,
            CourseId = quiz.CourseId,
            ModuleId = quiz.ModuleId,
            Title = quiz.Title,
            Description = quiz.Description,
            PassingScore = quiz.PassingScore,
            TimeLimitMinutes = quiz.TimeLimitMinutes,
            MaxAttempts = quiz.MaxAttempts,
            IsActive = quiz.IsActive,
            CreatedAt = quiz.CreatedAt,
            UpdatedAt = quiz.UpdatedAt,
            Questions = new List<QuestionDto>()
        });
    }

    public async Task<QuizAttemptResponse> StartQuizAttemptAsync(StartQuizAttemptDto startQuizAttemptDto)
    {
        var user = await _userRepo.GetByIdAsync(startQuizAttemptDto.UserId);
        if (user == null)
        {
            throw new ArgumentException("User not found", nameof(startQuizAttemptDto.UserId));
        }
        var quiz = await _quizRepo.GetByIdAsync(startQuizAttemptDto.QuizId);
        if (quiz == null)
        {
            throw new ArgumentException("Quiz not found", nameof(startQuizAttemptDto.QuizId));
        }
        var attempt = QuizAttempt.Create(startQuizAttemptDto.UserId, startQuizAttemptDto.QuizId);
        await _attemptRepo.AddAsync(attempt);
        return new QuizAttemptResponse
        {
            Id = attempt.Id,
            UserId = attempt.UserId,
            QuizId = attempt.QuizId,
            StartedAt = attempt.StartedAt,
            SubmittedAt = attempt.SubmittedAt,
            CorrectAnswers = attempt.CorrectAnswers,
            ScorePercentage = attempt.ScorePercentage,
            IsPassed = attempt.IsPassed,
            CreatedAt = attempt.CreatedAt,
            UpdatedAt = attempt.UpdatedAt
        };
    }

    public async Task<QuizAttemptResponse> SubmitQuizAttemptAsync(SubmitQuizRequest submitQuizRequest)
    {
        var attempt = await _attemptRepo.GetByIdAsync(submitQuizRequest.AttemptId);
        if (attempt == null)
        {
            throw new ArgumentException("Quiz attempt not found", nameof(submitQuizRequest.AttemptId));
        }

        var quiz = await _quizRepo.GetByIdAsync(attempt.QuizId);
        if (quiz == null)
        {
            throw new ArgumentException("Quiz not found", nameof(attempt.QuizId));
        }

        // calculate score and correctness based on submitted answers
        var answers = submitQuizRequest.Answers;
        var optionIds = answers.Values.ToList();
        var options = await _optionRepo.GetByIdsAsync(optionIds);
        int correctAnswersCount = options.Count(o => o.IsCorrect);
        double scorePercentage = (double)correctAnswersCount / answers.Count * 100;
        bool isPassed = scorePercentage >= quiz.PassingScore;


        var submitQuizAttemptDto = new SubmitQuizAttemptDto
        {
            AttemptId = submitQuizRequest.AttemptId,
            CorrectAnswers = correctAnswersCount,
            ScorePercentage = scorePercentage,
            IsPassed = isPassed
        };

        attempt = QuizAttempt.Submit(attempt, submitQuizAttemptDto.CorrectAnswers, submitQuizAttemptDto.ScorePercentage, submitQuizAttemptDto.IsPassed);
        await _attemptRepo.UpdateAsync(attempt);
        return new QuizAttemptResponse
        {
            Id = attempt.Id,
            UserId = attempt.UserId,
            QuizId = attempt.QuizId,
            StartedAt = attempt.StartedAt,
            SubmittedAt = attempt.SubmittedAt,
            CorrectAnswers = attempt.CorrectAnswers,
            ScorePercentage = attempt.ScorePercentage,
            IsPassed = attempt.IsPassed,
            CreatedAt = attempt.CreatedAt,
            UpdatedAt = attempt.UpdatedAt
        };
    }

    public async Task<QuestionOptionDto> UpdateOptionAsync(UpdateOptionDto updateOptionDto)
    {
        var option = await _optionRepo.GetByIdAsync(updateOptionDto.OptionId);
        if (option == null)
        {
            throw new ArgumentException("Option not found", nameof(updateOptionDto.OptionId));
        }
        option = QuestionOption.Update(option, updateOptionDto.OptionText, updateOptionDto.IsCorrect, updateOptionDto.OrderIndex);
        await _optionRepo.UpdateAsync(option);
        return new QuestionOptionDto
        {
            Id = option.Id,
            QuestionId = option.QuestionId,
            OptionText = option.OptionText,
            IsCorrect = option.IsCorrect,
            OrderIndex = option.OrderIndex,
            CreatedAt = option.CreatedAt,
            UpdatedAt = option.UpdatedAt
        };
    }

    public async Task<QuestionDto> UpdateQuestionAsync(UpdateQuestionDto updateQuestionDto)
    {
        var question = await _questionRepo.GetByIdAsync(updateQuestionDto.QuestionId);
        if (question == null)
        {
            throw new ArgumentException("Question not found", nameof(updateQuestionDto.QuestionId));
        }
        question = Question.Update(question, updateQuestionDto.QuestionText, updateQuestionDto.QuestionType, updateQuestionDto.Points, updateQuestionDto.OrderIndex);
        await _questionRepo.UpdateAsync(question);
        return new QuestionDto
        {
            Id = question.Id,
            QuizId = question.QuizId,
            QuestionText = question.QuestionText,
            QuestionType = question.QuestionType,
            Points = question.Points,
            OrderIndex = question.OrderIndex,
            CreatedAt = question.CreatedAt,
            UpdatedAt = question.UpdatedAt
        };
    }

    public async Task<QuizDto> UpdateQuizAsync(UpdateQuizDto updateQuizDto)
    {
        var quiz = await _quizRepo.GetByIdAsync(updateQuizDto.QuizId);
        if (quiz == null)
        {
            throw new ArgumentException("Quiz not found", nameof(updateQuizDto.QuizId));
        }
        quiz = Quiz.Update(quiz, updateQuizDto.Title, updateQuizDto.Description, updateQuizDto.PassingScore, updateQuizDto.TimeLimitMinutes, updateQuizDto.MaxAttempts, updateQuizDto.IsActive);
        await _quizRepo.UpdateAsync(quiz);
        return new QuizDto
        {
            Id = quiz.Id,
            Title = quiz.Title,
            Description = quiz.Description,
            PassingScore = quiz.PassingScore,
            TimeLimitMinutes = quiz.TimeLimitMinutes,
            MaxAttempts = quiz.MaxAttempts,
            IsActive = quiz.IsActive,
            CreatedAt = quiz.CreatedAt,
            UpdatedAt = quiz.UpdatedAt
        };
    }
}