using Godot;
using System;
using System.Collections;

namespace CharInfo{
public class CharInfo
{
    private double[,] armormultiplier = new double[,] {{0.1,0.2,0.55,0.75,0.8},{0.05,0.1,0.15,0.25,0.4}};

    private ReadGun tmp_reader;
    private int health_point, armor_point;
    private GunOBJ.GunOBJ primary_weapon, secondary_weapon;
    private int pw_ammo_loaded, pw_ammo_left; 
    private int sw_ammo_loaded, sw_ammo_left; 
    private int ergonomics, armor_class, armor_cap;
    private string op_icon;
    private ArrayList statuslist;
    private bool currentWeapon;



    public CharInfo()
    {
        tmp_reader = new ReadGun();
        set_pweapon("Rifle",6);
        set_sweapon("Pistol",0);

        health_point = 100;
        armor_point = armor_cap = 200;
        armor_class = 6;
        currentWeapon = true;
        op_icon = "Res/Operators/SpetzHelmetOn.png";
        statuslist = new ArrayList();
    }

    public GunOBJ.GunOBJ get_cweapon(){
        //1-primary 0 - secondary
        if(currentWeapon){
            return primary_weapon;
        } else{
            return secondary_weapon;
        }
    }
    public void cw_reload(){
        if(currentWeapon){
            pw_reload();
        } else{
            sw_reload();
        }
    }
    public bool cw_should_reload(){
        return (currentWeapon) ? pw_should_reload() : sw_should_reload();
    }
    public void cammo_update(){
        if(currentWeapon){
            pammo_update();
        } else{
            sammo_update();
        }
    }
    public void swap_current_weapon(){
        currentWeapon = !currentWeapon;
    }
    public int get_cw_ammo_loaded(){
        if(currentWeapon){
            return get_pw_ammo_loaded();
        } else{
            return get_sw_ammo_loaded();
        }
    }
    public int get_cw_ammo_left(){
        if(currentWeapon){
            return get_pw_ammo_left();
        } else{
            return get_sw_ammo_left();
        }
    }


    public void set_pweapon(string weapon_type, int id){
        if(weapon_type == "Rifle"){
            primary_weapon = tmp_reader.readRifle(id);
            //Console.WriteLine(primary_weapon.name);

            ergonomics += primary_weapon.ergo;
            pw_ammo_left = primary_weapon.cap * 2;
            pw_ammo_loaded = primary_weapon.cap;
        }
    }
    private GunOBJ.GunOBJ get_pweapon(){
        return primary_weapon;
    }

    public void set_sweapon(string weapon_type, int id){

        if(weapon_type == "Pistol"){
            secondary_weapon = tmp_reader.readPistol(id);
            ergonomics += ((byte)secondary_weapon.ergo);
            sw_ammo_left = secondary_weapon.cap * 2;
            sw_ammo_loaded = secondary_weapon.cap;
        }
    }
    private GunOBJ.GunOBJ get_sweapon(){
        return secondary_weapon;
    }

    public void set_armor(int id){
        ergonomics += 10;
    }


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    /* holy shit am i suppose to write a get and set for everyone????
    */
    public int get_health() { return health_point; }
    public int get_armor() { return armor_point; }
    public int get_armor_cap() {return armor_cap; }

    //use negative values
    public void update_health(int nhp, int t = 0) {
        if(nhp > 0){
            health_point += nhp; 
        } else{
            int dhp = (int)(nhp * (1-armormultiplier[t-1, armor_class-2]));
            int dap = nhp - dhp;
            health_point += dhp;
            armor_point += dap;
            //Console.WriteLine(health_point + " " + armor_point);
            //Console.WriteLine(nhp + " " + dhp + ":" + dap + "\n");
        }
        if(health_point > 100) health_point = 100;
        if(armor_point > armor_cap) armor_point = armor_cap;
    }
    public void update_armor(int ap) {
        armor_point += ap; 
        if(armor_point > armor_cap) armor_point = armor_cap;
    }

    //bug. will still shoot after being 0
    private void pammo_update(){
        pw_ammo_loaded--;
    } 
    private void sammo_update(){ 
        sw_ammo_loaded--;
    } 
    private void pw_reload(){
        int gunCap = primary_weapon.cap;
        int ammototal = pw_ammo_left + pw_ammo_loaded;
        if(ammototal >= gunCap){
            pw_ammo_loaded = gunCap;
            pw_ammo_left = ammototal - gunCap;
        } else{
            pw_ammo_loaded = ammototal;
            pw_ammo_left = 0;
        }
    }
    private bool pw_should_reload(){
        return primary_weapon.cap != pw_ammo_loaded && pw_ammo_left > 0;
    }

    private void sw_reload(){
        int gunCap = secondary_weapon.cap;
        int ammototal = sw_ammo_left + sw_ammo_loaded;
        if(ammototal >= gunCap){
            sw_ammo_loaded = gunCap;
            sw_ammo_left = ammototal - gunCap;
        } else{
            sw_ammo_loaded = ammototal;
            sw_ammo_left = 0;
        }
    }
    private bool sw_should_reload(){
        return secondary_weapon.cap != sw_ammo_loaded && sw_ammo_left > 0;
    }

    private int get_pw_ammo_loaded(){ return pw_ammo_loaded; }
    private int get_pw_ammo_left(){ return pw_ammo_left; }

    private int get_sw_ammo_loaded(){ return sw_ammo_loaded; }
    private int get_sw_ammo_left(){ return sw_ammo_left; }

    public int get_ergo(){ return ergonomics; }
    public int get_armor_class(){ return armor_class; }
    public string get_op_icon(){ return op_icon; }

    public void status_push(int code){
        statuslist.Add(code);
    }
    public ArrayList status_get(){
        return statuslist;
    }
    public void clearStatus(int code){
        statuslist.Remove(code);
    }
}

}