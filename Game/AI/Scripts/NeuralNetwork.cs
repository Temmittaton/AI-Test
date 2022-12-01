[System.Serializable]
public class NeuralNetwork {
    public Layer[] layer;

    public NeuralNetwork(int inputNodes, int midLayers, int midLayersNodes, int outputNodes){
        layer = new Layer[midLayers + 2];

        for (int i = 0; i < layer.Length; i++){
            if (i == 0){
                //Debug.Log("Input layer 0 created.");
                layer[i] = new Layer(inputNodes, 0);
            }
            else if (i == layer.Length - 1){
                //Debug.Log("Output layer " + System.Convert.ToString(i) + " created.");
                layer[i] = new Layer(outputNodes, (inputNodes * midLayersNodes) + midLayersNodes);
            }
            else {
                //Debug.Log("Mid layer : " + System.Convert.ToString(i) + "created.");
                layer[i] = new Layer(midLayersNodes, (layer[i - 1].node.Length - 1) * midLayersNodes + midLayersNodes);
            }
        }
    }

    public void RunThrought(){
        for (int l = 1; l < layer.Length; l++){
            //Debug.Log("Beginning runthrought for layer N° : " + Convert.ToString(l));

            for (int n = 0; n < layer[l].node.Length; n++){

                for (int pn = 0; pn < layer[l - 1].node.Length; pn++){

                    float currentNode = layer[l - 1].node[pn];
                    int currentWeightID = (n + pn * (layer[l].node.Length));

                    /*Debug.Log("PN = " + Convert.ToString(pn));
                    Debug.Log("Weight n ° : " + Convert.ToString(currentWeightID));
                    Debug.Log("Weight length = "  + Convert.ToString(layer[l].weight.Length));*/
                    float currentWeight = layer[l].weight[currentWeightID];

                    layer[l].node[n] += (currentNode * currentWeight);
                    //Debug.Log("Value : " + Convert.ToString(currentNode * currentWeight));
                }
            layer[l].node[n] += layer[l].bias[n];
            }
        }
    }

    public void Mutate(float amount){
        foreach(Layer layer in layer){
            for (int n = 0; n < layer.node.Length; n++){
                layer.node[n] += UnityEngine.Random.Range(-amount, amount);
            }
            for (int w = 0; w < layer.weight.Length; w++){
                layer.weight[w] += UnityEngine.Random.Range(-amount, amount);
            }
            for (int b = 0; b < layer.bias.Length; b++){
                layer.bias[b] += UnityEngine.Random.Range(-amount, amount);
            }
        }
    }

    public void Clone(NeuralNetwork target){
        target.layer = layer;
    }

    public void SetInputs(float[] inputs){
        for (int i = 0; i < inputs.Length; i++){
            //Debug.Log("Input n ° " + Convert.ToString(i) + " = " + Convert.ToString(inputs[i]));
            layer[0].node[i] = inputs[i];
        }
    }

    public float[] GetOutputs(){
        return layer[layer.Length - 1].node;
    }
}

public class Layer {
    public float[] node;
    public float[] weight;
    public float[] bias;

    public Layer(int nodes, int weights){
        node = new float[nodes];
        bias = new float[nodes];
        weight = new float[weights];
    }
}