using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AIManagement))]
public class AIManagerEditor : Editor {
  public override void OnInspectorGUI(){
    DrawDefaultInspector();

    AIManagement aiM = (AIManagement)target;

    if (GUILayout.Button("Initialize training")){
      aiM.TrainingInit();
    }
  }
}