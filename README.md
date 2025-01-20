# Half Chess Game - Server

## Overview
This repository contains the **server-side implementation** for the Half Chess Game project. The server is responsible for managing the game logic, handling player interactions, and providing game statistics through a web interface. The server also stores game data in a database and supports multiple game sessions simultaneously.

The game implements a simplified variant of chess called "Half Chess", which features a 4x8 board. Players interact with the server, and all game data (such as moves and game statistics) are handled by the server.

## Features
- Centralized Game Management: The server handles all aspects of game logic and manages multiple game sessions.
- Player Registration and Validation: Players must register before starting the game, and their information is validated before play.
- Game Data Recording: The server records game details, such as players, moves, and game results.
- Query Game Statistics: The server supports querying statistics such as top players, games played by a player, and other game data.
- Database Integration: The server integrates with a database to store game data persistently.
- Web Interface: A Razor Pages web interface allows for querying and displaying game statistics and results.

## Prerequisites
- Visual Studio 2022 or later.
- .NET 6 or higher.
- SQL Server or another compatible database.
- Razor Pages for the web interface.

### Web Interface
The game includes a web interface to view and query game statistics. Players can query game data such as:
- Display all players with their details (sorted by names, case-sensitive).
- Show games played by a specific player.
- Display all games with their details.
- List players by their countries, and more.

### Query Features
The web interface supports the following queries:
1. Display all players with their details (sorted case-sensitive).
2. Show the latest game played by each player (sorted by name).
3. Display all games with full details.
4. Display the first player from each country who played the game.
5. Show a list of players who have played the most games.
6. Group players by the number of games played and display results in tables (sorted by number of games).
7. Allow filtering games by country.
8. Ensure that each query is implemented using LINQ without loops, although loops can be used for displaying results.

### Player Registration and Gameplay
1. **Player Registration**: A player must register on the website before playing, and the registration must be successful.
2. **Game Start**: The game will display the player's information upon starting the game.
3. **Game Data Recording**: Each game will record details such as the players involved, the start time, game duration, and any other relevant information.
4. **Move Simulation**: The game generates valid moves for the player, without needing an algorithm to calculate the best move.

### Server and Database
- **Server**: Each player (client) plays against a central server.
- **Database**: The game saves all relevant data for each game, including player details and game statistics. The database is also used for generating queries and showing results on the web interface.

### Model and Query Implementation
1. **LINQ**: All database queries should be implemented using LINQ to process and generate the required results efficiently.
2. **Server-side Processing**: The server should handle game management, including storing and retrieving game data, and provide a responsive web interface for querying game results.


## Setup Instructions
1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/Half-Chess-Game-Server.git

## Usage
1. Run the server before starting the client.
2. Use the web interface to register players, view game statistics.
