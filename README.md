# The Horde — 2D Top-Down Tactical Wave Shooter

A 2D top-down wave-defense shooter developed in **Unity** with **C#**. The game features decoupled cursor-based aiming, frame-independent vector physics, modular weapon projectile mechanics, dynamic wave scaling, and a complete serialization/deserialization save system.

---

## Architecture & Technical Highlights

### 1. Vector Movement & Aiming (`PlayerController.cs`)
* **Normalized Diagonal Velocity:** Normalized raw directional inputs to eliminate diagonal speed inflation, applied via `Rigidbody2D.linearVelocity` in `FixedUpdate` for frame-independent physics.
* **Decoupled Sprite Rotation:** Calculated radians-to-degrees screen-space aiming angles using `Mathf.Atan2` against world-space cursor raycasts, orienting the player sprite independently of movement direction.

### 2. Wave Management & Spawning Engine (`GameManager.cs`)
* **Dynamic Wave Scaling:** Structured incremental wave progression with algorithmic scaling ($Enemies = Base + (Wave - 1) \times Multiplier$).
* **Safe-Zone Distance Checking:** Implemented distance-threshold validation (`do-while` vector loops) to guarantee enemies never spawn within an immediate radius of the player.
* **Boss Encounter Trigger:** Orchestrated state transitions on final wave thresholds to spawn designated boss entities alongside custom UI game-state triggers.

### 3. State Persistence & Serialization (`SaveSystem.cs`, `PlayerData.cs`, `EnemyData.cs`)
* **Full Game State Serialization:** Captured player transform/health, active wave progression, remaining enemy tallies, and dynamically active pickup locations into persistent binary/data formats.
* **Entity Snapshot Rebuilding:** Systematically despawns existing scene objects and reconstitutes exact coordinate arrays and enemy health states upon game load, preventing state soft-locks.

### 4. Modular Pickup & Ballistics Pipeline (`Shooting.cs`, `MinigunProjectile.cs`, `HealthPickup.cs`)
* **Polymorphic Power-Up Pipeline:** Trigger-based pickup architecture triggering weapon-state mutations and audio bus playback.
* **Ballistics Trajectory:** Implemented directional transform translations with lifetime garbage collection and trigger collision handling.

---

## Tech Stack
* **Engine:** Unity (2D)
* **Language:** C#
* **Design Patterns:** Singleton Pattern (`GameManager`, `AudioManager`), Component Pattern, State Serialization

---
