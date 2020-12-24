using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAirFlowerDropAndBomb : MonoBehaviour
{
    [Header("爆炸特效")]
    public GameObject mBombSEPreb;
    float mLiveT;
    float mT;
    [Header("初始向上速度")]
    public float mVyAdd=0;
    [Header("初始向上速度放大系数")]
    public float mDownAccMutil = 1;
    [Header("左右速度倍数")]
    public float mVxMutil=1;
    
    [Header("初始向上速度")]
    float mVy;
    [Header("重力，下落速度")]
    public float mDownAcc = -20;
  
    float mVx;
    public SpriteRenderer mR;

    public bool mIsRot = false;
    public float mRotV = 360;


    public void Init()
    {
        mLiveT = Random.Range(0.5f, 0.75f);
        mVy = Random.Range(4.0f, 8.0f)*mDownAccMutil +mVyAdd;
        mVx = Random.Range(-5.0f, 5.0f);
        mT = 0;
    }

    public void Init(float VX, float VY)
    {
        mLiveT = Random.Range(2.2f, 3.0f);
        mVy = VX;
        mVx = VY;
        mT = 0;
    }

    // Update is called once per frame
    void Update()
    {
        mT += Time.deltaTime;
        if (mT > mLiveT)
        {
           gameObject.SetActive(false);
           GameObject.Destroy(gameObject);
           if( mBombSEPreb != null )
           {
               GameObject o = GameObject.Instantiate(mBombSEPreb);
               o.transform.position = transform.position;
           }
           
        }
        else
        {
            UpdateDrop();
        }
    }

    void UpdateDrop()
    {
        if (mT > mLiveT * 0.5f)
        {
            float a = 1.0f - (mT - mLiveT * 0.5f) / mLiveT * 0.5f;
            mR.color = new Color(1, 1, 1, a);
        }

        mVy += mDownAcc * Time.deltaTime;

        Vector3 pos = transform.position;
        pos.x += mVx * Time.deltaTime;
        pos.y += mVy * Time.deltaTime;
    
        transform.position = pos;

        if (mIsRot)
        {
            transform.Rotate(0, 0, -Time.deltaTime * mRotV, Space.World);
        }
    }
}
