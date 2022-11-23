using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BackEnd.Controllers;

public class DayInfo
{
    public DayInfo(DateOnly date, string name)
    {
        Date = date;
        Name = name;
    }

    public int WeekNumber => 
        new GregorianCalendar()
            .GetWeekOfYear(Date.ToDateTime(new TimeOnly()), CalendarWeekRule.FirstDay, System.DayOfWeek.Monday);
    
    public DateOnly Date { get; set; }
    public string Name { get; set; }
    public string? TaskText { get; set; }
}

[ApiController]
[Route("api/calendars/{name}")]
public class CalendarsController : ControllerBase
{
    private readonly ILogger<CalendarsController> _logger;

    public CalendarsController(ILogger<CalendarsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<DayInfo> Get(string name)
    {
        List<DayInfo> result = new List<DayInfo>();
        DateOnly day = new DateOnly(2022, 11, 27);
        
        for (int i = 0; i < 28; ++i)
        {
            DayInfo dayInfo = new DayInfo(day, day.DayOfWeek.ToString());
            if (i < 14)
            {
                dayInfo.TaskText = "Napij się herbaty przed modlitwą";
            }
            result.Add(dayInfo);
            day = day.AddDays(1);
        }

        return result;
    }
}