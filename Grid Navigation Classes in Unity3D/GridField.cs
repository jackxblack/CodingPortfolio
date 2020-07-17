using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridField : MonoBehaviour
{
    public enum GridState { NotChecked, CurrentlyChecked, Ignored, Target };

    public int intRepresentationX; public int intRepresentationY;
    public string stringRepresentation;

    public bool isWalkable = true;

    public GridState state = GridState.NotChecked;

    public GameObject mesh;

    private void Start()
    {
        string[] name = this.transform.name.Split('_');
        intRepresentationX = int.Parse(name[1]); intRepresentationY = int.Parse(name[2]);
        stringRepresentation = name[1] + "," + name[2];
    }

    private void Update()
    {
        
    }

}
