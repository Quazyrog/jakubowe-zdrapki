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
    
    public DayInfo(DateOnly date, string name, string taskText)
    {
        Date = date;
        Name = name;
        TaskText = taskText;
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
    static string[] _tasks = new String[]
    {
        "Podziękuj Bogu za dar życia",
        "Zaplanuj dziś 15 min na modlitwę w ciągu dnia",
        "Zaplanuj odpowiednio długi i dobry sen",
        "Zaplanuj przynajmniej 30 min odpoczynku w ciągu dnia",
        "Ogranicz dzisiaj korzystanie z mediów społecznościowych do niezbędnego minimum",
        "Obejrzyj film z watykańskiej listy filmowej",
        "Odmów koronkę do Bożego Miłosierdzia w intencji dusz czyśćcowych",
        "Zadbaj o odświętny strój na Mszy",
        "Podziękuj rodzicom za przekazanie życia i/lub pomódl się za nich",
        "Porozmawiaj z kimś, z kim chcesz utrzymywać relację",
        "Włącz się w akcję pomocową dla potrzebujących",
        "Pomódl się o dar cierpliwości wobec konkretnego bliźniego",
        "Zaplanuj z kimś wspólne czytanie Ewangelii z dnia i modlitwę",
        "Pomódl się za kogoś, kto dziś wykonał pracę wobec Ciebie (np. sprzedawca w sklepie, kierowca autobusu, nauczyciel)",
        "Przyjmij komunię w intencji pokoju na świecie",
    };

    private readonly ILogger<CalendarsController> _logger;

    public CalendarsController(ILogger<CalendarsController> logger)
    {
        _logger = logger;
    }

    private string dayName(DateOnly date)
    {
        if (date == new DateOnly(2022, 11, 27)) return "I niedziela adwentu";
        if (date == new DateOnly(2022, 12,  4)) return "II niedziela adwentu";
        if (date == new DateOnly(2022, 12, 11)) return "III niedziela adwentu";
        if (date == new DateOnly(2022, 12, 18)) return "IV niedziela adwentu";
        switch (date.DayOfWeek)
        {
            case DayOfWeek.Sunday: return "Niedziela";
            case DayOfWeek.Monday: return "Poniedziałek";
            case DayOfWeek.Tuesday: return "Wtorek";
            case DayOfWeek.Wednesday: return "Środa";
            case DayOfWeek.Thursday: return "Czwartek";
            case DayOfWeek.Friday: return "Piątek";
            case DayOfWeek.Saturday: return "Sobota";
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    [HttpGet]
    public IEnumerable<DayInfo> Get(string name)
    {
        List<DayInfo> result = new List<DayInfo>();
        DateOnly day = new DateOnly(2022, 11, 27);
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        
        var pl = new CultureInfo("PL-pl");
        var info = pl.DateTimeFormat;

        for (int i = 0; i < 28; ++i)
        {
            DayInfo dayInfo = new DayInfo(day, dayName(day));
            if (i < _tasks.Length && day <= today)
            {
                dayInfo.TaskText = _tasks[i];
            }
            result.Add(dayInfo);
            day = day.AddDays(1);
        }

        return result;
    }
}