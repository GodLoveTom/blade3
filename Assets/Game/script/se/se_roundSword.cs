using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_roundSword : MonoBehaviour
{

 [Header("Npc回旋镖 飞行速度")]
    public float mV = 20;
    Vector3 mDestPos;
    Vector3 mOriPos;
    Vector3 mNpcPos;
    CNpcInst mNpc;
    int mDamge;
    bool mIsAtked = false;
    bool mIsFlyToDest = true;

    // Update is called once per frame
    void Update()
    {
        //calc move
        if (mIsFlyToDest)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, mDestPos, Time.deltaTime * mV);
            if (Vector3.Distance(pos, mDestPos) < 0.01f)
            {
                mIsFlyToDest = false;
                mIsAtked = false;
            }
            transform.position = pos;
        }
        else
        {
            if (mNpc.IsLive())
                mDestPos = mNpc.mNpc.mThrowPoint.transform.position;
            Vector3 pos = Vector3.MoveTowards(transform.position, mDestPos, Time.deltaTime * mV);
            if (Vector3.Distance(pos, mDestPos) < 0.01f)
            {
                gameObject.SetActive(false);
                GameObject.Destroy(gameObject);
                return;
            }
            transform.position = pos;
        }
        
        // calc damage.//
        if(!mIsAtked)
        {
            if(  
                Mathf.Abs (gDefine.GetPCTrans().position .x - transform.position.x) < 1 && 
               Mathf.Abs ( gDefine.GetPCTrans().position.y -  transform.position.y)<3) 
               {
                   gDefine.PcBeAtk(mDamge);
                   mIsAtked = true;
               }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="Npc"></param>
    /// <param name="Dest"></param>
    /// <param name="Damage"></param>
    /// <param name="V">-1使用设置的值</param>
    public void Init(Vector3 pos, CNpcInst Npc, Vector3 Dest, int Damage, float V = -1)
    {
        transform.position = pos;
        mDestPos = Dest;
        mDamge = Damage;
        mV = V > 0 ? V : mV;
        mNpc = Npc;

        transform.rotation = new Quaternion();

        if (Dest.x < pos.x)
            transform.Rotate(0, 180, 0, Space.World);

        mIsFlyToDest = true;

        mOriPos = pos;
    }

   
}
