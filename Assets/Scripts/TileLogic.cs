using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class TileLogic : MonoBehaviour
{
    public LogicManagment logicManager;
    private static bool mustTumble;

    public GameObject bluePuck;
    public GameObject orangePuck;
    private GameObject[] puckList = new GameObject[2];

    private float heightOffset;
    Vector3 spawnPos;

    private int stackHeight;
    private bool[] stackComposition = new bool[7];

    public GameObject northNeighbor;
    public GameObject southNeighbor;
    public GameObject leftNeighbor;
    public GameObject rightNeighbor;

    // Start is called before the first frame update
    void Start()
    {
        stackHeight = 0;
        
        logicManager = logicManager.GetComponent<LogicManagment>();
        heightOffset = 2.1f * bluePuck.transform.localScale.y;

        puckList[0] = orangePuck;
        puckList[1] = bluePuck;
    }

    // Update is called once per frame
    void Update()
    {        
        if(stackHeight >= 4)
        {
            mustTumble = true;
        }
    }

    private void OnMouseEnter()
    {
        Debug.Log("In Range");
    }

    private void OnMouseExit()
    {
        Debug.Log("Gone");
    }

    [ContextMenu("Click")]
    private void OnMouseDown()
    {
        Debug.Log("Click is go");

        //choose a stack to tumble
        if (stackHeight >= 4)
        {
            Tumble();
        }
        //place a puck subroutine.
        else if (!mustTumble)
        {
            RoutingManager(logicManager.GetTurn());
            //puck decrement must be called in the clicker logic and not the instantiate logic. Taking a turn is what causes the stash to decrease, not updating
            //the positions of pucks on the board.
            logicManager.Decrement(logicManager.GetTurn());

            logicManager.UpdateTurn();
        }
    }

    //At somepoint I wrote the InstantiatePuck subroutine, joining an orange and a blue routine together, and checking the turn parity init.
    private void RoutingManager(bool blue)
    {
        if (!(gameObject.CompareTag("TheVoid")))
        {
            InstantiatePuck(blue);
        }
        else if (gameObject.CompareTag("TheVoid"))
        {
            logicManager.Increment(blue);
        }
    }

    private GameObject InstantiatePuck(bool blue)
    {
        int puckColor = blue ? 1 : 0;

        spawnPos = new Vector3(transform.position.x, (transform.position.y) + (++stackHeight * heightOffset) , transform.position.z);

        //Both instantiates the new puck and makes it a child of the correct tile
        Instantiate(puckList[puckColor], spawnPos, transform.rotation).transform.parent = transform;

        //Fill the stack array with the correct value at the correct position
        stackComposition[stackHeight - 1] = blue;

        //As far as I know, this function has never been set equal to anything
        return puckList[puckColor];
    }

    private void Tumble()
    {        
        FillNeighbors();

        ResetTile();
    }

    private void FillNeighbors()
    {
        int offset = logicManager.GetTurn() ? 2 : 0;
        Debug.Log("offset = " + offset);

        for (int i = 1; i <= stackHeight; i++)
        {
            switch ((i + offset) % 4)
            {
                case 1:
                    northNeighbor.GetComponent<TileLogic>().RoutingManager(stackComposition[stackHeight - i]);
                    break;
                case 2:
                    rightNeighbor.GetComponent<TileLogic>().RoutingManager(stackComposition[stackHeight - i]);
                    break;
                case 3:
                    southNeighbor.GetComponent<TileLogic>().RoutingManager(stackComposition[stackHeight - i]);
                    break;
                case 0:
                    leftNeighbor.GetComponent<TileLogic>().RoutingManager(stackComposition[stackHeight - i]);
                    break;
                default:
                    print("Something has gone terribly wrong with the universe");
                    break;
            }
        }
    }

    private void ResetTile()
    {
        Transform[] childArray = GetComponentsInChildren<Transform>();

        foreach (Transform child in childArray)
        {
            if (child != this.transform)
            {
                Destroy(child.gameObject);
            }

            //StartCoroutine(Waiter(child));
        }

        stackHeight = 0;
        mustTumble = false;
    }

    IEnumerator Waiter(Transform child)
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);

        if (child != this.transform)
        {
            Destroy(child.gameObject);
        }

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

    [ContextMenu("Read out Stack")]
    void ReadOutStack()
    {
        if(stackHeight > 0) {
            for (int i = 0; i < stackHeight; i++)
            {
                Debug.Log(stackComposition[i]);
            }
        }
    }
}
