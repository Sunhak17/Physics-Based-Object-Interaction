# Physics-Based Object Interaction

## Overview

This Unity project demonstrates physics-based object interaction using Rigidbody and Unity’s physics engine. The main features include:
- Player movement and jumping using physics (`AddForce`)
- Interactive objects (doors, buttons, levers, plugs)
- Collision and trigger detection (`OnCollisionEnter`, `OnTriggerEnter`)
- Customizable physics properties (mass, drag, etc.)
- Environmental effects (wind zones, speed boosters, bounce)
- Prefabs for character, door, button, lever, plug, and socket

## Controls

- **WASD**: Move player
- **Space**: Jump
- **E**: Interact (pick up plug, press button, etc.)
- **k**: Kick the ball
- **Arrow Keys**: Rotate camera 

## Key Scripts

- `AN_HeroController.cs`: Player movement, jumping, and physics
- `AN_Button.cs`: Button/lever/valve interaction logic
- `AN_DoorScript.cs`: Door open/close logic
- `AN_PlugScript.cs`: Plug pickup and socket connection
- `Firework.cs`: Firework visual effect
- `MoveObject.cs`, `SprintJoint.cs`: Additional physics object examples

## Physics Features

- Uses `Rigidbody.AddForce` for movement, jumping, wind, and bounce
- Uses `OnCollisionEnter` and `OnTriggerEnter` for collision/trigger events

- All interactive objects have colliders and rigidbodies for realistic physics

