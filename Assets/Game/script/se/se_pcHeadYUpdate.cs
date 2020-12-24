using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_pcHeadYUpdate : MonoBehaviour
{
    public GameObject mRefUIHpObj;

    // Update is called once per frame
    void Update()
    {
        if (mRefUIHpObj != null)
        {
            Vector3 pos = transform.localPosition;
            pos.y = mRefUIHpObj.activeSelf ? 1.3f : 1;
            transform.localPosition = pos;
        }
    }
}
