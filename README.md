# El Con Ring's Game Documentation

- GitHub Link: https://github.com/DuongBanh2022/project-5-ElConRing

## Project Overview

El Con Ring is a 2D Unity metroidvania game featuring player movement, jumping, combat, enemy encounters, breakable objects, checkpoints, and persistent save data. 

## Script Summary

The game consists of a main menu, loading scene, gameplay scene, basic combat interaction, 2D environment, pause menu, interactive chest, and level-completion system.

| Script | Purpose |
|---|---|
| `MainMenu.cs` | Controls the main menu, starting the game, and quitting the application. |
| `PressAnyButton.cs` | Displays the initial screen and loads the Main Menu after input. |
| `Continue.cs` | Checks whether saved player data exists and allows the player to continue. |
| `ContinueLoading.cs` | Handles the loading flow for continuing gameplay. |
| `ExittoMainMenu.cs` | Loads a transition scene when returning to the Main Menu. |
| `LoadingManager.cs` | Loads the Gameplay scene asynchronously and displays loading progress. |
| `PMovement.cs` | Controls player movement, jumping, sprite flipping, and animations using the Unity Input System. |
| `BulletSpawner.cs` | Fires pooled bullets and triggers the player's swing animations. |
| `SimplePool.cs` | Creates and reuses bullet objects. |
| `Bullet.cs` | Handles bullet movement, lifetime, enemy damage, breakable-object damage, and pool return. |
| `EnemyNPC.cs` | Controls enemy movement, health, damage, and defeat. |
| `EnemyHP.cs` | Displays health bars and updates them when damage occurs. |
| `WallHP.cs` | Implements health and damage behavior for breakable objects. |
| `CheckpointTrigger.cs` | Detects checkpoint entry and saves checkpoint information using `PlayerPrefs`. |
| `PauseManager.cs` | Controls the pause menu, gameplay pausing, and resuming. |
| `QuitMenu.cs` | Saves the player's current position before returning to the Main Menu. |
| `QuitGame.cs` | Exits the application from the Main Menu. |
| `CheckpointSaver.cs` | Saves and loads the player's last saved position. |
| `Reset.cs` | Clears saved player data for testing. |
| `Chest.cs` | Detects nearby chest interaction and reveals the apple reward when opened. |
| `Apple.cs` | Handles apple collection and displays the level-completion panel. |
| `CameraFollowScript.cs` | Keeps the camera following the player during gameplay. |

## Scene Descriptions

### MainMenu

The MainMenu scene is the game's starting scene.

**Key GameObjects**
- Main Camera
- PressAnyButtonPanel
- MenuCanvas
- BlackBackground
- Continue Button
- Start Button
- Quit Button
- MenuManager
- EventSystem

**Interaction**

The player first sees the press-any-button screen. After input is detected, the main menu appears. Selecting New Game loads the `LoadingScene`, while Continue loads gameplay using saved player data.

### LoadingScenes (New/Continue/Exit)

The LoadingScene transitions between the Main Menu and Gameplay scenes.

**Key GameObjects**
- Main Camera
- Canvas
- Loading background
- Loading text
- Loading bar
- EventSystem
- LoadingManager

**Interaction**

The LoadingScene receives the request from the New Game button and asynchronously loads the Gameplay scene while updating loading progress.

### Gameplay

The Gameplay scene contains the main playable level and gameplay mechanics.

**Key GameObjects**
- Player
- Chapel/Grid Tilemap
- Enemy and BossEnemy
- Bullet's Pool
- WallBreakable Object
- Checkpoint
- Chest
- Apple
- CompletionPanel
- Pause Menu
- Main Camera
- Canvas

**Interaction**

The player moves through the tilemap using the Unity Input System. The player can jump, fire pooled bullets, damage enemies and breakable objects, activate checkpoints, and interact with the chest.

When the player approaches the chest and presses `E`, the chest opens and reveals an apple. When the player touches the apple, the completion panel appears and gameplay pauses.

## Project Requirement Satisfaction

### Requirement 1 — Scenes and Scene Management

The project contains a Main Menu, LoadingScene, and Gameplay scene. New Game loads through the LoadingScene into Gameplay, while the pause and quit systems provide a path back to the Main Menu.

### Requirement 2 — Input System Player Control

Player movement and jumping use the Unity Input System through the `PlayerControls.inputactions` asset.

### Requirement 3 — Tilemap Level Layout

The Gameplay scene uses the `ChapelGround` and `Grid` Tilemap to create the playable level. A `TilemapCollider2D` provides collision between the level and the player.

### Requirement 4 — Prefab Pooling

The bullet system uses `SimplePool` to create a limited number of bullet objects and reuse inactive bullets instead of repeatedly calling `Instantiate` and `Destroy`.

### Requirement 5 — Tags, Layers, and Triggers

The project uses tags, layers, and trigger interactions. Checkpoints detect the Player using `CompareTag("Player")`, while bullets detect enemies using `CompareTag("Enemy")`. The chest and apple also use 2D trigger colliders.

### Requirement 6 — Pause Menu

The Gameplay scene includes a pause menu that opens with Escape. `PauseManager` uses `Time.timeScale = 0` to stop gameplay and provides Resume, Save Checkpoint, and Quit to Main Menu functionality.

### Requirement 7 — PlayerPrefs Persistence

The project uses `PlayerPrefs` to save the player's last position and checkpoint information. Saved data is loaded when the player continues the game, allowing progress to persist after leaving and relaunching the game.

### Requirement 8 — Lighting / Material / Shader

The Gameplay scene includes a 2D lighting or material visual effect to provide an additional visual treatment beyond the basic sprites and tilemap.

## Third-Party Assets, Packages, and Tutorials

The project uses Unity's built-in systems and packages, including:

- Unity Input System for keyboard, mouse, and gamepad input.
- Unity 2D Tilemap system for level design and tile-based collision.
- TextMeshPro for menu, loading, completion, and other UI text.
- Unity Physics 2D for Rigidbody2D, Collider2D, and trigger interactions.
- Unity Animator for player movement and swing animations.

Some visual assets, sprites, sounds, or animation assets may originate from third-party sources or course-provided resources. These assets are used as project resources rather than as original code written for the project. The Main Menu image is drawn and designed by hand; the project contains no AI-generated assets.

The gameplay scripts and systems were written and configured specifically for this project, including player movement, pooling, enemy health, breakable objects, checkpoints, saving/loading, chest interaction, item collection, pause functionality, and scene management. 

Any third-party tutorials or examples used during development were used as learning references and adapted to the project's requirements, not submitted as unchanged tutorial projects.



