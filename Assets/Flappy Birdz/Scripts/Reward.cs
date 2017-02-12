using System.Collections;
using System.Collections.Generic;

public class Goal
{
	private string name;
	private uint value;
	private string activation;
	private uint activationValue;
	private uint initialValue;

    

	public Goal (string name, uint initial_value, string activation, uint activation_value) 
	{
		this.name = name;
		this.activation = activation;
		this.activationValue = activation_value;
		value = initialValue = initial_value;
	}

    public Goal(string name, uint value, uint initial_value, string activation, uint activation_value)
    {
        this.name = name;
        this.activation = activation;
        this.activationValue = activation_value;
        this.value = value;
        initialValue = initial_value;
    }

    public Goal(Goal goal)
    {
        this.name = goal.name;
        this.activation = goal.activation;
        this.activationValue = goal.activationValue;
        this.initialValue = goal.initialValue;
    }

    public bool IsFinished()
	{
		bool result = false;
	 
		switch(activation) 
		{
		    case Achievement.ACTIVE_IF_GREATER_THAN: result = value > activationValue; break;
		    case Achievement.ACTIVE_IF_LESS_THAN: result = value < activationValue; break;
		    case Achievement.ACTIVE_IF_EQUALS_TO: result = value == activationValue; break;
		}
 
		return result;
	}

    public string Name
    {
        get { return name; }
    }

    public uint Value
    {
        get { return value; }
    }

    public uint InitialValue
    {
        get { return initialValue; }
    }

    public uint ActivationValue
    {
        get { return activationValue; }
    }

    public string Activation
    {
        get { return activation; }
        
    }
}


public class Milestone
{
    private string title; // milestone title
    private string description; // milestone description
    private List<Goal> goals; // array of related goals
    private bool completed; // milestone is completed or not

    public Milestone (string title, string description, List<Goal> goals)
    {
        this.title = title;
        this.description = description;
        this.goals = goals;
        completed = false;
    }

    public Milestone(string title, string description, List<Goal> goals, bool completed)
    {
        this.title = title;
        this.description = description;
        this.goals = goals;
        this.completed = completed;
    }

    public Milestone(Milestone milestone)
    {
        this.title = milestone.title;
        this.description = milestone.description;
        this.goals = milestone.goals;
        this.completed = milestone.completed;
    }

    public string Title
    {
        get { return title; }
    }

    public string Description
    {
        get { return description; }
    }

    public List<Goal> Goals
    {
        get { return goals; }
    }

    public bool IsCompleted
    {
        get { return completed; }
    }
}


public class Achievement
{
	// activation rules
	public const string ACTIVE_IF_GREATER_THAN= ">";
	public const string ACTIVE_IF_LESS_THAN = "<";
	public const string ACTIVE_IF_EQUALS_TO = "==";

	private Dictionary<string, Goal> goals; // dictionary of properties
	private Dictionary<string, Milestone> milestones; // dictionary of achievements

	public Achievement () 
	{
        goals = new Dictionary<string, Goal>();
        milestones = new Dictionary<string, Milestone>();
	}
	
	public void DefineGoal(string name, uint initial_value, string activation_mode, uint value)
	{
        goals[name] = new Goal(name, initial_value, activation_mode, value);
	}

    public void DefineGoal(Goal goal)
    {
        goals[goal.Name] = new Goal(goal);
    }

    public void DefineMilestone(string title, string description, List<Goal> goals)
	{
		milestones[title] = new Milestone(title, description, goals);
	}

    public void DefineMilestone(Milestone milestone)
    {
        milestones[milestone.Title] = new Milestone(milestone);
    }

    public void ResetGoals()
    {

    }

	public uint GetValue (string name)
	{
        return goals[name].Value;
	}

	public void SetValue(string name, uint value) 
	{
		// Which activation rule?
		switch(goals[name].Activation) 
		{
			case ACTIVE_IF_GREATER_THAN:
                value = value > goals[name].Value ? value : goals[name].Value;
			break;
			case ACTIVE_IF_LESS_THAN:
                value = value < goals[name].Value ? value : goals[name].Value;
			break;
		}

        Goal g = goals[name];
        goals[name] = new Goal(g.Name, value, g.InitialValue, g.Activation, g.ActivationValue);
	}

	public void AddValue(List<Goal> goals, uint value) 
	{
		for (int i = 0; i < goals.Count; i++) 
		{
			string name = goals[i].Name;
			SetValue(name, GetValue(name) + value);
		}
	}

	public List<Milestone> CheckMilestones() 
	{
        List<Milestone> achievements = new List<Milestone>();

		foreach (KeyValuePair<string, Milestone> table in milestones) 
		{
            Milestone milestone = table.Value;

			if (!milestone.IsCompleted) 
			{
				int finishedGoals = 0;

				for (int i = 0; i < milestone.Goals.Count; i++) 
				{
					Goal goal = goals[milestone.Goals[i].Name];

					if (goal.IsFinished()) 
					{
                        finishedGoals++;
					}
				}

				if (finishedGoals == milestone.Goals.Count) 
				{
                    Milestone m = milestone;
                    milestone = new Milestone(m.Title, m.Description, m.Goals, true);
					achievements.Add(milestone);
				}
			}
		}
		
		return achievements;
	}
}