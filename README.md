# Verity Gorilla Tag Mod

A BepInEx-based companion mod for Gorilla Tag inspired by the ThatMob YouTube series. Verity is an AI companion that follows the player, executes commands, and provides interactive gameplay modifications.

## Features

### Core Companion System
- **Verity Spawning**: Spawn a Verity companion (sphere model placeholder) that follows the player using smooth AI pathfinding
- **Follow Behavior**: Verity uses lerp-based movement to smoothly trail behind the player at a configurable distance
- **Companion Teleportation**: Instantly teleport Verity to the player's position

### Player Teleportation
- Teleport to **Stump**, **Forest**, and **City** map locations
- Velocity reset on teleport for safe landing

### Gameplay Modifiers
- **Speed Boost**: 1.5x movement multiplier (toggleable)
- **Low Gravity**: Reduced gravity mode (3.0 units vs normal 9.81) for floaty gameplay
- **Platform Spawning**: Create temporary platforms under the player's position for platforming challenges

### Cosmetics
- Change Verity's color (Red, Blue, Green, or custom colors)
- Extensible system for adding hats and cosmetic items

### Control Panel
- **Toggle Menu**: Press `Tab` to show/hide the control panel
- **GUI Interface**: Simple on-screen menu for all mod commands
- **Real-time Feedback**: Console logging for all actions

---

## Installation & Setup

### Prerequisites
1. **Gorilla Tag** (VR game)
2. **BepInEx 5.4.x or later** - Download from [BepInEx GitHub](https://github.com/BepInEx/BepInEx/releases)
3. **Utilla** - Gorilla Tag mod framework (install via BepInEx)
4. **Unity 2021.3.x** (for development)
5. **Visual Studio 2022** or equivalent C# IDE

### Required Assembly References
These DLLs are needed for compilation and must be available in your reference paths:

```
BepInEx/core/
  - BepInEx.dll
  - BepInEx.Logging.Console.dll

Unity/ (from Gorilla Tag installation)
  - UnityEngine.dll
  - UnityEngine.CoreModule.dll
  - UnityEngine.PhysicsModule.dll

GorillaTag/ (from Gorilla Tag installation)
  - GorillaLocomotion.dll
  - GorillaNetworking.dll
  - Assembly-CSharp.dll

Utilla/
  - Utilla.dll
```

### Build Instructions

1. **Clone the repository**:
   ```bash
   git clone https://github.com/v30n666/verity-gtag-mod.git
   cd verity-gtag-mod
   ```

2. **Update assembly reference paths** in `VerityMod.csproj`:
   - Adjust the `<HintPath>` values to point to your Gorilla Tag installation and BepInEx folder locations

3. **Build the project**:
   ```bash
   dotnet build
   ```

4. **Copy the DLL** to your BepInEx plugins directory:
   ```
   <GameFolder>/BepInEx/plugins/VerityMod.dll
   ```

5. **Launch Gorilla Tag** in a private modded room (use Utilla's `[ModdedGamemode]` attribute to prevent bans)

6. **Press `Tab`** in-game to open the Verity control panel

---

## Usage Guide

### In-Game Controls

| Action | Key |
|--------|-----|
| Toggle Control Panel | `Tab` |
| Spawn Verity | Click "Spawn Verity Companion" button |
| Despawn Verity | Click "Despawn Verity" button |
| Teleport Verity to You | Click "Teleport Verity to Player" button |
| Teleport to Locations | Use map teleport buttons (Stump, Forest, City) |
| Enable Speed Boost | Toggle "Speed Boost (1.5x)" checkbox |
| Enable Low Gravity | Toggle "Low Gravity" checkbox |
| Spawn Platform | Click "Spawn Platform Under Player" button |
| Change Verity's Color | Click color buttons (Red, Blue, Green) |

### Example Gameplay Loop
1. Press `Tab` to open the menu
2. Click "Spawn Verity Companion" - Verity appears and starts following you
3. Toggle "Low Gravity" for floaty platforming
4. Click "Spawn Platform Under Player" to create solid platforms
5. Use map teleports to explore different areas with Verity in tow
6. Change Verity's color for cosmetic customization

---

## Technical Architecture

### Plugin Structure

**VerityPlugin.cs** - Main mod class inheriting from `BaseUnityPlugin`
- Handles initialization via BepInEx and Utilla registration
- Manages UI menu rendering with `OnGUI()`
- Orchestrates all companion and gameplay systems
- Coordinates Verity spawning, AI updates, and modifier application

**VerityAIController.cs** - Companion AI behavior
- Tracks player position via `GorillaLocomotion.Player.Instance`
- Implements smooth following using `Vector3.Lerp()` for natural movement
- Calculates distance-based stopping behavior
- Called each frame from the main plugin's `Update()` loop

### Key Architecture Decisions

1. **Lerp-Based Following**: Verity follows the player using smooth interpolation rather than direct position assignment, creating organic movement
2. **Distance Checking**: The AI respects a stopping distance (2 units) to avoid overlapping with the player
3. **Modular Modifiers**: Gameplay effects (speed, gravity, platforms) are toggled independently
4. **Safety through Modded Gamemode**: Uses Utilla's `ChangeRegion()` to mark the room as modded, preventing bans in public lobbies
5. **Temporary Platforms**: Spawned platforms auto-destruct after 10 seconds to prevent clutter

### How Verity Tracks the Player

```
1. Player Instance Access: GorillaLocomotion.Player.Instance gives us the player's transform
2. Distance Calculation: Vector3.Magnitude calculates distance between Verity and player
3. Direction Normalization: (targetPos - currentPos).normalized gives movement direction
4. Smooth Movement: Vector3.Lerp interpolates position over time for smooth motion
5. Stopping Logic: Movement stops when distance < stoppingDistance (prevents jittering)
```

---

## Customization & Extension

### Adding New Commands

To add a new teleport location, modify the `OnGUI()` method:

```csharp
if (GUILayout.Button("Teleport to [YourLocation]", GUILayout.Height(30)))
{
    TeleportPlayerToLocation(new Vector3(x, y, z), "[YourLocation]");
}
```

### Modifying Verity's Appearance

Replace the sphere primitive with your own 3D model:

```csharp
// In SpawnVerity(), replace:
verityCompanion = GameObject.CreatePrimitive(PrimitiveType.Sphere);

// With your model:
verityCompanion = Instantiate(yourVerityModelPrefab);
```

### Adjusting AI Behavior

In `VerityAIController.cs`, modify these values:

```csharp
private float followSpeed = 5f;              // Higher = faster following
private float stoppingDistance = 2f;         // Distance to maintain from player
```

### Adding Custom Cosmetics

Extend the `ChangeVerityColor()` method or create new cosmetic methods:

```csharp
private void ApplyVerityHat(int hatID)
{
    // Add hat to Verity's model
}
```

---

## Troubleshooting

### "Verity not spawning"
- Ensure you're in a **private modded room** (not public lobbies)
- Check BepInEx console for error messages (press F5 for console)
- Verify all assembly references are correctly resolved

### "Mod not loading"
- Confirm BepInEx is properly installed
- Ensure `VerityMod.dll` is in `BepInEx/plugins/` directory
- Check the BepInEx console for loading errors
- Verify Utilla mod is installed

### "Verity not following smoothly"
- Check the `followSpeed` value in `VerityAIController.cs`
- Ensure the player's FPS is stable (lower FPS = choppier motion)
- Verify `UpdateVerityPosition()` is being called every frame

### "Platform spawning is slow"
- Platforms are set to `isKinematic = true` to prevent physics calculations
- If needed, adjust the spawn position or platform size in `SpawnPlatform()`

---

## Development Notes

### Frame Timing & Lerp Behavior
The `UpdateVerityPosition()` method uses `Time.deltaTime` to ensure frame-rate-independent movement:
```csharp
Vector3 moveDirection = directionToPlayer.normalized;
Vector3 targetPosition = transform.position + (moveDirection * followSpeed * Time.deltaTime);
transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
```

This ensures smooth motion regardless of framerate. The `0.1f` lerp factor (10% per frame) provides predictable interpolation.

### Gravity Modification
When low gravity is toggled, `Physics.gravity` is directly modified. **Note**: This affects all rigidbodies in the scene. For more granular control, modify individual rigidbody gravity scales instead.

### Safety & Ban Prevention
The mod uses `Utilla.Utilities.Utils.ChangeRegion("com.v30n666.verity")` to mark the room as modded, preventing ranked lobby access and bans.

---

## Credits

- **Inspired by**: ThatMob YouTube series "Verity Gorilla Tag"
- **Built with**: BepInEx, Utilla, Gorilla Locomotion
- **Unity Version**: 2021.3.x
- **Language**: C# 9.0

---

## License

This project is provided as-is for educational and personal use. Ensure compliance with Gorilla Tag's terms of service and use only in private modded rooms.

---

## Support & Contributing

For issues, questions, or contributions:
1. Check existing GitHub issues
2. Review troubleshooting section above
3. Submit a detailed GitHub issue with:
   - Error messages from BepInEx console
   - Steps to reproduce the issue
   - Your Gorilla Tag and BepInEx versions

Enjoy playing with Verity! 🎮
