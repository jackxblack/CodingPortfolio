using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GridNavigationAgent : MonoBehaviour
{
    public GridField currentField;
    public GridField nextField;

    NavMeshAgent navMeshAgent;
    BaseCharater character;
    GameObject target;
    GridScript grid;

    bool isMoving = false;
    bool startUpdate = false;


    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        character = this.GetComponent<BaseCharater>();
        grid = GameObject.Find("GameField").transform.GetChild(0).GetComponent<GridScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startUpdate)
        {
            if (!isMoving && !character.isTargetInRange())
            {
                isMoving = true;
                Debug.Log(target);
                MoveToField(target.GetComponent<GridField>());
            }
            else if (isMoving && (this.transform.position.x == nextField.transform.position.x) && (this.transform.position.z == nextField.transform.position.z))
            {
                nextField = null;
                if (!character.isTargetInRange())
                {                    
                    MoveToField(target.GetComponent<GridField>());
                }
                else if (character.isTargetInRange())
                {                    
                    isMoving = false;
                    target = null;
                    character.target = null;
                    nextField = null;
                    character.state = CharacterStates.CharacterState.Idle;
                    startUpdate = false;
                }
            }
        }
    }

    public void MoveToTarget(GameObject moveTarget)
    {        
        target = moveTarget;
        startUpdate = true;
    }

    void MoveToField(GridField field)
    {
        nextField = GridNavigation.GetNextStep(currentField, target.GetComponent<GridField>(), grid);                
        currentField = nextField;                
        navMeshAgent.SetDestination(nextField.transform.position);
    }

    public void SetCurrentField(GridField field)
    {
        Debug.Log("Setting current field to " + field.stringRepresentation);
        currentField = field;
        Debug.Log("Current field after setting is " + currentField.stringRepresentation);
    }
}
