using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VersionText : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Text>().text = "Version " + Application.version;

    }
}
