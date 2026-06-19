using UnityEngine;
using System.Collections.Generic;

public class PlatformGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform player;
    [SerializeField] PlatformSection[] sectionPrefabs;
    [SerializeField] Transform sectionParent;

    [Header("Layout rules")]
    [Tooltip("Distance between block centers on Z. Matches block-grass-large-tall spacing.")]
    [SerializeField] float blockSpacing = 2.082125f;

    [Tooltip("Number of block rows per section prefab.")]
    [SerializeField] int blocksPerSection = 4;

    [Header("Spawn / despawn rules")]
    [Tooltip("How many section lengths to keep spawned in front of the player.")]
    [SerializeField] int sectionsAhead = 4;

    [Tooltip("How many section lengths to keep behind the player before deleting.")]
    [SerializeField] int sectionsBehind = 1;

    [Tooltip("Do not pick the same section prefab twice in a row.")]
    [SerializeField] bool avoidSamePrefabTwice = true;

    [Header("Starting path")]
    [Tooltip("World Z where the first generated section begins.")]
    [SerializeField] float startZ = -2.082125f;

    readonly Queue<SpawnedSection> activeSections = new Queue<SpawnedSection>();
    float nextSpawnZ;
    int lastPrefabIndex = -1;

    public float BlockSpacing => blockSpacing;
    public float SectionLength => blockSpacing * blocksPerSection;
    public int SectionsAhead => sectionsAhead;
    public int SectionsBehind => sectionsBehind;
    public bool AvoidSamePrefabTwice => avoidSamePrefabTwice;
    public float StartZ => startZ;
    public Transform Player => player;
    public PlatformSection[] SectionPrefabs => sectionPrefabs;
    public Transform SectionParent => sectionParent;

    void Awake()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        if (sectionParent == null)
        {
            GameObject parentObject = new GameObject("GeneratedLevel");
            sectionParent = parentObject.transform;
        }
    }

    void Start()
    {
        if (!CanGenerate())
        {
            enabled = false;
            return;
        }

        nextSpawnZ = startZ;
        SpawnSectionsAhead();
    }

    void Update()
    {
        if (!CanGenerate())
        {
            return;
        }

        SpawnSectionsAhead();
        DespawnSectionsBehind();
    }

    void SpawnSectionsAhead()
    {
        float spawnAheadZ = player.position.z + sectionsAhead * SectionLength;
        while (nextSpawnZ < spawnAheadZ)
        {
            SpawnNextSection();
        }
    }

    void SpawnNextSection()
    {
        int prefabIndex = PickPrefabIndex();
        PlatformSection prefab = sectionPrefabs[prefabIndex];
        PlatformSection section = Instantiate(
            prefab,
            new Vector3(0f, 0f, nextSpawnZ),
            Quaternion.identity,
            sectionParent
        );

        section.name = $"{prefab.name}_{activeSections.Count:00}";

        float length = Mathf.Max(0.1f, section.LengthZ);
        activeSections.Enqueue(new SpawnedSection(section.gameObject, nextSpawnZ, nextSpawnZ + length));
        nextSpawnZ += length;
        lastPrefabIndex = prefabIndex;
    }

    int PickPrefabIndex()
    {
        if (sectionPrefabs.Length == 1 || !avoidSamePrefabTwice || lastPrefabIndex < 0)
        {
            return Random.Range(0, sectionPrefabs.Length);
        }

        int prefabIndex = Random.Range(0, sectionPrefabs.Length);
        if (prefabIndex == lastPrefabIndex)
        {
            prefabIndex = (prefabIndex + Random.Range(1, sectionPrefabs.Length)) % sectionPrefabs.Length;
        }

        return prefabIndex;
    }

    void DespawnSectionsBehind()
    {
        float despawnBehindZ = player.position.z - sectionsBehind * SectionLength;
        while (activeSections.Count > 0 && activeSections.Peek().EndZ < despawnBehindZ)
        {
            SpawnedSection oldSection = activeSections.Dequeue();
            if (oldSection.GameObject != null)
            {
                Destroy(oldSection.GameObject);
            }
        }
    }

    bool CanGenerate()
    {
        return player != null && sectionPrefabs != null && sectionPrefabs.Length > 0;
    }

    void OnValidate()
    {
        blockSpacing = Mathf.Max(0.1f, blockSpacing);
        blocksPerSection = Mathf.Max(1, blocksPerSection);
        sectionsAhead = Mathf.Max(1, sectionsAhead);
        sectionsBehind = Mathf.Max(0, sectionsBehind);
    }

    readonly struct SpawnedSection
    {
        public SpawnedSection(GameObject gameObject, float startZ, float endZ)
        {
            GameObject = gameObject;
            StartZ = startZ;
            EndZ = endZ;
        }

        public GameObject GameObject { get; }
        public float StartZ { get; }
        public float EndZ { get; }
    }
}
