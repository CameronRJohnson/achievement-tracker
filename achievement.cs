using System.Collections.Generic;

public class Goal
{
    public string Description { get; set; }
    public bool IsCompleted { get; set; }

    public Goal(string description, bool isCompleted = false)
    {
        Description = description;
        IsCompleted = isCompleted;
    }
}

public class Achievement
{
    public string Title { get; set; }
    public List<Goal> Goals { get; set; }

    public Achievement(string title)
    {
        Title = title;
        Goals = new List<Goal>();
    }
}

