using Godot;
using System;

public class leftBox : TextureRect
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        itemTexture = GetNode<TextureRect>("ItemImg");
        itemList = GetNode<HBoxContainer>("ItemHboxList");
    }

    public void _on_Player_updateLeftBox(string path, int cnt){
        foreach (Node it in itemList.GetChildren()){
            it.QueueFree();
        }
        itemTexture.Texture = GD.Load<Texture>(path);
        itemTexture.RectScale = (new Vector2(1.5F,1.5F));

        //anymore than 5 just don't render
        for(int i = 1; i < cnt && i < 5; i++){
            TextureRect cpy = new TextureRect();
            cpy.Texture = GD.Load<Texture>(path);
            itemList.AddChild(cpy);
        }
    }

    private TextureRect itemTexture;
    private HBoxContainer itemList;

    public ImageTexture loadImage(string image_name){
        Image tmp = new Image(); 
        ImageTexture itex = new ImageTexture();
        tmp.Load(image_name);
        itex.CreateFromImage(tmp,1);        //flag = 1 for pixel art. always use that
        return itex;
    }
}
