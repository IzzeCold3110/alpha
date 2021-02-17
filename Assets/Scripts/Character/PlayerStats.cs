using System.Collections;
using UnityEngine;


public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [SerializeField]
    public Vector3 playerPos;

    [SerializeField]
    public int gold;

    [SerializeField]
    public float baseHitpoints;

    [SerializeField]
    public float currentHitpoints;

    [SerializeField]
    public float hitpointsBonus;

    [SerializeField]
    private float baseStamina;

    [SerializeField]
    private float currentStamina;

    [SerializeField]
    private float staminaBonus;

    [SerializeField]
    private float baseArmor;

    [SerializeField]
    private float currentArmor;

    [SerializeField]
    private float armorBonus;

    [SerializeField]
    private int baseExperience;

    [SerializeField]
    private int currentExperience;

    [SerializeField]
    private int experienceGainBonus;

    [SerializeField]
    public int level;

    [SerializeField]
    public string characterName;

    public PlayerReference playerRef;

    [SerializeField]
    public int[] levelProgressionArray = {100 , 500 , 750, 1200, 1800, 2500, 3000,
                                            4000, 5000, 6500, 7500, 8200, 9000, 10500,
                                                12000, 15000, 20000, 25000 ,29000, 300000 };
    bool isGettingTime;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
    
    public void Start()
    {
        playerRef.player = this.transform.gameObject;
        if (experienceGainBonus <= 0)
            experienceGainBonus = 1;

        if (level <= 0)
            level = 1;

        currentExperience = baseExperience;
    }

    private void Update()
    {
        if (isGettingTime)
            StartCoroutine(GetPlayerPos(1f));
    }

    IEnumerator GetPlayerPos(float time)
    {
        isGettingTime = true;
        yield return new WaitForSeconds(time);

        playerPos = this.transform.position;

        isGettingTime = false;
    }

    public void AddExperience(int xP)
    {
        currentExperience += xP * experienceGainBonus;

        for (int i = level - 1; i < levelProgressionArray.Length; i++)
        {
            try
            {
                if (currentExperience >= levelProgressionArray[i])
                {
                    level += 1;
                    LevelUp();
                }
            }
            catch
            {
                Debug.Log("Level " + level + " ist the current maximum level.");
            }
        }

    }


    public void AddHealth(int health)
    {
        hitpointsBonus += health;

        currentHitpoints = baseHitpoints + hitpointsBonus;

        currentHitpoints = Mathf.Clamp(currentHitpoints, 0, baseHitpoints + hitpointsBonus);
    }

    public void LevelUp()
    {
        baseStamina += level + 1;
        baseArmor += level + 1;
        baseHitpoints += level * 2;

        currentStamina = baseStamina + staminaBonus;
        currentHitpoints = baseHitpoints + hitpointsBonus;
        currentArmor = baseArmor + armorBonus;
    }

    public void IncreaseArmor(int armor)
    {
        armorBonus += armor;

        currentArmor = baseArmor + armorBonus;

        currentArmor = Mathf.Clamp(currentArmor, 0, Mathf.Infinity);
    }

    public void AddStamina(int stamina)
    {
        staminaBonus += stamina;

        currentStamina = baseStamina + staminaBonus;

        currentStamina = Mathf.Clamp(currentStamina, 0, Mathf.Infinity);
    }

    public void ConsumeStamina(int stamina)
    {

        currentStamina -= stamina;

        currentStamina = Mathf.Clamp(currentStamina, 0, Mathf.Infinity);
    }

    public void RendArmor(int armor)
    {

        currentArmor -= armor;

        currentArmor = Mathf.Clamp(currentArmor, 0, Mathf.Infinity);
    }

    public void SufferDamage(int damage)
    {
        //Damage reduction
        if (damage > currentArmor)
        {
            currentHitpoints += currentArmor - damage;
        }


        currentHitpoints = Mathf.Clamp(currentHitpoints, 0, baseHitpoints + hitpointsBonus);

        if (currentHitpoints == 0)
        {
            //Death Logic
        }
    }

    public void Heal(float healing)
    {
        currentHitpoints += healing;

        currentHitpoints = Mathf.Clamp(currentHitpoints, 0, baseHitpoints + hitpointsBonus);
    }
}
