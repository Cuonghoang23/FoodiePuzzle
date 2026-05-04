using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance;

    [Header("UI")]
    public Image fillImage;
    public TMP_Text percentText;
    public string sceneName;

    [Header("Fake Loading")]
    public float fakeLoadTime = 2f; // thời gian fake loading

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        sceneName = GameData.Instance.NextScene;
        LoadScene(sceneName);
    }
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadAsync(sceneName));
    }

    IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        float timer = 0f;

        while (!op.isDone)
        {
            timer += Time.deltaTime;

            float realProgress = Mathf.Clamp01(op.progress / 0.9f);
            float fakeProgress = Mathf.Clamp01(timer / fakeLoadTime);

            float progress = Mathf.Min(realProgress, fakeProgress);

            fillImage.fillAmount = progress;

            // 🔥 update text %
            int percent = Mathf.RoundToInt(progress * 100f);
            percentText.text = percent.ToString("00") + "%";

            if (realProgress >= 1f && fakeProgress >= 1f)
            {
                yield return new WaitForSeconds(0.2f);
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}