💎 Day 05 – Match-3 Puzzle Game (Unity 2D)

This project is part of my **10 Days – 10 Games Challenge**, where I build a complete game each day to sharpen my Unity, C#, and game-development workflow.

**Day 05** is a fully featured **Match-3 Puzzle Game**, inspired by the classics — you swap gems to create matches, trigger cascades, score points, and race against time!

---

🎮 Play the Game

🔗 [Play on Itch.io](https://momoshomo.itch.io/day-05-match3-puzzle)
🔗 [Source Code on GitHub](https://github.com/yogesh4547/10-Days-10-Game-Day05-Match3Puzzle--candycrush-)

---

🕹 Gameplay Overview

* Swap adjacent gems to form 3+ in a row or column
* Matches vanish, gems above drop, new gems spawn
* Chain‐reactions and cascading combos possible
* Timer counts down; end when time runs out
* Score tallied; best score saved

---

✨ Features

* 8×8 gem board with random initialization
* Gem prefabs (colored sprites, idle/selection/destroy animations)
* Swap logic, match detection (horizontal & vertical), combo detection
* Falling refills and cascading matches
* Score system + high-score saving
* Timer UI, Start/Win/GameOver screens
* Clean UI using TextMeshPro
* Modern polish: gem animations, particle effects, sound effects

---

🎛 Controls

* Click or tap a gem to select it
* Click or tap an adjacent gem to swap
* Game Over or Win panel appears when timer ends or target achieved

---

📂 Project Structure

```
Assets/
 ├── Scripts/
 │     ├── Board.cs
 │     ├── Gem.cs
 │     ├── GameManager.cs
 │     └── (others)
 ├── Prefabs/
 ├── Sprites/     (gem sprites from asset pack)
 ├── Audio/       (optional SFX)
 ├── Scenes/
 └── UI/
```

---

📚 Assets

Gem sprites used from: *[https://tmatdev.itch.io/gems](https://tmatdev.itch.io/gems)*

---

🛠 Build & Deployment

1. Open in Unity 2022 LTS
2. Build → WebGL (or another platform)
3. Upload build to Itch.io (~link above)
4. Commit code & structure into this GitHub repo

---

🚀 Part of the 10 Days – 10 Games Challenge

1- Pong
2- Flappy Bird
3- Roll-a-Ball 3D
4- 2D Platformer
5- Match-3 Puzzle ← you are here!


