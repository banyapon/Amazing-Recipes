# Amazing Recipe: Sichang Story

A simpler version of *Overcooked*, one level. Playable with keyboard and gamepad. Playable as a single player, local multiplayer (with gamepads) or over network.<br>
***
### Overview
![Amazing Recipe: Sichang Story](https://github.com/banyapon/Amazing-Recipes/blob/main/Assets/_Assets/4A.png?raw=true)
The player is enclosed in a kitchen. Recipes are lining up. The player has to use correct ingredients to deliver a correct recipe. Vegetables and cheese have to be chopped, the meat has to be fried and everything has to be combined in a bun on a plate. The player has to deliver as many recipes as (s)he can before the level timer runs out. There is currently a limited selection of recipes available.
***
### Gameplay
The gameplay is controlled by a simple state machine with four states:
* WaitingToStart - the tutorial screen is displayed, overview of key bindings
* CountdownToStart - after the player presses the interaction key, the countdown is triggered
* GamePlayingTime - after the countdown finishes, the player can start interacting with kitchen objects and counters to create recipes
* GameOver - after the level timer is finished, the total number of recipes delivered is displayed, along with the high score. The game can then be replayed. 
***
### Input system
The game uses the new Unity input system and currently supports keyboard and gamepads. I plan to implement touch screen as well.<br>
Default controls: 
* keyboard: move with WASD/arrows, interact with E, alternative interact with F, pause with ESCAPE; 
* gamepad / joystick: move with the left analog stick, interact with A, alternative interact with X, pause with START.
***
### Playing options
![Amazing Recipe: Sichang Story Multiplayer System](https://github.com/banyapon/Amazing-Recipes/blob/main/Assets/_Assets/46.png?raw=true)
The game is playable as a single player game, or as a multiplayer with up to 4 players. 
Multiplayer System used Netcode SDK for GameObjects to implement multiplayer. 
* used NetworkVariable and server/client RPCs for synching
* players can create private or public lobbies
* players can join public lobbies from a list of available lobbies or enter a lobby code to join a private lobby
* players can change their player name and color of apron
* appropriate messages are shown when the player disconnects, when the host disconnects, when the session is waiting for all players to start the game or when the session is loading/synching data
* the game uses Relay for networking
***
### Code
Main system use resources from this repo [GitHub: Kitchen Chaos by Dorole]([https://pages.github.com/](https://github.com/Dorole/KitchenChaos)).
***
