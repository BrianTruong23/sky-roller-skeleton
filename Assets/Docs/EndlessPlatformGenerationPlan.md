# Endless Platform Generation Plan

## Current Goal
Build an endless procedural platform system for the Sky Roller scene. The player moves forward on +Z, new platform sections spawn ahead, and old sections despawn behind the player.

## Division of Work

### User Tasks: Check and Confirm
- Open Unity and let scripts compile after code changes.
- Confirm the seven generated prefabs appear in `Assets/Prefabs/PlatformSections/`.
- Open each prefab and visually confirm the layout looks right.
- Press Play after each phase and report what looks wrong.
- Confirm gameplay feel: gaps, overlaps, hazard placement, notifications, and difficulty.

### Coding Agent Tasks: Heavy Lifting
- Create and maintain prefab-generation editor tooling.
- Generate the initial section prefabs automatically when missing.
- Implement runtime spawn/despawn logic in `PlatformGenerator`.
- Wire or document scene setup for generator references.
- Fix compile/import errors, invalid Unity GUIDs, and scene/script references.
- Adjust section spacing, prefab contents, hazards, and cleanup rules from user feedback.

## Phase Status

### Phase 1: Rules and Data
Status: In progress / mostly implemented.

Implemented:
- `PlatformSection` stores section length.
- `PlatformGenerator` stores generator rules and references.

Rules:
- Block spacing: `2.082125` units.
- Blocks per section: `4`.
- Section length: `8.3285` units.
- Keep `4` sections ahead.
- Keep `1` section behind.
- Avoid repeating the same prefab twice.

### Phase 2: Create Seven Platform Prefabs
Status: In progress.

The editor tool creates:
- `Section_Straight`
- `Section_Wide`
- `Section_Queue`
- `Section_Tree`
- `Section_SpeedBoost`
- `Section_Spikes`
- `Section_CoasterLaunch`

Your check:
- Confirm each prefab exists.
- Confirm each root has `PlatformSection`.
- Confirm floor blocks have colliders.
- Confirm queue/tree/speed/spike/coaster sections include their hazard objects.

### Phase 3: Runtime Generator
Status: Not implemented yet.

Agent will:
- Spawn starting sections.
- Spawn new sections ahead of the player.
- Destroy sections behind the player.
- Randomly pick section prefabs.
- Keep the hierarchy organized under `GeneratedLevel`.

User will:
- Confirm the path appears continuously during Play mode.
- Confirm old sections disappear behind the player.

### Phase 4: Scene Integration
Status: Not implemented yet.

Agent will:
- Help wire `PlatformGenerator` to Player and section prefabs.
- Decide how to disable or replace the old static `Level`.
- Adjust DeathZone for endless play.

User will:
- Confirm old level is disabled only when the generated path is working.

### Phase 5: Polish
Status: Future.

Agent will:
- Add prefab weights or difficulty ramping.
- Tune hazard frequency.
- Add more section variants.

User will:
- Playtest and confirm what feels fun.
