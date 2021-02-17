using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

public class QuestGenerator_V1 : EditorWindow
{
    #region Goals Variables
    string goalName;
    QuestCompletiontype goalType;
    int countRequired;
    GameObject goal;
    Vector3 coordinate;
    public int goalAmount = 1;
    bool isDrawn = false;

    List<QuestGoal> createdGoalsList = new List<QuestGoal>();

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

    bool isGoalsFin = false;
    bool isRewardAmountSet = false;

    Vector2 scrollPosition;
    #endregion

    [MenuItem("Item Tools/Goal Creator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(QuestGenerator_V1));

    }

    public void OnGUI()
    {

        if (!Directory.Exists("Assets/Quests"))
        {
            Directory.CreateDirectory("Assets/Quests");
        }
        //Scrolling Start
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height));
        goalAmount = EditorGUILayout.IntField("Amount of Goals", goalAmount);

        if (GUILayout.Button("Set Goal Amount"))
        {
            //Creates the Input Fields
            for (int i = 0; i < goalAmount; i++)
            {
                //GUILayout.Label("\nGoal" + i, EditorStyles.boldLabel);
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
                //Draws the Input fields and we can fill them now
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
                    qItemReward.Add((ItemObject)EditorGUILayout.ObjectField("Item reward (Prefab)", qItemRewardSingle, typeof(ItemObject), false));

                }

                isRewardAmountSet = true;
            }

            if (isRewardAmountSet)
            {
                for (int i = 0; i < qRewardAmount; i++)
                {
                    qItemReward[i] = (ItemObject)EditorGUILayout.ObjectField("Item reward", qItemReward[i], typeof(ItemObject), false);
                }
            }

        }

        if (GUILayout.Button("Create Quest"))
        {

            //in for
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

        //End Scrolling
        GUILayout.EndScrollView();
    }



}
