using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPlay : UIBase
{
    public static UIPlay instance;

    public TextMeshProUGUI txtNameLevel;

    [SerializeField] private string sceneName = "Loading";



    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }

    public void PlayGame()
    {
        LoadScene();
    }

    private void LoadScene()
    {
        GameData.Instance.NextScene = "Game";
        SceneManager.LoadScene(sceneName);
    }

    public void SetNameLevel()
    {
        int currentLevel = GameData.Instance.CurrentLevel;
        txtNameLevel.text = "Level " + currentLevel.ToString();
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        SetNameLevel();
    }

}