using Godot;
using System;

public class shooting_pointer : Sprite
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        arrow = GetNode<Sprite>(".");
        arrowImg = new Texture[3];
    
        /*
        arrowImg[0] = loadImage("res://Res/GUI/Arrow/horizontalArrow.png");
        arrowImg[1] = loadImage("res://Res/GUI/Arrow/verticalArrow.png");
        arrowImg[2] = loadImage("res://Res/GUI/Arrow/diagonalArrow.png");
        */

        arrowImg[0] = GD.Load<Texture>("res://Res/GUI/Arrow/horizontalArrow.png");
        arrowImg[1] = GD.Load<Texture>("res://Res/GUI/Arrow/verticalArrow.png");
        arrowImg[2] = GD.Load<Texture>("res://Res/GUI/Arrow/diagonalArrow.png");

        //assuming initial direction is (1,0);
        dir = new Vector2(1,0);
    }

    public void _on_Player_changedir(Vector2 ndir){
        dir = ndir;
        arrow.FlipV = arrow.FlipH = false;

        if(ndir.y == 0){
            //horizontal case
            arrow.Texture = arrowImg[0];
            if(ndir.x == -1){
                arrow.FlipH = true;
            }
            arrow.Position = new Vector2(12 * ndir.x,0);
        } else if(ndir.x == 0){
            //vertical case
            arrow.Texture = arrowImg[1];
            if(ndir.y == 1){
                arrow.FlipV = true;
            }
            arrow.Position = new Vector2(0,12 * ndir.y);
        } else{
            //diagonal case
            arrow.Texture = arrowImg[2];
            if(ndir.x == -1){
                arrow.FlipH = true;
            }
            if(ndir.y == 1){
                arrow.FlipV = true;
            }
            arrow.Position = new Vector2(9 * ndir.x,9 * ndir.y);
        }
    }

    private Sprite arrow;
    private Vector2 dir;

    private Texture[] arrowImg;

    public ImageTexture loadImage(string image_name){
        Image tmp = new Image(); 
        ImageTexture itex = new ImageTexture();
        tmp.Load(image_name);
        itex.CreateFromImage(tmp,1);        //flag = 1 for pixel art. always use that
        return itex;
    }
}
