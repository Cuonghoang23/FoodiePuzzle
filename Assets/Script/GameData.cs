using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;
    public BoosterData boosterData;
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
    public int LevelUnlock
    {
        get => PlayerPrefs.GetInt("LevelUnlock", 1);
        set
        {
            PlayerPrefs.SetInt("LevelUnlock", value);
            PlayerPrefs.Save();
        }
    }

    #region num Booster
    private string GetBoosterCountKey(BoosterType type)
    {
        return $"Booster_{type}_Count";
    }

    private string GetBoosterStateKey(BoosterType type)
    {
        return $"Booster_{type}_Active";
    }

    private string GetBoosterTimeKey(BoosterType type)
    {
        return $"Booster_{type}_Time";
    }

    public int GetBoosterCount(BoosterType type)
    {
        return PlayerPrefs.GetInt(GetBoosterCountKey(type), 0);
    }

    public void SetBoosterCount(BoosterType type, int value)
    {
        PlayerPrefs.SetInt(GetBoosterCountKey(type), value);
        PlayerPrefs.Save();
    }

    public bool IsBoosterActive(BoosterType type)
    {
        return PlayerPrefs.GetInt(GetBoosterStateKey(type), 0) == 1;
    }

    public void SetBoosterActive(BoosterType type, bool value)
    {
        PlayerPrefs.SetInt(GetBoosterStateKey(type), value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public float GetBoosterTime(BoosterType type)
    {
        return PlayerPrefs.GetFloat(GetBoosterTimeKey(type), 0f);
    }

    public void SetBoosterTime(BoosterType type, float value)
    {
        PlayerPrefs.SetFloat(GetBoosterTimeKey(type), value);
        PlayerPrefs.Save();
    }
    #endregion

    #region setting

    public bool IsMusic
    {
        get => PlayerPrefs.GetInt("IsMusicOn", 1) == 1;
        set
        {
            PlayerPrefs.SetInt("IsMusicOn", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public bool IsSound
    {
        get => PlayerPrefs.GetInt("IsSoundOn", 1) == 1;
        set
        {
            PlayerPrefs.SetInt("IsSoundOn", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public bool IsVibration
    {
        get => PlayerPrefs.GetInt("IsVibrationOn", 1) == 1;
        set
        {
            PlayerPrefs.SetInt("IsVibrationOn", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    #endregion



}
