using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlanetGen))]
public class PlanetGenEditor : Editor
{
public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlanetGen pgen = (PlanetGen)target;
        if(GUILayout.Button("Make Planet"))
        {
            pgen.CreatePlanet();
        }
    }
}
