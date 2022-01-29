using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(Transform))]
public class GeometrySnap : Editor
{
    Transform transform;
    private void OnSceneGUI()
    {
        Event e = Event.current;
        transform = (Transform)target;
        switch (e.type)
        {
            case EventType.KeyDown:
                {
                    if (Event.current.keyCode == (KeyCode.U))
                    {
                        Snap();
                    }
                    break;
                }
        }
    }

    void Snap()
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Undo.RecordObject(transform, "snap");

            transform.position = hit.point;
            transform.forward = hit.normal;

            PrefabUtility.RecordPrefabInstancePropertyModifications(transform);
        }
    }
}
