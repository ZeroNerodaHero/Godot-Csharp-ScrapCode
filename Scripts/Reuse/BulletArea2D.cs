using Godot;
using System;

public class BulletArea2D : Area2D
{
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
        caliber = gun.caliber;
        speed = 10;
        dir = vec;
        frames = 10;
        bulletScene = GetNode<Area2D>(".");
        bulletSprite = GetNode<Sprite>("CollisionShape2D/bulletSprite");

        //EX. "res://Res/Items/Bullet/riflediagonal.png"
        string filePath = "res://Res/Items/Bullet/";
        if(gun.caliber == "9mm" || gun.caliber == ".45 ACP"){
            filePath += "pistol";
        } else{
            filePath += "rifle";
        }
        
        bool flipH = false;
        if(vec.x < 0) flipH = true;
        filePath += "horizontal.png";
        //bulletSprite.Texture = loadImage(filePath);
        bulletSprite.Texture = GD.Load<Texture>(filePath);
        bulletScene.Rotate((float)Math.Atan(vec.y/vec.x));
        //Console.WriteLine(vec.x + " " + vec.y + " " + (float)Math.Atan(vec.y/vec.x));
        
        bulletSprite.FlipH = flipH;
        //bulletSprite.FlipV = flipV;
        //Console.WriteLine("shooting a bullet");
    }
    public int get_damage(){
        return damage;
    }
    public string get_caliber(){
        return caliber;
    }

    public void _on_Area2D_area_entered(Area2D area_enter){
        if(!(area_enter.IsInGroup("CollisionPrev") || area_enter.IsInGroup("FOV")))
            bulletScene.QueueFree();
    }

    private Area2D bulletScene;
    private int damage, speed, frames;
    private string caliber;
    private Vector2 dir;
    private Sprite bulletSprite;

    public static ImageTexture loadImage(string image_name){
            Image tmp = new Image(); 
            ImageTexture itex = new ImageTexture();
            tmp.Load(image_name);
            itex.CreateFromImage(tmp,1);        //flag = 1 for pixel art. always use that
            return itex;
    }
}
