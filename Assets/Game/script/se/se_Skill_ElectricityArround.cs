using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSESkill_EA_Npc
{
    public CNpcInst mNpc;
    public float []mBall3 = new float[3];
}

public class se_Skill_ElectricityArround : MonoBehaviour
{
    [Header("伤害距离")]
    public float mAtkL = 2.0f;
    [Header("伤害间隔时间")]
    public float mDamageSpareT = 1;
    public float mDamageT = 0;
    [Header("伤害参数")]

    public float mDamageParam = 1;
    [Header("环绕的球")]
    public GameObject[] mBall;
    int mDamage;

    [Header("旋转速度")]
    public float mRotV = 180;

    List<CSESkill_EA_Npc> mNpcDict = new List<CSESkill_EA_Npc>();

    List<CNpcInst> mTmplist = new List<CNpcInst>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateDamage();
        UpdateBall();
    }

    void UpdateDamage()
    {

        for (int j = 0; j < mBall.Length; j++)
        {
            List<CNpcInst> tmp = GetNpcInstList(j);
            float perc = 0.1f;

            CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.ElectricityArround);
            if (d != null && d.mLearnNum > 0)
            {
                perc += 0.1f * d.mLearnNum;
            }

            mDamage = (int)(gDefine.gPlayerData.mDamage * perc);
            CNpcInst[] npcArr = gDefine.gNpc.DoDamageLR(mBall[j].transform.position, 0.6f, mDamage, false,
             CNpcInst.eNpcClass.OnGround, true, tmp);

            for (int i = 0; i < npcArr.Length; i++)
            {
                if (npcArr[i].IsLive())
                    AddNpcInst(npcArr[i], j);
            }
        }


        // if(Time.time >= mDamageT)
        // {
        //     mDamageT = Time.time + mDamageSpareT;
        //     CNpcInst[] npcArr = gDefine.gNpc.DoDamageLR(transform.position, mAtkL,mDamage, false, CNpcInst.eNpcClass.OnGround,true );

        //      CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.ElectricityArround);
        //      if(d!=null && d.mLearnNum>0)
        //      {
        //         for(int i=0;i<npcArr.Length; i++)
        //         {
        //             if(!ExistNpcInst(npcArr[i]))
        //             {
        //                 AddNpcInst(npcArr[i]);
        //                 npcArr[i].AddBuff(CBuff.eBuff.Paralysis, 1 );
        //             }
        //         }
        //      }
        // }
    }

    void AddNpcInst(CNpcInst Npc, int Index)
    {
        foreach (CSESkill_EA_Npc v in mNpcDict)
        {
            if (v.mNpc == Npc)
            {
                v.mBall3[Index] = Time.time + 1;
                return;
            }
        }

        CSESkill_EA_Npc d = new CSESkill_EA_Npc();
        d.mNpc = Npc;
        d.mBall3[Index] = Time.time + 1;
        mNpcDict.Add(d);
    }

    List<CNpcInst> GetNpcInstList(int Index)
    {
        mTmplist.Clear();
        for (int i = 0; i < mNpcDict.Count; i++)
        {
            CSESkill_EA_Npc v = mNpcDict[i];
            if (v.mNpc == null || !v.mNpc.IsLive() || (Time.time > v.mBall3[0] && Time.time > v.mBall3[1]
            && Time.time > v.mBall3[2]))
            {
                mNpcDict.RemoveAt(i);
                continue;
            }

            if (Time.time < v.mBall3[Index])
                mTmplist.Add(v.mNpc);
        }
        return mTmplist;
    }

    void UpdateBall()
    {
        transform.Rotate(0, Time.deltaTime * mRotV, 0, Space.Self);
        for (int i = 0; i < mBall.Length; i++)
        {

            if (mBall[i].transform.position.z > transform.position.z)
            {
                mBall[i].transform.forward = Vector3.back;
                //mBall[i].GetComponent<SpriteRenderer>().flipX = true;
                mBall[i].GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
            else
            {
                mBall[i].transform.forward = Vector3.forward;
                //mBall[i].GetComponent<SpriteRenderer>().flipX = false;
                mBall[i].GetComponent<SpriteRenderer>().sortingOrder = 4;
            }
            //    mBall[i].transform.rotation = new Quaternion(0,0.5f,0,0.5f);
            //else

        }
    }

    public void Init(Transform T)
    {
        transform.SetParent(T);
        transform.localPosition = Vector3.zero;
        mDamage = (int)(gDefine.gPlayerData.mDamage * mDamageParam);
    }
}
