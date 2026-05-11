using UnityEngine;

public class UISetting : UIBase
{
    [Header("Sound")]
    [SerializeField] private GameObject soundOn;
    [SerializeField] private GameObject soundOff;

    [Header("Vibration")]
    [SerializeField] private GameObject vibrationOn;
    [SerializeField] private GameObject vibrationOff;

    private void Start()
    {
        RefreshUI();
    }

    public void ButtonSoundClick()
    {
        GameData.Instance.IsSound = !GameData.Instance.IsSound;
        RefreshUI();
    }


    public void ButtonVibrationClick()
    {
        GameData.Instance.IsVibration = !GameData.Instance.IsVibration;
        RefreshUI();
    }

    private void RefreshUI()
    {
        bool isSound = GameData.Instance.IsSound;
        bool isVibration = GameData.Instance.IsVibration;

        if (soundOn != null) soundOn.SetActive(isSound);
        if (soundOff != null) soundOff.SetActive(!isSound);

        if (vibrationOn != null) vibrationOn.SetActive(isVibration);
        if (vibrationOff != null) vibrationOff.SetActive(!isVibration);
    }
}