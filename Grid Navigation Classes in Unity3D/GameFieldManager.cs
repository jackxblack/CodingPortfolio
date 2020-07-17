using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFieldManager : MonoBehaviour
{
    public int rows;
    public int columns;

    public bool isGridInstantiated = false;

    public GameObject currentGridPrefab; //Later on instead of instantiating a bunch of identical GridFields I should be loading a prefab and using some method to randomize randomizable parameters on it.

    public delegate void GridInstantiatedEventHandler(object source, EventArgs args);
    public event GridInstantiatedEventHandler GridInstantiated;

    // Start is called before the first frame update
    void Start()
    {
        #region errors
        if (rows == null || rows == 0)
        {
            Debug.Log("Grid has no rows");
        }
        if (columns == null || columns == 0)
        {
            Debug.Log("Grid has no columns.");
        }
        #endregion
        GameObject.Instantiate(currentGridPrefab, this.transform);
        OnGridInstantiated();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Transform GetFieldTransform(int x, int y)
    {
        return GameObject.Find("Field_0" + x.ToString() + "_0" + y.ToString()).transform;
    }

    protected virtual void OnGridInstantiated()
    {
        isGridInstantiated = true;
        if (GridInstantiated != null)
        {
            GridInstantiated(this, EventArgs.Empty);
        }
    }

    public Vector3 GetFieldPosition(int x, int y)
    {
        GameObject node = GameObject.Find("Field_" + x.ToString() + "_" + y.ToString());
        return node.transform.position;
    }
}
