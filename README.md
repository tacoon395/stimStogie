# stimStogie

Mod that adds a cigar to the game. Homage to the phantom cigar, effects not canon.

## Overview 
- Uses `CUCoreLib` and the ScavTemplate! 

## Build

1. Open the project in Visual Studio or JetBrains Rider. (Or any other IDE)
2. Add Assembly-CSharp.dll as a reference to the project. (Usually located in `Steam\steamapps\common\Scavengers\Scavengers_Data\Managed` or similar)
3. Build `stimStogie/Template.csproj` via Ctrl + Shift + B (`dotnet build`)
4. If auto-detection misses your setup, open the linked `vars.targets` file from the project and override `BaseGamePath` 

## Usage

- Copy the built DLL from `stimStogie/bin/Debug` or relevant build directory to your BepInEx `plugins` folder.
