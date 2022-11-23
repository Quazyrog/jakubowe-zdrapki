using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers;

public class Task
{
    public Task(DateOnly date,  string text)
    {
        Date = date;
        Text = text;
    }
    public DateOnly Date { get; set; }
    public string Text { get; set; }
}

[ApiController]
[Route("api/tasks/today")]
public class TasksController : ControllerBase
{
    private readonly ILogger<TasksController> _logger;

    public TasksController(ILogger<TasksController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public Task Get()
    {
        return new Task(new DateOnly(), "Niech przed modlitwą napiją się herbaty.");
    }
}