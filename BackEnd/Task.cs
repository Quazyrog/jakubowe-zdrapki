namespace BackEnd;

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