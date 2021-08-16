using Godot;
using System;

namespace BottomCont{
public class BottomContainer : Container
{
    public override void _Ready()
    {
        /*
        _controlInfo = GetNode<Container>("/root/MainScene/controlInfo");
        _leftBox = GetNode<TextureRect>("/root/MainScene/controlInfo/leftBox");
        _rightBox = GetNode<TextureRect>("/root/MainScene/controlInfo/rightBox");
        _armorProgress = GetNode<TextureProgress>("../controlInfo/armorBar");
        _healthProgress = GetNode<TextureProgress>("../controlInfo/healthBar");
        */

        _controlInfo = GetNode<Container>(".");
        _leftBox = GetNode<TextureRect>("leftBox");
        _rightBox = GetNode<TextureRect>("rightBox");
        _armorProgress = GetNode<TextureProgress>("armorBar");
        _healthProgress = GetNode<TextureProgress>("healthBar");

        screen_size = OS.WindowSize;
    }

    public void _on_Player_update_health(int val){
        _healthProgress.Value = val;
    }
    public void _on_Player_update_armor(int val, int armor_cap){
        _armorProgress.Value = (0.0+val)/armor_cap * 100;
        //Console.WriteLine(_armorProgress.Value);
    }


    private Container _controlInfo;
    private TextureRect _leftBox, _rightBox;
    
    private Vector2 screen_size;
    private TextureProgress _armorProgress, _healthProgress;

    public ImageTexture loadImage(string image_name){
        Image tmp = new Image(); 
        ImageTexture itex = new ImageTexture();
        tmp.Load(image_name);
        //if(debug) Console.WriteLine(playerInfo.get_op_icon());
        itex.CreateFromImage(tmp,1);        //flag = 1 for pixel art. always use that
        return itex;
    }
}
}

/*
Scrap CODE

this was used to try and set size if the screen dim changes(ie iphone/android...)

        Console.WriteLine(screen_size.x + " " + screen_size.y);
        //_controlInfo.SetPosition(new Vector2(0,33));
        Console.WriteLine(_controlInfo.RectPosition.x + " " + _controlInfo.RectPosition.y);

*/