/// <summary>
/// 散花弹的掉落
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_NpcFlowerBombDrop : MonoBehaviour
{
    float mT;
    [Header("初始向上速度")]
    float mVy;

    [Header("重力，下落速度")]
    public float mDownAcc = -20;
    float mVx;

    bool mIsDrop = false;

    public void Init()
    {
        mIsDrop = true;
        mVy = Random.Range(4.0f, 8.0f) * 2.5f;
        mVx = Random.Range(-5.0f, 5.0f);
        mT = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (mIsDrop)
        {
            mT += Time.deltaTime;

            mVy += mDownAcc * Time.deltaTime;

            Vector3 pos = transform.position;
            pos.x += mVx * Time.deltaTime;
            pos.y += mVy * Time.deltaTime;

            if (pos.y < gDefine.gGrounY)
            {
                pos.y = gDefine.gGrounY;
                mIsDrop = false;
            }
            
            transform.position = pos;

        }


    }

    public bool IsDropEnd()
    {
        if (Mathf.Abs(transform.position.y - gDefine.gGrounY) < 0.01f)
            return true;
        else
            return false;
    }
}
