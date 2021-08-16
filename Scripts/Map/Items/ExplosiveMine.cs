using Godot;
using System;

public class ExplosiveMine : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        explosiveScene = GetNode<Area2D>(".");
    }

    private Area2D explosiveScene;

    public void _on_ExplosiveMine_area_entered(Area2D area_enter){
        if(area_enter.IsInGroup("Operator"))
            explosiveScene.QueueFree();
    }

}
