using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButtonPuzzle : MonoBehaviour
{
    public Material green;
    public Material red;
    private string[] materialStates = new string[25];
    private Renderer[] renderers = new Renderer[25];
    public bool win = false;

    void Start()
    {
        for(int i = 0; i < transform.childCount; i++){            
            GameObject button = transform.GetChild(i).gameObject;
            renderers[i] = button.GetComponent<Renderer>();
            if(i%2==1){
                renderers[i].material = red;
                materialStates[i] = "red";
            }else{
                renderers[i].material = green;
                materialStates[i] = "green";
            }
        }
        renderers[12].material = red;
        materialStates[12] = "red";
    }
    public void touch(int index){
        if(win){
            return;
        }
        index -= 1;
        changeMaterial(index);
        if(index%5!=0){
            changeMaterial(index-1);
        }
        if(index%5!=4){
            changeMaterial(index+1);
        }
        if(index>=5){
            changeMaterial(index-5);
        }
        if(index<20){
            changeMaterial(index+5);
        }
        checkGame();
    }

    public void changeMaterial(int index){
        if(materialStates[index]=="red"){
            renderers[index].material = green;
            materialStates[index] = "green";
        }else{
            renderers[index].material = red;
            materialStates[index] = "red";
        }
    }

    public bool checkGame(){
        for(int i = 0; i < 25; i++){
            if(materialStates[i]=="red"){
                return false;
            }
        }
        win = true;
        Debug.Log("win the puzzle!!!");
        return win;
    }
}
