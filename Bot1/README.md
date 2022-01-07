# ANN+BT RLBot

## Usage Instructions

### Prerequisites
Make sure you've installed [Python 3.7 64 bit](https://www.python.org/ftp/python/3.7.3/python-3.7.3-amd64.exe) or newer. During installation:
   - Select "Add Python to PATH"
   - Make sure pip is included in the installation
   
Set up RLBotGUI
1. Follow instructions at https://youtu.be/oXkbizklI2U for instructions.
1. Use Add -> Load folder in RLBotGUI on the current directory. This bot should appear in the list.


### Using Visual Studio
1. Install Visual Studio 2015 or newer. It should come with .NET Framework 4.6.1 or newer.
1. Open NeuralBot/Bot.sln in Visual Studio.
1. In Visual Studio, click the "Start" button, 
1. In RLBotGUI, put the bot on a team and start the match.

### Using Rider
1. Install Rider. If you do not have Visual Studio installed alongside Rider, follow [this article](https://rider-support.jetbrains.com/hc/en-us/articles/207288089-Using-Rider-under-Windows-without-Visual-Studio-prerequisites) to set up Rider.
1. Open NeuralBot/Bot.sln in Rider.
1. In Rider, click the "Run Default" button, which should compile and run the bot. Leave it running.
   - The first time you click it, you may be given a dialog to set up the configuration. Click the "Run" button in the dialog to continue.
1. In RLBotGUI, put the bot on a team and start the match.

## Upgrades

This project uses a package manager called NuGet to keep track of the RLBot framework.
The framework will get updates periodically, and you'll probably want them, especially if you want to make sure
your bot will work right in the next tournament!

### Upgrading in Visual Studio
1. In Visual Studio, right click on the Bot C# project and choose "Manage NuGet Packages..."
1. Click on the "Installed" tab. You should see a package called "RLBot.Framework".
1. If an upgrade is available, it should say so and give you the option to upgrade.

### Upgrading in Rider
1. In Rider, right click on the Bot C# project and choose "Manage NuGet Packages".
1. In the "Installed Packages" section, click on the package called "RLBot.Framework".
1. If the "Version" dropdown contains a higher version than what your project currently has, you can select that version and click the Upgrade button next to the dropdown to upgrade.

## Training and Loading Brains

The AI by default does not contain a neural network brain, instead the order of behaviour tree nodes is preset in the Neural Selector Node (NSN). A brain must be loaded during runtime for the NSN to use a neural network. This is done using the UI.

1. (Optional) Train an AI by pressing the "Train AI" button and selecting a JSON dataset. Once training is completed a prompt will appear for saving the brain.
2. Load an AI using the "Load AI" button, which loads and assigns the brain to the NSN.

## Notes

- Bot behavior is controlled by `NeuralBot/Bot/Bot.cs`
- Bot appearance is controlled by `PythonAgent/appearance.cfg`

## Overview of how the C# bot interacts with Python

The C# bot executable is a server that listens for Python clients.
When `PythonAgent/PythonAgent.py` is started by the RLBot framework, it connects to the C# bot server and tells it its info.
Then, the C# bot server controls the bot through the `RLBot_Core_Interface` DLL.
