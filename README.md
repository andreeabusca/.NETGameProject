# CrowFightsFox

A simple 2D fighting game built using C#.  
The player controls a crow warrior and battles a fox.

---

## Gameplay Overview

CrowFightsFox is a side-view fighting game where the objective is to defeat the enemy before losing all your health.

- You control a **crow (Karasu Tengu)**
- You fight against a **fox (Kitsune)**
- Both characters can move, attack, and take damage

---

## Player Features

- Move left and right
- Jump with gravity-based physics
- Perform attacks with different animations
- Has **7 HP**

---

## Enemy Features

- Moves automatically left and right
- Changes direction at screen edges
- Randomly performs attacks
- Has **3 HP**

---

## Combat System

- Damage occurs when:
  - Player and enemy collide
  - One of them is attacking
- Each attack only deals damage **once per animation**
- Prevents repeated damage using a flag system

---

## Collision Detection


If the rectangles of the player and enemy overlap → a collision is detected.

---

## Animation System

- Uses **sprite sheets**
- Each character has:
  - Running animation
  - Multiple attack animations
  - Death animation

Frames are updated over time to create smooth animations.

---

## Game Loop

The game follows a standard loop:

1. Process input  
2. Update game state  
3. Render frame  

This loop runs continuously until the player exits the game.

---

## Rendering

- Background image fills the screen
- Player and enemy are rendered using textures
- Rendering is handled through SDL via Silk.NET

---

## Controls

| Key | Action |
|-----|--------|
| ←   | Move left |
| →   | Move right |
| ↑   | Jump |
| TAB | Attack |

---

## Win / Lose Conditions

- **Win** → Enemy HP reaches 0  
-  **Lose** → Player HP reaches 0  

When the game ends:
- A death animation plays
- The game stops updating

---
