using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_v2 : ScriptableObject
{
    public Quest partOf;
    public QuestCompletiontype goalType;
    public bool Initialized;
    public bool done;
    public bool isMainCampagneQuest;
    public GameObject goal;
    public Vector3 coordinate;
}
