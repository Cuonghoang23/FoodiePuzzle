using UnityEngine;
using UnityEditor;
using System.IO;

public class BulkRenameImages : EditorWindow
{
    private string folderPath = "Assets/";
    private string baseName = "image";
    private int startIndex = 1;
    private int padding = 2; // 01, 02, 03

    [MenuItem("Tools/Bulk Rename Images")]
    public static void ShowWindow()
    {
        GetWindow<BulkRenameImages>("Bulk Rename Images");
    }

    private void OnGUI()
    {
        GUILayout.Label("Rename Images Tool", EditorStyles.boldLabel);

        folderPath = EditorGUILayout.TextField("Folder Path", folderPath);
        baseName = EditorGUILayout.TextField("Base Name", baseName);
        startIndex = EditorGUILayout.IntField("Start Index", startIndex);
        padding = EditorGUILayout.IntField("Number Padding", padding);

        if (GUILayout.Button("Rename Images"))
        {
            RenameImages();
        }
    }

    private void RenameImages()
    {
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Debug.LogError("❌ Folder không hợp lệ!");
            return;
        }

        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), folderPath);

        string[] files = Directory.GetFiles(fullPath);

        int index = startIndex;

        foreach (string file in files)
        {
            if (file.EndsWith(".meta")) continue;

            string ext = Path.GetExtension(file).ToLower();

            // chỉ xử lý ảnh
            if (ext != ".png" && ext != ".jpg" && ext != ".jpeg")
                continue;

            string newName = baseName + index.ToString().PadLeft(padding, '0');
            string assetPath = file.Replace(Directory.GetCurrentDirectory() + "\\", "").Replace("\\", "/");

            string error = AssetDatabase.RenameAsset(assetPath, newName);

            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError("Rename lỗi: " + error);
            }

            index++;
        }

        AssetDatabase.Refresh();
        Debug.Log("✅ Rename hoàn tất!");
    }
}