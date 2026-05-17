using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

     private LevelDatabase levelDatabase;

    [Header("Runtime Level")]
    [SerializeField] private int currentLevel = 1;

    public LevelData CurrentLevelData { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        levelDatabase= GameData.Instance.ItemDatabase.levelDatabase;
        currentLevel = GameData.Instance.CurrentLevel;
        LoadLevel(currentLevel);
    }

    public void LoadLevel(int level)
    {
        CurrentLevelData = GetLevelData(level);

        if (CurrentLevelData == null)
        {
            Debug.LogError("Không tìm thấy LevelData level: " + level);
            return;
        }

        Debug.Log("Load Level: " + CurrentLevelData.level);
        Debug.Log("Stove Count: " + CurrentLevelData.stoveCount);
        Debug.Log("Total Item: " + CurrentLevelData.totalItem);
        Debug.Log("Play Time: " + CurrentLevelData.playTime);

        InitLevel();
    }

    private LevelData GetLevelData(int level)
    {
        for (int i = 0; i < levelDatabase.levels.Count; i++)
        {
            if (levelDatabase.levels[i].level == level)
                return levelDatabase.levels[i];
        }

        return null;
    }

    private void InitLevel()
    {
        // 1. Tạo số bếp
        GrillLevelManager.Instance.CreateStoves(CurrentLevelData.stoveCount);

        // 2. Tạo item cho level
        GrillLevelManager.Instance.SpawnItems(CurrentLevelData);

        // 3. Set timer
        if (CurrentLevelData.playTime > 0)
        {
            // TimerManager.Instance.StartTimer(CurrentLevelData.playTime);
        }

        // 4. Reset gameplay
        // ComboManager.Instance.ResetCombo();
        // UIManager.Instance.UpdateLevelUI(CurrentLevelData.level);
    }

    public void WinLevel()
    {
        currentLevel++;

        GameData.Instance.CurrentLevel = currentLevel;

        Debug.Log("WIN LEVEL");

        LoadLevel(currentLevel);
    }

    public void LoseLevel()
    {
        Debug.Log("LOSE LEVEL");

        // trừ energy nếu cần
        // UIMainMenu.Instance.UseEnergy(1);
    }
}