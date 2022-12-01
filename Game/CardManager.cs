using UnityEngine;

public class CardManager : MonoBehaviour{
    public Board[] boards;
    public int length, height;
    public GameObject cardPrefab;
    private GameObject[] visualizer;

    public void InitializeBoards(int batchSize = 1){
        boards = new Board[batchSize];

        for (int b = 0; b < batchSize; b++){
            boards[b] = new Board(length, height);
        }

        BoardsDestroy();
        for (int b = 0; b < boards.Length; b++){
            //Debug.Log("Board n° " + Convert.ToString(b));
            for (int h = 0; h < height; h++){
                for (int l = 0; l < length; l++){
                    GameObject _card = Object.Instantiate(cardPrefab);
                    int _value = boards[b].card[l, h].value;

                    _card.transform.position = new Vector3(5 * b + l, -h, 0);
                    _card.transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);

                    if (_value == 1){_card.GetComponent<SpriteRenderer>().color = Color.white;}
                    else if (_value == -1){_card.GetComponent<SpriteRenderer>().color = Color.black;}
                    else {_card.GetComponent<SpriteRenderer>().color = Color.grey;}

                    boards[b].cardInstances[l, h] = _card;
                }
            }
        }

        visualizer = new GameObject[batchSize];

        for (int i = 0; i < batchSize; i++){
            GameObject _card = Object.Instantiate(cardPrefab);
            _card.transform.position = new Vector3(i * (length + 1), 2, 0);
            visualizer[i] = _card;
        }
    }

    public string CardSelect(int[] inputs, Board board, int NN = 0){
        int x = inputs[0];
        int y = inputs[1];

        //Debug.Log("x = " + Convert.ToString(x) + " , y = " + Convert.ToString(y));

        if ((x >= length) || (y >= height) || (y < 0) || (x < 0)){
            //Debug.Log("OutOfRange");
            visualizer[NN].GetComponent<SpriteRenderer>().color = Color.red;
            return "OutOfRange";
        }
        else if (board.card[x, y].value != 1){
            //Debug.Log("InvalidCard");
            visualizer[NN].GetComponent<SpriteRenderer>().color = Color.yellow;
            return "InvalidCard";
        }
        else {
            board.card[x, y].value = 0;
            if (x + 1 < length){
                board.card[x + 1, y].value = -(board.card[x + 1, y].value);
            }
            if (x - 1 > -1){
                board.card[x - 1, y].value = -(board.card[x - 1, y].value);
            }
            if (y + 1 < height){
                board.card[x, y + 1].value = -(board.card[x, y + 1].value);
            }
            if (y - 1 > -1){
                board.card[x, y - 1].value = -(board.card[x, y - 1].value);
            }
        }
        RenderBoards();
        //Debug.Log("Done");
        visualizer[NN].GetComponent<SpriteRenderer>().color = Color.green;
        return "Done";
    }

    public void RenderBoards(){
        for (int b = 0; b < boards.Length; b++){
            for (int h = 0; h < height; h++){
                for (int l = 0; l < length; l++){
                    int _value = boards[b].card[l, h].value;
                    GameObject _card = boards[b].cardInstances[l, h];

                    if (_value == 1){_card.GetComponent<SpriteRenderer>().color = Color.white;}
                    else if (_value == -1){_card.GetComponent<SpriteRenderer>().color = Color.black;}
                    else {_card.GetComponent<SpriteRenderer>().color = Color.grey;}
                }
            }
        }
    }

    public void BoardsDestroy(){
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");
        foreach(GameObject card in cards){
            Object.DestroyImmediate(card);
        }
    }

    public string GameEnd(Board board){
        foreach(Card card in board.card){
            if (card.value == 1){
                return "Continue";
            }
        }
        foreach(Card card in board.card){
            if (card.value == -1){
                return "Lost";
            }
        }
        return "Won";
    }
}

public class Board {
    public Card[,] card;
    public GameObject[,] cardInstances;

    public Board(int length, int height){
        card = new Card[length, height];
        cardInstances = new GameObject[length, height];

        for (int l = 0; l < length; l++){
            for (int h = 0; h < height; h++){
                int  _value;
                float _rand = 1/*(UnityEngine.Random.Range(-1f, 1f))*/;

                _value = (int)Mathf.Round(_rand);
                if (_value == 0){_value = 1;}

                card[l, h] = new Card(_value);
            }
        }
    }
}

public class Card {
    public int value;

    public Card(int givenValue){
        value = givenValue;
    }
}