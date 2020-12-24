using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_Skill_XianJingHit : MonoBehaviour
{
    [Header("生存时间")]
    public float mLiveT = 3;
    [Header("伤害百分比")]
    public float mDamagePerc = 0.1f;
    [Header("伤害间隔")]
    public float mDamageSapreT = 1;
    [Header("每次伤害附带麻痹时间")]
    public float mfrozenT = 0.5f;
    float mT;
    float mDamageT;
    CNpcInst mNpc;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > mT || !mNpc.IsLive())
        {
            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
            return;
        }

        if (Time.time > mDamageT)
        {
            float damagePrec = mDamagePerc;
            CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.XianJing);
            if (d != null)
                damagePrec += 0.1f * d.mLearnNum;

            int damage = (int)(mNpc.GetMaxHp() * damagePrec);
            if (damage > 0.001f)
                mNpc.BeDamage(damage, false, false, true);

            mDamageT = Time.time + mDamageSapreT;

            mNpc.AddBuff(CBuff.eBuff.Paralysis, mfrozenT);
        }
    }

    public void Init(CNpcInst Npc)
    {

        mT = Time.time + mLiveT;
        CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.XianJing);
        if (d != null)
            mT += d.mLearnNum;
        mDamageT = Time.time;
        mNpc = Npc;
        transform.SetParent(Npc.GetRefMid().transform);
        transform.localPosition = Vector3.zero;
    }

    public void Init(float LiveT, CNpcInst Npc, float DamagePrec, float FrozenT)
    {
        mLiveT = LiveT;

        mT = Time.time + mLiveT;
        mDamageT = Time.time;
        mNpc = Npc;
        transform.SetParent(Npc.GetRefMid().transform);
        transform.localPosition = Vector3.zero;

        mDamagePerc = DamagePrec;
        mfrozenT = FrozenT;
    }
}
