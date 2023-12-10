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
        "Zrób rachunek sumienia i zaplanuj spowiedź w najbliższym tygodniu. Pomódl się o dobrą spowiedź oraz za spowiednika.",
        "Pójdź na spacer w ciszy (bez słuchawek, najlepiej z wyłączonym telefonem).",
        "Przeczytaj 1 Kor 15, 9-10. Zastanów się kim jesteś (w czym czujesz się wyjątkowy) i podziękuj Bogu za to, że takim Cię stworzył.",
        "Zaplanuj czas przed pójściem spać: ustal godzinę, kiedy skończysz pracę/naukę i zastanów się, co dla siebie w tym czasie zrobisz.",
        "Sporządź pisemnie listę Twoich zainteresowań.",
        "Ogranicz dzisiaj czas spędzony w Internecie do koniecznego minimum.",
        "Zastanów się, co masz do zrobienia na kolejny tydzień, zajmij się tym dzisiaj na tyle, by jutro mieć czas wolny dla siebie.",
        "Daj sobie przestrzeń na zadbanie o swoje pasje. Wybierz jedno z zainteresowań i spędź przy nim trochę czasu.",

                
        "Zaplanuj czas ze swoim przyjacielem, z którym dawno się nie widziałeś.",
        "Zastanów się w jaki sposób spotkanie z ludźmi pomogło Ci się zbliżyć do Boga i podziękuj mu za to. Rozważ Syr, 22,23.",
        "Przeczytaj Łk, 6, 31. Przygotuj coś miłego dla kogoś z kim wiesz, że dziś się spotkasz.",
        "Pomódl się o dar cierpliwości wobec konkretnego bliźniego.",
        "Podejmij dodatkowe wyrzeczenie w intencji osoby potrzebującej z Twojego otoczenia.",
        "Zadzwoń do rodziców lub dziadków i poświęć im trochę czasu.",
        "Zaplanuj z kimś wspólne czytanie Ewangelii i modlitwę.",

        "Pomyśl o pewnej swojej wadzie. Powierz ją Bogu. Przez następny tydzień postaraj się nad nią pracować.",
        "Przeczytaj dzisięjszą Ewangelię: Łk 1,5-25.",
        "W najbliższych dniach wybierz na roraty.",
        "Zaplanuj czas na spotkanie z Bogiem w ciszy.",
        "Postaraj się zachować ciszę od dzisiejszej modlitwy wieczornej do jutrzejszej modlitwy porannej.",
        "Przeczytaj Flp 2,5-11. Zastanów się jakiego Boga oczekujesz w czasie tych świąt.",
        "Przyjdź na Mszę 15 minut wcześniej, pomyśl o minionym tygodniu, z czym przychodzisz na tą Mszę.",
    };

    private readonly ILogger<CalendarsController> _logger;

    public CalendarsController(ILogger<CalendarsController> logger)
    {
        _logger = logger;
    }

    private string dayName(DateOnly date)
    {
        if (date == new DateOnly(2023, 12, 3)) return "I niedziela adwentu";
        if (date == new DateOnly(2023, 12,  10)) return "II niedziela adwentu";
        if (date == new DateOnly(2023, 12, 17)) return "III niedziela adwentu";
        if (date == new DateOnly(2023, 12, 24)) return "IV niedziela adwentu";
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
        DateOnly day = new DateOnly(2023, 12, 3);
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        
        var pl = new CultureInfo("PL-pl");
        var info = pl.DateTimeFormat;

        for (int i = 0; i < 22; ++i)
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
