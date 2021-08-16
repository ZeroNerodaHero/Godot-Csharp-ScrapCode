using Godot;
using System;

public class SharedAsset : Node2D
{
    [Signal]
    public delegate void actionComplete();
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        shootingTimer = GetNode<Timer>("../SharedAsset/ShootingTimer");
        shootingTimer.OneShot = true;
    }
    //I need to set time to reset

    public void updateAsset(float timeReset){
        timetoReset = timeReset;
        shootingTimer.WaitTime = timeReset;
    }
    
    public void _on_EnemyNode_isChambering(){
        shootingTimer.WaitTime = timetoReset;
        shootingTimer.Start();
    }

    public void _on_Player_isChambering(){
        shootingTimer.WaitTime = timetoReset;
        shootingTimer.Start();
    }

    public void _on_ShootingTimer_timeout(){
        EmitSignal("actionComplete");
    }

    private Timer shootingTimer;
    private float timetoReset;
}
