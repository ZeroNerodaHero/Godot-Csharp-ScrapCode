using Godot;
using System;

public class collisionArea2Dprevent : Node2D
{
	[Signal]
	public delegate void cannotMove(int x,int y);
	[Signal]
	public delegate void canMove(int x,int y);

	public void _on_topArea_area_entered(Area2D area_entered){
		if(area_entered.IsInGroup("RigidOBJ")){
			movecnt[0]++;
			EmitSignal("cannotMove",0,-1);
		}
	}
	public void _on_bottomArea_area_entered(Area2D area_entered){
		if(area_entered.IsInGroup("RigidOBJ")){
			movecnt[1]++;
			EmitSignal("cannotMove",0,1);
		}
	}
	public void _on_leftArea_area_entered(Area2D area_entered){
		if(area_entered.IsInGroup("RigidOBJ")){
			movecnt[2]++;
			EmitSignal("cannotMove",1,0);
		}
	}
	public void _on_rightArea_area_entered(Area2D area_entered){
		if(area_entered.IsInGroup("RigidOBJ")){
			movecnt[3]++;
			EmitSignal("cannotMove",-1,0);
		}
	}

	//area exit to unlock direction
	public void _on_topArea_area_exited(Area2D area_exited){
		if(area_exited.IsInGroup("RigidOBJ")){
			movecnt[0]--;
			if(movecnt[0] == 0) EmitSignal("canMove",0,-1);
		}
	}
	public void _on_bottomArea_area_exited(Area2D area_exited){
		if(area_exited.IsInGroup("RigidOBJ")){
			movecnt[1]--;
			if(movecnt[1] == 0) EmitSignal("canMove",0,1);
		}
	}
	public void _on_leftArea_area_exited(Area2D area_exited){
		if(area_exited.IsInGroup("RigidOBJ")){
			movecnt[2]--;
			if(movecnt[2] == 0) EmitSignal("canMove",1,0);
		}
	}
	public void _on_rightArea_area_exited(Area2D area_exited){
		if(area_exited.IsInGroup("RigidOBJ")){
			movecnt[3]--;
			if(movecnt[3] == 0) EmitSignal("canMove",-1,0);
		}
	}
	private int[] movecnt = new int[4]{0,0,0,0};
}
