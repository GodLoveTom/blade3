using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_beAtkFlashOnScreen : MonoBehaviour
{
    float mT = 0;

    public void Show()
    {
        mT = Time.time + 0.2f;
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if( Time.time > mT)
        {
            gameObject.SetActive(false);
        }
    }
}
