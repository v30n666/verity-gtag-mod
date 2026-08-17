# Verity Mod - Quick Setup Guide

## For Users (No Coding Required)

### Prerequisites
- Gorilla Tag installed on your PC
- BepInEx 5.4+ installed in your Gorilla Tag folder
- Utilla mod installed via BepInEx

### Installation Steps

1. **Download the DLL**
   - Go to the [Releases page](https://github.com/v30n666/verity-gtag-mod/releases)
   - Download the latest `VerityMod.dll`

2. **Place in Plugins Folder**
   - Navigate to: `<Your Gorilla Tag Folder>/BepInEx/plugins/`
   - If the `plugins` folder doesn't exist, create it
   - Paste `VerityMod.dll` into this folder

3. **Launch Gorilla Tag**
   - Start Gorilla Tag in **private modded room mode** (NOT public lobbies)
   - Wait for the game to fully load

4. **Open the Control Panel**
   - Press `Tab` to toggle the Verity control panel
   - You should see the menu appear on screen

5. **Start Using Verity!**
   - Click "Spawn Verity Companion" to create your companion
   - Use the menu to teleport, modify gameplay, and customize

### Troubleshooting

**"Mod not loading"**
- Verify BepInEx is working (check for `BepInEx` folder in game directory)
- Ensure Utilla is installed
- Check that DLL is in correct folder
- Open BepInEx console (press F5) to see error messages

**"Menu doesn't appear when I press Tab"**
- Make sure you're in a **private modded room** (not public)
- Try pressing Tab again or restarting the game
- Check BepInEx console for errors

**"Verity won't spawn"**
- Ensure player is loaded in the game world
- Check if you're in a modded room
- Look at BepInEx console for error messages

---

## For Developers (Building from Source)

### Prerequisites
- Visual Studio 2022 or Visual Studio Code
- .NET 6.0 SDK or higher
- Gorilla Tag installed (for assembly references)
- BepInEx installed

### Build Steps

1. **Clone the Repository**
   ```bash
   git clone https://github.com/v30n666/verity-gtag-mod.git
   cd verity-gtag-mod
   ```

2. **Configure Assembly Paths**
   - Open `VerityMod.csproj` in a text editor
   - Update all `<HintPath>` values to match your system:
     ```xml
     <Reference Include="BepInEx">
       <HintPath>C:\Path\To\Your\BepInEx\core\BepInEx.dll</HintPath>
     </Reference>
     ```
   - See `ASSEMBLY_REFERENCES.md` for detailed guidance

3. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

4. **Build the Project**
   ```bash
   dotnet build --configuration Release
   ```

5. **Locate the DLL**
   - Built DLL is at: `bin/Release/VerityMod.dll`

6. **Install Locally**
   - Copy `bin/Release/VerityMod.dll` to your BepInEx plugins folder
   - Launch Gorilla Tag and test

### Project Structure

```
verity-gtag-mod/
├── VerityMod/
│   └── Plugin.cs              # Main mod code
├── VerityMod.csproj           # Project configuration
├── ASSEMBLY_REFERENCES.md     # Detailed reference guide
├── SETUP_GUIDE.md             # This file
├── README.md                  # Full documentation
└── .gitignore
```

### Building with Visual Studio GUI

1. Open `verity-gtag-mod` folder in Visual Studio
2. Right-click project → **Edit Project File**
3. Update `<HintPath>` values for your system
4. Build → **Build Solution** (Ctrl+Shift+B)
5. DLL appears in `bin/Release/`

### Modifying the Code

Key files to modify:
- `VerityMod/Plugin.cs` - Main plugin logic, UI, and commands

To add new features:
1. Edit `Plugin.cs`
2. Rebuild: `dotnet build --configuration Release`
3. Copy new DLL to plugins folder
4. Restart Gorilla Tag to test

### Common Build Errors

**Error: "Cannot find assembly 'GorillaLocomotion'"**
- Update `<HintPath>` in `.csproj` to point to correct DLL location
- Verify DLL exists at the path you specified

**Error: "The type or namespace 'UnityEngine' could not be found"**
- Ensure Unity DLLs are referenced in `.csproj`
- Check paths point to `Gorilla Tag_Data/Managed/`

**Build succeeds but DLL won't load**
- Verify all referenced DLLs match Gorilla Tag's version
- Check BepInEx console (F5) for runtime errors
- Ensure correct folder structure: `BepInEx/plugins/VerityMod.dll`

---

## Support

- **User Issues**: Check the troubleshooting section or open a GitHub issue
- **Development Questions**: Review code comments in `Plugin.cs`
- **Build Problems**: See `ASSEMBLY_REFERENCES.md` for detailed setup

Happy modding! 🎮
