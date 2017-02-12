using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementController : MonoBehaviour {

    public static AchievementController Instance;

    public List<RawImage> rawImages;
    public Text title;
    public Text description;

    Animator animatorComponent;
    Achievement birdzAchievement;

    void Awake()
    {
        Instance = this;
        animatorComponent = GetComponent<Animator>();
        animatorComponent.SetBool("locked", true);
    }

	// Use this for initialization
	void Start ()
    {
        birdzAchievement = new Achievement();

        Goal goal = new Goal("Score25", 0, Achievement.ACTIVE_IF_EQUALS_TO, 25);
        birdzAchievement.DefineGoal(goal);
        birdzAchievement.DefineMilestone("Velvet", "Scored 25 points", new List<Goal> { goal });

        goal = new Goal("Score50", 0, Achievement.ACTIVE_IF_EQUALS_TO, 50);
        birdzAchievement.DefineGoal(goal);
        birdzAchievement.DefineMilestone("Cool", "Scored 50 points", new List<Goal> { goal });

        goal = new Goal("Score80", 0, Achievement.ACTIVE_IF_EQUALS_TO, 80);
        birdzAchievement.DefineGoal(goal);
        birdzAchievement.DefineMilestone("Funny", "Scored 80 points", new List<Goal> { goal });
    }

    public void Fill(Milestone m)
    {
        title.text = m.Title;
        description.text = m.Description;

        //int index = 2;
        int index = m.Title == "Funny" ? 2 : m.Title == "Cool" ? 1 : 0;
        Debug.Log("Index:" + index);

        for (int i = 0; i < rawImages.Count; i++)
        {
            rawImages[i].gameObject.SetActive(false);
        }

        rawImages[index].gameObject.SetActive(true);
        Toast();
    }

    public void Toast()
    {
        animatorComponent.SetBool("locked", false);
        Invoke("End", 2.5f);
    }

    public void End()
    {
        animatorComponent.SetBool("locked", true);
    }

    public Achievement BirdzAchievement
    {
        get { return birdzAchievement; }
    }
}
