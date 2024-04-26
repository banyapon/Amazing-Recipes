# Amazing Recipe! Sichang Life
Amazing Recipe! Sichang Life is a fast-paced, cooperative cooking game where players take on the roles of chefs in a bustling restaurant located on the beautiful Sichang Island in Thailand. Players must work together efficiently to prepare and serve delicious Thai dishes to a demanding clientele within a time limit. The game combines the chaotic fun of Overcooked with the unique charm and flavors of Thai cuisine.
![Amazing Recipe! Sichang Life](https://github.com/banyapon/Amazing-Recipes/blob/main/Assets/ScreenShot/a1.png?raw=true)

# Gameplay Mechanics
![Gameplay Mechanics](https://github.com/banyapon/Amazing-Recipes/blob/main/Assets/ScreenShot/a5.png?raw=true)

![Gameplay Mechanics](https://github.com/banyapon/Amazing-Recipes/blob/main/Assets/ScreenShot/a3.png?raw=true)

Cooperative Multiplayer: Up to 4 players can team up online or locally to manage the restaurant kitchen.

# Overcooked-inspired Gameplay: 
Players will frantically dash around the kitchen collecting ingredients, prepping stations, cooking dishes, and plating meals for customers. Stations will be inspired by real-life Thai kitchens, featuring woks, steamers, mortars and pestles, and charcoal grills.

# Sichang Island Setting: 
The game will be set in a vibrant restaurant on Sichang Island, Thailand. The kitchen will be visually inspired by local aesthetics, and the soundtrack will feature upbeat fun music.
# Customization and Progression: 
Players can unlock new cosmetic items for their chefs and restaurant as they progress through the game. This can include new outfits, decorations, and even unique recipe variations.

# Difficulty Levels:
The game will offer multiple difficulty levels to cater to players of all skillsets. Higher difficulty levels will introduce more complex recipes, faster customer arrival times, and additional kitchen hazards.

![Gameplay Mechanics](https://github.com/banyapon/Amazing-Recipes/blob/main/Assets/ScreenShot/a2.png?raw=true)

# Netcode for Multiplayer: 
Unity's built-in networking solution, UnityLời Networking (LLAPI), or similar solutions like Mirror or Forge Networking can be used to enable smooth online multiplayer functionality.
Using Unity 3D with Netcode. Unity 3D is a powerful game engine well-suited for creating 3D games like Amazing Recipe! Sichang Life. Here's a basic breakdown of how Unity and Netcode can be used to build the game:

# Game Design and Development:

* Design the kitchen environment, characters (chefs), food items, and user interface using Unity's development tools.
* Create 3D models and animations for the kitchen environment, characters, and food items.
* Develop the core gameplay mechanics like ingredient gathering, cooking, plating, and customer service.
* Integrate Unity's physics engine to simulate realistic interactions between characters and objects in the kitchen.
* 
# Netcode Implementation:

* Choose a suitable Netcode solution (LLAPI, Mirror, or Forge).
* Implement networking functionalities to handle player communication, synchronization of game state (kitchen state, character positions, etc.) across all clients.
* Ensure smooth movement and responsiveness for online play by implementing features like latency compensation and prediction.

# Testing and Polishing:
* Rigorously test the game in both singleplayer and multiplayer modes to identify and fix bugs and balance issues.
* Polish the game by adding sound effects, background music, and visual effects.
* Additional Considerations

Thai Cultural Immersion: Consider incorporating elements of Thai culture into the game beyond just the cuisine. This could involve including short cutscenes or informational pop-ups about Thai ingredients, cooking techniques, or cultural customs.

# Humorous Tone: 
Maintain a lighthearted and humorous tone similar to Overcooked, even when the kitchen gets chaotic. This can be achieved through character voice lines, sound effects, and visual gags.

# Post-Launch Content: 
Plan for future content updates that introduce new dishes, kitchen layouts, and gameplay mechanics to keep players engaged.
Building a full-fledged game requires a skilled development team with expertise in areas like 3D modeling, animation, programming, and networking. However, this concept provides a solid foundation for creating a fun and engaging cooking game experience set in the beautiful island of Sichang.

### Input system
The game uses the new Unity input system and currently supports keyboard and gamepads. I plan to implement touch screen as well.<br>
Default controls: 
* keyboard: move with WASD/arrows, interact with E, alternative interact with F, pause with ESCAPE; 
* gamepad: move with the left analog stick, interact with A, alternative interact with X, pause with START.
***

### Playing options
The game is playable as a single player game, or as a multiplayer with up to 4 players. Used Netcode for GameObjects to implement multiplayer. 
* used NetworkVariable and server/client RPCs for synching
* players can create private or public lobbies
* players can join public lobbies from a list of available lobbies or enter a lobby code to join a private lobby
* players can change their player nick and color
* appropriate messages are shown when the player disconnects, when the host disconnects, when the session is waiting for all players to start the game or when the session is loading/synching data
* the game uses Relay for networking
***
### Code
The game uses events, scriptable objects and state machines extensively. Since the states are very simple, I didn't bother with creating a separate state class for every state and instead I used a dictionary (for gameplay states) or switch statements (in super simple cases, for example when it comes to input). There is currently one case I plan to refactor from using a switch to  at least using a dictionary (stove counter states).

Logic is nicely separated from visuals and from UI. I also used namespaces to make sure there are no unnecessary dependencies.
Everything in this project fork from [https://github.com/Dorole/KitchenChaos](https://github.com/Dorole/KitchenChaos)

There are several singletons. I made sure there are a minimal number of them (3 so far), but I plan to refactor this by using a service locator pattern, to practice the use of the pattern and to prepare for what may come in the multiplayer implementation.

Saving system is using PlayerPrefs. Since this is just a practice project and the only things that are saved are key bindings, music/sound volume and high score, I don't think I should be using a more complicated system at this point.
***
