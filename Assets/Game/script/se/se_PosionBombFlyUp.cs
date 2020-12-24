using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_PosionBombFlyUp : MonoBehaviour
{
    public float mV = 15;
    float mDy=0;
    int  mNextNum = 0;
    int mDamage = 0;

    // Update is called once per frame
    void Update()
    {
        float dy = Time.deltaTime * mV;
        mDy += dy;
        transform.Translate(0,dy, 0, Space.World);
        if(mDy > 20)
        {
            GameObject.Destroy(gameObject);
            //
            if(mNextNum > 0 )
            {
                GameObject o = GameObject .Instantiate(gDefine.gData.mPosionPurSuerPosionBallPreb);
                se_PosionBall s = o.GetComponent<se_PosionBall>();
                s.Init(mNextNum,mDamage);
            }
        }
    }
    
    public void Init(Vector3 Pos, int DamageNum, int Damage )
    {
        transform.position = Pos;
        mDamage = Damage;
        mNextNum = DamageNum;
    }
}
