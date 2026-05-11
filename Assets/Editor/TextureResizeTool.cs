#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class TextureResizeTool : EditorWindow
{
    private DefaultAsset targetFolder;

    private int textureSize = 512;

    private FilterMode filterMode = FilterMode.Bilinear;

    private TextureImporterCompression compression =
        TextureImporterCompression.Uncompressed;

    private int pixelsPerUnit = 100;

    [MenuItem("Tools/Texture Setup")]
    public static void ShowWindow()
    {
        GetWindow<TextureResizeTool>("Texture Setup");
    }

    private void OnGUI()
    {
        GUILayout.Label("Texture Auto Setup", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        targetFolder = (DefaultAsset)EditorGUILayout.ObjectField(
            "Target Folder",
            targetFolder,
            typeof(DefaultAsset),
            false
        );

        textureSize = EditorGUILayout.IntField(
            "Texture Size",
            textureSize
        );

        filterMode = (FilterMode)EditorGUILayout.EnumPopup(
            "Filter Mode",
            filterMode
        );

        compression =
            (TextureImporterCompression)EditorGUILayout.EnumPopup(
                "Compression",
                compression
            );

        pixelsPerUnit = EditorGUILayout.IntField(
            "Pixels Per Unit",
            pixelsPerUnit
        );

        EditorGUILayout.Space();

        if (GUILayout.Button("🔥 Setup Textures"))
        {
            SetupTextures();
        }
    }

    private void SetupTextures()
    {
        if (targetFolder == null)
        {
            Debug.LogError("❌ Folder is NULL");
            return;
        }

        string folderPath =
            AssetDatabase.GetAssetPath(targetFolder);

        string[] guids = AssetDatabase.FindAssets(
            "t:Texture2D",
            new[] { folderPath }
        );

        foreach (string guid in guids)
        {
            string path =
                AssetDatabase.GUIDToAssetPath(guid);

            TextureImporter importer =
                AssetImporter.GetAtPath(path)
                as TextureImporter;

            if (importer == null)
                continue;

            importer.textureType =
                TextureImporterType.Sprite;

            importer.spriteImportMode =
                SpriteImportMode.Single;

            importer.maxTextureSize =
                textureSize;

            importer.textureCompression =
                compression;

            importer.filterMode =
                filterMode;

            importer.spritePixelsPerUnit =
                pixelsPerUnit;

            importer.alphaIsTransparency = true;

            AssetDatabase.ImportAsset(path);

            Debug.Log($"✅ Setup: {path}");
        }

        AssetDatabase.Refresh();

        Debug.Log("🔥 ALL TEXTURES SETUP DONE!");
    }
}
#endif