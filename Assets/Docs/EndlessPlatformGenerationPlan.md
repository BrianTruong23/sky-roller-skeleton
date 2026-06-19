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
Status: Implemented in code; scene wiring/testing comes next.

Agent will:
- Spawn starting sections.
- Spawn new sections ahead of the player.
- Destroy sections behind the player.
- Randomly pick section prefabs.
- Keep the hierarchy organized under `GeneratedLevel`.

Implemented:
- `PlatformGenerator` fills the starting runway from `startZ`.
- It keeps `sectionsAhead` generated in front of the player.
- It deletes old sections once they are farther back than `sectionsBehind`.
- It avoids spawning the same prefab twice in a row by default.
- It auto-finds the Player by tag if the reference is not assigned.
- It creates a `GeneratedLevel` parent automatically if none is assigned.

User will:
- Confirm the path appears continuously during Play mode.
- Confirm old sections disappear behind the player.

### Phase 4: Scene Integration
Status: Implemented; ready for Unity playtest.

Agent will:
- Help wire `PlatformGenerator` to Player and section prefabs.
- Decide how to disable or replace the old static `Level`.
- Adjust DeathZone for endless play.

Implemented:
- `GameScene` now has a `PlatformGenerator` object wired to the Player.
- The generator references all seven section prefabs.
- The old hand-built `Level` is disabled in `GameScene`.
- The old static hazard trigger root is disabled in `GameScene`.
- The previous hand-built test layout is preserved as `GameScene_ObjectTest`.

User will:
- Open `GameScene` and press Play to test endless generation.
- Open `GameScene_ObjectTest` if you want to return to the old fixed-path object test scene.

### Phase 5: Polish
Status: Future.

Agent will:
- Add prefab weights or difficulty ramping.
- Tune hazard frequency.
- Add more section variants.

User will:
- Playtest and confirm what feels fun.
