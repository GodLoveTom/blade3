using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_skill_lun_juneng : MonoBehaviour
{
    int mDamage;
    [Header("伤害间隔时间")]
    public float mDamageSpareT = 0.7f;

    [Header("技能生存时间")]
    public float mLiveT = 1.5f;

    float mBeginx ;

    float mBt = 0;

    bool mFaceRight = true;

 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mLiveT -= Time.deltaTime;
        if (mLiveT <=0 )
        {
            gDefine.PcSkillActEndCallBack();
            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
           
        }
        else
        {
            if( Time.time > mBt )
            {
                mBt = Time.time + mDamageSpareT;
                float x = mFaceRight ? transform.position.x + 1000 :
                        transform.position.x - 10000;
                CalcDamge(x);

            }
        }
    }

    void CalcDamge(float X)
    {
        List<CNpcInst> tmpDict = new List<CNpcInst>();
        gDefine.gNpc.DoDamageShoot(mBeginx, X, mDamage, tmpDict, CNpcInst.eNpcClass.OnGround,true);
    }

    
    public void Init(float Bx, bool FaceRight, int Damage)
    {
       
        mDamage = Damage;
        mBt = Time.time + 0.3f ;
        mBeginx = Bx;
        mFaceRight = FaceRight;

        transform.rotation = new Quaternion();

        if (!FaceRight)
        {
            transform.Rotate(0, 180, 0, Space.Self);
        }
            
    }

    
}
