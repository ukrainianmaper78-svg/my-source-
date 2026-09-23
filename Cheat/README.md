# CoreDev Unity Cheat Source

## Overview

Complete IMGUI-based Unity cheat source with ESP, hooks, and various game modifications. This source is designed for Unity 2021+ and includes:

- **ESP System**: Box, Line, Name, Health, Skeleton, Distance
- **Hook System**: Method detouring, MonoBehaviour hooks, Camera hooks
- **IMGUI Overlay**: Full menu system with tabs and settings
- **Aimbot**: Smooth aim, bone targeting, FOV check, prediction
- **Trigger Bot**: Auto-fire with delay and randomization
- **No Recoil**: Recoil cancellation
- **No Spread**: Bullet spread removal
- **Wall Hack**: See through walls with transparency
- **Speed Hack**: Movement speed modifier
- **Teleport**: Instant teleportation with noclip

## Features

### ESP System
- 2D Box ESP around players
- Snap lines to targets
- Player name display
- Health bar with percentage
- Bone/skeleton ESP
- Distance display
- Weapon name display
- Team color differentiation
- Visibility check (only show visible players)
- Configurable max distance
- Customizable colors

### Hook System
- Unity method hooking (OnGUI, Update, etc.)
- MonoBehaviour component hooks
- Camera pre/post render hooks
- Type-based method detouring
- Original method preservation

### IMGUI Menu
- Tab-based navigation (ESP, Aimbot, Misc, Players, Settings)
- Draggable window
- Toggle switches for all features
- Sliders for numeric values
- Color pickers
- Key binding
- Real-time preview

### Aimbot
- Toggle with keybind
- Configurable FOV
- Smooth aim with adjustable speed
- Bone targeting (Head, Neck, Chest, Hips, Random)
- Movement prediction
- Through walls option
- Auto-fire option

### Trigger Bot
- Auto-fire when targeting enemy
- Configurable activation delay
- Randomization to appear more human
- Headshot only option
- Visibility check
- Max distance limit

### No Recoil
- Full recoil removal
- Configurable reduction percentage
- Vertical and horizontal recoil control
- Per-weapon settings

### No Spread
- Bullet spread removal
- Configurable reduction percentage
- Per-weapon settings

### Wall Hack
- See through walls
- Configurable transparency
- Player and weapon highlighting
- Shader modification

### Speed Hack
- Movement speed multiplier
- Toggle with keybind
- Local player only or all players
- Instant speed change

### Teleport
- Teleport forward/backward/up/down
- Noclip mode
- Configurable teleport distance
- Teleport to position/player

## Usage

### Installation
1. Copy the `Cheat` folder into your Unity project's `Assets` folder
2. Add the `CheatLoader` component to a GameObject in your scene
3. Or use the provided `CheatLoader.prefab`

### Activation
- Press **INSERT** key to toggle the menu
- Navigate through tabs to configure features
- Toggle features on/off as needed

### Keybinds
- **INSERT**: Toggle menu
- **Mouse2 (Right Click)**: Aimbot activation (configurable)
- **Mouse0 (Left Click)**: Trigger bot activation (configurable)
- **T**: Teleport forward
- **G**: Teleport backward
- **Y**: Teleport up
- **H**: Teleport down
- **N**: Toggle noclip
- **F**: Toggle speed hack

## Configuration

All settings are configurable through the IMGUI menu:

### ESP Settings
- Enable/Disable ESP
- Show Box, Line, Name, Health, Skeleton, Distance, Weapon, Team
- Max Distance (50-1000)
- Only Visible toggle
- Box Color (RGB sliders)

### Aimbot Settings
- Enable/Disable Aimbot
- Aim Key selection
- FOV (1-360)
- Smooth Aim toggle
- Smooth Amount (1-50)
- Target Bone selection
- Through Walls toggle

### Misc Settings
- Show Crosshair toggle
- Crosshair Size (5-50)
- Show FPS toggle
- Show Player Count toggle

## Code Structure

```
Cheat/
├── Hooks/              # Hook system
│   ├── UnityHooks.cs   # Main Unity hooking system
│   └── MethodHooks.cs  # Method detouring
├── ESP/                # ESP system
│   ├── ESPManager.cs   # Main ESP manager
│   └── PlayerESP.cs    # Per-player ESP
├── UI/                 # User interface
│   ├── OverlayGUI.cs   # Main overlay GUI
│   └── MenuGUI.cs      # Tab-based menu
├── Utils/              # Utility features
│   ├── Aimbot.cs       # Aimbot system
│   ├── TriggerBot.cs   # Trigger bot
│   ├── NoRecoil.cs     # No recoil
│   ├── NoSpread.cs     # No spread
│   ├── WallHack.cs     # Wall hack
│   ├── SpeedHack.cs    # Speed hack
│   └── Teleport.cs     # Teleport system
└── CheatLoader.cs      # Main cheat loader
```

## Implementation Notes

### For Unity Developers
1. This source uses Unity's IMGUI system for rendering
2. All hooks use MonoBehaviour component attachment
3. No external dependencies required
4. Compatible with Unity 2021+

### For Game Hacking
1. The source includes hooks for common Unity game components
2. ESP system works with standard Unity Renderers
3. Aimbot uses raycasting for target detection
4. All features are toggleable at runtime

### Customization
1. Modify `ESPManager.cs` for custom ESP styles
2. Update `Aimbot.cs` for custom targeting logic
3. Add new features in the `Utils` folder
4. Customize menu appearance in `OverlayGUI.cs` and `MenuGUI.cs`

## Building

1. Copy the `Cheat` folder to your Unity project
2. Build as usual with Unity
3. No additional build steps required

## Troubleshooting

- **ESP not showing**: Check if `ESPManager.Settings.Enabled` is true
- **Aimbot not working**: Verify camera reference and target detection
- **Menu not appearing**: Ensure `OverlayGUI` component is active
- **Performance issues**: Reduce ESP max distance or disable features

## License

This source is provided as-is for educational purposes only. Use at your own risk.

## Credits

CoreDev - Senior Unity Developer

---

**Note**: This source is designed for Unity 2021+ and may require adjustments for other versions.
