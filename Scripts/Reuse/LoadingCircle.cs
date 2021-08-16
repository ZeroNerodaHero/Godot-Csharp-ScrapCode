using Godot;
using System;

public class LoadingCircle : TextureProgress
{
    [Signal]
    public delegate void finish_timer();
    
    public override void _Ready()
    {
        circleProgress = GetNode<TextureProgress>(".");
        timeforreset = GetNode<Timer>("CountDownTime");

        //timeforreset.WaitTime = 10;
    }

    public void startTimer(float t){
        timeforreset.Start(t);
    }

    public override void _Process(float delta)
    {
        //syncs texture with timer
        base._Process(delta);
        circleProgress.Value = (timeforreset.WaitTime-timeforreset.TimeLeft)/timeforreset.WaitTime * 100;
    }

    public void _on_CountDownTime_timeout(){
        //Console.WriteLine("Timer done");
        EmitSignal("finish_timer");
    }

    private TextureProgress circleProgress;
    private Timer timeforreset;
}
