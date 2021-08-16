using Godot;
using System;
using System.Collections.Generic;
using MapRoom;

public class RoomInter : Node2D
{
    
    public override void _Ready()
    {
        roomSelf = GetNode<Node2D>(".");
        roomFloor = GetNode<Sprite>("FloorSprite");
        room = new MapRoom.MapRoom(72,72,0);

        roomFloor.Texture = GD.Load<Texture>(room.floortile);
        foreach(KeyValuePair<string,int> tmp in room.mapOBJspawn){
            int x = tmp.Value;
            //xxxxxx
            int o = x & 1; x>>=1; 
            int y = (x & ((1<<11)-1)); x>>=11;
            
            Area2D decor = (Area2D)GD.Load<PackedScene>(tmp.Key).Instance();
            roomSelf.AddChildBelowNode(roomFloor,decor);

            float nx = 2*x + (x) * 16 + roomFloor.Position.x+2 + ((o == 1) ? 16 : 0);
            float ny = 2*y + (y) * 16 +roomFloor.Position.y+2;
            decor.Position = new Vector2(nx, ny);
            //Console.WriteLine(tmp.Key + " ("+x+","+y+") \t+ " + roomFloor.Position + "=" + decor.Position + "\t: " + o);

            if(o == 1) {decor.Rotate((float)Math.PI/2);}
        }
    }
    private Node2D roomSelf;
    private Sprite roomFloor;
    private MapRoom.MapRoom room;
}
