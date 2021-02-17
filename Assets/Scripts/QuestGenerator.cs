using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

public class QuestGenerator : EditorWindow
{
    #region Goals Variables
    private string goalName;
    private QuestCompletiontype goalType;
    private int countRequired;
    private GameObject goal;
    private Vector3 coordinate;
    public int goalAmount = 1;
    private bool isDrawn = false;
    private List<QuestGoal> createdGoalsList = new List<QuestGoal>();
    private List<string> goalNames = new List<string>();
    private List<QuestCompletiontype> goalTypes = new List<QuestCompletiontype>();
    private List<int> countsRequired = new List<int>();
    private List<GameObject> goals = new List<GameObject>();
    private List<Vector3> coordinates = new List<Vector3>();
    #endregion


    #region Quest Variables
    private string qTitle;
    private string qDescription;
    private List<ItemObject> qItemReward = new List<ItemObject>();
    private Item qItemRewardSingle;
    private int qGoldReward;
    private int qExperienceReward;
    private int qRewardAmount;
    private bool isGoalsFin = false;
    private bool isRewardAmountSet = false;
    private Vector2 scrollPosition;
    #endregion

    [MenuItem("Item Tools/Goal Creator v2")]
    public static void ShowWindow()
    {
        GetWindow(typeof(QuestGenerator));
    }

    public void OnGUI()
    {
        if (!Directory.Exists("Assets/Quests"))
        {
            Directory.CreateDirectory("Assets/Quests");
        }
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height));
        goalAmount = EditorGUILayout.IntField("Amount of Goals", goalAmount);

        if (GUILayout.Button("Set Goal Amount"))
        {
            for (int i = 0; i < goalAmount; i++)
            {
                goalNames.Add(EditorGUILayout.TextField("Name of the Goal", goalName));
                goalTypes.Add((QuestCompletiontype)EditorGUILayout.EnumPopup("Goal Name", goalType));
                countsRequired.Add(EditorGUILayout.IntField("Goal Amount neded", countRequired));
                goals.Add((GameObject)EditorGUILayout.ObjectField("Object Goal (Prefab)", goal, typeof(GameObject), false));
                coordinates.Add(EditorGUILayout.Vector3Field("Corrdinates", coordinate));
            }

            isDrawn = true;
        }

        if (isDrawn)
        {
            for (int i = 0; i < goalAmount; i++)
            {
                GUILayout.Label("Qoal" + i, EditorStyles.boldLabel);
                goalNames[i] = EditorGUILayout.TextField("Name of the Goal", goalNames[i]);
                goalTypes[i] = (QuestCompletiontype)EditorGUILayout.EnumPopup("Goal Name", goalTypes[i]);
                countsRequired[i] = EditorGUILayout.IntField("Goal Amount neded", countsRequired[i]);
                goals[i] = (GameObject)EditorGUILayout.ObjectField("Object Goal (Prefab)", goals[i], typeof(GameObject), false);
                coordinates[i] = EditorGUILayout.Vector3Field("Corrdinates", coordinates[i]);
            }
        }

        if (GUILayout.Button("Create Goals"))
        {
            for (int i = 0; i < goalAmount; i++)
            {
                QuestGoal temp = (QuestGoal)ScriptableObject.CreateInstance(typeof(QuestGoal));
                temp.goalType = goalTypes[i];
                temp.countRequired = countsRequired[i];
                temp.goal = goals[i];
                temp.coordinate = coordinates[i];
                createdGoalsList.Add(temp);
                if(!Directory.Exists("Assets/Quests/Goals"))
                {
                    Directory.CreateDirectory("Assets/Quests/Goals");
                }
                AssetDatabase.CreateAsset(temp, "Assets/Quests/Goals/goal_" + goalNames[i] + ".asset");
            }
            AssetDatabase.SaveAssets();

            isGoalsFin = true;

        }

        if (isGoalsFin)
        {
            qTitle = EditorGUILayout.TextField("Questname", qTitle);
            qDescription = EditorGUILayout.TextField("Description", qDescription);
            qGoldReward = EditorGUILayout.IntField("Gold reward", qGoldReward);
            qExperienceReward = EditorGUILayout.IntField("XP reward", qExperienceReward);
            qRewardAmount = EditorGUILayout.IntField("Item reward amount", qRewardAmount);

            if (GUILayout.Button("Set Reward Armount"))
            {
                for (int j = 0; j < qRewardAmount; j++)
                {
                    qItemReward.Add((ItemObject)EditorGUILayout.ObjectField("Item reward (Prefab)", qItemRewardSingle, typeof(Item), false));

                }

                isRewardAmountSet = true;
            }

            if (isRewardAmountSet)
            {
                for (int i = 0; i < qRewardAmount; i++)
                {
                    qItemReward[i] = (ItemObject)EditorGUILayout.ObjectField("Item reward", qItemReward[i], typeof(Item), false);
                }
            }
        }

        if (GUILayout.Button("Create Quest"))
        {
            Quest tempQuest = (Quest)ScriptableObject.CreateInstance(typeof(Quest));
            tempQuest.title = qTitle;
            tempQuest.description = qDescription;
            tempQuest.experienceReward = qExperienceReward;
            tempQuest.goldReward = qGoldReward;
            tempQuest.goals = new List<QuestGoal>();
            tempQuest.goals = createdGoalsList;
            tempQuest.itemReward = new List<ItemObject>();
            for (int x = 0; x < qRewardAmount; x++)
            {
                tempQuest.itemReward.Add(qItemReward[x]);
            }
            if (!Directory.Exists("Assets/Quests"))
            {
                Directory.CreateDirectory("Assets/Quests");
            }

            string assetPathQuest = "Assets/Quests/" + qTitle + ".asset";
            AssetDatabase.CreateAsset(tempQuest, assetPathQuest);
            AssetDatabase.SaveAssets();
            this.Close();
        }

        GUILayout.EndScrollView();
    }
}
