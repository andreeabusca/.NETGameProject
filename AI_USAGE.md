# AI Usage Disclosure

This document outlines the the role of AI tools during the development stage of the project.

## Tools:
- **ChatGPT (Free Version, 5.3-min)**: chat-based code suggestions, rubber-ducking
- **Gemini (Free Version, Gemini for Students )**: alternative chat-based code suggestions rubber-ducking (when ChatGPT did not provide good suggestions)

## Usage Methods
- **Chat-based Code Suggestions**: generation of code regarding mathematical formulas and physics algorithms, debugging (build errors, errors with damage when Player and Enemy are attacking, etc)
- **Rubber-ducking** : verification and clarification of code

## List of files/regions fully AI-generated
- **Models/EnemyObject.cs (in Update function)** : modification of X coordonate so that Enemy object changes position according to direction
- **Models/PlayerObject.cs (in Update function)** : logic for jump and going back to ground formulas
- **SpriteSheet.cs (GetFrame function)**:  formula for specifiyng animation frame
- **GameLogic.cs (Collides function)**:  verification of Player and Enemy frames collision 
- **HPRenderer.cs (in RenderLives function)**:  position on screen (dst) for Player and Enemy lives
