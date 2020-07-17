using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridScript : MonoBehaviour
{
    public int rows;
    public int columns;
    public List<GridField> fields;
    public List<GridField> unwalkableFields;

    // Start is called before the first frame update
    void Start()
    {
        rows = this.transform.childCount;
        columns = this.transform.GetChild(0).transform.childCount;

        #region gridVerification
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).transform.childCount != columns)
            {
                throw new ArgumentException("Row number " + i.ToString() + " doesn't have incorrect amount of children. Expected amount: " + columns.ToString() + ". Existing amount: " + this.transform.GetChild(i).transform.childCount);
            }
            else
            {
                Debug.Log("Row " + i.ToString() + " verified correctly.");
            }
        }
        #endregion

        #region field lists population
        for (int i = 0; i < rows; i++)
        {
            GameObject row = this.transform.GetChild(i).gameObject;
            for (int j = 0; j < columns; j++)
            {
                GridField field = row.transform.GetChild(j).gameObject.GetComponent<GridField>();
                fields.Add(field);                
                if (!field.isWalkable)
                {
                    unwalkableFields.Add(field);
                }
            }
        }        
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFieldState(int x, int y, GridField.GridState state)
    {
        GameObject.Find("Field_" + x + "_" + y).GetComponent<GridField>().state = state;
    }

    public bool isFieldWalkable(string s)
    {
        string[] splits = s.Split(',');
        int x = int.Parse(splits[0]); int y = int.Parse(splits[1]);
        GridField checkedField = GameObject.Find("Field_" + x + "_" + y).GetComponent<GridField>();
        if (unwalkableFields.Contains(checkedField))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public GridField GetFieldByCoord(int x, int y)
    {
        GridField searchedField;
        for (int i = 0; i < fields.Count -1; i++)
        {
            if (fields[i].intRepresentationX == x && fields[i].intRepresentationY == y)
            {                
                return fields[i];
            }
        }
        throw new Exception("Didn't find field with X: " + x + " Y: " + y + " in fields.");
    }

    public GridField GetFieldByCoord(string coord)
    {
        //Need to verify if coord string follows rules
        foreach (GridField field in fields)
        {
            if (field.stringRepresentation == coord)
            {
                return field;
            }
        }
        throw new Exception("Didn't find field " + coord + " in fields."); 
    }
}
