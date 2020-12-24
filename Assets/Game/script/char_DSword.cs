using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class char_DSword : MonoBehaviour
{
    public enum eState
    {
        Idle = 0,
        Atk0 = 1,
        Atk1 = 2,
        Atk2 = 3,
        Atk3 = 4,

        Fly = 5,
        JumpBack = 6,

        Skill_Readying = 7, //蓄力
        Skill_Ready = 8, //蓄力完毕

        Skill_Flush = 9,//闪
        Skill_FlashKillHide = 10, // 瞬斩消失
        Skill_FlashAppearKill = 11,//瞬斩出现并攻击
        Skill_Drop,//下落
        Skill_DownKill,//落斩
        Skill_DownKillHitGround,//落斩结束
        Skill_backFlash,//后闪

        Paralysis, //麻痹
        BeAtk,//被打

        Skill_FlashKill, // 瞬斩

        Die, //死亡

        EndKillFly, //终结技
        EndKillTake, //终结技，抓取
        Faint,//眩晕


    }

    eState mNextActState = eState.Idle;
    bool mNextActFaceRight = true;

    eState mCurActState = eState.Idle;
    public bool mCurFaceRight = true;

    Vector3 mMoveTarget; //当前要移动的目标

    Animator mAnimator;

    public CNpcInst mTargetObj;
    CNpcInst mEndKillTarget;

    [Header("跳跃距离")]
    public float mJumpL = 3;
    [Header("跳跃速度")]
    public float mJumpV = 6;

    public float mAtkL = 3.2f; // 攻击距离
    public float mHAtkL = 4.4f; // 重击距离
    public float mBestToNpcL = 2.8f; // 与npc最佳距离
    public float mAtk0MoveL = 6; //攻击移动距离
    public float mAtk1MoveL = 6; //攻击移动距离
    public float mAtk2MoveL = 6; //攻击移动距离
    public float mAtk3MoveL = 6; //攻击移动距离

    public float mAtk0MoveV = 6; //攻击移动距离
    public float mAtk1MoveV = 6; //攻击移动距离
    public float mAtk2MoveV = 6; //攻击移动距离
    public float mAtk3MoveV = 6; //攻击移动距离

    [Header("突击距离 - 最近")]
    public float mFlyMinL = 6;
    [Header("突击距离 - 最远")]
    public float mFlyMaxL = 10; //突击距离

    [Header("突击速度")]
    public float mFlyV = 20;

    [Header("特效人物中心参考点")]
    public GameObject mRefPointMid;

    float mAtkActCoolDown = 0;   //攻击动作冷却
    eState mOldAtkAct = eState.Idle; //上一次的动作是什么

    public GameObject mRefMingzhongSe;

    [Header("UI 血条")]
    public Text mHpText;
    public Image mHpImage;
    public GameObject mHpRoot;
    float mHpRootT = 0;

    [Header("技能一闪 特效")]
    public GameObject mRefSESkillFlushPreb;
    [Header("技能一闪 距离")]
    public float mSkillFlushL = 10;
    [Header("技能一闪 速度")]
    public float mSkillFlushV = 50;
    // [Tooltip("技能一闪 开始位置")]
    float mSkillFlushBX;
    // [Tooltip("二闪 三闪")]
    int mSkillFlush2 = 0;
    int mSkillFlushCount = 0; //技能闪到第几次，伤害使用
    public CPlayerBuff mBuff = new CPlayerBuff();

    CQuickKill mQuickKill = new CQuickKill();
    float mQuickKillCoolDownT = 0; // 浮空斩cooldown

    bool mIsEndKillKickDownSoundPlay = false;
    bool mIsEndKillTakeSoundPlay = false;

    float mGunCoolDown = 0; // cooldown 5s. 

    float mIsInSkillIgnorDamgeT = 0;
    float mActInputForbidT = 0; //操作禁止

    float mFaintT = 0; //眩晕的禁止时间

    float mReliveT = 0;

    public ui_PcHp mUIHp;

    // Start is called before the first frame update
    void Start()
    {
        mAnimator = GetComponent<Animator>();
        gDefine.gPcDSword = this;
        RefreshHPUI(false);
        gameObject.SetActive(false);
    }

    public bool IsInEndKillState()
    {
        return mCurActState == eState.EndKillFly || mCurActState == eState.EndKillTake;
    }

    public void PushBack(Vector3 Pos, CNpcInst OriNpc)
    {
        if (gDefine.gPlayerData.mHp <= 0)
            return;

        if (Mathf.Abs(transform.position.y - gDefine.gGrounY) > 3)
            return;

        if (!gameObject.activeSelf)
            return;

        if (mCurActState == eState.Idle || mCurActState == eState.BeAtk ||
        mCurActState == eState.Atk0 || mCurActState == eState.Atk1 || mCurActState == eState.Atk2
        || mCurActState == eState.Atk3 || mCurActState == eState.Paralysis)
        {
            //撞退
            CNpcInst Npc = gDefine.gNpc.FindByL(transform.position.x,
            Pos.x > transform.position.x ? false : true, 3, OriNpc);
            float l = 1.5f;
            if (Npc != null)
            {
                l = Mathf.Abs(Npc.GetPos().x - transform.position.x);
                l -= 1.5f;
                if (l > 1.5f)
                    l = 1.5f;
            }

            if (l > 0)
            {
                Vector3 pos = transform.position;
                pos.x += l * ((Pos.x > transform.position.x) ? -1 : 1);
                transform.position = pos;
            }

            //眩晕
            mFaintT = Time.time + 1;
            mCurActState = eState.Faint;
            GameObject o = GameObject.Instantiate(gDefine.gData.mFaintStarSEPreb);
            se_event s0 = o.GetComponent<se_event>();
            s0.InitLiftT(1);
            se_pcHeadYUpdate s1 = o.GetComponent<se_pcHeadYUpdate>();
            s1.mRefUIHpObj = mHpRoot;
            o.transform.SetParent(mRefPointMid.transform);
            if (mHpRoot.activeSelf)
                o.transform.localPosition = new Vector3(0, 1.3f, 0);
            else
            {
                o.transform.localPosition = new Vector3(0, 1, 0);
            }

            mAnimator.Play("beAtk", 0, 0);

        }
    }

    void Event_IgnorDamage(float T)
    {
        mIsInSkillIgnorDamgeT = Time.time + T;
    }

    void Event_ActInputForbid(float T)
    {
        mActInputForbidT = Time.time + T;
    }

    public void BuffBegin(CBuff.eBuff BuffType)
    {
        if (BuffType == CBuff.eBuff.Paralysis && (mCurActState != eState.Paralysis && mCurActState <= eState.Skill_Readying))
        {
            mCurActState = eState.Paralysis;
            mAnimator.Play("beAtk", 0, 0);
            Debug.Log("!!!=== buff be atk");
            transform.position = new Vector3(transform.position.x, gDefine.gGrounY, transform.position.z);
        }
    }

    public void BuffEnd(CBuff.eBuff BuffType)
    {
        if (BuffType == CBuff.eBuff.Paralysis && mCurActState == eState.Paralysis)
        {
            mNextActState = eState.Idle;
            ChangeToNextActNow();
        }
    }

    public bool CanUseGun()
    {
        if( Mathf.Abs( transform.position.y - gDefine.gGrounY) < 0.5f 
            && mCurActState < eState.Skill_Readying && Time.time > mReliveT)
            return true;
        else
            return false;
    }

    public void Relive()
    {
        mCurActState = eState.Idle;
        mAnimator.Play("DSGirl_Act_Idle");
        Debug.Log("!!!===   17");
        RefreshHPUI(true);
        mBuff.Close();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        if (gDefine.gPlayerData.mHp > 0)
        {
            mNextActState = eState.Idle;
            mNextActFaceRight = mCurFaceRight;
            mCurActState = eState.Idle;
            mAnimator.Play("DSGirl_Act_Idle");
            SetCurAct();
        }
        else
        {
            PlayAct("die");
        }
    }

    public void Event_PlaySound(int SoundId)
    {
        gDefine.PlaySound(SoundId);
    }

    public void ClearSkillSE()
    {
        Transform[] tarr = new Transform[mRefPointMid.transform.childCount];
        for (int i = 0; i < mRefPointMid.transform.childCount; i++)
            tarr[i] = mRefPointMid.transform.GetChild(i);
        for (int i = 0; i < tarr.Length; i++)
        {
            tarr[i].transform.SetParent(null);
            GameObject.Destroy(tarr[i].gameObject);
        }
    }

    public void AddBuff(CBuff.eBuff BuffType, float LastT, int Damage, Transform Trans, CBuff.BuffBegin_CallBack BFunc, CBuff.BuffEnd_CallBack EFunc)
    {
        if (gDefine.gPlayerData.mHp <= 0)
            return;

        if (gDefine.gPlayerData.IsCurIgonrDamage())
            return;

        if (Time.time < mIsInSkillIgnorDamgeT)
            return;

        if (gDefine.gPlayerData.mCurMagicData != null && gDefine.gPlayerData.mCurMagicData.mNum > 0
        && gDefine.gPlayerData.mCurMagicData.mType == CMagic.eMagic.Def)
        {
            gDefine.gPlayerData.mCurMagicData.mNum--;
            return;
        }

        if (Random.Range(0, 100) < gDefine.gPlayerData.mDodgePerc)
        {
            gDefine.gDamageShow.CreateDamageShow("闪避", transform.position + Vector3.up * 3, new Color(0.9f, 0.5f, 0.9f, 1));
            return;
        }

        CGird gird = gDefine.gPlayerData.mEquipGird[(int)gDefine.eEuqipPos.Ring];
        if (gird.mRefItem != null)
        {
            if (gird.mRefItem.mSpecialIndex == 1 && BuffType == CBuff.eBuff.Posion)
                return;
            else if (gird.mRefItem.mSpecialIndex == 2 && BuffType == CBuff.eBuff.Paralysis)
                return;
            else if (gird.mRefItem.mSpecialIndex == 3 && BuffType == CBuff.eBuff.Curse)
                return;
        }

        mBuff.AddBuff(BuffType, LastT, Damage, Trans, BFunc, EFunc);
    }

    public void BeAtk(int Damage, CNpcInst Npc)
    {
        if(gDefine.gLogic.mTeach.mIsInTeach && gDefine.gPlayerData.mHp<50)
            return;

        if (gDefine.gPlayerData.mHp <= 0)
            return;

        if (gDefine.gPlayerData.IsCurIgonrDamage())
            return;

        if (Time.time < mIsInSkillIgnorDamgeT)
            return;

        if (gDefine.gPlayerData.mCurMagicData != null && gDefine.gPlayerData.mCurMagicData.mNum > 0
        && gDefine.gPlayerData.mCurMagicData.mType == CMagic.eMagic.Def)
        {
            gDefine.gPlayerData.mCurMagicData.mNum--;
            return;
        }

        if (Random.Range(0, 100) < gDefine.gPlayerData.mDodgePerc)
        {
            gDefine.gDamageShow.CreateDamageShow("闪避", transform.position + Vector3.up * 3, new Color(0.9f, 0.5f, 0.9f, 1));
            return;
        }

        //披风特殊提供
        CGird gird = gDefine.gPlayerData.mEquipGird[(int)gDefine.eEuqipPos.Cloak];
        // if(gird.mRefItem!=null && gird.mRefItem.mSpecialIndex == 1)
        // {
        //     Damage = (int)(Damage * 0.95f);
        //     if(Damage<1) Damage = 1;
        // }
        if (gird.mRefItem != null && (float)gDefine.gPlayerData.mHp / (float)gDefine.gPlayerData.mHpMax < 0.3f
        && gird.mRefItem.mSpecialIndex == 2)
        {
            if (Random.Range(0, 100) < 10)
                return;
        }

        //衣服特殊提供
        gird = gDefine.gPlayerData.mEquipGird[(int)gDefine.eEuqipPos.Clothe];
        // if(gird.mRefItem!=null && gird.mRefItem.mSpecialIndex == 1)
        // {
        //     Damage = (int)(Damage * 0.95f);
        //     if(Damage<1) Damage = 1;
        // }
        if (gird.mRefItem != null && gird.mRefItem.mSpecialIndex == 3 && Npc != null)
        {
            if (Random.Range(0, 100) < 30)
                Npc.AddBuff(CBuff.eBuff.Paralysis, 3);
        }

        Damage = (int)(Damage * (1 - gDefine.gPlayerData.mDamageReduce));

        if (mCurActState == eState.Skill_FlashKill)
            return;

        if (Damage <= 0)
            return;

        gDefine.gPlayerData.mHp -= Damage;

        gDefine.gDamageShow.CreateDamageShow(Damage, transform.position + Vector3.up * 3, Color.red, false);

        gDefine.gGameMainUI.mBeAtkUI.Show();

        if (gDefine.gPlayerData.mHp <= 0)
        {

            //检查元气护体先
            CSkill skill = gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.ForceAwaken);
            if (skill != null && skill.mParamInt == 0)
            {
                skill.mParamInt = 1;

                GameObject o = GameObject.Instantiate(gDefine.gData.mForceAwakeSEPreb);
                se_Force_Awaken script = o.GetComponent<se_Force_Awaken>();
                script.Init(mRefPointMid.transform);

                CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.ForceAwaken);
                if (d != null)
                    gDefine.gPlayerData.mHp = (int)(gDefine.gPlayerData.mHpMax * (0.5f + 0.1f * d.mLearnNum));
                else
                    gDefine.gPlayerData.mHp = gDefine.gPlayerData.mHpMax / 2;

                gDefine.gDamageShow.CreateDamageShow(skill.mName, transform.position + Vector3.up * 3, new Color(0.9f, 0.5f, 0.9f, 1));
                
                mAnimator.Play("relive", 0, 0);

                mCurActState = eState.Idle;

                mReliveT = Time.time + 2f;
                
                gDefine.PlaySound(54);
            }
            else
            {

                Debug.Log("!!!===   pc anim die");

                gDefine.gPlayerData.mHp = 0;

                mAnimator.Play("die", 0, 0);

                //gDefine.gGameMainUI.ShowFailUI();
                gDefine.gPause = true;

                Event_PlaySound(27);

                mCurActState = eState.Die;
            }
        }
        else if (mCurActState == eState.Idle)
        {
            mAnimator.Play("beAtk", 0, 0);
            mCurActState = eState.BeAtk;

            Debug.Log("!!!===   pc be atk");
        }
        RefreshHPUI(true);
        ShowHPUI();
    }

    void Event_DieEnd()
    {
        if( !gDefine.gLogic.mOpenStory.mIsOpen)
        {
            Debug.Log("!!!!=====   pc show  die ui");
            gDefine.gGameMainUI.ShowFailUI();
        }
    }

    void Event_BeAtkEnd()
    {
        if (mCurActState != eState.Paralysis)
        {
            if (gDefine.gNpc.HasSomeoneBeTake())
            {
                int kkk = 1;
            }
            //mNextActState = eState.Idle;
            //ChangeToNextActNow();
            mCurActState = eState.Idle;
            SetCurAct();
        }
    }

    public void ClearTarget()
    {
        mTargetObj = null;
    }

    void Event_ActEnd(int Index)
    {

        gDefine.gIsInEndKill = false;
        if (Index >= 1000 && gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.DSWordGirl_FlashKill) != null
                && Random.Range(0, 100) <= 30 && Time.time > mQuickKillCoolDownT)
        {
            CNpcInst npc = gDefine.gNpc.FindByR(transform.position.x, 5, CNpcInst.eNpcClass.Air0);
            if (npc != null)
            {
                mCurActState = eState.Skill_FlashKill;
                //mQuickKill.Init(npc, this);
                //mQuickKill.InitFlash(npc, this);
                mQuickKill.InitHide(npc, this);
                mQuickKillCoolDownT = Time.time + 4;
                return;
            }
        }

        {
        //     //如果有空中怪，那么任何动作都
        //     bool calcUseGun = false;
        //     float useGunPerc = gDefine.gPlayerData.mChangeToGunPerc;
        //     CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.GunMaster);
        //     if (d != null)
        //         useGunPerc += d.mLearnNum * 5;
        //     CNpcInst npc = gDefine.gNpc.FindByR(transform.position.x, 7, CNpcInst.eNpcClass.All);
        //     if (npc != null)
        //     {
        //         // calcUseGun = false;
        //         if (gDefine.gNpc.ThereIsOnlyAirNpc())
        //         {
        //             // calcUseGun = true;
        //             useGunPerc += 100;
        //         }
        //         // else  
        //         if (Time.time > mGunCoolDown && Index == 1003)
        //         {
        //             calcUseGun = true;
        //             //useGunPerc = 100;
        //         }
        //         //else if (Index == 1003)
        //         // if(Random.Range(0,100)<40)
        //         // {
        //         //      calcUseGun = true;
        //         //     useGunPerc = 100;
        //         // }

        //         if (calcUseGun)
        //         {
        //             if (Random.Range(0, 100) <= useGunPerc)
        //             {
        //                 mNextActState = eState.Idle;
        //                 gDefine.UseShootGirlNow(transform.position, mCurFaceRight, 2f);
        //                 gameObject.SetActive(false);

        //                 mNextActState = eState.Idle;

        //                 mGunCoolDown = Time.time + 4;
        //                 return;
        //             }
        //         }
        //     }



            ChangeToNextActNow();
        }
    }

    public void UseGunGirl()
    {
        mNextActState = eState.Idle;
        gDefine.UseShootGirlNow(transform.position, mCurFaceRight, 2f);
        gameObject.SetActive(false);
        mNextActState = eState.Idle;
    }

    public void Event_BeginSwordKill()
    {

        if (gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.DSWordKill) != null)
        {
            int perc = 40;
            CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.DSWordKill);
            if (d != null)
                perc += d.mLearnNum * 5;
            if (Random.Range(0, 100) < perc)
            {
                //剑气斩
                GameObject o = GameObject.Instantiate(gDefine.gData.mDSwordQuickKillDownFlyFlashPreb);
                se_DSwordFlash script = o.GetComponent<se_DSwordFlash>();
                Vector3 bpos = transform.position;
                bpos.y = gDefine.gGrounY;
                Vector3 epos = bpos + (mCurFaceRight ? Vector3.right * 20 : Vector3.left * 20);

                script.Init(bpos, epos, gDefine.gPlayerData.mDamage);
            }
        }


    }

    void Event_Atk()
    {
        if (gDefine.gNpc.DoDamage(transform.position,
                     mAtkL, mCurFaceRight, (int)gDefine.gPlayerData.mDamage, false, false, CNpcInst.eNpcClass.OnGround, false))
        {
            GameObject mingZhongSE = GameObject.Instantiate(mRefMingzhongSe);
            mingZhongSE.transform.position = transform.position +
               ((mCurFaceRight) ? new Vector3(2.2f, 1.5f, -0.5f) : new Vector3(-2.2f, 1.5f, -0.5f));

            int[] hitSound = new int[] { 19, 22 };
            int index = Random.Range(0, hitSound.Length);

            AudioClip clip = gDefine.gData.GetSoundClip(hitSound[index]);
            gDefine.gSound.Play(clip);

            int bloodSuck = gDefine.gPlayerData.GetBloodSuckHp();
            if (bloodSuck > 0 && !mBuff.HasBuff(CBuff.eBuff.Curse))
            {
                gDefine.gPlayerData.mHp += bloodSuck;
                if (gDefine.gPlayerData.mHp > gDefine.gPlayerData.mHpMax)
                    gDefine.gPlayerData.mHp = gDefine.gPlayerData.mHpMax;

                gDefine.gDamageShow.CreateDamageShow(bloodSuck, transform.position + Vector3.up * 3, Color.green, false);
                RefreshHPUI(true);
                ShowHPUI();
            }
        }
        RefreshHPUI(true);

        //闪电链

        if (gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.LightChain) != null && Random.Range(0, 100) < 15)
        {
            // CNpcInst npc = gDefine.gNpc.FindWithR(transform.position.x, 6, CNpcInst.eNpcClass.OnGround);
            // if (npc != null)
            // {
            //     GameObject obj = GameObject.Instantiate(gDefine.gData.mLightChainSEPreb);
            //     se_Skill_LightChain script = obj.GetComponent<se_Skill_LightChain>();
            //     int nextNum = Random.Range(0, 5);
            //     script.Init(mRefPointMid.transform, npc, nextNum, (int)(gDefine.gPlayerData.mDamage * 1.3f));
            // }
            // else
            {
                GameObject obj = GameObject.Instantiate(gDefine.gData.mLightChainNullPreb);
                se_lightChainNull script = obj.GetComponent<se_lightChainNull>();
                script.Init(mRefPointMid.transform, mCurFaceRight ? Vector3.right * 8 : Vector3.left * 8,
                (int)(gDefine.gPlayerData.mDamage * 1.3f), 8);
            }


        }
    }

    void Event_HAtk()
    {
        bool kickback = mCurActState == eState.Atk3 ? true : false;
        if (gDefine.gNpc.DoDamage(transform.position,
                     mHAtkL, mCurFaceRight, (int)gDefine.gPlayerData.mDamage * 2, true, kickback, CNpcInst.eNpcClass.OnGround,
                     false))
        {
            GameObject mingZhongSE = GameObject.Instantiate(mRefMingzhongSe);
            mingZhongSE.transform.position = transform.position +
               ((mCurFaceRight) ? new Vector3(2.2f, 1.5f, -0.5f) : new Vector3(-2.2f, 1.5f, -0.5f));

            int bloodSuck = gDefine.gPlayerData.GetBloodSuckHp();
            if (bloodSuck > 0 && !mBuff.HasBuff(CBuff.eBuff.Curse))
            {
                gDefine.gPlayerData.mHp += bloodSuck;
                if (gDefine.gPlayerData.mHp > gDefine.gPlayerData.mHpMax)
                    gDefine.gPlayerData.mHp = gDefine.gPlayerData.mHpMax;

                gDefine.gDamageShow.CreateDamageShow(bloodSuck, transform.position + Vector3.up * 3, Color.green, false);
                RefreshHPUI(true);
                ShowHPUI();
            }
        }
        RefreshHPUI(true);

        //闪电链
        if (gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.LightChain) != null && Random.Range(0, 100) < 15)
        {
            // CNpcInst npc = gDefine.gNpc.FindWithR(transform.position.x, 6, CNpcInst.eNpcClass.OnGround);
            // if (npc != null)
            // {
            //     GameObject obj = GameObject.Instantiate(gDefine.gData.mLightChainSEPreb);
            //     se_Skill_LightChain script = obj.GetComponent<se_Skill_LightChain>();
            //     int nextNum = Random.Range(0, 5);
            //     script.Init(mRefPointMid.transform, npc, nextNum, (int)(gDefine.gPlayerData.mDamage * 1.3f));
            // }
            // else
            {
                GameObject obj = GameObject.Instantiate(gDefine.gData.mLightChainNullPreb);
                se_lightChainNull script = obj.GetComponent<se_lightChainNull>();
                script.Init(mRefPointMid.transform, mCurFaceRight ? Vector3.right * 8 : Vector3.left * 8,
                (int)(gDefine.gPlayerData.mDamage * 1.3f), 8);
            }


        }
    }

    /// <summary>
    ///终结技，开始突击
    /// </summary>
    public void Event_EndKillBeginFlush()
    {
        if (mEndKillTarget != null)
        {
            mMoveTarget = (mCurFaceRight == true) ?
                        mEndKillTarget.GetPos() + Vector3.right * 10
            :
                        mEndKillTarget.GetPos() + Vector3.left * 10;

            mCurActState = eState.EndKillFly;

            if (mAnimator == null)
                mAnimator = GetComponent<Animator>();

            mAnimator.Play("endKillFly");

            mIsEndKillTakeSoundPlay = false;
            mIsEndKillKickDownSoundPlay = false;
        }


    }

    public void RefreshHPUI(bool Show)
    {
        mUIHp.Refresh(gDefine.gPlayerData.mHp / (float)gDefine.gPlayerData.mHpMax, Show);
        //mHpText.text = gDefine.gPlayerData.mHp.ToString();
        //mHpImage.transform.localScale = new Vector3(gDefine.gPlayerData.mHp / (float)gDefine.gPlayerData.mHpMax, 1, 1);
    }

    public void ShowHPUI()
    {
        //mHpRoot.SetActive(true);
        // mHpRootT = Time.time + 1;
    }

    public void Btn_Down(bool IsRight)
    {
        if(Time.time < mReliveT )
            return;

        if (gDefine.IsUseGunGirl())
            return;

        if (Time.time < mActInputForbidT)
            return;

        if (gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.DSWordGirl_Flush) == null)
            return;

        if (mCurActState == eState.Paralysis || mCurActState == eState.Die)
            return;

        // if (mCurActState == eState.Skill_Readying ||
        //     mCurActState == eState.Skill_Ready)
        //     return;

        if (mCurActState >= eState.Skill_Readying)
            return;

        if (gDefine.gNpc.HasSomeoneBeTake())
        {
            int kkk = 1;
        }

        mNextActState = eState.Idle;
        mNextActFaceRight = IsRight;

        if (mCurActState == eState.Idle || mCurActState == eState.JumpBack)
        {
            mCurActState = eState.Skill_Readying;
            mCurFaceRight = IsRight;
            mMoveTarget = transform.position;
            SetCurAct();
        }
        else
        {
            mNextActFaceRight = IsRight;
            mNextActState = eState.Skill_Readying;
        }
    }

    public void Event_Idle()
    {
        if (gDefine.gNpc.HasSomeoneBeTake())
        {
            int kkk = 1;
        }
        mCurActState = eState.Idle;
    }

    public void Btn_Click(bool IsRight)
    {
        if( Time.time< mReliveT )
            return;

        if (gDefine.IsUseGunGirl())
            return;

        if (mCurActState >= eState.Skill_Readying)
            return;

        if (mCurActState == eState.Die || mCurActState == eState.Paralysis)
            return;

        if (Time.time < mActInputForbidT)
            return;

        // if (mIsInKilledPlay)
        // return;

        if (IsRight == mCurFaceRight)
        {
            //按钮与当前方向同，则继续攻击

            //查看攻击范围内是否有目标
            //1
            //如果有目标，判断目标的远近。
            //--如果远，则跃飞过去攻击
            //-------
            //--如果近，则判断是否在可攻击范围内
            //------不在攻击范围内，于是1，2段攻击前行
            //------在范围内，则1，2，3，4交替攻击
            //2
            //如果目标不存在，则1，2攻击前进

            CheckTarget();
            if (mTargetObj != null)
            {
                float l = CalcTargetL();

                if (l > mFlyMinL && l <= mFlyMaxL)
                {
                    mNextActState = eState.Fly;
                    if (mCurActState == eState.Idle || mCurActState == eState.JumpBack)
                    {
                        ChangeToNextActNow();
                    }
                }
                //else if (l > gDefine.gPlayerData.GetPcAtkL() && l <= gDefine.gPlayerData.GetPcMoveAtkL())
                //{

                //    mNextActState = eState.Atk0;


                //    if (mCurActState == eState.Idle || mCurActState == eState.JumpBack)
                //    {
                //        ChangeToNextActNow();
                //    }
                //}
                else
                {
                    mNextActState = eState.Atk0;

                    if (mCurActState == eState.Idle || mCurActState == eState.JumpBack)
                    {

                        ChangeToNextActNow();
                    }
                }
            }
            else
            {
                mNextActState = eState.Atk0;

                if (mCurActState == eState.Idle || mCurActState == eState.JumpBack)
                {
                    ChangeToNextActNow();
                }
            }
        }
        else
        {
            //按钮与当前方向反，则攻击或后退
            //背后有人么？
            //---有，那么距离远么
            //------远，飞跃过去攻击
            //------近，开启atk0
            //---如果背后没有人，那么我当前方向上有目标么
            //-------有，向后跳跃
            //-------没有，调转方向，atk0，攻击前行

            CNpcInst inst = CheckBackTarget();

            if (inst != null)
            {
                mTargetObj = inst;
                float l = CalcTargetL();
                float backFlashPrec = 20;
                CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.BackFlash);
                if (d != null)
                    backFlashPrec += 10 * d.mLearnNum;
                if (gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.BackFlash) != null && l < 7.5f
                && Random.Range(0, 100) < backFlashPrec)
                {
                    BeginSkillBackFlash();
                    return;
                }
                else if (l > mFlyMinL)
                {
                    //CheckBackTarget
                    mNextActState = eState.Fly;
                    mNextActFaceRight = IsRight;
                }
                else
                {
                    mNextActState = eState.Atk0;
                    mNextActFaceRight = IsRight;
                }

                ChangeToNextActNow();
            }
            else if (mTargetObj != null && Mathf.Abs(transform.position.x - mTargetObj.GetPos().x) < 7)
            {
                mNextActState = eState.JumpBack;
                mNextActFaceRight = mCurFaceRight;

                if (mCurActState != eState.JumpBack)
                {
                    ChangeToNextActNow();
                }
            }
            else
            {
                mNextActState = eState.Atk0;
                mNextActFaceRight = IsRight;
                ChangeToNextActNow();
            }
        }
    }


    /// <summary>
    /// 查找面对的敌人
    /// </summary>
    /// <returns></returns>
    CNpcInst CheckTarget()
    {
        if (mTargetObj != null && mTargetObj.IsLive() && mTargetObj.mCanBeSelect)
            return mTargetObj;
        else
        {

            CNpcInst obj = gDefine.gNpc.FindByL(transform.position.x, mCurFaceRight, mFlyMaxL);
            mTargetObj = obj;
            return mTargetObj;
        }

    }

    float CalcTargetL()
    {
        if (mTargetObj != null && !mTargetObj.IsLive())
            mTargetObj = null;

        if (mTargetObj == null)
            return 10000000;
        else
        {
            // if ((mCurActState >= eState.Atk0 && mCurActState <= eState.Atk3) || mCurActState == eState.Fly)
            //     return Mathf.Abs(mMoveTarget.x - mTargetObj.GetPos().x);
            // else
            return Mathf.Abs(transform.position.x - mTargetObj.GetPos().x);
        }
    }

    bool IsFaceTarget()
    {
        if (mTargetObj == null)
            return false;
        else
            if (mCurFaceRight && mTargetObj.GetPos().x > transform.position.x)
            return true;
        else if (!mCurFaceRight && mTargetObj.GetPos().x < transform.position.x)
            return true;

        return false;
    }

    bool ChangeToNextActNow()
    {
        if (gDefine.gPlayerData.mHp <= 0)
            return false;

        if (mCurActState == eState.Faint)
            return false;

        //如果有攻击对象
        ///--如果是攻击动作，则确认距离
        ///--如果是飞攻动作，也确认距离
        ///-------如果位置足够近，则切换向0号攻击动作

        ///如果没有对象
        ///--如果是攻击动作，则确认距离
        ///--如果是飞攻动作，替换为攻击动作，确认距离

        ///--如果是后退，要确认距离

        eState oldAct = mCurActState; //

        mCurActState = mNextActState;
        mNextActState = (mCurActState == eState.Fly) ? eState.Atk0 : eState.Idle;
        mCurFaceRight = mNextActFaceRight; //设置朝向

        CheckTarget();

        if (mCurActState == eState.Skill_Readying)
        {
            mMoveTarget = transform.position;
        }
        else if (mCurActState == eState.JumpBack)
        {
            CNpcInst npc = gDefine.gNpc.FindByL(transform.position.x, !mCurFaceRight, 7);
            float backFlashPrec = 20;
            CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.BackFlash);
            if (d != null)
                backFlashPrec += 10 * d.mLearnNum;
            if (npc != null && Random.Range(0, 100) < backFlashPrec)
            {
                BeginSkillBackFlash();
                return true;
            }
            else
            {
                mMoveTarget = (mCurFaceRight == true) ?
                   transform.position + mJumpL * Vector3.left
               :
                   transform.position + mJumpL * Vector3.right;
            }
        }
        else
        {

            // if (mCurActState == eState.Atk0 && gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.DSWordGirl_FlashKill) != null
            //     && Random.Range(0, 100) <= 30 && gDefine.gNpc.FindAirByL(transform.position.x, mCurFaceRight, 4) != null)
            // {
            //     mCurActState = eState.Skill_FlashKillHide;
            // }
            // else
            if (mTargetObj != null)
            {
                float l = CalcTargetL();

                if (mTargetObj.mNpcType == npcdata.eNpcType.HalfUp)
                {
                    if (l < 1.5f)
                    {
                        //直接播放动作
                        mCurActState = eState.Atk0;//借一个动作明目
                        if (mAnimator == null)
                            mAnimator = GetComponent<Animator>();
                        mAnimator.Play("endKill0", 0, 0);
                        mTargetObj.BeEndKill0();
                        return true;


                    }
                    //     else
                    //     {
                    //         //飞过去
                    //         mMoveTarget = (mCurFaceRight == true) ?
                    //                mTargetObj.GetPos() + Vector3.left
                    //    :
                    //                mTargetObj.GetPos() + Vector3.right;

                    //         mCurActState = eState.Fly;

                    //         if (mAnimator == null)
                    //             mAnimator = GetComponent<Animator>();

                    //         mAnimator.Play("DSGirl_Act_Fly");
                    //         mTargetObj.BeTaken();

                    //     }

                }
                else if (mTargetObj.CanBeEndKill() && !gDefine.gLogic.mTeach.mIsInTeach/* && Random.Range(0, 100) < 20*/
                 && gDefine.gNpc.GetLiveNpcNumByType(npcdata.eNpcType.BareHand) <= 3 &&
                 l < 2.5f && IsFaceTarget() && Time.time > gDefine.gPlayerData.mEndKillCoolDownT)
                {
                    gDefine.gPlayerData.mEndKillCoolDownT = Time.time + 15;
                    // mMoveTarget = (mCurFaceRight == true) ?
                    //             mTargetObj.GetPos() + Vector3.right * 5.0f
                    // :
                    //             mTargetObj.GetPos() + Vector3.left * 5.0f;

                    // mCurActState = eState.EndKillFly;

                    // if (mAnimator == null)
                    //     mAnimator = GetComponent<Animator>();

                    // mAnimator.Play("endKillFly");

                    // mIsEndKillTakeSoundPlay = false;
                    // mIsEndKillKickDownSoundPlay = false;

                    if (mAnimator == null)
                        mAnimator = GetComponent<Animator>();

                    mAnimator.Play("endKillTake");

                    mCurActState = eState.EndKillTake;

                    mTargetObj.BeTaken();

                    mEndKillTarget = mTargetObj;

                    transform.rotation = new Quaternion();
                    mHpRoot.transform.rotation = new Quaternion();

                    if (!mCurFaceRight)
                    {
                        transform.Rotate(0, 180, 0, Space.World);
                        mHpRoot.transform.Rotate(0, 180, 0, Space.Self);
                    }

                    return true;
                }

                //再确认下位置,位置不够的情况下还是要改成攻击前进，或者是攻击
                if (mCurActState == eState.Fly && l <= mAtk0MoveL)
                    mCurActState = eState.Atk0;

                if (mCurActState == eState.Fly)
                {
                    mMoveTarget = ((mCurFaceRight == true) ?
                                mTargetObj.GetPos() + mBestToNpcL * Vector3.left
                    :
                                mTargetObj.GetPos() + mBestToNpcL * Vector3.right);
                }

                if (mCurActState == eState.Atk0)
                {
                    //此时重算一个攻击动作
                    mCurActState = GetNextAtkAct(oldAct);


                    if (l < mAtkL)
                    {
                        if (mCurActState == eState.Atk3)
                        {
                            mMoveTarget = (mCurFaceRight == true) ?
                           transform.position + 1.5f * Vector3.right
                       :
                           transform.position + 1.5f * Vector3.left;

                            mAtk3MoveV = 2.2f;
                        }
                        else
                            mMoveTarget = transform.position;
                    }
                    else if (l < GetAtkMoveL() + mBestToNpcL)
                    {
                        mMoveTarget = (mCurFaceRight == true) ?
                            mTargetObj.GetPos() + mBestToNpcL * Vector3.left
                        :
                            mTargetObj.GetPos() + mBestToNpcL * Vector3.right;

                        mAtk3MoveV = Mathf.Abs(mMoveTarget.x - transform.position.x) * 1.1f;
                    }
                    else
                    {
                        //攻击前进
                        //若没有目标，则为当前默认的移动
                        if (mCurActState == eState.Atk3)
                            mAtk3MoveV = GetAtkMoveL() * 1.4f;

                        mMoveTarget = (mCurFaceRight == true) ?
                            transform.position + GetAtkMoveL() * Vector3.right
                        :
                            transform.position + GetAtkMoveL() * Vector3.left;
                    }
                }


            }
            else
            {
                if (mCurActState == eState.Fly)
                    mCurActState = eState.Atk0;

                if (mCurActState == eState.Atk0)
                {

                    mCurActState = GetNextAtkAct(oldAct);

                    if (mCurActState == eState.Atk3)
                        mAtk3MoveV = GetAtkMoveL() * 1.4f;

                    mMoveTarget = (mCurFaceRight == true) ?
                       transform.position + GetAtkMoveL() * Vector3.right
                    :
                        transform.position + GetAtkMoveL() * Vector3.left;

                }
            }

        }

        if (mCurActState == eState.Idle)
            mAtkActCoolDown = 0.3f;
        else
            mAtkActCoolDown = 10000;

        SetCurAct();

        return true;
    }

    void SetCurAct()
    {
        if (gDefine.gPlayerData.mHp <= 0)
            return;

        if (mAnimator == null)
            mAnimator = GetComponent<Animator>();

        switch (mCurActState)
        {
            case eState.Idle:
                if (gDefine.gNpc.HasSomeoneBeTake())
                {
                    int kkk = 1;
                }
                mAnimator.Play("DSGirl_Act_Idle");
                // Debug.Log("!!!===   1");
                break;
            case eState.Atk0:
                mAnimator.Play("DSGirl_Act_Atk0");
                // Debug.Log("!!!===   2");
                break;
            case eState.Atk1:
                mAnimator.Play("DSGirl_Act_Atk1");
                //Debug.Log("!!!===   3");
                break;
            case eState.Atk2:
                mAnimator.Play("DSGirl_Act_Atk2");
                // Debug.Log("!!!===   4");
                break;
            case eState.Atk3:
                mAnimator.Play("DSGirl_Act_Atk3");
                //  Debug.Log("!!!===   5");
                break;
            case eState.Fly:
                mAnimator.Play("DSGirl_Act_Fly");
                //  Debug.Log("!!!===   6");
                break;
            case eState.JumpBack:
                mAnimator.Play("DSGirl_Act_Jumpback", 0, 0);
                // Debug.Log("!!!===   7");
                break;
            case eState.Skill_Readying:
                mAnimator.Play("DSGirl_Act_SkillReadying");
                //Debug.Log("!!!===   8");
                break;
            case eState.Skill_FlashKillHide:
                mAnimator.Play("hide");
                // Debug.Log("!!!===   9");
                break;
        }

        transform.rotation = new Quaternion();
        mHpRoot.transform.rotation = new Quaternion();

        if (!mCurFaceRight)
        {
            transform.Rotate(0, 180, 0, Space.World);
            mHpRoot.transform.Rotate(0, 180, 0, Space.Self);
        }

    }

    public void ResetDir()
    {
        if (mCurActState != eState.JumpBack)
        {
            transform.rotation = new Quaternion();
            mHpRoot.transform.rotation = new Quaternion();

            if (!mCurFaceRight)
            {
                transform.Rotate(0, 180, 0, Space.World);
                mHpRoot.transform.Rotate(0, 180, 0, Space.Self);
            }

        }
    }

    float GetAtkMoveL()
    {
        switch (mCurActState)
        {
            case eState.Idle:
                return 0;
            case eState.Atk0:
                return mAtk0MoveL;

            case eState.Atk1:
                return mAtk1MoveL;

            case eState.Atk2:
                return mAtk2MoveL;

            case eState.Atk3:
                return mAtk3MoveL;


        }
        return 0;

    }

    float GetMoveV()
    {
        switch (mCurActState)
        {
            case eState.Atk0:
                return mAtk0MoveV;

            case eState.Atk1:
                return mAtk1MoveV;

            case eState.Atk2:
                return mAtk2MoveV;

            case eState.Atk3:
                return mAtk3MoveV;

            case eState.Fly:
                return mFlyV;

            case eState.JumpBack:
                return mJumpV;

            case eState.Skill_Flush:
            case eState.Skill_backFlash:
                return mSkillFlushV;
            case eState.EndKillFly:
                return mFlyV * 0.8f;


        }
        return 0;
    }

    /// <summary>
    /// 查身后的敌人
    /// </summary>
    /// <returns></returns>
    CNpcInst CheckBackTarget()
    {
        float l = mFlyMaxL;
        CNpcInst obj = gDefine.gNpc.FindByL(gameObject.transform.position.x, !mCurFaceRight, l);
        return obj;
    }


    /// <summary>
    /// 获取下一次动作
    /// </summary>
    /// <param name="OldAct"></param>
    /// <returns></returns>
    eState GetNextAtkAct(eState OldAct)
    {


        if (mAtkActCoolDown <= 0)
            return eState.Atk0;
        else
        {
            switch (OldAct)
            {
                case eState.Atk0:
                    return eState.Atk1;
                case eState.Atk1:
                    return eState.Atk2;
                case eState.Atk2:
                    return eState.Atk3;
                case eState.Atk3:
                    return eState.Atk0;
                default:
                    return eState.Atk0;
            }
        }

        //return OldAct;
    }

    void Update()
    {
        //if (mIsInKilledPlay)
        //   return;
        //AnimatorStateInfo info = mAnimator.GetCurrentAnimatorStateInfo(0);
        // mAnimator.getcur
        // info.

        //if (Time.time > mHpRootT)
        // mHpRoot.SetActive(false);

        if( Time.time < mReliveT)
            return;

        if (!gDefine.gPause && mCurActState != eState.Die)
        {

            if (mCurActState == eState.Skill_Ready)
            {
                if (!Input.GetMouseButton(0))
                {
                    Btn_Up(mCurFaceRight);
                    return;
                }
            }

            if (gDefine.gNpc.HasSomeoneBeTake() && mCurActState == eState.Idle)
            {
                {
                    mCurActState = eState.Atk0;//借一个动作明目
                    if (mAnimator == null)
                        mAnimator = GetComponent<Animator>();
                    mAnimator.Play("endKillAtk", 0);
                    mEndKillTarget.BeEndKill1();
                    gDefine.gIsInEndKill = true;
                }
                return;
            }

            UpdateLogic();
            UpdateMove(Time.deltaTime);
        }

    }


    void UpdateLogic()
    {
        mAtkActCoolDown -= Time.deltaTime;

        if (mCurActState == eState.Faint)
        {
            if (Time.time < mFaintT)
                return;
            else
            {
                if (gDefine.gNpc.HasSomeoneBeTake())
                {
                    int kkk = 1;
                }
                mCurActState = eState.Idle;
                mNextActState = eState.Idle;
                ChangeToNextActNow();
            }
        }

        if (mTargetObj != null && !mTargetObj.IsLive())
        {
            mTargetObj = null;
        }

        if (mCurActState != eState.JumpBack && mTargetObj != null)
        {
            if (mTargetObj.GetPos().x > transform.position.x && !mCurFaceRight)
                mTargetObj = null;
            else if (mTargetObj.GetPos().x < transform.position.x && mCurFaceRight)
                mTargetObj = null;
        }

        mBuff.Update();
        mQuickKill.Update();

    }


    void UpdateMove(float T)
    {
        //如果当前没到位，则移动
        if ((mCurActState >= eState.Atk0 && mCurActState <= eState.JumpBack)
            || mCurActState == eState.Fly || mCurActState == eState.Skill_Flush
            || mCurActState == eState.Skill_backFlash || mCurActState == eState.EndKillFly
           )
        {

            float moveV = GetMoveV();

            // if( mTargetObj!=null && mTargetObj.mNpcType == npcdata.eNpcType.BareHand
            // && mCurActState != eState.EndKillFly)
            // {
            //     mMoveTarget =  mTargetObj.GetPos() + 
            //     ((transform.position.x < mTargetObj.GetPos().x)? Vector3.left * 1 : Vector3.right *1 );
            // }

            mMoveTarget.y = gDefine.gGrounY;

            //gameObject.transform.pasrent.transform.position =
            Vector3 newPos = Vector3.MoveTowards(transform.position, mMoveTarget, moveV * Time.deltaTime);
            //Vector3 deltPos = newPos - transform.position;
            //transform.Translate(deltPos, Space.World);
            transform.position = newPos;

            if (mCurActState == eState.EndKillFly && mEndKillTarget != null && mEndKillTarget.IsLive())
            {
                //if (Mathf.Abs(targetPos.x - transform.position.x) < 1)
                if (mEndKillTarget != null)
                {
                    Vector3 targetPos = mEndKillTarget.GetPos();
                    if (mCurFaceRight)
                    {
                        //带飞
                        targetPos.x = transform.position.x + 1.5f;
                    }
                    else
                    {
                        targetPos.x = transform.position.x - 1.5f;
                    }


                    mEndKillTarget.mNpc.SetPos(targetPos);

                }

                int num = gDefine.gNpc.KickDownAll(transform.position, mCurFaceRight, mTargetObj);
                //&& !mIsEndKillKickDownSoundPlay)
                {
                    for (int i = 0; i < num; i++)
                        gDefine.PlaySound(65, i * 0.1f);
                    //mIsEndKillKickDownSoundPlay = true;
                }


            }


            if (mCurActState != eState.JumpBack)
            {
                transform.rotation = new Quaternion();
                mHpRoot.transform.rotation = new Quaternion();

                if (!mCurFaceRight)
                {
                    transform.Rotate(0, 180, 0, Space.World);
                    mHpRoot.transform.Rotate(0, 180, 0, Space.Self);
                }
            }

            if ((mCurActState == eState.Fly || mCurActState == eState.EndKillFly) &&
              Vector3.Distance(newPos, mMoveTarget) < 0.01f)
            {
                if (mCurActState == eState.EndKillFly && mEndKillTarget.IsLive())
                {
                    // if (mEndKillTarget != null)
                    {
                        mCurActState = eState.Atk0;//借一个动作明目
                        if (mAnimator == null)
                            mAnimator = GetComponent<Animator>();
                        mAnimator.Play("endKillAtk", 0);
                        mEndKillTarget.BeEndKill1();
                        gDefine.gIsInEndKill = true;
                    }
                }
                else
                if (mTargetObj != null && mTargetObj.mNpcType == npcdata.eNpcType.HalfUp
                    && Mathf.Abs(transform.position.x - mTargetObj.GetPos().x) <= 1.5f)


                {
                    mCurActState = eState.Atk0;//借一个动作明目
                    if (mAnimator == null)
                        mAnimator = GetComponent<Animator>();
                    mAnimator.Play("endKill0", 0);
                    mTargetObj.BeEndKill0();
                    gDefine.gIsInEndKill = true;
                }

                else
                {
                    //     if (mTargetObj.CanBeEndKill() && Random.Range(0, 100) < 20
                    //  && gDefine.gNpc.GetLiveNpcNumByType(npcdata.eNpcType.BareHand) <= 3
                    //  && Mathf.Abs(transform.position.x - mTargetObj.GetPos().x) < 1.5f)
                    //     {

                    //         if (mAnimator == null)
                    //             mAnimator = GetComponent<Animator>();

                    //         mAnimator.Play("endKillTake");

                    //         mCurActState = eState.EndKillTake;

                    //         mTargetObj.BeTaken();

                    //         mEndKillTarget = mTargetObj;

                    //         gDefine.PlaySound(63);


                    //     }
                    //     else
                    {
                        Event_Atk();
                        ChangeToNextActNow();

                    }
                }

            }
            //gDefine.CalcSceMove(deltPos);
        }
        else if (mCurActState == eState.Skill_Drop || mCurActState == eState.Skill_DownKill)
        {
            Vector3 pos = transform.position;
            pos.y = gDefine.gGrounY;

            transform.position = Vector3.MoveTowards(transform.position, pos, 35 * Time.deltaTime);

            if (Mathf.Abs(transform.position.y - gDefine.gGrounY) < 0.01f)
            {
                if (mCurActState == eState.Skill_DownKill)
                {
                    mCurActState = eState.Skill_DownKillHitGround;
                    mAnimator.Play("downKillHit");
                    Debug.Log("!!!===   10");
                }
                else
                {
                    ChangeToNextActNow();
                }
            }
        }
    }

    public bool ThrowItem(CSkill.eSkill SkillType, int Num)
    {
        if (mCurActState == eState.Skill_FlashKillHide ||
        mCurActState == eState.Skill_FlashAppearKill ||
        mCurActState == eState.Skill_Drop ||
        mCurActState == eState.Skill_DownKill ||
        mCurActState == eState.Skill_DownKillHitGround ||
        mCurActState == eState.Skill_FlashKill
        )

            return false;

        for (int i = 0; i < Num; i++)
        {
            GameObject o = GameObject.Instantiate(gDefine.gData.mThrowItemSEPreb);
            se_Pc_ThrowItem script = o.GetComponent<se_Pc_ThrowItem>();
            script.Init(mRefPointMid.transform.position,
                mCurFaceRight ? transform.position + Vector3.right * 2 * (i + 1) : transform.position + Vector3.left * 2 * (i + 1),
                SkillType, mCurFaceRight);
        }

        return true;
    }


    public void EventSkill_Ready()
    {
        mCurActState = eState.Skill_Ready;
    }

    public void Btn_Up(bool IsRight)
    {

        if (gDefine.IsUseGunGirl())
            return;

        if( Time.time < mReliveT)
        {
            return;
        }

        if (mCurActState == eState.Paralysis || mCurActState == eState.Die)
            return;

        if (mCurActState == eState.Skill_Ready || mCurActState == eState.Skill_Readying)
        {
            if (IsRight != mCurFaceRight)
                return;
        }




        if (mCurActState == eState.Skill_Ready)
        {
            mCurActState = eState.Skill_Flush;
            mAnimator.Play("DSGirl_Act_SkillFlush");
            Debug.Log("!!!===   11");

            transform.rotation = new Quaternion();
            mHpRoot.transform.rotation = new Quaternion();

            mMoveTarget = mCurFaceRight ? transform.position + Vector3.right * mSkillFlushL :
                transform.position - Vector3.right * mSkillFlushL;

            if (!mCurFaceRight)
            {
                transform.Rotate(0, 180, 0, Space.World);
                mHpRoot.transform.Rotate(0, 180, 0, Space.Self);
            }

            GameObject o = GameObject.Instantiate(mRefSESkillFlushPreb);
            o.transform.position = transform.position + Vector3.up * 1.5f;
            if (!mCurFaceRight)
            {
                o.transform.rotation = new Quaternion();
                o.transform.Rotate(0, 180, 0, Space.Self);
            }

            mSkillFlushBX = transform.position.x;
            mSkillFlushCount = 0;

            if (Random.Range(0, 100) < 50)
            {

                CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.DSWordGirl_Flus);
                if (d.mLearnNum == 3 && Random.Range(0, 100) < 50)
                    mSkillFlush2 = 3;
                else if (d.mLearnNum == 2 && Random.Range(0, 100) < 60)
                    mSkillFlush2 = 2;
                else if (d.mLearnNum == 2 && Random.Range(0, 100) < 70)
                    mSkillFlush2 = 1;

                gDefine.gDamageShow.CreateDamageShow("连闪", transform.position + Vector3.up * 3, new Color(0.9f, 0.5f, 0.9f, 1));
                Camera.main.transform.SetParent(null);
            }
            else
            {
                gDefine.gDamageShow.CreateDamageShow("一闪", transform.position + Vector3.up * 3, new Color(0.9f, 0.5f, 0.9f, 1));
            }
        }
        else
        {
            //change to next.
            ChangeToNextActNow();
        }

    }
    //后闪
    public void BeginSkillBackFlash()
    {
        mCurFaceRight = !mCurFaceRight;

        mCurActState = eState.Skill_backFlash;
        mAnimator.Play("DSGirl_Act_SkillFlush");
        Debug.Log("!!!===   12");

        transform.rotation = new Quaternion();
        mHpRoot.transform.rotation = new Quaternion();

        mMoveTarget = mCurFaceRight ? transform.position + Vector3.right * mSkillFlushL :
            transform.position - Vector3.right * mSkillFlushL;

        if (!mCurFaceRight)
        {
            transform.Rotate(0, 180, 0, Space.World);
            mHpRoot.transform.Rotate(0, 180, 0, Space.Self);
        }

        GameObject o = GameObject.Instantiate(mRefSESkillFlushPreb);
        o.transform.position = transform.position + Vector3.up * 1.5f;
        if (!mCurFaceRight)
        {
            o.transform.rotation = new Quaternion();
            o.transform.Rotate(0, 180, 0, Space.Self);
        }

        mSkillFlushBX = transform.position.x;

        gDefine.gDamageShow.CreateDamageShow("后闪", transform.position + Vector3.up * 3, new Color(0.9f, 0.5f, 0.9f, 1));

    }

    public void EventSkill_FlashKill()
    {
        gDefine.gDamageShow.CreateDamageShow("瞬斩", transform.position + Vector3.up * 3, new Color(0.9f, 0.5f, 0.9f, 1));
    }



    public void EventSkill_FlushEnd()
    {
        ChangeToNextActNow();
        //gDefine.gDamageShow.CreateDamageShow("一闪", transform.position + Vector3.up * 3, new Color(0.9f, 0.5f, 0.9f, 1));
        // if( mSkillFlush2 <= 0 )
        //  Camera.main.gameObject.GetComponent<camfollow>().ContinueFollow();
    }

    public void EventSkill_FlushAtk()
    {
        CNpcInst[] arr = gDefine.gNpc.DoDamageLineWithDeadPrec(mSkillFlushBX, mMoveTarget.x,
                    (int)(gDefine.gPlayerData.mDamage * 1.2f), 10, CNpcInst.eNpcClass.OnGround, true,
                    mSkillFlushCount == 1 ? CSkill.eSkill.DSWordGirl_FlushSecond : CSkill.eSkill.DSWordGirl_FlashKill);
        for (int i = 0; i < arr.Length; i++)
        {
            // GameObject mingZhongSE = GameObject.Instantiate(mRefMingzhongSe);
            //  mingZhongSE.transform.position = transform.position +
            //  ((mCurFaceRight) ? new Vector3(2.2f, 1.5f, -0.5f) : new Vector3(-2.2f, 1.5f, -0.5f));

            int bloodSuck = gDefine.gPlayerData.GetBloodSuckHp();
            if (bloodSuck > 0 && !mBuff.HasBuff(CBuff.eBuff.Curse))
            {
                gDefine.gPlayerData.mHp += bloodSuck;
                if (gDefine.gPlayerData.mHp > gDefine.gPlayerData.mHpMax)
                    gDefine.gPlayerData.mHp = gDefine.gPlayerData.mHpMax;

                gDefine.gDamageShow.CreateDamageShow(bloodSuck, gameObject.transform.parent.transform.position + Vector3.up * 3, Color.green, false);
                RefreshHPUI(true);
                ShowHPUI();
            }

            if (Random.Range(0, 100) < 10)
            {
                arr[i].BeKilled();
            }
        }
        RefreshHPUI(true);

    }

    public void EventSkill_FlushContinue()
    {
        if (mSkillFlush2 > 0)
        {
            //继续闪
            mCurActState = eState.Skill_Flush;
            mAnimator.Play("DSGirl_Act_SkillFlush", 0, 0);
            Debug.Log("!!!===   13");

            mCurFaceRight = !mCurFaceRight;

            transform.rotation = new Quaternion();
            mHpRoot.transform.rotation = new Quaternion();

            mMoveTarget = mCurFaceRight ? transform.position + Vector3.right * mSkillFlushL :
             transform.position - Vector3.right * mSkillFlushL;


            if (!mCurFaceRight)
            {
                transform.Rotate(0, 180, 0, Space.World);
                mHpRoot.transform.Rotate(0, 180, 0, Space.Self);
            }

            GameObject o = GameObject.Instantiate(mRefSESkillFlushPreb);
            o.transform.position = transform.position + Vector3.up * 1.5f;
            if (!mCurFaceRight)
            {
                o.transform.rotation = new Quaternion();
                o.transform.Rotate(0, 180, 0, Space.Self);
            }

            mSkillFlushBX = transform.position.x;

            mSkillFlush2--;
            mSkillFlushCount++;
        }

    }
    /// <summary>
    /// 技能顺斩隐身
    /// </summary>
    public void Event_Skill_HideFinish()
    {
        mQuickKill.HideFinish();
    }

    public void Event_Skill_AppearFinish()
    {
        mQuickKill.AppearFinish();
    }

    /// <summary>
    /// 技能顺斩结束
    /// </summary>
    public void Event_Skill_FlashKillEnd()
    {
        if (gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.DSWordGirl_FlashKillSecond) != null)
        {
            CNpcInst airNpc = gDefine.gNpc.FindAirByL(transform.position.x, mCurFaceRight, 8);
            if (airNpc != null)
            {
                transform.position = airNpc.GetPos() + (mCurFaceRight ? Vector3.left * 2 : Vector3.right * 2);
                mAnimator.Play("appearKill");
                Debug.Log("!!!===   14");
                return;
            }
        }


        //if (gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.DSWordGirl_DownKill) != null)
        {
            mCurActState = eState.Skill_DownKill;
            mAnimator.Play("downKill");
            Debug.Log("!!!===   15");
        }
        //else
        //{
        //    mCurActState = eState.Skill_Drop;
        //   mAnimator.Play("drop");
        // }
    }
    /// <summary>
    /// 技能瞬斩攻击
    /// </summary>
    public void Event_Skill_FlashKillAtk()
    {
        CNpcInst airNpc = gDefine.gNpc.FindAirByL(transform.position.x, mCurFaceRight, 8);
        if (airNpc != null)
        {
            if (Random.Range(0, 100) < 50)
                airNpc.BeKilled();
            else
            {
                CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.GunMaster);
                if (d != null)
                {
                    float damage = gDefine.gPlayerData.mDamage * (1.2f + 0.05f * d.mLearnNum);
                    airNpc.BeDamage((int)damage, false, false, true, false, CSkill.eSkill.DSWordGirl_FlashKill);
                }
            }
        }
    }
    /// <summary>
    /// 落斩撞击地面
    /// </summary>
    public void Event_Skill_DownKillHitGround()
    {
        gDefine.gNpc.DoDamageLR(transform.position, 2, (int)(gDefine.gPlayerData.mDamage * 1.5f), false,
        CNpcInst.eNpcClass.OnGround, true);
    }

    /// <summary>
    /// 落斩结束··1
    /// </summary>
    public void Event_Skill_DownKillHitGroundEnd()
    {
        ChangeToNextActNow();
    }

    void Event_Skill_DropKillAtk()
    {
        GameObject o = GameObject.Instantiate(gDefine.gData.mDSwordQuickKillDownFlyFlashPreb);
        se_DSwordFlash script = o.GetComponent<se_DSwordFlash>();
        Vector3 bpos = transform.position;
        bpos.y = gDefine.gGrounY;
        Vector3 epos = bpos + (mCurFaceRight ? Vector3.right * 20 : Vector3.left * 20);

        script.Init(bpos, epos, gDefine.gPlayerData.mDamage);
    }

    public void PlayAct(string ActName)
    {
        if (gDefine.gNpc.HasSomeoneBeTake() && ActName == "DSGirl_Act_Idle")
        {
            int kkk = 1;
        }
        mAnimator.Play(ActName, 0, 0);
        Debug.Log("!!!===   16");
    }

    public void ChangeToStateFlashKill()
    {
        mCurActState = eState.Skill_FlashKill;

    }



}

//-------------//
class CQuickKill
{
    Vector3 mNextPos;  //下一个位置
    CNpcInst mNpc; //瞬杀
    char_DSword mSelf; //自身索引
    float mV = 35; //移动速度  

    float mWaitT = 0;

    enum eState
    {
        Null = 0,
        FlyToBPos,//飞到瞬杀开始的起始位置
        WaitToKill,
        KillFlyIN,   //瞬杀飞入（前半段）
        KillFlyOut,   //瞬杀飞出（后半段）
        WaitToNext,
        DorpDown, //落地斩
        DorpKill, //落地斩
        Flash, //一闪
        Hide,
        Appear,

    }
    eState mState = eState.Null;
    Vector3 mFlushBPos;
    Vector3 mAimNpcPos;

    public void InitHide(CNpcInst npcInst, char_DSword Self)
    {
        mNextPos = npcInst.GetHitSEPos();
        mAimNpcPos = mNextPos;
        if (mNextPos.x > Self.transform.position.x)
            mNextPos.x -= 3;
        else

            mNextPos.x += 3;

        mNextPos.y -= Self.mRefPointMid.transform.localPosition.y;

        mState = eState.Hide;
        mSelf = Self;

        Self.ChangeToStateFlashKill();
        Self.PlayAct("hide");

        mNpc = npcInst;
    }

    public void HideFinish()
    {
        mState = eState.Appear;
        mSelf.PlayAct("appear");

        mSelf.mCurFaceRight = mSelf.transform.position.x < mNextPos.x ? true : false;
        mSelf.ResetDir();

        mSelf.transform.position = mNextPos;
    }

    public void AppearFinish()
    {
        mNextPos = mAimNpcPos; //mNpc.GetHitSEPos();
        Vector3 dir = mNextPos - mSelf.transform.position;
        dir.y = 0; dir.z = 0;
        mNextPos += dir.normalized * 10;

        mState = eState.Flash;

        mFlushBPos = mSelf.transform.position;

        mSelf.PlayAct("quickKillFly1");

        mV = 60;

        GameObject obj = GameObject.Instantiate(gDefine.gData.mSkillBackFlshSEPreb);
        obj.transform.position = mSelf.mRefPointMid.transform.position;
        obj.transform.right = dir.normalized;

        mSelf.mCurFaceRight = mSelf.transform.position.x < mNextPos.x ? true : false;
        mSelf.ResetDir();
    }


    public void Init(CNpcInst npcInst, char_DSword Self)
    {
        mNextPos = npcInst.GetHitSEPos();
        if (mNextPos.x > Self.transform.position.x)
            mNextPos.x -= 2;
        else

            mNextPos.x += 2;
        mNextPos.y -= 1;

        mState = eState.FlyToBPos;
        mSelf = Self;

        Self.ChangeToStateFlashKill();
        Self.PlayAct("flashKillFly");

        mNpc = npcInst;

        mV = 42;
    }

    public void InitFlash(CNpcInst npcInst, char_DSword Self)
    {
        mNextPos = npcInst.GetHitSEPos();
        Vector3 dir = mNextPos - Self.transform.position;
        mNextPos += dir.normalized * 2;

        mState = eState.Flash;
        mSelf = Self;

        Self.ChangeToStateFlashKill();
        Self.PlayAct("quickKillFly1");

        mNpc = npcInst;

        mV = 42;

        GameObject obj = GameObject.Instantiate(gDefine.gData.mSkillBackFlshSEPreb);
        obj.transform.position = mSelf.mRefPointMid.transform.position;
        obj.transform.right = dir.normalized;

        mSelf.mCurFaceRight = mSelf.transform.position.x < mNextPos.x ? true : false;
        mSelf.ResetDir();
    }

    public void Update()
    {
        switch (mState)
        {
            case eState.FlyToBPos:
                UpdateFlyToBPos();
                break;
            case eState.WaitToKill:
                UpdateWaitToKill();
                break;
            case eState.KillFlyIN:
                UpdateKillIn();
                break;
            case eState.KillFlyOut:
                UpdateKillOut();
                break;
            case eState.WaitToNext:
                UpdateWaitToNext();
                break;
            case eState.DorpDown:
                UpdateDropDown();
                break;
            case eState.DorpKill:
                break;
            case eState.Flash:
                UpdateFlash();
                break;
        }

    }

    void UpdateFlash()
    {
        mSelf.mCurFaceRight = mSelf.transform.position.x < mNextPos.x ? true : false;
        mSelf.ResetDir();
        Vector3 pos = Vector3.MoveTowards(mSelf.transform.position, mNextPos, Time.deltaTime * mV);
        mSelf.transform.position = pos;
        if (Vector3.Distance(pos, mNextPos) < 0.01f)
        {
            mState = eState.WaitToNext;
            // Vector3 npcPos = mNpc.GetHitSEPos();
            mWaitT = Time.time + 0.1f;
            mSelf.PlayAct("wait");

            //mNpc.BeKilled();

            //show se...
            // GameObject obj = GameObject.Instantiate(gDefine.gData.mDSwordQuickKillFlashPreb);
            //obj.transform.position = npcPos;

            CNpcInst[] npcArr = gDefine.gNpc.FindByLine(mFlushBPos.x, mNextPos.x, CNpcInst.eNpcClass.Air);

            float deadPrec = 20;
            CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.DSWordAirFlush);
            if (d != null)
                deadPrec += 5 * d.mLearnNum;

            foreach (CNpcInst Inst in npcArr)
            {
                Vector3 npos = Inst.GetPos();
                if (Mathf.Abs(npos.y - mSelf.gameObject.transform.position.y) > 1.6f)
                    continue;


                if (Random.Range(0, 100) <= deadPrec)
                    Inst.BeKilled();
                else
                {
                    Inst.BeDamage((int)(gDefine.gPlayerData.mDamage * 1.2f), false, false, true, false,
                        CSkill.eSkill.DSWordGirl_FlashKill);

                    GameObject se = GameObject.Instantiate(gDefine.gData.mDSwordHitSEPreb);
                    se.transform.SetParent(Inst.GetRefMid().transform);
                    se.transform.localPosition = Vector3.zero;
                }

            }
        }
    }

    void UpdateFlyToBPos()
    {
        mSelf.mCurFaceRight = mSelf.transform.position.x < mNextPos.x ? true : false;
        mSelf.ResetDir();
        Vector3 pos = Vector3.MoveTowards(mSelf.transform.position, mNextPos, Time.deltaTime * mV);
        mSelf.transform.position = pos;
        if (Vector3.Distance(pos, mNextPos) < 0.01f)
        {
            mState = eState.WaitToKill;
            Vector3 npcPos = mNpc.GetHitSEPos();
            mWaitT = Time.time + 0.1f;
            mSelf.PlayAct("wait");
        }



    }

    void UpdateWaitToKill()
    {
        if (Time.time >= mWaitT)
        {
            mState = eState.KillFlyIN;
            mSelf.PlayAct("flashKillFly");
        }
    }

    void UpdateKillIn()
    {
        mNextPos = mSelf.transform.position;
        Vector3 pos = Vector3.MoveTowards(mSelf.transform.position, mNpc.GetHitSEPos(), Time.deltaTime * mV);
        mSelf.transform.position = pos;
        if (Vector3.Distance(pos, mNpc.GetHitSEPos()) < 0.01f)
        {
            mState = eState.KillFlyOut;
            Vector3 npcPos = mNpc.GetHitSEPos();
            mNpc.BeKilled();
            //show se...
            GameObject obj = GameObject.Instantiate(gDefine.gData.mDSwordQuickKillFlashPreb);
            obj.transform.position = npcPos;

            //next pos
            if (mNextPos.x < npcPos.x)
                mNextPos.x += 2;
            else
            {
                mNextPos.x -= 2;
            }
            mNextPos.y += 1;
        }
    }

    void UpdateKillOut()
    {
        Vector3 pos = Vector3.MoveTowards(mSelf.transform.position, mNextPos, Time.deltaTime * mV);
        mSelf.transform.position = pos;
        if (Vector3.Distance(pos, mNextPos) < 0.01f)
        {
            mState = eState.WaitToNext;
            mWaitT = Time.time + 0.1f;
            mSelf.PlayAct("wait");

            mSelf.mCurFaceRight = mSelf.transform.position.x < mNpc.GetHitSEPos().x ? true : false;
            mSelf.ResetDir();
        }
    }

    void UpdateWaitToNext()
    {
        if (Time.time >= mWaitT)
        {
            Vector3 pos = mSelf.transform.position;
            CNpcInst npc = gDefine.gNpc.FindByR(pos.x, 6, CNpcInst.eNpcClass.OnGround);

            if (npc != null)
            {
                mNextPos = mNpc.GetPos();
                if (mNextPos.x > pos.x)
                    mNextPos.x -= 1;
                else
                    mNextPos.x += 1;

                mSelf.mCurFaceRight = mSelf.transform.position.x < mNextPos.x ? true : false;
                mSelf.ResetDir();
            }
            else
            {
                mNextPos = pos;
            }
            mNextPos.y = gDefine.gGrounY;

            mSelf.PlayAct("dropDown");
            mV = 12.0f;

            mState = eState.DorpDown;

        }
    }

    void UpdateDropDown()
    {
        mNextPos = mSelf.transform.position;
        mNextPos.y = gDefine.gGrounY;

        Vector3 pos = Vector3.MoveTowards(mSelf.transform.position, mNextPos, Time.deltaTime * mV);
        mSelf.transform.position = pos;
        if (Vector3.Distance(pos, mNextPos) < 0.01f)
        {
            mState = eState.DorpKill;
            mSelf.PlayAct("dropKill");

            gDefine.gNpc.DoDamageLR(mSelf.transform.position, 2.5f, (int)(gDefine.gPlayerData.mDamage * 2), false,
                CNpcInst.eNpcClass.OnGround, true);

            gDefine.gFollowCam.PlayVibrate();
        }

    }
}
