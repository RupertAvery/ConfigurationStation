# ConfigurationStation

A tool for generating and updating `es_systems.cfg` for EmulationStation with RetroArch cores.

# Prerequisites

You must have installed EmulationStation and RetroArch before running ConfigurationStation

# Usage

ConfigurationStation will look for your `%USERPROFILE%\.emulationstation` folder and `%APPDATA%\RetroArch`. 
It will verify that there is a `es_systems.cfg` and a `retroarch.exe` in the respective folders before you are allowed to proceed.

* Select systems you want to be available for EmulationStation. 
* Select the folders where the ROMs can be found.
* Select additional options

# Options

## Cores

ConfigurationStation can download any missing cores for you (or overwrite existing), based on your selected systems.  
It will download the correct version (32-bit or 64-bit) by checking the version number that RetroArch reports.

## PPSSPP Assets

ConfigurationStation can download  http://github.com/hrydgard/ppsspp/archive/master.zip and extract 
the contents of `ppsspp-master/assets` into  `RetroArch\system\PPSSPP`. This is needed for MemoryStick 
dialogs to work for the libretro core.

# TODO

* Add advanced mode for manually configuring systems.
* Support Linux?

