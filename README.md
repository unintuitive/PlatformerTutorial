## Platformer Tutorial in C# for Godot 3.2.2

### Prerequisites
* Godot 3.2.2: https://godotengine.org

### Installation
1. Clone the repo or download it. Uncompress the .zip.
2. Open the project in Godot either by double-clicking the project.godot file or by first opening Godot and choosing the Import option.
3. Build and run the game by clicking the "play" button in the top right corner of the window.

### Development Notes
This game is a C# port of the Godot 3 Platformer Tutorial by UmaiPixel, found here: https://youtu.be/MMsMtPVUtUE

Where possible I kept the flow of the code close to the original to make it easier to follow along with the screencasts. The code differs in some places where GDScript does not translate precisely to C#. Those issues are noted in the comments. Differences between the GDScript and C# APIs can be found here: https://docs.godotengine.org/en/stable/getting_started/scripting/c_sharp/c_sharp_differences.html 

The original tutorial uses customized private assets, so I've substituted them with game art found on https://opengameart.org. This means there are some visual differences between this port and the original, particularly in animations and the background.

### Gameplay Notes
Click the Start button to begin. Arrow keys move the player character left and right. Press up to jump. Press up twice to double-jump. Tab button shoots a fireball. Touching an enemy or falling in a bottomless pit is lethal!

### Additional Details

For more details on porting games from GDScript to C#, please see this blog post: https://unintuitive.net/code/how-i-learned-the-godot-c#-api

### License
This game code is distributed under the terms of the MIT license. See the LICENSE.txt file for details.
