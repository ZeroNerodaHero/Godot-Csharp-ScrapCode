using Godot;
using System;
using System.IO;
using System.Text;
using GunOBJ;
//using System.Text.Json;
using Newtonsoft.Json;

public class ReadGun : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    private string rifle_fileName, rifle_jsonString;
    private Newtonsoft.Json.Linq.JArray rifle_deserial;

    private string SMG_fileName, SMG_jsonString;
    private Newtonsoft.Json.Linq.JArray SMG_deserial;

    private string pistol_fileName, pistol_jsonString;
    private Newtonsoft.Json.Linq.JArray pistol_deserial;

    private string shotgun_fileName, shotgun_jsonString;
    private Newtonsoft.Json.Linq.JArray shotgun_deserial;

    public ReadGun()
    {
        rifle_fileName = "Res/Items/Gun/Stat/RifleInfo.json";
        rifle_jsonString = System.IO.File.ReadAllText(rifle_fileName);
        rifle_deserial = Newtonsoft.Json.Linq.JArray.Parse(rifle_jsonString);
        //rifle_deserial = JsonConvert.DeserializeObject(rifle_jsonString);
        //Console.WriteLine(rifle_jsonString);

        SMG_fileName = "Res/Items/Gun/Stat/SMGInfo.json";
        SMG_jsonString = System.IO.File.ReadAllText(SMG_fileName);
        SMG_deserial = Newtonsoft.Json.Linq.JArray.Parse(SMG_jsonString);

        pistol_fileName = "Res/Items/Gun/Stat/PistolInfo.json";
        pistol_jsonString = System.IO.File.ReadAllText(pistol_fileName);
        pistol_deserial = Newtonsoft.Json.Linq.JArray.Parse(pistol_jsonString);

        //not done
        shotgun_fileName = "Res/Items/Gun/Stat/RifleInfo.json";
        shotgun_jsonString = System.IO.File.ReadAllText(shotgun_fileName);
        shotgun_deserial = Newtonsoft.Json.Linq.JArray.Parse(shotgun_jsonString);
    }

    public GunOBJ.GunOBJ readRifle(int id){
        //Console.WriteLine("Read Rifle");
        return readWeapon(rifle_deserial, id);
    }
    public GunOBJ.GunOBJ readPistol(int id){
        return readWeapon(pistol_deserial,id);
    }

    private GunOBJ.GunOBJ readWeapon(Newtonsoft.Json.Linq.JArray deserial, int id){
        Newtonsoft.Json.Linq.JToken tmp = deserial[id];
        GunOBJ.GunOBJ ret_gun = new GunOBJ.GunOBJ();
        {
            ret_gun.name = tmp["name"].ToString();
            ret_gun.ergo = ((int)tmp["ergo"]);
            ret_gun.fire_rate = ((int)tmp["fire_rate"]);
            ret_gun.stability = ((int)tmp["stability"]);
            ret_gun.damage = ((int)tmp["damage"]);
            ret_gun.cap = ((int)tmp["cap"]);
            ret_gun.desc = tmp["desc"].ToString();
            ret_gun.file = tmp["file"].ToString();
            ret_gun.caliber = tmp["caliber"].ToString();
        }
        return ret_gun;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
