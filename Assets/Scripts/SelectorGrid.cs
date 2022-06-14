using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SelectorGrid : MonoBehaviour
{
    private Vector3 originalPosition;

    private Transform previewCube;
    private float previewCubeError;
    public Tuple<int, int, int> hover = Tuple.Create(0, 0, 0);
    public GameObject cellPrefab;

    private static int Width, Height, Depth;
    [SerializeField]
    public Vector3 dimensions;

    public float cubeSize;
    public float rotation = 0;
    public float tickTime;

    public GameObject[,,] cellObjectArray;

    public List<(int, int, int)> seedList = new List<(int, int, int)>();

    void Start()
    {
        originalPosition = gameObject.transform.position;
        previewCube = gameObject.transform.Find("PreviewCube");
        UpdateSlidersValues();
        previewCubeError = cubeSize / 10;
        GenerateGrid();
    }
    public void InitiateGrid(List<(int, int, int)> seedList)
    {
        this.seedList = seedList;
    }

    void Update()
    {
        gameObject.transform.position = new Vector3(0, 1.3f, -1.2f);
        previewCube.localScale = new Vector3(Width, Height, Depth) * (cubeSize + previewCubeError);
        GetSlidersValues();
        if (GetSlidersValues().Item5 != tickTime)
        {
            UpdateSlidersValues();
        }
        else if (GetSlidersValues() != (Width, Height, Depth, cubeSize, tickTime))
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            DestroyGrid();
            UpdateSlidersValues();
            GenerateGrid();

            cellObjectArray[hover.Item1, hover.Item2, hover.Item3].GetComponent<SelectorCell>().hover = false;
            hover = Tuple.Create(0, 0, 0);
            cellObjectArray[hover.Item1, hover.Item2, hover.Item3].GetComponent<SelectorCell>().hover = true;
        }

        rotation = GameObject.Find("/OptionsCanvas/RotationSlider/Slider").GetComponent<Slider>().value;
        gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
    }

    void UpdateSlidersValues()
    {
        var values = GetSlidersValues();

        Width = values.Item1;
        Height = values.Item2;
        Depth = values.Item3;
        cubeSize = values.Item4;
        tickTime = values.Item5;

        dimensions = new Vector3(Width, Height, Depth);
    }

    (int, int, int, float, float) GetSlidersValues()
    {
        int width = (int)GameObject.Find("/OptionsCanvas/MainSection/WidthSlider/Slider").GetComponent<Slider>().value;
        int height = (int)GameObject.Find("/OptionsCanvas/MainSection/HeightSlider/Slider").GetComponent<Slider>().value;
        int depth = (int)GameObject.Find("/OptionsCanvas/MainSection/DepthSlider/Slider").GetComponent<Slider>().value;
        float cubeSize = GameObject.Find("/OptionsCanvas/MainSection/CubeSizeSlider/Slider").GetComponent<Slider>().value;
        float tickTime = GameObject.Find("/OptionsCanvas/MainSection/TickTimeSlider/Slider").GetComponent<Slider>().value;
        return (width, height, depth, cubeSize, tickTime);
    }

    void GenerateGrid()
    {
        cellObjectArray = new GameObject[Width, Height, Depth];

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

                    newCellObject.GetComponent<SelectorCell>().Init(x, y, z, seedList.Contains((x, y, z)), cubeSize, this);
                    cellObjectArray[x, y, z] = newCellObject;
                }
            }
        }

        cellObjectArray[hover.Item1, hover.Item2, hover.Item3].GetComponent<SelectorCell>().hover = true;
    }

    void DestroyGrid()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int z = 0; z < Depth; z++)
                {
                    Destroy(cellObjectArray[x, y, z]);
                    cellObjectArray[x, y, z] = null;
                }
            }
        }

        cellObjectArray = new GameObject[Width, Height, Depth];
    }

    public List<(int, int, int)> GetSeedList()
    {
        List<(int, int, int)> seedList = new List<(int, int, int)>();
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int z = 0; z < Depth; z++)
                {
                    if (cellObjectArray[x, y, z].GetComponent<SelectorCell>().alive)
                    {
                        seedList.Add((x, y, z));
                    }
                }
            }
        }
        return seedList;
    }

    public void Hover(int x, int y, int z)
    {
        Tuple<int, int, int> newHover = Tuple.Create(x, y, z);
        if (hover != newHover)
        {
            cellObjectArray[hover.Item1, hover.Item2, hover.Item3].GetComponent<SelectorCell>().hover = false;
            hover = newHover;
            cellObjectArray[hover.Item1, hover.Item2, hover.Item3].GetComponent<SelectorCell>().hover = true;
        }
    }

    public void HoverX(int amount)
    {
        if (Enumerable.Range(0, (int)dimensions.x).Contains(hover.Item1 + amount))
        {
            cellObjectArray[hover.Item1, hover.Item2, hover.Item3].GetComponent<SelectorCell>().hover = false;
            hover = Tuple.Create(hover.Item1 + amount, hover.Item2, hover.Item3);
            cellObjectArray[hover.Item1, hover.Item2, hover.Item3].GetComponent<SelectorCell>().hover = true;
        }
    }

    public void HoverY(int amount)
    {
        if (Enumerable.Range(0, (int)dimensions.y).Contains(hover.Item2 + amount))
        {
            cellObjectArray[hover.Item1, hover.Item2, hover.Item3].GetComponent<SelectorCell>().hover = false;
            hover = Tuple.Create(hover.Item1, hover.Item2 + amount, hover.Item3);
            cellObjectArray[hover.Item1, hover.Item2, hover.Item3].GetComponent<SelectorCell>().hover = true;
        }
    }

    public void HoverZ(int amount)
    {
        if (Enumerable.Range(0, (int)dimensions.z).Contains(hover.Item3 + amount))
        {
            cellObjectArray[hover.Item1, hover.Item2, hover.Item3].GetComponent<SelectorCell>().hover = false;
            hover = Tuple.Create(hover.Item1, hover.Item2, hover.Item3 + amount);
            cellObjectArray[hover.Item1, hover.Item2, hover.Item3].GetComponent<SelectorCell>().hover = true;
        }
    }

    public void SelectCell()
    {
        GetComponent<AudioSource>().Play();
        cellObjectArray[hover.Item1, hover.Item2, hover.Item3].GetComponent<SelectorCell>().Select();
        seedList = GetSeedList();
    }
}
