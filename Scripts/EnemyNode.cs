using Godot;
using System;

public class EnemyNode : Node2D
{
    enum Enemy_states{
        Standby, Patrol, Pursuit, Search
    }
    //debug
    private const bool debug = false;
    // Called when the node enters the scene tree for the first time.

    [Signal]
    public delegate void isChambering();
    
    public override void _Ready()
    {
        //collision_OBJ = GetNode<Node2D>("/root/player/CollisionCalc");
        enemyNode = GetNode<Node2D>(".");
        collision_OBJ = GetNode<Node2D>("CollisionCalc");
        sharedAsset = GetNode<Node2D>("SharedAsset");
        canShoot = true;

        create_enemy();
        sharedAsset.Call("updateAsset",0.2);
        
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if(curState == Enemy_states.Standby){
            standby_process();
        } else if(curState == Enemy_states.Patrol){
            patrol_process();
        } else if(curState == Enemy_states.Pursuit){
            pursuit_process();
        } else if(curState == Enemy_states.Search){
//Console.WriteLine("searching");
            search_process();
        }
    }

    private void create_enemy(){
        enemyInfo = new CharInfo.CharInfo();
        collision_OBJ.Call("update_sprite",GD.Load<Texture>("res://Res/Operators/Poliz1.png"));
        //calling update sprite must be after all things are intinatiated
        dir = new Vector2(1,0);
        collisionDir = 15;
        curState = Enemy_states.Standby;
    }

    public void _on_SharedAsset_actionComplete(){
        canShoot = true;
    }

    public void _on_CollisionCalc_hit_by_object(int damage, string caliber){
        enemyInfo.update_health(-damage,2);
        //Console.WriteLine("fucking hit");
        if(enemyInfo.get_health() <= 0){
            enemyNode.QueueFree();
        }
    }

    public void _on_PlayerDetection_player_detect(Vector2 sig_dir){
        //Console.WriteLine("Signal Dir: " + sig_dir.x + " " + sig_dir.y);
        dir = sig_dir;
        curState = Enemy_states.Pursuit;
    }
    public void _on_PlayerDetection_player_undetect(Vector2 pos){
        curState = Enemy_states.Search;
        Sdist = new Vector2(Math.Abs(pos.x-Position.x),Math.Abs(pos.y-Position.y));
        trackPos = pos;
    }

    public void _on_CollisionCalc_canMove(int mx, int my){
        //we have 4 directions {top,bottom,left,right}
        if(my != 0){
            if(my == -1) collisionDir |= 1;
            if(my == 1) collisionDir |= (1<<1); 
        }
        if(mx != 0){
            if(mx == 1) collisionDir |= (1<<2); 
            if(mx == -1) collisionDir |= (1<<3); 
        }
    }

    public void _on_CollisionCalc_cannotMove(int mx, int my){
        if(my != 0){
            if(my == -1) collisionDir &= ~(1);
            if(my == 1) collisionDir &= ~(1<<1); 
        }
        if(mx != 0){
            if(mx == 1) collisionDir &= ~(1<<2); 
            if(mx == -1) collisionDir &= ~(1<<3); 
        }
    }

    private CharInfo.CharInfo enemyInfo;
    private Node2D collision_OBJ, enemyNode, sharedAsset;
    private Timer shootingTimer;

    private bool canShoot;
    private Vector2 dir, trackPos, Sdist;
    private Enemy_states curState;
    private float speed = 0.5F;
    private short collisionDir;

    private void standby_process(){
        //do nothing
    }

    private void patrol_process(){
        
    }

    private void pursuit_process(){
        shoot_bullet();
        bool canMoveX = XcanMove();
        bool canMoveY = YcanMove();

        float nx = enemyNode.Position.x;
        if(canMoveX){
            nx += dir.x * speed;
        }
        float ny = enemyNode.Position.y;
        if(canMoveY){
            ny += dir.y * speed;
        }
        enemyNode.Position = new Vector2(nx,ny);
    }

    private void search_process(){
        bool canMoveX = XcanMove();
        bool canMoveY = YcanMove();
        //Console.WriteLine(canMoveX + " " + canMoveY + " " + collisionDir + " ::: " + (collisionDir&(1<<2)));
        if((Sdist.x > 0 && canMoveX) || (Sdist.y > 0 && canMoveY)){
            float ny = enemyNode.Position.y, nx = enemyNode.Position.x; 
            if(Sdist.x > 0 && canMoveX) {
                float dx = dir.x * speed;
                nx += dx;
                Sdist.x -= Math.Abs(dx);
            }
            if(Sdist.y > 0 && canMoveY) {
                float dy = dir.y * speed;
                ny += dy * speed;
                Sdist.y -= Math.Abs(dy);
            } 
            enemyNode.Position = new Vector2(nx,ny);
        } else{
            curState = Enemy_states.Patrol;
            canMoveX = canMoveX = false;
        }
        
    }
    private bool XcanMove() {return (dir.x > 0) ? (((collisionDir>>3)&1) == 1) : (((collisionDir>>2)&1) == 1); }
    private bool YcanMove() {return (dir.y > 0) ? (((collisionDir>>1)&1) == 1) : ((collisionDir&(1)) == 1); }
    private void shoot_bullet(){
        if(canShoot){
            var stage_node = this.GetParent();
            Area2D bullet = (Area2D)GD.Load<PackedScene>("res://Scenes/Reuse/BulletArea2D.tscn").Instance();
            bullet.Position = new Vector2(enemyNode.Position.x + dir.x * 10, enemyNode.Position.y + dir.y * 10);
            bullet.Call("create",enemyInfo.get_cweapon(),dir);
            stage_node.AddChildBelowNode(enemyNode,bullet);

            canShoot = false;
            EmitSignal("isChambering");
        }
    }
}
