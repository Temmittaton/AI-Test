using UnityEngine;
using System;

public class Tester : MonoBehaviour{
    public int startSize, targetSize;
    public CardManager cM;

    private int[] GetOptiCard(){
        int x = 0;
        int y = 0;
        while (cM.boards[0].card[x, y] != 1){
            if (x < cM.length){
                x++;
            }
            else {
                x = 0;
                y++;
            }
        }
        int[] array = new int[2];
        array[0] = x;
        array[1] = y;
        return array;
    }

    public void BoardsInit(){
        cM.InitializeBoards();
    }

    void Start(){
        BoardsInit();
        cM.height = startSize;
        cM.length = startSize;
    }

    void Update(){
        string result = cM.GameEnd(cM.boards[0]);

        if (result == "Continue"){
            cM.CardSelect(GetOptiCard(), cM.boards[0]);
        }
        else if (cM.height != targetSize){
            BoardsInit();
            cM.height++;
            cM.length++;
        }
    }
}
