using UnityEngine;
using System.Collections;
using UnityEditor;
//referenced from https://github.com/SebLague/Field-of-View/blob/master/Episode%2001/Editor/FieldOfViewEditor.cs
//modified to work in 2D game
[CustomEditor(typeof(AgentFOV))]
public class FieldOfViewEditor : Editor
{

    void OnSceneGUI()
    {
        AgentFOV fov = (AgentFOV)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.forward, Vector3.right, 360, fov.viewRadius);
        Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle / 2, false);
        Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle / 2, false);

        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewRadius);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in fov.visibleTargets)
        {
            Handles.DrawLine(fov.transform.position, visibleTarget.position);
        }
        foreach (Transform visiblePowerup in fov.visiblePowerups)
        {
            Handles.DrawLine(fov.transform.position, visiblePowerup.position);
        }
    }

}