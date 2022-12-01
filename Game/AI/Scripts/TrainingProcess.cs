using UnityEngine;
using System;

public class TrainingProcess : MonoBehaviour {
    public CardManager cardManager;
    public float amount;
    public NeuralNetwork[] neuralNetworks;
    public int[] cost;
    public bool[] canContinue;
    private AIManagement aiManager;

    public TrainingProcess(CardManager gameManagerSource){
        cardManager = gameManagerSource;
    }

    public void NNsInit(int batchSize, int inputNodes, int midLayers, int midLayersNodes, int outputNodes){
        neuralNetworks = new NeuralNetwork[batchSize];
        cost = new int[batchSize];
        canContinue = new bool[batchSize];
        for (int i = 0; i < canContinue.Length; i++){
            canContinue[i] = true;
        }

        for (int i = 0; i < batchSize; i++){
            neuralNetworks[i] = new NeuralNetwork(inputNodes, midLayers, midLayersNodes, outputNodes);
            NeuralNetwork NN = neuralNetworks[i];

            for (int l = 0; l < midLayers + 2; l++){
                for (int n = 0; n < NN.layer[l].node.Length - 1; n++){
                    NN.layer[l].node[n] = 0f;
                    NN.layer[l].bias[n] = UnityEngine.Random.Range(-1f, 1f);

                    if (l != 0){
                        for (int w = 0; w < (NN.layer[l].node.Length - 1 * NN.layer[l - 1].node.Length - 1); w++){
                            NN.layer[l].weight[w] = UnityEngine.Random.Range(-1f, 1f);
                        }
                    }
                }
                //Debug.Log("Layer " + System.Convert.ToString(l) + " initialized.");
            //Debug.Log("NN " + System.Convert.ToString(i) + " initialized.");
            }
        }
    }

    public void RunNNs(){
        for (int n = 0; n < neuralNetworks.Length; n++){

            if (!canContinue[n]){continue;}

            int cardIndex = 0;
            float[] NNinputs = new float[neuralNetworks[n].layer[0].node.Length];

            foreach(Card card in cardManager.boards[n].card){
                NNinputs[cardIndex] = card.value;
            }

            neuralNetworks[n].SetInputs(NNinputs);
            neuralNetworks[n].RunThrought();
            float[] outputs = neuralNetworks[n].GetOutputs();

            int[] intOutputs = new int[2];
            intOutputs[0] = Mathf.FloorToInt(outputs[0]);
            intOutputs[1] = Mathf.FloorToInt(outputs[1]);

            String result = cardManager.CardSelect(intOutputs, cardManager.boards[n], n);

            if (result == "OutOfRange"){
                cost[n] += 5;
            }
            else if (result == "InvalidCard"){
                cost[n] += 1;
            }
        }
    }

    public int[] GetBest(){
        int[] bests = new int[2];
        bests[0] = 999;
        bests[1] = 999;

        for (int n = 0; n < cost.Length; n++){
            if (cost[n] < bests[1]){
                if (cost[n] < bests[0]){
                    bests[1] = bests[0];
                    bests[0] = n;
                }
                else {
                    bests[1] = n;
                }
            }
        }

        return bests;
    }

    public void EndTest(){
        for (int n = 0; n < neuralNetworks.Length; n++){
            String result = cardManager.GameEnd(cardManager.boards[n]);
            if (result == "Won"){
                cost[n] -= 10;
                canContinue[n] = false;
            }
            else if (result == "Lost"){
                cost[n] += 8;
                canContinue[n] = false;
            }
        }
    }
}