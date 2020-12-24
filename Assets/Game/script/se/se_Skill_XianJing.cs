using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_Skill_XianJing : MonoBehaviour
{
     [Header("存在时间")]
    public float mLiveT = 10;
    float mT;
    [Header("命中麻痹特效")]
    public GameObject mHitSEPreb; 
     [Header("触发距离")]
    public float mFindL; 

    // Update is called once per frame
    void Update()
    {
        if(Time.time > mT)
        {
            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
            return;
        }

        CNpcInst npc = gDefine.gNpc.FindByL(transform.position.x, mFindL,CNpcInst.eNpcClass.OnGround);
        if( npc != null )
        {
            GameObject o = GameObject.Instantiate(mHitSEPreb);
            //o.transform.SetParent( npc.GetRefMid().transform);
            //o.transform.localPosition = Vector3.zero;

            se_Skill_XianJingHit script = o.GetComponent<se_Skill_XianJingHit>();
            script.Init( npc );

            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
        }
    }

    public void Init()
    {
        mT = Time.time + mLiveT;
    }
}
