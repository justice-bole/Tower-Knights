# Tower Knights
Tower Knights is a single-player tower siege game, where you purchase units to counter your opponent and eventually take their tower before they take yours!

**Link to project:** https://justice-bole.itch.io/tower-knights

![alt tag](https://github.com/justice-bole/Tower-Knights/blob/main/Assets/Gifs/towerknights.gif)

## How It's Made:

**Tech used:** Unity, C#

For the title scene I used "TextMeshPro" (TMP) for the game title and start button. For the parallax background I created three seperate background layers and scrolled each layer at different speeds, the further the layer is from the camera the slower it moves, which gives the illusion of depth to the 2D scene. I used Unity's "AudioSource" component to play the background music and sound effects. The main game scene, the "BattleScene", is where all of the gameplay takes place. I used all of the aforementioned technology in this scene for similar reasons, but added a few extra components. For the dialogue system I used TMP, as well as coroutines and C#'s "foreach" keyword to display the dialogue in a delayed sequence to give the impression the text was being written live, instead of all of the text appearing instantly at once. I created a SpawnManager script, that uses the Random.Range method to select a random enemy to spawn from an array of enemy types. I timer is also kept to make sure enemies are spawned at a steady and managable rate. When enemies collide with the player each object calls the damage, sfx, and particle effects functions periodically, until a unit is defeated. Then the unit is destroyed and the "GoldManager" script updates the players current gold based on what enemy type was just defeated. 

## Lessons Learned:

This was one of my first game projects in Unity and I learned quiite a lot about Unity, C#, and about creating games while working on this project. One thing I regret however is the lack of prior planning going into this project. Because I had not layed out a specific goal or feature list I found myself adding and removing code quite often. This lead to some parts of the project's code being quite nested and complicated, as I was adding functionality to scripts that had not been designed to be open and adaptable. This project really helped me understand the importance of pre-planning and class design, long before you begin to code. Because of this I now do extensive planning, I create design documents, scope checks, class, responsibilities, collaberators (CRC) cards, and I'm always thinking about messages, and keeping my classes abstract, until I'm sure I'm ready to make them concrete, for optimization. 

## Other Projects:

Take a look at these other projects in my portfolio:

**Poseidon:** https://github.com/justice-bole/Poseidon

**FPS-Controller:** https://github.com/justice-bole/FPS-Controller

**Lissajous Curves:** https://github.com/justice-bole/Python-Projects/tree/main/ProcessingScripts/lissajous_curves
