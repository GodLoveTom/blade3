using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_Skill_lightball : MonoBehaviour
{
    [Header("生存周期")]
    public float mLifeT = 12;
    [Header("移动速度")]
    public float mV = 10;
    [Header("攻击距离")]
    public float mAtkL = 2.5f;

    [Header("闪电链")]
    public GameObject mLightChainPreb;
    float mDamage;
    float mDamageT; // 攻击计时
    float mBT; //出生时间

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float t = mBT + mLifeT;
        CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.LightBall);
        if (d != null)
            t += 3.0f * d.mLearnNum;

        if (Time.time > t)
        {
            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
            return;
        }

        CNpcInst npc = gDefine.gNpc.FindByL(transform.position.x, 40, CNpcInst.eNpcClass.All);
        if (npc != null)
        {
            float npcX = npc.GetPos().x;
            float l = Mathf.Abs(npcX - transform.position.x);
            if (l > mAtkL)
            {
                float x = transform.position.x;
                if (npcX > x)
                {
                    x += Time.deltaTime * mV;
                    if (x > npcX - mAtkL)
                        x = Mathf.Clamp(x, npcX - mAtkL, npcX - mAtkL + 0.5f);
                }
                else
                {
                    x -= Time.deltaTime * mV;
                    if (x < npcX + mAtkL)
                        x = Mathf.Clamp(x, npcX + mAtkL - 0.5f, npcX + mAtkL);


                }
                transform.position = new Vector3(x, transform.position.y, transform.position.z);


            }
            else
            {
                if (Time.time >= mDamageT)
                {
                    mDamageT = Time.time + 1;
                   // npc.BeDamage((int)(mDamage*1.2f), false, false, true,false,CSkill.eSkill.LightBall);
                    ShowLightChain(npc);

                }
            }
        }
    }

    void ShowLightChain(CNpcInst Npc)
    {
        GameObject obj = GameObject.Instantiate(mLightChainPreb);
        se_Skill_LightChain script = obj.GetComponent<se_Skill_LightChain>();
        script.Init(transform, Npc, 0, 1.0f);
    }

    public void Init(Vector3 BPos)
    {
        BPos.y = gDefine.gGrounY + 1.2f;
        transform.position = BPos;
        mDamageT = Time.time;

        mDamage = gDefine.gPlayerData.mDamage;
        mBT = Time.time;
    }
}
