using FluentValidation;

namespace TaskManagementAPI.DTOs;

public class TodoDto
{
    public required string Title { get; set; }
    
    public required string Duration { get; set; }
    
}

public class TodoDtoValidator : AbstractValidator<TodoDto>
{
    public TodoDtoValidator()
    {
        RuleFor(todo => todo.Title)
            .NotEmpty()
            .WithMessage("Title should not be empty.")
            .Matches(@"^[a-zA-Z0-9\s]+$")
            .WithMessage("Title must only contain letters, numbers, and spaces.");
        RuleFor(todo => todo.Duration)
            .NotEmpty()
            .WithMessage("Duration should not be empty.")
            .MinimumLength(6);
    }
}