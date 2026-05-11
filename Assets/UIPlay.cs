using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPlay : UIBase
{
    public static UIPlay instance;
    [Header("Scene")]
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


}