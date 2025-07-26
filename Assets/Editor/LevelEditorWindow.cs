using UnityEditor;
using UnityEngine;

public class LevelEditorWindow : EditorWindow
{
    private GameObject blockPrefab;
    private int rows = 3;
    private int cols = 3;
    private int layers = 3;
    private float spacing = 2;
    private Vector3 origin = Vector3.zero;

    [MenuItem("Tools/Level Editor 3D")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditorWindow>("Level Editor 3D");
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Grid Settings", EditorStyles.boldLabel);

        blockPrefab = (GameObject)EditorGUILayout.ObjectField("Block Prefab", blockPrefab, typeof(GameObject), false);
        rows = EditorGUILayout.IntField("Rows (Z)", rows);
        cols = EditorGUILayout.IntField("Columns (X)", cols);
        layers = EditorGUILayout.IntField("Layers (Y)", layers);
        spacing = EditorGUILayout.FloatField("Spacing", spacing);
        origin = EditorGUILayout.Vector3Field("Origin", origin);

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate 3D Grid"))
        {
            if (blockPrefab == null)
            {
                Debug.LogError("❌ Block prefab is not assigned!");
                return;
            }

            GenerateGrid3D();
        }
    }

    private void GenerateGrid3D()
    {
        GameObject parent = new GameObject("LevelGrid");

        for (int y = 0; y < layers; y++)
        {
            for (int z = 0; z < rows; z++)
            {
                for (int x = 0; x < cols; x++)
                {
                    Vector3 spawnPos = origin + new Vector3(x * spacing, y * spacing, z * spacing);
                    GameObject blockGO = (GameObject)PrefabUtility.InstantiatePrefab(blockPrefab, parent.transform);
                    blockGO.transform.position = spawnPos;

                    Block blockScript = blockGO.GetComponent<Block>();
                    if (blockScript != null)
                    {
                        // Gán hướng di chuyển mặc định hoặc random nếu muốn
                        blockScript.moveDirection = Vector3.forward;
                    }
                }
            }
        }

        Debug.Log($"✅ Generated {rows * cols * layers} blocks in a {cols}x{layers}x{rows} grid.");
    }
}
