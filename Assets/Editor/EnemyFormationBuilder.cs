using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class EnemyFormationBuilder
{
    [MenuItem("Tools/Space Invaders/Build Beta (Windows)")]
    public static void BuildBeta()
    {
        string outputFolder = Path.Combine(Directory.GetCurrentDirectory(), "Builds", "BetaComplete");
        Directory.CreateDirectory(outputFolder);

        string[] scenes = EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = Path.Combine(outputFolder, "Space Invader Beta.exe"),
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        Debug.Log($"Beta build result: {report.summary.result}; output: {options.locationPathName}");
    }

    [MenuItem("Tools/Space Invaders/Build 55 Enemy Formation")]
    public static void BuildFormation()
    {
        const string prefabPath = "Assets/Prefabs/Enemy.prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        GameObject formation = GameObject.Find("EnemyFormation");
        if (prefab == null || formation == null)
        {
            Debug.LogError("Could not find Enemy.prefab or EnemyFormation in the open scene.");
            return;
        }

        Transform parent = formation.transform;
        Undo.RegisterFullObjectHierarchyUndo(formation, "Build Enemy Formation");
        for (int i = parent.childCount - 1; i >= 0; i--)
            Undo.DestroyObjectImmediate(parent.GetChild(i).gameObject);

        const int columns = 11;
        const int rows = 5;
        const float horizontalSpacing = 1.45f;
        const float verticalSpacing = 0.82f;
        const float topY = 3.35f;

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                GameObject enemy = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
                enemy.name = $"Enemy_R{row + 1}_C{column + 1}";
                enemy.tag = "Enemy";
                enemy.transform.localPosition = new Vector3(
                    (column - (columns - 1) / 2f) * horizontalSpacing,
                    topY - row * verticalSpacing,
                    0f);
            }
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        Debug.Log("Built and saved a 5-row by 11-column formation (55 enemies).");
    }
}
