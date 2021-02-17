using System.Collections.Generic;
using UnityEngine;

public class Quest : ScriptableObject
{
    public QuestCompletiontype completionType;
    public List<QuestGoal> goals;
    public string title;
    public string descr;
    public string description;
    public int experienceReward;
    public int goldReward;
    public List<ItemObject> itemReward;
}
