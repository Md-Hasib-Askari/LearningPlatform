using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("")]
public class QuizController : ControllerBase
{
    private readonly IQuizService _quizService;
    private readonly ILogger<QuizController> _logger;

    public QuizController(IQuizService quizService, ILogger<QuizController> logger)
    {
        _quizService = quizService;
        _logger = logger;
    }

    // Quiz Management Endpoints

    [HttpGet("quizzes/{quizId:guid}")]
    public async Task<IActionResult> GetQuizById(Guid quizId)
    {
        try
        {
            var quiz = await _quizService.GetQuizByIdAsync(quizId);
            return Ok(quiz);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("courses/{courseId:guid}/quizzes")]
    public async Task<IActionResult> GetQuizzesByCourse(Guid courseId)
    {
        try
        {
            var quizzes = await _quizService.GetQuizzesByCourseAsync(courseId);
            return Ok(quizzes);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost("courses/{courseId:guid}/quizzes")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> CreateQuiz(Guid courseId, [FromBody] CreateQuizDto createQuizDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            createQuizDto.CourseId = courseId;
            var quiz = await _quizService.CreateQuizAsync(createQuizDto);
            return CreatedAtAction(nameof(GetQuizById), new { quizId = quiz.Id }, quiz);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("quizzes/{quizId:guid}")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> UpdateQuiz(Guid quizId, [FromBody] UpdateQuizDto updateQuizDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            updateQuizDto.QuizId = quizId;
            var quiz = await _quizService.UpdateQuizAsync(updateQuizDto);
            return Ok(quiz);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("quizzes/{quizId:guid}")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> DeleteQuiz(Guid quizId)
    {
        try
        {
            await _quizService.DeleteQuizAsync(quizId);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // Question Management Endpoints

    [HttpPost("quizzes/{quizId:guid}/questions")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> AddQuestionToQuiz(Guid quizId, [FromBody] AddQuestionToQuizDto addQuestionDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            addQuestionDto.QuizId = quizId;
            var question = await _quizService.AddQuestionToQuizAsync(addQuestionDto);
            return CreatedAtAction(nameof(GetQuizById), new { quizId }, question);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("questions/{questionId:guid}")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> UpdateQuestion(Guid questionId, [FromBody] UpdateQuestionDto updateQuestionDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            updateQuestionDto.QuestionId = questionId;
            var question = await _quizService.UpdateQuestionAsync(updateQuestionDto);
            return Ok(question);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("questions/{questionId:guid}")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> DeleteQuestion(Guid questionId)
    {
        try
        {
            await _quizService.DeleteQuestionAsync(questionId);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // Question Option Management Endpoints

    [HttpPost("questions/{questionId:guid}/options")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> AddOptionToQuestion(Guid questionId, [FromBody] AddOptionDto addOptionDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            addOptionDto.QuestionId = questionId;
            var option = await _quizService.AddOptionToQuestionAsync(addOptionDto);
            return CreatedAtAction(nameof(GetQuizById), new { quizId = option.QuestionId }, option);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("options/{optionId:guid}")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> UpdateOption(Guid optionId, [FromBody] UpdateOptionDto updateOptionDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            updateOptionDto.OptionId = optionId;
            var option = await _quizService.UpdateOptionAsync(updateOptionDto);
            return Ok(option);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("options/{optionId:guid}")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> DeleteOption(Guid optionId)
    {
        try
        {
            await _quizService.DeleteOptionAsync(optionId);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // Quiz Attempt Endpoints

    [HttpPost("quizzes/{quizId:guid}/attempts/start")]
    [Authorize]
    public async Task<IActionResult> StartQuizAttempt(Guid quizId)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var startQuizDto = new StartQuizAttemptDto
            {
                UserId = userId,
                QuizId = quizId
            };
            var attempt = await _quizService.StartQuizAttemptAsync(startQuizDto);
            return Ok(attempt);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("quizzes/attempts/{attemptId:guid}/submit")]
    [Authorize]
    public async Task<IActionResult> SubmitQuizAttempt(Guid attemptId, [FromBody] SubmitQuizRequest submitRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            submitRequest.AttemptId = attemptId;
            var attempt = await _quizService.SubmitQuizAttemptAsync(submitRequest);
            return Ok(attempt);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("quizzes/attempts/{attemptId:guid}/result")]
    [Authorize]
    public async Task<IActionResult> GetQuizAttemptResult(Guid attemptId)
    {
        try
        {
            var attempt = await _quizService.GetQuizAttemptByIdAsync(attemptId);
            return Ok(attempt);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("quizzes/{quizId:guid}/attempts/my-attempts")]
    [Authorize]
    public async Task<IActionResult> GetMySingleQuizAttempts(Guid quizId)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var attempts = await _quizService.GetQuizAttemptsByUserAsync(userId);

            var filteredAttempts = attempts.Where(a => a.QuizId == quizId);
            return Ok(filteredAttempts);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
