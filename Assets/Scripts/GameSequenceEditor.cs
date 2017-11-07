#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameSequence))]
public class GameSequenceEditor : Editor
{

    GameSequence gs;

    public void OnEnable()
    {
      
    }
    public override void OnInspectorGUI()
    {
        if(gs == null)
            gs = (GameSequence)target;
        EditorGUILayout.LabelField("Enemies Count : " +gs.Spawners.Count.ToString());
        EditorGUILayout.LabelField("Nb Screens : " + gs.nbScreens);
    }
}
#endif