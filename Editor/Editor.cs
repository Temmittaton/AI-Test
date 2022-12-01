using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tester))]
public class GameManagerEditor : Editor {
  public override void OnInspectorGUI(){
    DrawDefaultInspector();

    Tester tester = (Tester)target;

    if (GUILayout.Button("Initialize board")){
      tester.BoardsInit();
    }
  }
}