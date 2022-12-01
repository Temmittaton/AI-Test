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
                    int _value = boards[b].card[l, h];

                    _card.transform.position = new Vector3((batchSize + 8) * b + l, -h, 0);
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
            _card.transform.position = new Vector3(i * (length + 2), 2, 0);
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
        else if (board.card[x, y] != 1){
            //Debug.Log("InvalidCard");
            visualizer[NN].GetComponent<SpriteRenderer>().color = Color.yellow;
            return "InvalidCard";
        }
        else {
            board.card[x, y] = 0;
            if (x + 1 < length){
                board.card[x + 1, y] = -(board.card[x + 1, y]);
            }
            if (x - 1 > -1){
                board.card[x - 1, y] = -(board.card[x - 1, y]);
            }
            if (y + 1 < height){
                board.card[x, y + 1] = -(board.card[x, y + 1]);
            }
            if (y - 1 > -1){
                board.card[x, y - 1] = -(board.card[x, y - 1]);
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
                    int _value = boards[b].card[l, h];
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
        foreach(int card in board.card){
            if (card == 1){
                return "Continue";
            }
        }
        foreach(int card in board.card){
            if (card == -1){
                return "Lost";
            }
        }
        return "Won";
    }
}

public class Board {
    public int[,] card;
    public GameObject[,] cardInstances;

    public Board(int length, int height){
        card = new int[length, height];
        cardInstances = new GameObject[length, height];

        for (int l = 0; l < length; l++){
            for (int h = 0; h < height; h++){
                int  _value;
                float _rand = (UnityEngine.Random.Range(-1f, 1f));

                _value = Mathf.RoundToInt(_rand);
                if (_value == 0){_value = 1;}

                card[l, h] = _value;
            }
        }
    }
}