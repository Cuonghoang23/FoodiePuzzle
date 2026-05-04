using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // giữ object khi load scene
        }
        else
        {
            Destroy(gameObject); // tránh duplicate
        }
    }

    public string NextScene
    {
        get => PlayerPrefs.GetString("NextScene", "MainMenu"); 
        set
        {
            PlayerPrefs.SetString("NextScene", value);
            PlayerPrefs.Save();
        }
    }
    public int Coin
    {
        get => PlayerPrefs.GetInt("Coin", 0);
        set
        {
            PlayerPrefs.SetInt("Coin", value);
            PlayerPrefs.Save();
        }
    }

    public int CurrentLevel
    {
        get => PlayerPrefs.GetInt("CurrentLevel", 0);
        set
        {
            PlayerPrefs.SetInt("CurrentLevel", value);
            PlayerPrefs.Save();
        }
    }
}
