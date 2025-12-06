# Contributing to AutoTrader

Thank you for your interest in contributing to the AutoTrader mod for Mount & Blade II: Bannerlord! This document will help you get set up and started with development.

## Prerequisites

Before you begin, make sure you have the following installed:

- **Visual Studio 2019 or later** 
- **.NET Framework 4.8 SDK**
- **Mount & Blade II: Bannerlord** 

## Initial Setup

### 1. Clone the Repository

```
git clone https://github.com/Eskalior/BannerlordMods.git
```

### 2. Configure Your Environment

To make the project portable, we use a user-specific configuration file to point to your local paths. Currently that is only the Bannerlord installation path.

1. Copy the template file:
   ```
   cd BannerlordMods/AutoTrader
   copy paths.props.user.template paths.props.user
   ```

1. Update the `BannerlordPath` to point to your Bannerlord installation directory:
   ```xml
   <BannerlordPath>YOUR_PATH_HERE\Mount &amp; Blade II Bannerlord</BannerlordPath>
   ```

   **Common installation paths:**
   - Steam (default): `C:\Program Files (x86)\Steam\steamapps\common\Mount & Blade II Bannerlord`
   - Steam (custom library): `D:\SteamLibrary\steamapps\common\Mount & Blade II Bannerlord`
   - Epic Games: `C:\Program Files\Epic Games\Mount & Blade II Bannerlord`

**Note:** The `paths.props.user` file is gitignored and will not be committed to the repository. This keeps everyone's local configuration private.


## Build and Deployment

### Building the Mod

- **Debug Build**: Press `F5` or select Debug > Start Debugging
  - This will build the mod and launch Bannerlord with the mod enabled
  - The debugger will be attached for debugging
  
- **Release Build**: Change configuration to Release and build (Ctrl+Shift+B)

### Automatic Deployment

When you build the project, it will automatically deploy the mod to your Bannerlord installation:

```
[BannerlordPath]\Modules\AutoTrader\
SubModule.xml
bin\
    Win64_Shipping_Client\
        AutoTrader.dll
        AutoTrader.pdb
GUI\
    Brushes\
        AutoTraderBrushes.xml
    Prefabs\
        AutoTraderConfigScreen.xml
ModuleData\
    Languages\
        module_strings.xml
        CNs\
            module_strings.xml
```

This matches the exact structure required by Bannerlord.

## License

By contributing to this project, you agree that your contributions will be licensed under the same license as the project.

---

Thank you for contributing to AutoTrader!