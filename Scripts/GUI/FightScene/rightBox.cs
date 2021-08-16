using Godot;
using System;

public class rightBox : TextureRect
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        readyBox();
    }

    public void readyBox(){
        weaponTexture = GetNode<TextureRect>("WeaponImg");
        weaponLabel = GetNode<Label>("WeaponLabel");
    }

    public void _on_Player_update_gunSprite(string img_path,int cloaded, int tleft){
        //ready is called after signal
        if(!gotReady) readyBox();
        //weaponTexture.Texture = loadImage(img_path);
        weaponTexture.Texture = GD.Load<Texture>(img_path);
        
        weaponLabel.Text = cloaded + " / " + tleft;
    }
    
    public void _on_Player_update_gunCounter(int cloaded,int tleft){
        weaponLabel.Text = cloaded + " / " + tleft;
    }

    private TextureRect weaponTexture;
    private Label weaponLabel;
    private bool gotReady = false;

    public ImageTexture loadImage(string image_name){
        Image tmp = new Image(); 
        ImageTexture itex = new ImageTexture();
        tmp.Load(image_name);
        itex.CreateFromImage(tmp,1);        //flag = 1 for pixel art. always use that
        return itex;
    }
}
