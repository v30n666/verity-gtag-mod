# Assembly References Guide

This document details all the required DLL references needed to compile the Verity Gorilla Tag mod.

## Reference Structure

The mod requires assemblies from three main sources:
1. **BepInEx Framework** - Mod loading and logging
2. **Unity Engine** - Game engine core
3. **Gorilla Tag** - Game-specific code

---

## BepInEx References

These files come from your BepInEx installation in the `BepInEx/core/` directory.

### BepInEx.dll
- **Purpose**: Core BepInEx framework for plugin loading and initialization
- **Location**: `BepInEx/core/BepInEx.dll`
- **Usage**: Provides `BaseUnityPlugin` base class and `[BepInPlugin]` attribute

### BepInEx.Logging.Console.dll
- **Purpose**: Console logging system for debugging
- **Location**: `BepInEx/core/BepInEx.Logging.Console.dll`
- **Usage**: Provides `ManualLogSource` for logging plugin messages

---

## Unity Engine References

These DLLs come from your **Gorilla Tag installation directory** in `Gorilla Tag_Data/Managed/`.

### UnityEngine.dll
- **Purpose**: Core Unity engine functionality
- **Location**: `<GameFolder>/Gorilla Tag_Data/Managed/UnityEngine.dll`
- **Used For**:
  - `Vector3` - 3D vector math
  - `Transform` - GameObject positioning and rotation
  - `GameObject` - Game object creation and management
  - `Input` - Keyboard and controller input
  - `Rigidbody` - Physics and movement

### UnityEngine.CoreModule.dll
- **Purpose**: Core module containing fundamental types
- **Location**: `<GameFolder>/Gorilla Tag_Data/Managed/UnityEngine.CoreModule.dll`
- **Used For**:
  - `Color` - Color representation and manipulation
  - `Renderer` - Mesh rendering
  - `Material` - Visual material properties

### UnityEngine.PhysicsModule.dll
- **Purpose**: Physics simulation and colliders
- **Location**: `<GameFolder>/Gorilla Tag_Data/Managed/UnityEngine.PhysicsModule.dll`
- **Used For**:
  - `Physics.gravity` - Global gravity modification
  - `Collider` - Collision detection
  - `Rigidbody.isKinematic` - Physics object properties

---

## Gorilla Tag Specific References

These DLLs are from the **Gorilla Tag installation** in `Gorilla Tag_Data/Managed/`.

### GorillaLocomotion.dll
- **Purpose**: Player movement and locomotion system
- **Location**: `<GameFolder>/Gorilla Tag_Data/Managed/GorillaLocomotion.dll`
- **Critical For**:
  - `GorillaLocomotion.Player.Instance` - Access to the player object
  - `Player.transform` - Player's position and rotation
  - `Player.bodyCollider` - Player's collision data
  - Platform interaction and position manipulation

**Key Classes Used**:
```csharp
using GorillaLocomotion;

Player player = Player.Instance;  // Singleton access to player
player.transform.position = newPosition;  // Move player
player.bodyCollider.attachedRigidbody.velocity = Vector3.zero;  // Reset velocity
```

### GorillaNetworking.dll
- **Purpose**: Networking and multiplayer synchronization
- **Location**: `<GameFolder>/Gorilla Tag_Data/Managed/GorillaNetworking.dll`
- **Used For**: Room state and player synchronization
- **Note**: May be needed for future networking features

### Assembly-CSharp.dll
- **Purpose**: Contains all Gorilla Tag game code
- **Location**: `<GameFolder>/Gorilla Tag_Data/Managed/Assembly-CSharp.dll`
- **Used For**: Game-specific classes and game logic (for future expansions)
- **Size**: Large file (~10-15 MB) - needed for comprehensive game access

---

## Utilla Framework

### Utilla.dll
- **Purpose**: Standard modding framework utilities for Gorilla Tag
- **Location**: Install via BepInEx, typically: `BepInEx/plugins/Utilla/Utilla.dll`
- **Critical For**:
  - `Utilla.Utilities.Utils.ChangeRegion()` - Mark room as modded to prevent bans
  - Room/lobby management
  - Standard mod utilities

**Key Usage**:
```csharp
using Utilla.Utils;

// Marks the room as modded to prevent ranked play and bans
Utilla.Utilities.Utils.ChangeRegion("com.v30n666.verity");
```

---

## How to Set Up References in Visual Studio

### Option 1: Edit `.csproj` File (Recommended)

1. Open `VerityMod.csproj` in a text editor
2. Update `<HintPath>` values to match your system:

```xml
<Reference Include="BepInEx">
  <HintPath>C:\Path\To\BepInEx\core\BepInEx.dll</HintPath>
</Reference>

<Reference Include="UnityEngine">
  <HintPath>C:\Path\To\Gorilla Tag_Data\Managed\UnityEngine.dll</HintPath>
</Reference>
```

3. Save and reload the project in Visual Studio

### Option 2: Visual Studio GUI

1. Right-click on the project → **Add Reference**
2. Click **Browse**
3. Navigate to each DLL location and add:
   - BepInEx DLLs from `BepInEx/core/`
   - Unity DLLs from `Gorilla Tag_Data/Managed/`
   - Gorilla Tag DLLs from `Gorilla Tag_Data/Managed/`
   - Utilla DLL from `BepInEx/plugins/`

### Option 3: NuGet (Limited Support)

Some assemblies may be available on NuGet, but for Gorilla Tag-specific DLLs, manual references are required.

---

## Finding Your Gorilla Tag Installation

### Windows (Steam)
1. Open Steam
2. Right-click **Gorilla Tag** → **Properties** → **Local Files** → **Browse**
3. Note the folder path (e.g., `D:\SteamLibrary\steamapps\common\Gorilla Tag\`)
4. The managed DLLs are in `Gorilla Tag_Data/Managed/`

### Quest (PC Link)
1. Gorilla Tag may be in: `C:\Program Files\Gorilla Tag\`
2. Or in your custom installation directory
3. Check the same `Gorilla Tag_Data/Managed/` subfolder

---

## Compilation Dependencies Summary

| DLL | Source | Required | Purpose |
|-----|--------|----------|---------|
| BepInEx.dll | BepInEx | ✅ | Plugin base class |
| UnityEngine.dll | Gorilla Tag | ✅ | Core engine types |
| UnityEngine.CoreModule.dll | Gorilla Tag | ✅ | Rendering & colors |
| UnityEngine.PhysicsModule.dll | Gorilla Tag | ✅ | Physics system |
| GorillaLocomotion.dll | Gorilla Tag | ✅ | Player access |
| Assembly-CSharp.dll | Gorilla Tag | ⚠️ | Game code (optional for basics) |
| Utilla.dll | BepInEx | ✅ | Mod utilities |

---

## Troubleshooting Reference Issues

### Error: "The type or namespace name 'GorillaLocomotion' could not be found"
- **Cause**: GorillaLocomotion.dll not referenced
- **Solution**: Add `GorillaLocomotion.dll` from `Gorilla Tag_Data/Managed/`

### Error: "Cannot resolve symbol 'Player'"
- **Cause**: Missing `GorillaLocomotion` namespace or incorrect reference
- **Solution**: 
  1. Verify `using GorillaLocomotion;` is at the top
  2. Check GorillaLocomotion.dll path is correct

### Error: "Assembly with name 'UnityEngine' does not exist"
- **Cause**: Incorrect Unity DLL path
- **Solution**: Point to the exact Unity version (2021.3.x) used by Gorilla Tag

### Build Succeeds but DLL Won't Load in Game
- **Cause**: Runtime assembly loading failure
- **Solution**:
  1. Verify all DLLs match Gorilla Tag's version
  2. Check BepInEx console for detailed error messages
  3. Ensure DLLs are in correct folder structure

---

## Reference Versions

**Required Versions**:
- **Unity**: 2021.3.x (matches Gorilla Tag)
- **C#/.NET**: Standard 2.1 or higher
- **BepInEx**: 5.4.x or later
- **Utilla**: Latest version

To check versions:
- BepInEx version: Check `BepInEx/core/BepInEx.dll` properties
- Unity version: Right-click `UnityEngine.dll` → Properties → Details
- Gorilla Tag version: Steam library → Properties

---

## Quick Reference Paths

```
Your PC Path Structure:
C:\SteamLibrary\steamapps\common\Gorilla Tag\
├── Gorilla Tag_Data\
│   └── Managed\                    # ← Unity & Gorilla DLLs here
│       ├── UnityEngine.dll
│       ├── UnityEngine.CoreModule.dll
│       ├── UnityEngine.PhysicsModule.dll
│       ├── GorillaLocomotion.dll
│       ├── Assembly-CSharp.dll
│       └── GorillaNetworking.dll
└── BepInEx\
    ├── core\                       # ← BepInEx DLLs here
    │   ├── BepInEx.dll
    │   └── BepInEx.Logging.Console.dll
    └── plugins\
        └── Utilla\
            └── Utilla.dll          # ← Utilla DLL here
```

---

For issues or questions about references, consult the BepInEx documentation or Gorilla Tag modding communities.
