using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private List<UIBase> uiList = new();

    private Dictionary<Type, UIBase> uiDict = new();

    private void Awake()
    {
        Instance = this;

        InitDictionary();
    }

    // ================= INIT =================

    private void InitDictionary()
    {
        uiDict.Clear();

        foreach (UIBase ui in uiList)
        {
            if (ui == null)
                continue;

            Type type = ui.GetType();

            if (!uiDict.ContainsKey(type))
            {
                uiDict.Add(type, ui);
            }
        }
    }

    // ================= AUTO SCAN IN EDITOR =================

#if UNITY_EDITOR
    private void OnValidate()
    {
        uiList.Clear();

        UIBase[] allUI =
            GetComponentsInChildren<UIBase>(true);

        HashSet<UIBase> uniqueUI = new();

        foreach (UIBase ui in allUI)
        {
            if (ui == null)
                continue;

            if (uniqueUI.Contains(ui))
                continue;

            uniqueUI.Add(ui);
        }

        uiList.AddRange(uniqueUI);

        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif

    // ================= SHOW =================

    public T Show<T>() where T : UIBase
    {
        Type type = typeof(T);

        if (uiDict.TryGetValue(type, out UIBase ui))
        {
            ui.Show();
            return ui as T;
        }

        Debug.LogWarning($"UI Not Found: {type.Name}");
        return null;
    }

    // ================= HIDE =================

    public void Hide<T>() where T : UIBase
    {
        Type type = typeof(T);

        if (uiDict.TryGetValue(type, out UIBase ui))
        {
            ui.Hide();
        }
    }

    // ================= GET =================

    public T GetUI<T>() where T : UIBase
    {
        Type type = typeof(T);

        if (uiDict.TryGetValue(type, out UIBase ui))
        {
            return ui as T;
        }

        return null;
    }
}