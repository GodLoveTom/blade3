using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class com_hit_se : MonoBehaviour
{
    void Close()
    {
        gameObject.SetActive(false);
        GameObject.Destroy(gameObject);
    }
}
