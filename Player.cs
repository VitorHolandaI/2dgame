using Godot;
using System;

// This script controls the player character in a 2D game using Godot (C#).
public partial class Player : Area2D
{
    // The movement speed of the player, exposed in the editor for easy tweaking.
    [Export]
    public int Speed { get; set; } = 400;

    // Holds the size of the screen (viewport).
    public Vector2 ScreenSize;

    // Called when the node is added to the scene.
    public override void _Ready()
    {
        // Get the size of the screen so we can use it later to keep the player inside bounds.
        ScreenSize = GetViewportRect().Size;

        // Hide the player at the beginning (can be shown later when needed).
        Hide();
    }

    // Called every frame; delta is the time passed since the last frame.
    public override void _Process(double delta)
    {
        // Initialize velocity to zero.
        var velocity = Vector2.Zero;

        // Check input and adjust velocity accordingly.
        if (Input.IsActionPressed("move_right"))
        {
            velocity.X += 1;
        }

        if (Input.IsActionPressed("move_left"))
        {
            velocity.X -= 1;
        }

        if (Input.IsActionPressed("move_down"))
        {
            velocity.Y += 1;
        }

        if (Input.IsActionPressed("move_up"))
        {
            velocity.Y -= 1;
        }

        // Get the AnimatedSprite2D node to control animations.
        var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        // If there's movement input:
        if (velocity.Length() > 0)
        {
            // Normalize so diagonal movement isn't faster, then multiply by speed.
            velocity = velocity.Normalized() * Speed;

            // Play walking animation.
            animatedSprite2D.Play();
        }
        else
        {
            // Stop animation when no movement input.
            animatedSprite2D.Stop();
        }

        // Move the player by velocity scaled with delta time for frame-rate independent movement.
        Position += velocity * (float)delta;

        // Keep the player inside the screen boundaries.
        Position = new Vector2(
            x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
            y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
        );

        // Set animation direction based on velocity.
        if (velocity.X != 0)
        {
            // Set walking animation and flip based on direction.
            animatedSprite2D.Animation = "walk";
            animatedSprite2D.FlipV = false;
            animatedSprite2D.FlipH = velocity.X < 0;  // Flip horizontally if going left.
        }
        else if (velocity.Y != 0)
        {
            // Use "up" animation for vertical movement.
            animatedSprite2D.Animation = "up";

            // Flip vertically if moving down.
            animatedSprite2D.FlipV = velocity.Y > 0;
        }
    }
}
