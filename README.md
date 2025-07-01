# Bald Gator 4: Rise of Kingdoms Edition (GDAPDEV)

## Overview

This project is a simplified mobile RPG game developed in Unity, inspired by the mechanics of Baldur's Gate 3. It features dice-based stat checks, turn-based grid combat, stat-driven dialogue outcomes, and branching quest progression created for mobile gameplay with device sensor integration.

## Key Features

### Dice Roll System

- 3D d20 Dice Asset used for skill checks and combat outcomes.
- External Dice Roll: Shake the phone using the accelerometer to roll.
- Internal Dice Roll: Programmatic checks used behind the scenes.
- Integrated across dialogue, combat, and quest mechanics.

### Characters and Stats

- 4-Character Party system (1 Player + 3 Companions).
- Each character has unique Stat Values:
  - Strength, Dexterity, Constitution, Intelligence, Wisdom, Charisma (ranging from 8 to 20).
- Job Classes define stat distribution and combat behavior (e.g., Fighter, Rogue, Mage).
- Stat values affect success thresholds for dialogue and combat rolls.

### Area Design

- A themed 3D Area divided into at least 4 distinct Sub Areas.
- Each Sub Area contains NPCs, interactable environments, and one Boss Room.
- Active Character system: switch between party members to leverage different stats for dialogue.
- Player character is the only visible one during exploration; full party appears in combat.

### Quest Progression

- At least 3 quests implemented:
  - 1 Main Quest (ends in a Boss Battle and determines the Ending)
  - 2 or more Sub Quests (affect endings and offer buffs, debuffs, or equipment)
- Linear Step-Based Structure: players must complete parts of a quest in order.
- Sub Quest outcomes have lasting effects on gameplay.

### Dialogue System

- At least 4 Dialogue Options per interaction:
  - 2 stat-based options that use dice rolls
  - INITIATE COMBAT
  - LEAVE
- Each stat-based option can only be rolled once.
- Optionally allows a single reroll by watching an advertisement.
- Dialogue choices influence quest progress and the final game ending.

### Turn-Based Grid Combat

- Combat takes place on a separate 2D grid scene within the 3D game world.
- Turn order is determined by Dexterity.
- Characters can ATTACK, HEAL, or MOVE per turn.
- Dice Rolls determine:
  - Hit or miss
  - Damage or critical hit
  - Heal effectiveness
- Job Class restrictions:
  - Fighters: melee, 2-tile movement
  - Rogues: ranged, 3-tile movement
  - Mages: ranged, 1-tile movement and can heal
- Enemies are static and do not move, but always attack.

### Game Endings

- The game has 3 different possible endings:
  - GOOD, NEUTRAL, BAD
- Endings are determined by dialogue decisions and Sub Quest outcomes.
- A hidden alignment meter tracks moral leaning based on player choices.

### Mobile Integration

- Designed specifically for mobile platforms (Android/iOS).
- Uses:
  - Tap and on-screen joystick for navigation and interaction
  - Accelerometer for dice rolling
  - Two additional gesture inputs such as swipe, drag, or pinch

## Tech Stack
- Engine: Unity (2022 or newer recommended)
- Language: C#
- Platform: Android/iOS
- Input: Touch, Accelerometer, Optional Gesture APIs
- Rendering: 3D assets and Unity UI system
