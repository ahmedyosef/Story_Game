using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BatchSpriteSlicer : EditorWindow
{
    private int columns = 1;
    private int rows = 1;
    private bool useCellSize = false;
    private float pixelSizeX = 16f;
    private float pixelSizeY = 16f;

    [MenuItem("Tools/Batch Sprite Slicer")]
    public static void ShowWindow()
    {
        GetWindow<BatchSpriteSlicer>("Sprite Slicer");
    }

    void OnGUI()
    {
        GUILayout.Label("Select textures in Project window, then click Slice", EditorStyles.boldLabel);

        useCellSize = EditorGUILayout.Toggle("Use Cell Size (instead of Count)", useCellSize);

        if (useCellSize)
        {
            pixelSizeX = EditorGUILayout.FloatField("Pixel Size X", pixelSizeX);
            pixelSizeY = EditorGUILayout.FloatField("Pixel Size Y", pixelSizeY);
        }
        else
        {
            columns = EditorGUILayout.IntField("Columns", columns);
            rows = EditorGUILayout.IntField("Rows", rows);
        }

        if (GUILayout.Button("Slice Selected Textures"))
        {
            SliceSelectedTextures();
        }
    }

    void SliceSelectedTextures()
    {
        Object[] selected = Selection.GetFiltered(typeof(Texture2D), SelectionMode.Assets);

        if (selected.Length == 0)
        {
            EditorUtility.DisplayDialog("No Textures Selected", "Please select one or more texture assets in the Project window.", "OK");
            return;
        }

        foreach (Texture2D tex in selected)
        {
            string assetPath = AssetDatabase.GetAssetPath(tex);
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

            if (importer == null) continue;

            // Configure importer
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.spritePixelsPerUnit = 100; // adjust if needed
            importer.filterMode = FilterMode.Point; // optional: for pixel art

            // Clear existing sprites
            importer.spritesheet = new SpriteMetaData[0];

            // Apply slicing
            if (useCellSize)
            {
                importer.spritesheet = GenerateSpriteSheetByCellSize(tex, pixelSizeX, pixelSizeY);
            }
            else
            {
                importer.spritesheet = GenerateSpriteSheetByCellCount(tex, columns, rows);
            }

            EditorUtility.SetDirty(importer);
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }

        Debug.Log($"Sliced {selected.Length} texture(s) successfully!");
    }

    SpriteMetaData[] GenerateSpriteSheetByCellCount(Texture2D texture, int colCount, int rowCount)
    {
        int width = texture.width;
        int height = texture.height;
        float pixelSizeX = (float)width / colCount;
        float pixelSizeY = (float)height / rowCount;

        List<SpriteMetaData> sheets = new List<SpriteMetaData>();

        for (int y = 0; y < rowCount; y++)
        {
            for (int x = 0; x < colCount; x++)
            {
                SpriteMetaData smd = new SpriteMetaData();
                smd.rect = new Rect(x * pixelSizeX, (rowCount - 1 - y) * pixelSizeY, pixelSizeX, pixelSizeY);
                smd.pivot = new Vector2(0.5f, 0.5f);
                smd.alignment = (int)SpriteAlignment.Center;
                smd.name = $"Sprite_{y}_{x}";
                sheets.Add(smd);
            }
        }

        return sheets.ToArray();
    }

    SpriteMetaData[] GenerateSpriteSheetByCellSize(Texture2D texture, float cellWidth, float cellHeight)
    {
        int width = texture.width;
        int height = texture.height;
        int colCount = Mathf.FloorToInt((float)width / cellWidth);
        int rowCount = Mathf.FloorToInt((float)height / cellHeight);

        List<SpriteMetaData> sheets = new List<SpriteMetaData>();

        for (int y = 0; y < rowCount; y++)
        {
            for (int x = 0; x < colCount; x++)
            {
                SpriteMetaData smd = new SpriteMetaData();
                smd.rect = new Rect(x * cellWidth, (rowCount - 1 - y) * cellHeight, cellWidth, cellHeight);
                smd.pivot = new Vector2(0.5f, 0.5f);
                smd.alignment = (int)SpriteAlignment.Center;
                smd.name = $"Sprite_{y}_{x}";
                sheets.Add(smd);
            }
        }

        return sheets.ToArray();
    }
}