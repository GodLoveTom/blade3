using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_Skill_LifeSpring : MonoBehaviour
{
    public string mSkillName=""; 
    public float mLiveT = 5;
    public float mAddHpSpareT = 1;
    public float mT;
    public float mAddHpPerc = 0.02f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if( Time.time > mLiveT )
        {
            transform.SetParent(null);
            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
        }

        else
        {
            if(Time.time > mT)
            {
                float addPerc = mAddHpPerc;
                if(mSkillName=="生命之泉")
                {
                    CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.LifeSpring);
                    if(d!=null)
                        addPerc += 0.01f*d.mLearnNum;
                }
                else
                {
                     CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.HealLight);
                    if(d!=null)
                        addPerc += 0.1f*d.mLearnNum;
                }
               
                int addHp = (int)(gDefine.gPlayerData.mHpMax * addPerc);
                gDefine.gPlayerData.mHp += addHp;

                if( gDefine.gPlayerData.mHp > gDefine.gPlayerData.mHpMax)
                    gDefine.gPlayerData.mHp = gDefine.gPlayerData.mHpMax;
                
                mT = Time.time + mAddHpSpareT;

                gDefine.gDamageShow.CreateDamageShow("+"+addHp.ToString(), gDefine.GetPCTrans().position + Vector3.up * 3, Color.green);
                gDefine.gDamageShow.CreateDamageShow(mSkillName, transform.position + Vector3.up * 3, new Color(0.9f, 0.5f, 0.9f, 1));

            }
        }
        
    }

    public void Init(Transform T)
    {
        transform.SetParent(T);
        transform.localPosition = Vector3.zero;
        mLiveT = Time.time + mLiveT;
        mT = 0;
    }
}
