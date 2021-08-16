using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public class Inventory
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public Inventory(){
        itemName = new string[]{"Medkit","Armor Plate","NoItem"};
        path = new string[]{"res://Res/Items/Reusable/Medkit.png","res://Res/Items/Reusable/Armor Plate.png","res://Res/Items/Reusable/NoItem.png"};
        point = 0; tcnt = 7;
        
        itemList = new Dictionary<string, int>();
        itemList.Add("Medkit",3);
        itemList.Add("Armor Plate",4);
        itemList.Add("NoItem",1);
    }

    public int getItemCnt(){
        return itemList[itemName[point]];
    }
    public string getPath(){
        return path[point];
    }
    public bool canUse(){
        return itemName[point] != "NoItem";
    }
    public void updateItem(){
        if(itemName[point] == "NoItem"){
            return;
        }
        itemList[itemName[point]]--;
        tcnt--;
        if(itemList[itemName[point]] <= 0) nextItem();
    }

    public void nextItem(){
        if(tcnt == 0){
            point = itemName.Length-1;
            return;
        }
        do{
            point++;
            if(point == itemName.Length-1) point = 0;
        } while(itemList[itemName[point]] <= 0);
    }

    private Dictionary<string,int> itemList;
    private string[] itemName, path;
    private int point, tcnt;
}
