using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 0.1f);
    }


}
