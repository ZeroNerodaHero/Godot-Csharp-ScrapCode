using Godot;
using System;

public class PlayerDetection : Node2D
{
    [Signal] public delegate void player_detect(Vector2 dir);
    [Signal] public delegate void player_undetect(Vector2 pos);

    public override void _Process(float delta)
    {
        base._Process(delta);
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
    }

    public override void _Ready()
    {
        detectNode = GetNode<Node2D>(".");
        face_dir = new Vector2(1,0);
    }

    public void _on_AFov_Bottom_area_entered(Area2D area_entered){
        if(!area_entered.IsInGroup("Player")) return;
        detectNode.Rotate((float)Math.PI/2);
        
        debugp("AFOV bot");
    }
    public void _on_AFov_Top_area_entered(Area2D area_entered){
        if(!area_entered.IsInGroup("Player")) return;
        detectNode.Rotate(-(float)Math.PI/2);
        
        debugp("AFOV top");
    }

    public void _on_DFov_player_detect(Vector2 dir){
        EmitSignal("player_detect",dir);
    }
    public void _on_DFov_player_undetect(Vector2 pos){
        EmitSignal("player_undetect",pos);
    }

    private Node2D detectNode;
    private Vector2 face_dir;
    
    private void debugp(string s){
        //Console.WriteLine(s);
    }

    //casts a ray to check player pos
    
}

/*
Initally i wrote this on the Alternative views
        //if(seePlayer != 0) return;
don't. bc we need to save the states and things will transform automatically

Just drawing
    public override void _Draw()
    {
        base._Draw();
        //DrawCircle(Position,10F,Color.Color8(128,0,0));
        if(area_in != null){
            Vector2 drawPos = new Vector2(area_in.GlobalPosition.x - GlobalPosition.x, area_in.GlobalPosition.y - GlobalPosition.y);
            DrawCircle(drawPos,10F,Color.Color8(128,0,0));
            //Console.WriteLine("\t" + area_in.GlobalPosition + " " + area_in.GlobalRotation );
            DrawLine(Position ,drawPos, Color.Color8(0,0,0));
            //DrawLine(Position ,area_in.Position, Color.Color8(0,0,0));
        }
    }

    private void viewPlayer(){
        if(area_in == null) return;
        Vector2 drawPos = new Vector2(area_in.GlobalPosition.x - GlobalPosition.x, area_in.GlobalPosition.y - GlobalPosition.y);

        var spaceState = GetWorld2d().DirectSpaceState;
        //var result = spaceState.IntersectRay(GlobalPosition, area_in.GlobalPosition);
        var result = spaceState.IntersectRay(GlobalPosition, area_in.GlobalPosition);
        if(result.Count != 0){
            Console.WriteLine("Player See");
        } else{
            Console.WriteLine("cannot see");
        }
    }

*/