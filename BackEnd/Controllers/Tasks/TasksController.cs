using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers.Tasks;

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