using Godot;
using System;

public class BulletCalc : Node2D
{
    //CODE IS DEAD DONT USE
    /*
    public override void _Process(float delta)
    {
        base._Process(delta);

        float nx = this.Position.x + speed * dir.x;
        float ny = this.Position.y + speed * dir.y;
        this.Position = new Vector2(nx,ny);
        if(frames <= 0) QueueFree();
        frames--;
    }

    public void create(GunOBJ.GunOBJ gun, Vector2 vec){
        damage = gun.damage;
        type = gun.caliber;
        speed = 10;
        dir = vec;
        frames = 10;
        bulletSprite = GetNode<Sprite>("Area2D/CollisionShape2D/bulletSprite");

        //EX. "res://Res/Items/Bullet/riflediagonal.png"
        string filePath = "res://Res/Items/Bullet/";
        if(gun.caliber == "9mm" || gun.caliber == ".45 ACP"){
            filePath += "pistol";
        } else{
            filePath += "rifle";
        }
        bool flipV = false, flipH = false;
        if(vec.y == 0){
            //horizontal
            filePath += "horizontal.png";
            if(vec.x == -1) flipH = true;
        } else if(vec.x ==0){
            //vertical
            filePath += "vertical.png";
            if(vec.y == 1) flipV = true;
        } else{
            //diagonal
            filePath += "diagonal.png";
            if(vec.x == -1) flipH = true;
            if(vec.y == 1) flipV = true;
        }
        bulletSprite.Texture = loadImage(filePath);
        bulletSprite.FlipH = flipH;
        bulletSprite.FlipV = flipV;
        //Console.WriteLine("shooting a bullet");
    }

    private int damage, speed;
    private string type;
    private Vector2 dir;
    private int frames;
    private Sprite bulletSprite;

    public static ImageTexture loadImage(string image_name){
            Image tmp = new Image(); 
            ImageTexture itex = new ImageTexture();
            tmp.Load(image_name);
            itex.CreateFromImage(tmp,1);        //flag = 1 for pixel art. always use that
            return itex;
    }
    */

}
