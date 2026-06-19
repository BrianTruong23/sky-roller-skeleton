#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

public static class CreatePlatformSectionPrefabs
{
    const string OutputFolder = "Assets/Prefabs/PlatformSections";
    const string BlockPath = "Assets/Sprites/FBX/block-grass-large-tall.fbx";
    const string QueuePath = "Assets/Sprites/FBX/queue-entrance.fbx";
    const string TreePath = "Assets/Sprites/FBX/tree-large.fbx";

    const float BlockSpacing = 2.082125f;
    const int BlockRows = 4;
    const float SectionLength = BlockSpacing * BlockRows;

    static readonly Vector3 ColliderSize = new Vector3(2.082125f, 2f, 2.082125f);
    static readonly Vector3 ColliderCenter = new Vector3(0f, 1f, 0f);

    [MenuItem("Sky Roller/Create Platform Section Prefabs")]
    public static void CreateAll()
    {
        GameObject blockSource = AssetDatabase.LoadAssetAtPath<GameObject>(BlockPath);
        GameObject queueSource = AssetDatabase.LoadAssetAtPath<GameObject>(QueuePath);
        GameObject treeSource = AssetDatabase.LoadAssetAtPath<GameObject>(TreePath);

        if (blockSource == null)
        {
            Debug.LogError("Missing block model at " + BlockPath);
            return;
        }

        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }

        if (!AssetDatabase.IsValidFolder(OutputFolder))
        {
            AssetDatabase.CreateFolder("Assets/Prefabs", "PlatformSections");
        }

        CreateStraightSection(blockSource);
        CreateWideSection(blockSource);
        CreateQueueSection(blockSource, queueSource);
        CreateTreeSection(blockSource, treeSource);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Created 4 platform section prefabs in " + OutputFolder);
    }

    static void CreateStraightSection(GameObject blockSource)
    {
        GameObject root = CreateSectionRoot("Section_Straight");
        for (int row = 0; row < BlockRows; row++)
        {
            PlaceBlock(blockSource, root.transform, 0f, RowZ(row), "Block_C_" + row);
        }

        SavePrefab(root, "Section_Straight.prefab");
    }

    static void CreateWideSection(GameObject blockSource)
    {
        GameObject root = CreateSectionRoot("Section_Wide");
        for (int row = 0; row < BlockRows; row++)
        {
            float z = RowZ(row);
            PlaceBlock(blockSource, root.transform, -BlockSpacing, z, "Block_L_" + row);
            PlaceBlock(blockSource, root.transform, 0f, z, "Block_C_" + row);
            PlaceBlock(blockSource, root.transform, BlockSpacing, z, "Block_R_" + row);
        }

        SavePrefab(root, "Section_Wide.prefab");
    }

    static void CreateQueueSection(GameObject blockSource, GameObject queueSource)
    {
        GameObject root = CreateSectionRoot("Section_Queue");
        for (int row = 0; row < BlockRows; row++)
        {
            PlaceBlock(blockSource, root.transform, 0f, RowZ(row), "Block_C_" + row);
        }

        if (queueSource != null)
        {
            GameObject queue = Object.Instantiate(queueSource, root.transform);
            queue.name = "queue-entrance";
            queue.transform.localPosition = new Vector3(0f, 0f, RowZ(2));

            BoxCollider trigger = queue.GetComponent<BoxCollider>();
            if (trigger == null)
            {
                trigger = queue.AddComponent<BoxCollider>();
            }

            trigger.isTrigger = true;
            trigger.center = new Vector3(0f, 2f, 0f);
            trigger.size = new Vector3(3f, 3f, 4f);

            if (queue.GetComponent<SlowZone>() == null)
            {
                queue.AddComponent<SlowZone>();
            }
        }

        SavePrefab(root, "Section_Queue.prefab");
    }

    static void CreateTreeSection(GameObject blockSource, GameObject treeSource)
    {
        GameObject root = CreateSectionRoot("Section_Tree");
        PlaceBlock(blockSource, root.transform, 0f, RowZ(0), "Block_C_0");
        PlaceBlock(blockSource, root.transform, 0f, RowZ(1), "Block_C_1");

        float treeRowZ = RowZ(2);
        PlaceBlock(blockSource, root.transform, -BlockSpacing, treeRowZ, "Block_L_2");
        PlaceBlock(blockSource, root.transform, BlockSpacing, treeRowZ, "Block_R_2");

        GameObject blocker = new GameObject("TreeBlockCenter");
        blocker.transform.SetParent(root.transform, false);
        blocker.transform.localPosition = new Vector3(0f, 1.75f, treeRowZ);
        BoxCollider blockerCollider = blocker.AddComponent<BoxCollider>();
        blockerCollider.size = new Vector3(1.7f, 3.5f, 2f);
        blocker.AddComponent<TreeBlocker>();

        if (treeSource != null)
        {
            GameObject tree = Object.Instantiate(treeSource, root.transform);
            tree.name = "tree-large";
            tree.transform.localPosition = new Vector3(0f, 0f, treeRowZ);

            CapsuleCollider trunk = tree.GetComponent<CapsuleCollider>();
            if (trunk == null)
            {
                trunk = tree.AddComponent<CapsuleCollider>();
            }

            trunk.isTrigger = false;
            trunk.radius = 0.85f;
            trunk.height = 3.5f;
            trunk.center = new Vector3(0f, 1.75f, 0f);

            if (tree.GetComponent<TreeBlocker>() == null)
            {
                tree.AddComponent<TreeBlocker>();
            }
        }

        PlaceBlock(blockSource, root.transform, 0f, RowZ(3), "Block_C_3");

        SavePrefab(root, "Section_Tree.prefab");
    }

    static GameObject CreateSectionRoot(string sectionName)
    {
        GameObject root = new GameObject(sectionName);
        PlatformSection section = root.AddComponent<PlatformSection>();

        SerializedObject serializedSection = new SerializedObject(section);
        serializedSection.FindProperty("lengthZ").floatValue = SectionLength;
        serializedSection.ApplyModifiedPropertiesWithoutUndo();

        return root;
    }

    static void PlaceBlock(GameObject blockSource, Transform parent, float x, float z, string blockName)
    {
        GameObject block = Object.Instantiate(blockSource, parent);
        block.name = blockName;
        block.transform.localPosition = new Vector3(x, 0f, z);

        BoxCollider collider = block.GetComponent<BoxCollider>();
        if (collider == null)
        {
            collider = block.AddComponent<BoxCollider>();
        }

        collider.isTrigger = false;
        collider.size = ColliderSize;
        collider.center = ColliderCenter;
    }

    static float RowZ(int row)
    {
        return row * BlockSpacing;
    }

    static void SavePrefab(GameObject root, string prefabFileName)
    {
        string path = Path.Combine(OutputFolder, prefabFileName);
        PrefabUtility.SaveAsPrefabAsset(root, path);
        Object.DestroyImmediate(root);
    }
}
#endif
