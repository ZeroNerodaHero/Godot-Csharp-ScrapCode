using Godot;
using System;

public class RayDetection : Area2D
{
    [Signal] public delegate void player_detect(Vector2 dir);
    [Signal] public delegate void player_undetect(Vector2 pos);

    public override void _Ready()
    {
        base._Ready();
        seenPlayer = false;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if(area_in != null) viewPlayer();

    }

    public void _on_DFov_area_entered(Area2D area_entered){
        if(!area_entered.IsInGroup("Player")) return;
        area_in = area_entered;
    }

    public void _on_DFov_area_exited(Area2D area_exited){
        if(!area_exited.IsInGroup("Player")) return;
        if(seenPlayer){
            EmitSignal("player_undetect",area_in.GlobalPosition);
            seenPlayer = false;
        }
        area_in = null;
    }

    private Area2D area_in;
    private bool seenPlayer;

    private void viewPlayer(){
        if(area_in == null) return;
        Vector2 drawPos = new Vector2(area_in.GlobalPosition.x - GlobalPosition.x, area_in.GlobalPosition.y - GlobalPosition.y);

        var spaceState = GetWorld2d().DirectSpaceState;
        //Console.WriteLine(CollisionMask);
        var result = spaceState.IntersectRay(GlobalPosition, area_in.GlobalPosition,new Godot.Collections.Array { null },CollisionMask,false,true);
        if(result.Count != 0 && ((Area2D)result["collider"]).IsInGroup("Player")){
            EmitSignal("player_detect",getSqrt());
            seenPlayer = true;
        } else if(seenPlayer){
            //cannot see player anymore
            EmitSignal("player_undetect",area_in.GlobalPosition);
            seenPlayer = false;
        }
    }

    //i want ot use invsqrt for fast stuff
    public Vector2 getSqrt(){
        float dx = (area_in.GlobalPosition.x-GlobalPosition.x);
        float dy = (area_in.GlobalPosition.y-GlobalPosition.y);
        float mag = (float)Math.Sqrt(dx * dx + dy * dy);
        Vector2 ret = new Vector2(dx/mag,dy/mag);
        return ret;
    }
}
