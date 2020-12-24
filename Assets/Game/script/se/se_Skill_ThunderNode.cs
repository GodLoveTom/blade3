using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_Skill_ThunderNode : MonoBehaviour
{
    int mDamage;
    CNpcInst mNpc = null;
    // Start is called before the first frame update
    [Header("地面爆裂特效")]
    public GameObject mHitSePreb;
    void Event_End()
    {
        if(mNpc!=null)
         {
              CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.Thunder);
              if(d!=null&&d.mLearnNum>0)
              {
                  int perc = d.mLearnNum * 10;
                  if(Random.Range(0,100) < perc )
                        mNpc.AddBuff(CBuff.eBuff.Paralysis, 0.8f);
              }

            
             mNpc.BeDamage(mDamage,false,false,true,false,CSkill.eSkill.Thunder);
             
         }   

        gameObject.SetActive(false);
        GameObject.Destroy(gameObject);
    }

    public void Event_PlaySound(int SoundId )
    {
        AudioClip clip = gDefine.gData.GetSoundClip(SoundId);
        if(clip != null)
            gDefine.gSound.Play(clip);
    }

    void Event_Hit()
    {
        GameObject o = GameObject.Instantiate(mHitSePreb);
        o.transform.position = transform.position;
    }

    public void Init( int Damage, Vector3 Pos, CNpcInst Npc)
    {
        mDamage = Damage;
        Pos.y = gDefine.gGrounY;
        transform.position = Pos;
        mNpc = Npc;
    }
}
