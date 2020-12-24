using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_Skill_GroundArrow_Arrow : MonoBehaviour
{
    [Header("飞行速度")]
    public float mV = 20;

    [Header("命中特效")]
    public GameObject mHitSEPreb;
    int mDamage;
    bool mFaceRight = true;
    float mBx;
    float mT;

    Vector3 mEpos;
    CNpcInst mNpc;


    // Update is called once per frame
    void Update()
    {
        if(Time.time > mT)
        {
            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
        }

        Vector3 pos = Vector3.MoveTowards(transform.position, mEpos, mV * Time.deltaTime);

        if( mNpc != null&&mNpc.IsLive())
        {
            pos = Vector3.MoveTowards(transform.position, mNpc.GetHitSEPos(), mV * Time.deltaTime);
            transform.position = pos;

             Vector3 dir = mNpc.GetHitSEPos() - transform.position;
        }

        //Vector3 pos = Vector3.MoveTowards(transform.position, mEpos, mV * Time.deltaTime);
        transform.position = pos;

        CNpcInst npc = gDefine.gNpc.FindWithRealRAndNpcHitPos(pos, 0.8f, CNpcInst.eNpcClass.All);
        if(npc!=null)
        {
            npc.BeDamage(mDamage,false,false,true);

            GameObject o = GameObject.Instantiate(mHitSEPreb);
            o.transform.position = npc.GetHitSEPos();

            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
        }
    }

    public void Init(Vector3 BPos, Vector3 EPos, CNpcInst Npc,  int Damage )
    {
        mT = Time.time + 5;
        mDamage = Damage;

        //mFaceRight = FaceRight;
        transform.position = BPos;

        Vector3 dir = EPos - BPos;
        dir.z = 0;
        dir.Normalize();
        
        mEpos = BPos + dir * 30;

        transform.right = dir;

        mNpc = Npc;
        //mV = FaceRight?mV:-mV;

        
       // transform.rotation = new Quaternion();
       // if(!FaceRight)
         //   transform.Rotate(0,180,0,Space.World);
      
    }
}
