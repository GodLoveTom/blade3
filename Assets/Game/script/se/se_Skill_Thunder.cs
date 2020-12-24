using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_Skill_Thunder : MonoBehaviour
{
    [Header("落雷子节点")]
    public GameObject mSwordPreb;
    [Header("落雷数量")]

    public int mSwordNum=12;
    [Header("落雷间隔")]

    public float mSpareT=0.1f;
    [Header("落雷速度")]

    public float mSwordV=50;
    float t;
    bool mBegin = false;
    Vector3 mCurAimPos;
    int mDamage;
    [Header("伤害系数")]
    public float mDamageParam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init()
    {
        t = mSpareT;
        mDamage = (int)(gDefine.gPlayerData.mDamage * mDamageParam);
    }

    // Update is called once per frame
    void Update()
    {
        if( mSwordNum > 0 )
        {
            t -= Time.deltaTime;
            if(t < 0 )
            {
                t = mSpareT;

                CNpcInst inst = gDefine.gNpc.FindRandomNpc( gDefine.GetPCTrans().position.x, 8);
                if( inst != null )
                {
                    mBegin = true;
                    mCurAimPos = inst.GetPos();
                }

                if( inst != null)
                {
                    GameObject sword = GameObject.Instantiate(mSwordPreb);

                    sword.GetComponent<se_Skill_ThunderNode>().Init( mDamage, inst.GetPos() , inst );

                }

                mSwordNum--;
                

            }
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
        
    }
}
