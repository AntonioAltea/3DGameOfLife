using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;


public class GridController : MonoBehaviour
{
    public GameObject cellPrefab;
    private Transform previewCube;
    float previewCubeError;

    public int setWidth, setHeight, setDepth;
    private static int Width, Height, Depth;
    private Vector3 dimensions;

    public int generation = 0;


    public float cubeSize;
    public float tickTime;
    public float rotation;

    // rules
    public (int, int) eRules = (4, 5);
    public (int, int) fRules = (4, 5);

    public Cell[,,] cellArray;

    public List<(int, int, int)> seedList;

    public void InitiateGrid(List<(int, int, int)> seedList, float cubeSize, float tickTime, float rotation, Vector3 dimensions)
    {
        this.seedList = seedList;
        this.cubeSize = cubeSize;
        this.tickTime = tickTime;
        this.rotation = rotation;
        Width = (int)dimensions.x;
        Height = (int)dimensions.y;
        Depth = (int)dimensions.z;
        this.dimensions = dimensions;

        previewCube = gameObject.transform.Find("PreviewCube");
        previewCubeError = cubeSize / 10;
        previewCube.localScale = dimensions * (cubeSize + previewCubeError);

        GenerateGrid();
        UpdateNNeighbour();
        StartLife();
    }

    void Update()
    {
        previewCube.localScale = new Vector3(Width, Height, Depth) * (cubeSize + previewCubeError);
        rotation = GameObject.Find("/OptionsCanvas/RotationSlider/Slider").GetComponent<Slider>().value;
        gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
    }

    void GetSlidersValues()
    {
        Width = (int)GameObject.Find("/OptionsCanvas/MainSection/WidthSlider/Slider").GetComponent<Slider>().value;
        Height = (int)GameObject.Find("/OptionsCanvas/MainSection/HeightSlider/Slider").GetComponent<Slider>().value;
        Depth = (int)GameObject.Find("/OptionsCanvas/MainSection/DepthSlider/Slider").GetComponent<Slider>().value;
        cubeSize = GameObject.Find("/OptionsCanvas/MainSection/CubeSizeSlider/Slider").GetComponent<Slider>().value;
        tickTime = GameObject.Find("/OptionsCanvas/MainSection/TickTimeSlider/Slider").GetComponent<Slider>().value;
    }

    void GenerateGrid()
    {
        cellArray = new Cell[Width, Height, Depth];

        Vector3 startingPoint = gameObject.transform.position - ((dimensions - new Vector3(1, 1, 1)) * cubeSize / 2);

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int z = 0; z < Depth; z++)
                {
                    GameObject newCellObject = Instantiate(
                        cellPrefab,
                        gameObject.transform.rotation * (new Vector3(x, y, z) * cubeSize + startingPoint),
                        gameObject.transform.rotation,
                        gameObject.transform);

                    Cell newCell = newCellObject.GetComponent<Cell>();
                    newCell.Init(x, y, z, seedList.Contains((x, y, z)), tickTime, cubeSize);
                    cellArray[x, y, z] = newCell;
                }
            }
        }
    }

    private void UpdateNNeighbour()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int z = 0; z < Depth; z++)
                {
                    Cell cell = cellArray[x, y, z];
                    cell.nNeighbour = GetCellNumberOfNeighbour(cell);
                }
            }
        }
    }

    public void StartLife()
    {
        InvokeRepeating(nameof(Tick), tickTime, tickTime);
    }

    private void Tick()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int z = 0; z < Depth; z++)
                {
                    Cell cell = cellArray[x, y, z];
                    cell.nNeighbour = GetCellNumberOfNeighbour(cell);
                    if (cell.isAlive)
                    {
                        if (AliveDies(cell.nNeighbour))
                        {
                            cell.Death();
                        }
                    }
                    else
                    {
                        if (DeadLives(cell.nNeighbour))
                        {
                            cell.Birth();
                        }
                    }
                }
            }
        }
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int z = 0; z < Depth; z++)
                {
                    cellArray[x, y, z].Tick();
                }

            }
        }
        generation++;
    }

    private bool AliveDies(int nNeighbour)
    {
        return (nNeighbour < eRules.Item1) || (nNeighbour > eRules.Item2);
    }

    private bool DeadLives(int nNeighbour)
    {
        return (nNeighbour >= fRules.Item1) && (nNeighbour <= fRules.Item2);
    }

    private int GetCellNumberOfNeighbour(Cell cell)
    {
        int nNeighbour = 0;
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int z = 0; z < Depth; z++)
                {
                    // if not the same cell
                    bool sameCell = (x == cell.x) && (y == cell.y) && (z == cell.z);
                    if (!sameCell)
                    {
                        bool xComponent = (x == (cell.x - 1)) || (x == cell.x) || (x == (cell.x + 1));
                        bool yComponent = (y == (cell.y - 1)) || (y == cell.y) || (y == (cell.y + 1));
                        bool zComponent = (z == (cell.z - 1)) || (z == cell.z) || (z == (cell.z + 1));
                        if (xComponent && yComponent && zComponent)
                        {
                            if (cellArray[x, y, z].isAlive)
                            {
                                nNeighbour++;
                            }
                        }
                    }

                }
            }
        }

        return nNeighbour;
    }
}
