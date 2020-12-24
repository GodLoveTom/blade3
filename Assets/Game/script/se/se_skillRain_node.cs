using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_skillRain_node : MonoBehaviour
{
    int mDamage;
    float mV;
    float mLiftT;
    Vector3 mBPos;
    Vector3 mEPos;
    bool mIsDone = false;
    float mDamageL = 1;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, mEPos, mV * Time.deltaTime);
        if( Mathf.Abs(transform.position.y - mEPos.y)<0.01f && !mIsDone)
        {
            gDefine.gNpc.DoDamageLR(transform.position, mDamageL, mDamage, false, CNpcInst.eNpcClass.All,
            true);
            mIsDone = true;
            //gDefine.gFollowCam.PlayVibrate();
            gDefine.PlaySound(19);
            transform.Rotate(0,0, Random.Range(-5,5));
        }

        mLiftT -= Time.deltaTime;
        if(mLiftT < 0 )
        {
            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
        }
    }

    public void Init(int Damage, Vector3 BPos, Vector3 EPos, float V, float LifeT,float DamageL)
    {
        mDamage = Damage;
        mV = V;
        mLiftT = LifeT;
        mDamageL = DamageL ;
        mBPos = BPos;
        mEPos = EPos;
        mIsDone = false;
        transform.position = BPos;
    }
}
