using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [Header("References (wired in Phase 3)")]
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
    [SerializeField] float startZ = 0f;

    public float BlockSpacing => blockSpacing;
    public float SectionLength => blockSpacing * blocksPerSection;
    public int SectionsAhead => sectionsAhead;
    public int SectionsBehind => sectionsBehind;
    public bool AvoidSamePrefabTwice => avoidSamePrefabTwice;
    public float StartZ => startZ;
    public Transform Player => player;
    public PlatformSection[] SectionPrefabs => sectionPrefabs;
    public Transform SectionParent => sectionParent;
}
