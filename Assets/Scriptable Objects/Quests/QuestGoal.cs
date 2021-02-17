using UnityEngine;

public class QuestGoal : ScriptableObject
{
    public Quest partOf;
    public bool Initialized;
    public bool done;
    public QuestCompletiontype goalType;
    public int countRequired;
    public GameObject goal;
    public Vector3 coordinate;
}