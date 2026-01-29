# MongoDB Dungeon Crawler (Console, C#)
<p align="center">
  <img src="https://raw.githubusercontent.com/Heijdenberg/img/refs/heads/main/DC.png" width="50%"/>
</p>

This is a small **console-based dungeon crawler** where you control a player on a map, encounter enemies, and play in a turn-based loop. The game uses **MongoDB** to store game data (for example saves/characters and other game content depending on the implementation).

---

## Prerequisites

- **.NET SDK** (to run `dotnet run`)
- **MongoDB Community Server** installed and running locally on Windows
- (Optional) **MongoDB Compass** to inspect the database visually

---

## Start MongoDB locally (Windows)

1. Install MongoDB Community Server  
2. Start the **MongoDB Server** service (Services)  
3. MongoDB should typically listen on: `mongodb://localhost:27017`

---

## Configure MongoDB connection

The project typically uses:
- **Connection string:** `mongodb://localhost:27017` (local)
- **Database name:** (may be hardcoded or stored in config)

If you need to change these, common places to check:
- `appsettings.json`
- a settings class (e.g. `DbSettings`, `MongoContext`, `GameRepository`)
- code that creates `MongoClient(...)`

---

## Add User Secrets in Visual Studio

Use User Secrets to keep credentials **out of source control** (for example if you use MongoDB Atlas or any authenticated connection string).

1. In **Visual Studio**, right-click your project in **Solution Explorer**
2. Click **Manage User Secrets**
3. Visual Studio opens a `secrets.json` file (stored locally on your machine)
4. Add your secrets as JSON, for example:

```json
{
  "ConnectionStrings": {
    "MongoDb": "mongodb://localhost:27017"
  }
}
```

---

## Run the project

1. Clone the repo and enter the folder:
```bash
git clone <your-repo-url>
cd <repo-folder>
```

2. Run:
```bash
dotnet run
```

On first run, the app will usually create its database/collections automatically if they are missing.

---

## How to play (typical controls)

- **Move:** `W A S D` or arrow keys  
- **Quit:** `Esc` (or a menu option)  
- **Open log/menu:** often `M`
---

## What is stored in MongoDB

Depending on your implementation, the game may store for example:
- Character/player (name, HP, position, stats)
- Level/map state (walls, enemies, discovered tiles)
- Turn/round counter
- Message log

You can inspect the database using **MongoDB Compass**:
- Host: `localhost`
- Port: `27017`

---
