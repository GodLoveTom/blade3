using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_feilun : MonoBehaviour
{
    Vector3 mBPos;
    Vector3 mEPos;
    public float mV=20;
    public float mDamageL = 4;
    int mDamage;
    bool mIsToEpos = true;
    float mDeltT = 0;
    bool mIsRight;
    List<CNpcInst> mDamageDict = new List<CNpcInst>();

    public void Init(Vector3 BPos, float L,  bool IsRight , int Damage)
    {
        if( IsRight)
        {
            mBPos = BPos;
            mBPos.x += 0.8f;
            mEPos = mBPos;
            mEPos.x = mBPos.x + L;
            mIsRight = true;
        }
        else
        {
            mBPos = BPos;
            mBPos.x -= 0.8f;
            mEPos = mBPos;
            mEPos.x = mBPos.x - L;
            mIsRight = false;
        }

        gameObject.SetActive(true);
        transform.position = BPos;
        mDeltT = 0.5f;
        mDamage = Damage;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (mIsToEpos)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, mEPos, Time.deltaTime * mV);
            if (Mathf.Abs(pos.x - mEPos.x) < 0.01f)
            {
                mIsToEpos = false;
                mDamageDict.Clear();
            }
            gameObject.transform.position = pos;
        }
        else
        {
            Transform t = gDefine.GetPCTrans();
            Vector3 epos = t.position;
            epos.y = mBPos.y;
            Vector3 pos = Vector3.MoveTowards(transform.position, epos, Time.deltaTime * mV);
            if (Mathf.Abs(pos.x - t.position.x) < 0.01f)
            {
                //结束
                gameObject.SetActive(false);
                //gDefine.gPcLun.EndFiLunAct();
                
            }
            gameObject.transform.position = pos;
        }

        UpdateDamage();
        
    }

    void UpdateDamage()
    {

        float l = Mathf.Abs(transform.position.x - mBPos.x) + mDamageL * 0.5f;
        float ex = transform.position.x + mDamageL*0.5f;
        if (!mIsRight)
            ex -=  mDamageL;

        CNpcInst[] npc = gDefine.gNpc.DoDamageShoot( mBPos.x,ex , mDamage, mDamageDict, CNpcInst.eNpcClass.OnGround,true);
        if( npc != null)
        {
            for (int i = 0; i < npc.Length; i++)
                mDamageDict.Add(npc[i]);
        }


        //do damage..
        gDefine.DoThrowDamage(npc, mDamage);
    }

   
}
