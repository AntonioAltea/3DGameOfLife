using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectorCell : MonoBehaviour
{
    public int x, y, z;
    public bool alive = false;
    public float cubeSize;
    public bool hover = false;
    private Color originalColor;
    private Renderer objectRenderer;
    private SelectorGrid selectorGrid;

    public void Init(int x, int y, int z, bool alive, float cubeSize, SelectorGrid selectorGrid)
    {
        this.x = x;
        this.y = y;
        this.z = z;

        this.alive = alive;
        this.cubeSize = cubeSize;

        gameObject.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

        this.selectorGrid = selectorGrid;

        objectRenderer = gameObject.GetComponent<Renderer>();
        originalColor = objectRenderer.material.GetColor("_Color");
        UpdateColor();
    }

    public void Update()
    {
        UpdateColor();
    }

    public void UpdateColor()
    {
        if (hover)
        {
            objectRenderer.material.SetColor("_Color", Color.blue);
            objectRenderer.enabled = true;

        }
        else
        {
            if (alive)
            {
                objectRenderer.material.SetColor("_Color", Color.green);
                objectRenderer.enabled = true;
            }
            else
            {
                objectRenderer.material.SetColor("_Color", originalColor);
                objectRenderer.enabled = false;
            }
        }
    }

    public void Select()
    {
        if (alive)
            alive = false;
        else
            alive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10 | other.gameObject.layer == 11)
        {
            selectorGrid.Hover(x, y, z);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 10 | other.gameObject.layer == 11)
        {
            selectorGrid.Hover(x, y, z);
        }
    }
}
