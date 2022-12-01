using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardManager))]
public class CardManagerEditor : Editor {
  public override void OnInspectorGUI(){
    DrawDefaultInspector();

    CardManager cM = (CardManager)target;

    if (GUILayout.Button("Destroy Boards")){
      cM.BoardsDestroy();
    }

    if (GUILayout.Button("Create Boards")){
      cM.InitializeBoards(10);
    }
  }
}