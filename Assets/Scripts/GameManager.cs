using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gridPrefab;
    public GameObject selectorGridPrefab;

    private bool selectionStage = true;

    public GameObject selectorGridObject;
    public GameObject gridObject;
    // Start is called before the first frame update
    void Start()
    {
        selectorGridObject = Instantiate(selectorGridPrefab, new Vector3(0, 1.3f, -1.2f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (selectionStage)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
                selectorGridObject.GetComponent<SelectorGrid>().HoverX(1);
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                selectorGridObject.GetComponent<SelectorGrid>().HoverX(-1);
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                selectorGridObject.GetComponent<SelectorGrid>().HoverY(1);
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                selectorGridObject.GetComponent<SelectorGrid>().HoverY(-1);
            else if (Input.GetKeyDown(KeyCode.W))
                selectorGridObject.GetComponent<SelectorGrid>().HoverZ(1);
            else if (Input.GetKeyDown(KeyCode.S))
                selectorGridObject.GetComponent<SelectorGrid>().HoverZ(-1);
            else if (Input.GetKeyDown(KeyCode.E))
                selectorGridObject.GetComponent<SelectorGrid>().SelectCell();
            else if (Input.GetKeyDown(KeyCode.L))
                StartLife();
            else if (Input.GetKeyDown(KeyCode.K))
                ClearSelector();

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                selectorGridObject.GetComponent<SelectorGrid>().SelectCell();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.L))
                StopLife();
        }
    }

    public void StartLife()
    {
        gridObject = Instantiate(gridPrefab, selectorGridObject.transform.position, Quaternion.identity);
        Destroy(selectorGridObject);
        GridController grid = gridObject.GetComponent<GridController>();
        SelectorGrid sGrid = selectorGridObject.GetComponent<SelectorGrid>();
        grid.InitiateGrid(sGrid.GetSeedList(), sGrid.cubeSize, sGrid.tickTime, sGrid.rotation, sGrid.dimensions);
        selectionStage = false;
    }

    public void ClearSelector()
    {
        Vector3 selectorGridObjectPosition = selectorGridObject.transform.position;
        Destroy(selectorGridObject);
        selectorGridObject = Instantiate(selectorGridPrefab, selectorGridObjectPosition, Quaternion.identity);

    }

    public void StopLife()
    {
        selectorGridObject = Instantiate(selectorGridPrefab, gridObject.transform.position, Quaternion.identity);
        Destroy(gridObject);
        selectorGridObject.GetComponent<SelectorGrid>().InitiateGrid(gridObject.GetComponent<GridController>().seedList);
        selectionStage = true;
    }

    public void HoverX(int amount)
    {
        selectorGridObject.GetComponent<SelectorGrid>().HoverX(amount);
    }

    public void HoverY(int amount)
    {
        selectorGridObject.GetComponent<SelectorGrid>().HoverY(amount);
    }

    public void HoverZ(int amount)
    {
        selectorGridObject.GetComponent<SelectorGrid>().HoverZ(amount);
    }

    public void SelectCell()
    {
        selectorGridObject.GetComponent<SelectorGrid>().SelectCell();
    }
}
