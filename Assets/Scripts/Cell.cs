using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isAlive, newState;
    public int x, y, z, nNeighbour;
    public float tickTime;
    private Vector3 aliveScale;
    private Vector3 deadScale;

    public void Init(int x, int y, int z, bool isAliveInit, float tickTime, float cubeSize)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.isAlive = isAliveInit;
        this.newState = isAliveInit;
        this.tickTime = tickTime;

        aliveScale = new Vector3(cubeSize, cubeSize, cubeSize);
        deadScale = new Vector3(0, 0, 0);
        if (this.isAlive)
        {
            gameObject.transform.localScale = aliveScale;
        }
        else
        {
            gameObject.transform.localScale = deadScale;
        }
    }

    public void Death()
    {
        this.newState = false;
        ScaleToTarget(deadScale, tickTime);
    }

    public void Birth()
    {
        this.newState = true;
        ScaleToTarget(aliveScale, tickTime);
    }

    public void Tick()
    {
        isAlive = newState;
    }

    public void ScaleToTarget(Vector3 targetScale, float duration)
    {
        StartCoroutine(ScaleToTargetCoroutine(targetScale, duration));
    }

    private IEnumerator ScaleToTargetCoroutine(Vector3 targetScale, float duration)
    {
        Vector3 startScale = gameObject.transform.localScale;
        float timer = 0.0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            //smoother step algorithm
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        yield return null;
    }
}
