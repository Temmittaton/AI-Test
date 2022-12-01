using System.Collections;
using UnityEngine;

public class AIManagement : MonoBehaviour {
    [SerializeField]
    private SaveSystem save;
    public CardManager cM;
    public TrainingProcess tP;
    public int batchSize, inputNodes, midLayers, midLayersNodes, outputNodes, iteration, iterationNumber;
    public bool renderBoards, renderNNS;
    private bool end;
    public float timeInterval;
    
    public void TrainingInit(){
        cM.InitializeBoards(batchSize);
    }

    public void SaveNNs(){
        save.NNSave(tP.neuralNetworks[0]);
    }

    public void LoadNNs(){
        foreach(NeuralNetwork NN in tP.neuralNetworks){
            NN.layer = save.NNLoad().layer;
        }
    }

    public void ResetSave(){
        save.ResetData();
    }
    public void BoardsDestroy(){
        cM.BoardsDestroy();
    }

    public void LoopStart(){
        TrainingInit();
        iteration = 0;
        for (int i = 0; i < batchSize; i++){
            tP.cost[i] = 0;
            tP.canContinue[i] = true;
        }
        Loop();
    }

    public void Loop(){
        tP.RunNNs();
        tP.EndTest();

        if (renderBoards){cM.RenderBoards();}
    }

    public void LoopEnd(){
        int[] bests = tP.GetBest();

        NeuralNetwork best = tP.neuralNetworks[bests[0]];
        NeuralNetwork best2 = tP.neuralNetworks[bests[1]];

        for (int i = 0; i < batchSize; i++){
            if (i < batchSize/2){
                tP.neuralNetworks[i] = best;
            }
            else {
                tP.neuralNetworks[i] = best2;
            }
        }
        for (int i = 0; i < batchSize; i++){
            tP.neuralNetworks[i].Mutate(tP.amount);
        }
    }

    public void Start(){
        tP.NNsInit(batchSize, inputNodes, midLayers, midLayersNodes, outputNodes);
        LoopStart();
    }

    public void Update(){
        end = true;
        for (int i = 0; i < batchSize; i++){
            if (tP.canContinue[i]){
                end = false;
                break;
            }
        }

        if ((!end) && (iteration < iterationNumber)){
            Loop();
            iteration++;
        }
        else {
            LoopEnd();
            LoopStart();
        }
    }
}