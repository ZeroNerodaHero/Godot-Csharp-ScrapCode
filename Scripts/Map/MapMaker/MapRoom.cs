using System;
using System.Collections.Generic;

namespace MapRoom{
public class MapRoom
{
    public MapRoom() {}
    public MapRoom(int X_size, int Y_size, int Rtype){
        rand = new Random();

        roomSize_X = X_size;
        roomSize_X = Y_size;
        type = Rtype;
        
        mapOBJspawn = new List<KeyValuePair<String,int>>();

        generateRoom();

    }

    private void generateRoom(){
        generateMedicRoom();
    }
    public List<KeyValuePair<String,int>> mapOBJspawn;


    private int roomSize_X, roomSize_Y, type;
    KeyValuePair<int,int> PlayerSpawn;
    public string floortile;
    private Random rand;


    public void generateMedicRoom(){
        generateFloorTile72();
        //select the current spawn spot. make random if multiple or get from position if selected
        PlayerSpawn = new KeyValuePair<int, int>(1,3);
        //select orientation of beds-
        int bedOrient = rand.Next(0,6);
        string bedFileLoc = "res://Scenes/Map/RoomDecor/Bed.tscn";
        if(bedOrient <= 2){
            mapOBJspawn.Add(new KeyValuePair<string,int>(bedFileLoc,hashOBJ(0,0,0)));
        } else{
            mapOBJspawn.Add(new KeyValuePair<string,int>(bedFileLoc,hashOBJ(0,0,1)));
        }

        if(bedOrient == 1){
            mapOBJspawn.Add(new KeyValuePair<string,int>(bedFileLoc,hashOBJ(2,0,0)));
        } else if(bedOrient == 2){
            mapOBJspawn.Add(new KeyValuePair<string,int>(bedFileLoc,hashOBJ(0,1,0)));
        } else if(bedOrient == 4){
            mapOBJspawn.Add(new KeyValuePair<string,int>(bedFileLoc,hashOBJ(1,0,1)));
        } else if(bedOrient == 5){
            mapOBJspawn.Add(new KeyValuePair<string,int>(bedFileLoc,hashOBJ(0,2,1)));
        }



        //select where to place medic cabinet
        List<KeyValuePair<int,int>> cabinet = new List<KeyValuePair<int,int>>();
        cabinet.Add(new KeyValuePair<int,int>(3,3));
        
        if(bedOrient != 5) cabinet.Add(new KeyValuePair<int,int>(0,3));
        if(bedOrient != 1) cabinet.Add(new KeyValuePair<int,int>(3,0));

        int cabinetOrient = rand.Next(0,cabinet.Count);
        int cabinetHash = hashOBJ(cabinet[cabinetOrient].Key,cabinet[cabinetOrient].Value,0);
        mapOBJspawn.Add(new KeyValuePair<string, int>("res://Scenes/Map/RoomDecor/cabinet.tscn",cabinetHash));
    }

    //stores the orientation of the bed: x(0,3); y(0,3); orientation(true or false)
    // xxxxxxxxxxxyyyyyyyyyyyo
    private int hashOBJ(int x, int y, int orient) { return x << 12 | y << 1 | orient; }
    private void generateFloorTile72(){ 
        //use of random to replace string
        //res://Res/Room_Res/FloorTile/floortile_{0}.png, the {0} can be subsitutued 
        floortile = "res://Res/Room_Res/FloorTile/floorPattern_" + rand.Next(0,3) + ".png"; 
    }
}
}

//Types of Medic Rooms can be
/*
Bb__    BbBb    Bb__    B___    BB__    B___    
____    ____    Bb__    b___    bb__    b___
____    ____    ____    ____    ____    B___
____    ____    ____    ____    ____    b___

0,0,0   0,0,0   0,0,0   0,0,1   0,0,1   0,0,1   
        2,0,0   0,1,0           1,0,1   0,2,1
*/