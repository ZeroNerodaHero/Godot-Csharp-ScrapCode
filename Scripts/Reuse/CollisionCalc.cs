using Godot;
using System;

public class CollisionCalc : Node2D
{
    [Signal]
    public delegate void hit_by_object(int damage,string cal);
    [Signal]
    public delegate void hit_by_explosion(int damage);
    [Signal]
    public delegate void cannotMove(int x, int y);
    [Signal]
    public delegate void canMove(int x, int y);

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        collisionCalc = GetNode<Node2D>("../CollisionCalc");
        collisionArea = GetNode<Area2D>("../CollisionCalc/Area2D");
        collisionShape = GetNode<CollisionShape2D>("../CollisionCalc/Area2D/CollisionBox");
        sprite = GetNode<Sprite>("../CollisionCalc/Area2D/CollisionBox/CharSprite");
    }

    public void update_sprite(ImageTexture imgText){
        sprite.Texture = imgText;
    }

    public void _on_Area2D_area_entered(Area2D area_enter){
        if(area_enter.IsInGroup("BulletObject")){
            //Console.WriteLine("AREA ENTERED" + area_enter.Call("get_damage") + "  " + area_enter.Call("get_caliber"));
            EmitSignal("hit_by_object",area_enter.Call("get_damage"),area_enter.Call("get_caliber"));
        } else if(area_enter.IsInGroup("ExplosiveObject")){
            EmitSignal("hit_by_explosion",40);
        }
    }

    public void _on_collisionPrev_x16_cannotMove(int x, int y){
        EmitSignal("cannotMove",x,y);
    }
    public void _on_collisionPrev_x16_canMove(int x, int y){
        EmitSignal("canMove",x,y);
    }

    private Node2D collisionCalc;
    private Area2D collisionArea;
    private CollisionShape2D collisionShape;
    private Sprite sprite;

}
