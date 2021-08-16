using Godot;
using System;

public class Player : Node2D
{
    //Debug
    private const bool debug = false;
    private const float pressDelay = 0.5F;
    
    [Signal]
    public delegate void update_health(int val);
    [Signal]
    public delegate void update_armor(int val, int val_cap);
    [Signal]
    public delegate void update_gunSprite(string path, int cpool, int tpool);
    [Signal]
    public delegate void update_gunCounter(int cpool, int tpool);
    [Signal]
    public delegate void isChambering();
    [Signal]
    public delegate void changedir(Vector2 ndir);
    [Signal]
    public delegate void updateLeftBox(string item_path, int cnt);

    //get player info and update stuff
    public override void _Ready()
    {
        //collision_OBJ = GetNode<Node2D>("/root/player/CollisionCalc");
        playerNode = GetNode<Node2D>(".");
        collision_OBJ = GetNode<Node2D>("CollisionCalc");
        sharedAsset = GetNode<Node2D>("SharedAsset");
        loadingCircle = GetNode<TextureProgress>("CircleProgress");
        keyTimer = GetNode<Timer>("keyStrokeTimer");
        create_player();
        
        //EmitSignal("update_health",1);
        //EmitSignal("update_armor",playerInfo.get_armor());
    }

    public override void _Process(float delta)
    {
        base._Process(delta);


        int xmove = 0, ymove = 0;
        {   //movement code. just vectors should have no problems
            if(Input.IsActionPressed("KEY_UP") && movedir[0] == true){
                ymove -= 1;
            } else if(Input.IsActionPressed("KEY_DOWN") && movedir[1]== true){
                ymove += 1;
            }
            if(Input.IsActionPressed("KEY_LEFT") && movedir[2]== true){
                xmove -= 1;
            } else if(Input.IsActionPressed("KEY_RIGHT") && movedir[3]== true){
                xmove += 1;
            }
        }
        if(xmove != 0 || ymove != 0){
            float nx = playerNode.Position.x + xmove * 1;
            float ny = playerNode.Position.y + ymove * 1;
            playerNode.Position = new Vector2(nx,ny);
            //what is this for?
            /*
            if(ymove != 0) movedir[0] = movedir[1] = true;
            if(xmove != 0) movedir[2] = movedir[3] = true;
            */
        }

        int bxmove = 0, bymove = 0;
        if(Input.IsActionPressed("BULLET_UP")){
            bymove -= 1;
        } else if(Input.IsActionPressed("BULLET_DOWN")){
            bymove += 1;
        }
        if(Input.IsActionPressed("BULLET_LEFT")){
            bxmove -= 1;
        } else if(Input.IsActionPressed("BULLET_RIGHT")){
            bxmove += 1;
        }
        if(bxmove != 0 || bymove != 0){
            Vector2 tmp = new Vector2(bxmove,bymove);
            if(shooting_direction != tmp){
                shooting_direction = tmp;
                EmitSignal("changedir", shooting_direction);
                //Console.WriteLine(shooting_direction);
            }
        }
        
        if(Input.IsActionPressed("SPACE") && canShoot == true && isIdle == true && playerInfo.get_cw_ammo_loaded() > 0){
            var stage_node = this.GetParent();
            Area2D bullet = (Area2D)GD.Load<PackedScene>("res://Scenes/Reuse/BulletArea2D.tscn").Instance();
            bullet.Position = new Vector2(playerNode.Position.x + shooting_direction.x * 10, playerNode.Position.y + shooting_direction.y * 10);
            bullet.Call("create",playerInfo.get_cweapon(),shooting_direction);
            stage_node.AddChildBelowNode(playerNode,bullet);
            
            playerInfo.cammo_update();
            EmitSignal("update_gunCounter",playerInfo.get_cw_ammo_loaded(),playerInfo.get_cw_ammo_left());
            
            canShoot = false;
            EmitSignal("isChambering");
        }

        if(isIdle){
            //Reload
            if(Input.IsActionPressed("RELOAD") && playerInfo.cw_should_reload()){
                isIdle = false;
                loadingCircle.Visible = true;
                loadingCircle.Call("startTimer",2);
            }
            else if(Input.IsActionPressed("SWAP") && canPress){
                keyTimer.Start(pressDelay);
                canPress = false;

                playerInfo.swap_current_weapon();
                EmitSignal("update_gunSprite",playerInfo.get_cweapon().file,playerInfo.get_cw_ammo_loaded(),playerInfo.get_cw_ammo_left());
            }
            else if(Input.IsActionPressed("NEXT_ITEM") && canPress){
                keyTimer.Start(pressDelay);
                canPress = false;

                playerInventory.nextItem();
                EmitSignal("updateLeftBox",playerInventory.getPath(),playerInventory.getItemCnt());
            }
            else if(Input.IsActionPressed("USE_ITEM") && canPress && playerInventory.canUse()){
                keyTimer.Start(pressDelay);
                canPress = false;

                playerInventory.updateItem();
                playerInfo.update_health(90);
                EmitSignal("updateLeftBox",playerInventory.getPath(),playerInventory.getItemCnt());
                EmitSignal("update_health",playerInfo.get_health());
            }
        }

    }

    //what is this???
    public void update_player(CharInfo.CharInfo playerInfo){
        this.playerInfo = playerInfo;
    }
    private void create_player(){
        playerInfo = new CharInfo.CharInfo();
        movedir = new bool[] {true,true,true,true};

        shooting_direction = new Vector2(1,0);

        //change atk speed
        canShoot = canPress = isIdle = true;
        sharedAsset.Call("updateAsset",0.4);
        loadingCircle.Visible = false;

        playerInventory = new Inventory();

        //collision_OBJ.Call("update_sprite",(loadImage(playerInfo.get_op_icon())));
        collision_OBJ.Call("update_sprite",GD.Load<Texture>(playerInfo.get_op_icon()));

        //calling update sprite must be after all things are intinatiated
        EmitSignal("update_gunSprite",playerInfo.get_cweapon().file,playerInfo.get_cw_ammo_loaded(),playerInfo.get_cw_ammo_left());
        EmitSignal("updateLeftBox",playerInventory.getPath(),playerInventory.getItemCnt());
    }

    public void _on_CollisionCalc_hit_by_explosion(int damage){
        playerInfo.update_health(-1* damage,1);
        EmitSignal("update_health",playerInfo.get_health());
        EmitSignal("update_armor",playerInfo.get_armor(),playerInfo.get_armor_cap());
    }
    public void _on_CollisionCalc_hit_by_object(int damage, string caliber){
        int cal = 1;
        if(caliber != "9mm" && caliber != ".45 ACP"){
            cal = 2;
        }
        playerInfo.update_health(-1* damage,cal);
        EmitSignal("update_health",playerInfo.get_health());
        EmitSignal("update_armor",playerInfo.get_armor(),playerInfo.get_armor_cap());

    }
    public void _on_SharedAsset_actionComplete(){
        //Console.WriteLine("can shoot");
        canShoot = true;
    }
    public void _on_CollisionCalc_cannotMove(int mx, int my){
        //we have 4 directions {top,bottom,left,right}
        if(my != 0){
            if(my == -1) movedir[0] = false;
            if(my == 1) movedir[1] = false;
        }
        if(mx != 0){
            if(mx == 1) movedir[2] = false;
            if(mx == -1) movedir[3] = false;
        }
    }
    public void _on_CollisionCalc_canMove(int mx, int my){
        if(my != 0){
            if(my == -1) movedir[0] = true;
            if(my == 1) movedir[1] = true;
        }
        if(mx != 0){
            if(mx == 1) movedir[2] = true;
            if(mx == -1) movedir[3] = true;
        }
    }
    public void _on_keyStrokeTimer_timeout(){
        canPress = true;
    }

    public void _on_CircleProgress_finish_timer(){
        loadingCircle.Visible = false;
        isIdle = true;
        playerInfo.cw_reload();
        EmitSignal("update_gunCounter",playerInfo.get_cw_ammo_loaded(),playerInfo.get_cw_ammo_left());
    }

    private CharInfo.CharInfo playerInfo;
    private Node2D collision_OBJ, playerNode, sharedAsset;
    private TextureProgress loadingCircle;
    private Godot.Collections.Array input_Map;
    private Timer keyTimer;

    private bool canShoot, canPress, isIdle;
    private bool[] movedir;
    private Vector2 shooting_direction;
    private Inventory playerInventory;

    public ImageTexture loadImage(string image_name){
        Image tmp = new Image(); 
        ImageTexture itex = new ImageTexture();
        tmp.Load(image_name);
        if(debug) Console.WriteLine(playerInfo.get_op_icon());
        itex.CreateFromImage(tmp,1);        //flag = 1 for pixel art. always use that
        return itex;
    }

}

//scrap code
/*
input_Map = InputMap.GetActions();
        for(int i = 0; i < input_Map.Count; i++){
            Console.WriteLine(input_Map[i]);
        }


bullet needs to be changed
*/