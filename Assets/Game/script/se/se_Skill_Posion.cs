using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_Skill_Posion : MonoBehaviour
{
    [Header("生存时间")]
    public float mLiveT = 30;
    float mT = 0;
    [Header("伤害间隔时间")]
    public float mDamageSpareT = 1;
    float mDamageT;
    [Header("每次伤害占Npc血量百分比")]
    public float mDamgePercOfNpcHp = 0.01f;
    int mDamage;
    CNpcInst mNpc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > mT )
        {
            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
            mNpc.mIsPosion = false;
        }
        else
        {
            if(Time.time > mDamageT )
            {
                if( !mNpc.IsLive())
                {
                    gameObject.SetActive(false);
                    GameObject.Destroy(gameObject);
                }
                else
                {
                    mDamageT = Time.time + mDamageSpareT;
                
                    mNpc.BeDamage(mDamage,false,false,true);
                    
                    Transform t = gDefine.GetPCTrans();
                    CSkill skill = gDefine.gSkill.GetSkill(CSkill.eSkill.Poison);
                    gDefine.gDamageShow.CreateDamageShow(skill.mName, t.position + Vector3.up * 3, new Color(0.9f, 0.5f, 0.9f, 1));
                }
                
               
            }
        }
    }

    public void Init(Transform T, CNpcInst Npc)
    {
        mT = Time.time + mLiveT;
        mDamageT = Time.time;
        transform.SetParent(T);
        transform.localPosition = Vector3.zero;
        mNpc = Npc;

        float damagePerc = mDamgePercOfNpcHp;
         CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.Poison);
         if(d!=null)
            damagePerc += d.mLearnNum * 0.01f;
        mDamage = (int)(Npc.GetMaxHp() * damagePerc); 
        if(mDamage<3) mDamage = 3;
        Npc.mIsPosion = true;
    }
}
