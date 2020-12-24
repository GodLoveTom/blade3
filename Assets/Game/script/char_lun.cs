using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class char_lun : MonoBehaviour
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
        Paralysis, //麻痹
        Skill_JieZhan_Readying = 7,
        Skill_JieZhan_Ready = 8,
        Skill_JieZhan_Throw = 9,
        Skill_JuNeng = 10,
        Skill_BackFlash = 11,
    }

    public enum eEventAtkEX
    {
        Atk01=0, //标准的攻击动作，在0.1s内只能被攻击一次
        Atk01PushBack1,//攻击效果同上，但会被向后推开1的距离（Boss不会） 
    }

    eState mNextActState = eState.Idle;
    bool mNextActFaceRight = true;

    eState mCurActState = eState.Idle;
    public bool mCurFaceRight = true;

    Vector3 mMoveTarget; //当前要移动的目标

    Animator mAnimator;

    CNpcInst mTargetObj;

    public float mJumpL = 6;
    public float mJumpV = 8;
    public float mAtkL = 2.2f; // 攻击距离
    public float mBestToNpcL = 1.8f; // 与npc最佳距离
    public float mAtk0MoveL = 6; //攻击移动距离
    public float mAtk1MoveL = 6; //攻击移动距离
    public float mAtk2MoveL = 6; //攻击移动距离
    public float mAtk3MoveL = 6; //攻击移动距离

    public float mAtk0L = 2;
    public float mAtk1L = 2;
    public float mAtk2L = 2;
    public float mAtk3L = 2;


    public float mAtk0MoveV = 6; //攻击移动距离
    public float mAtk1MoveV = 6; //攻击移动距离
    public float mAtk2MoveV = 6; //攻击移动距离
    public float mAtk3MoveV = 6; //攻击移动距离

    public float mDLunAtkL = 4;  //双轮攻击距离

    float mAtkActCoolDown = 0;   //攻击动作冷却
    eState mOldAtkAct = eState.Idle; //上一次的动作是什么

    public GameObject mRefFeiLunPreb;

    public GameObject mRefMingzhongSe;

    public Text mHpText;

    public Image mHpImage;

    public GameObject mHpRoot;
    float mHpRootT=0;

    [Header("技能 双向波")]
    public GameObject mRefSkill_LunBo_Preb;
    //[Tooltip("是否拥有双向波技能")]
    bool mHaveSkillLunBo = true;
    //[Tooltip("双向波技能计时")]
    float mSkillLunBoT = 0;
    [Header("技能 电锯")]
    public GameObject mRefSkill_LunSaw_Preb;
    //[Tooltip("是否拥有电锯技能")]
    bool mHaveSkillLunSaw = true;
    //[Tooltip("电锯技能计时")]
    float mSkillLunSawT = 0;

    [Header("技能 截斩")]
    public GameObject mRefSkill_JieZhan_FlyPreb;

    [Header("技能 聚能")]
    public GameObject mRefSkill_JuNeng_SEPreb;
    //[Tooltip("是否拥有聚能技能")]
    bool mHaveSkillJuNeng = true;
    //[Tooltip("聚能技能计时")]
    float mSkillJuNengT = 0;
    //[Tooltip("聚能技能准备好")]
    bool mSkillJuNengReady = false;

    [Tooltip("突击距离 - 最近")]
    public float mFlyMinL = 6;
    [Tooltip("突击距离 - 最远")]
    public float mFlyMaxL = 10; //突击距离
    [Tooltip("突击速度")]
    public float mFlyV = 20;

    [Tooltip("特效人物中心参考点")]
    public GameObject mRefPointMid;

    [Header("技能后闪距离")]
    public float mSkillBackFlashL = 10;
    [Header("技能后闪速度")]
    public float mSkillBackFalshV = 80;

    float mGunCoolDown = 0;
     public ui_PcHp mUIHp;

     public CPlayerBuff mBuff = new CPlayerBuff();



    // Start is called before the first frame update
    void Start()
    {
        mAnimator = GetComponent<Animator>();
        gDefine.gPcLun = this;
        mSkillLunBoT = Time.time;

        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        mNextActState = eState.Idle;
        mNextActFaceRight = mCurFaceRight;
        ChangeToNextActNow();
    }

    public bool CanUseGun()
    {
        if( Mathf.Abs( transform.position.y - gDefine.gGrounY) < 0.5f 
            && mCurActState < eState.Paralysis )
            return true;
        else
            return false;
    }

    public void ShowHPUI()
    {
        //mHpRoot.SetActive(true);
        //mHpRootT = Time.time +1;
    }


     public void BuffBegin(CBuff.eBuff BuffType)
    {
        if (BuffType == CBuff.eBuff.Paralysis && (mCurActState != eState.Paralysis && mCurActState <= eState.Skill_BackFlash))
        {
            mCurActState = eState.Paralysis;
            //mAnimator.Play("beAtk", 0, 0);???
            //Debug.Log("!!!=== buff be atk");
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

     public void AddBuff(CBuff.eBuff BuffType, float LastT, int Damage, Transform Trans, CBuff.BuffBegin_CallBack BFunc, CBuff.BuffEnd_CallBack EFunc)
    {
        if (gDefine.gPlayerData.mHp <= 0)
            return;

        if (gDefine.gPlayerData.IsCurIgonrDamage())
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

        CGird gird = gDefine.gPlayerData.mEquipGird[ (int)gDefine.eEuqipPos.Ring];
        if( gird.mRefItem!=null)
        {
            if( gird.mRefItem.mSpecialIndex == 1 && BuffType == CBuff.eBuff.Posion)
                return;
            else  if( gird.mRefItem.mSpecialIndex == 2 && BuffType == CBuff.eBuff.Paralysis)
                return;
            else  if( gird.mRefItem.mSpecialIndex == 3 && BuffType == CBuff.eBuff.Curse)
                return;
        }

        mBuff.AddBuff(BuffType, LastT, Damage, Trans, BFunc, EFunc);
    }

    public Vector3 GetPcCamFollowPos()
    {
        if (mCurActState == eState.Skill_JuNeng)
            return transform.position + (mCurFaceRight ? Vector3.right * 3 : Vector3.left * 3);
        else
            return transform.position;
    }

    public void BeAtk(int Damage, CNpcInst Npc)
    {
        if (gDefine.gPlayerData.IsCurIgonrDamage())
            return;

        if (Damage <= 0)
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

        CGird gird = gDefine.gPlayerData.mEquipGird[(int)gDefine.eEuqipPos.MainWeapon];
        if(gird.mRefItem!=null && gird.mRefItem.mSpecialIndex == 1)
        {
            if(Random.Range(0,100)<=10)
            {
                string str = gDefine.GetStr(385);
                 gDefine.gDamageShow.CreateDamageShow(str, transform.position + Vector3.up * 3, new Color(0.9f, 0.5f, 0.9f, 1),1);
      
                return;
            }
        }


        //披风特殊提供
         gird = gDefine.gPlayerData.mEquipGird[(int)gDefine.eEuqipPos.Cloak];
        // if(gird.mRefItem!=null && gird.mRefItem.mSpecialIndex == 1)
        // {
        //     Damage = (int)(Damage * 0.95f);
        //     if(Damage<1) Damage = 1;
        // }
        if( gird.mRefItem!=null && (float)gDefine.gPlayerData.mHp / (float)gDefine.gPlayerData.mHpMax < 0.3f 
        && gird.mRefItem.mSpecialIndex == 2)
        {
            if(Random.Range(0,100)<10)
                return;
        }

        //衣服特殊提供
        gird = gDefine.gPlayerData.mEquipGird[(int)gDefine.eEuqipPos.Clothe];
        // if(gird.mRefItem!=null && gird.mRefItem.mSpecialIndex == 1)
        // {
        //     Damage = (int)(Damage * 0.95f);
        //     if(Damage<1) Damage = 1;
        // }
         if(gird.mRefItem!=null && gird.mRefItem.mSpecialIndex == 3&& Npc!=null)
        {
            if(Random.Range(0,100)<30)
                Npc.AddBuff(CBuff.eBuff.Paralysis, 3);
        }


        gDefine.gPlayerData.mHp -= Damage;
        gDefine.gDamageShow.CreateDamageShow(Damage, gameObject.transform.parent.transform.position + Vector3.up * 3, Color.red, false);

        gDefine.gGameMainUI.mBeAtkUI.Show();

        if (gDefine.gPlayerData.mHp <= 0)
        {
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


            }
            else
            {
                mAnimator.Play("die", 0, 0);
                gDefine.gPause = true;

                Event_PlaySound(27);
            }
        }

        RefreshHPUI(true);

    }

    public void Event_PlaySound(int SoundId)
    {
        AudioClip clip = gDefine.gData.GetSoundClip(SoundId);
        if (clip != null)
            gDefine.gSound.Play(clip);
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

    /// <summary>
    /// 一个攻击动作会引发多个攻击事件，来保持攻击流畅
    /// 怪在某个事件段内只会被攻击一次
    /// </summary>
    /// <param name="IgnorT"></param>
    void Event_AtkEX(float IgnorT)
    {
        if (gDefine.gNpc.DoDamage(transform.position,
                     mAtkL, mCurFaceRight, (int)gDefine.gPlayerData.mDamage, false, false, CNpcInst.eNpcClass.OnGround, false))
        {
            GameObject mingZhongSE = GameObject.Instantiate(mRefMingzhongSe);
            mingZhongSE.transform.position = transform.position +
               ((mCurFaceRight) ? new Vector3(2.2f, 1.5f, -0.5f) : new Vector3(-2.2f, 1.5f, -0.5f));

            int bloodSuck = gDefine.gPlayerData.GetBloodSuckHp();
            if (bloodSuck > 0)
            {
                gDefine.gPlayerData.mHp += bloodSuck;
                if (gDefine.gPlayerData.mHp > gDefine.gPlayerData.mHpMax)
                    gDefine.gPlayerData.mHp = gDefine.gPlayerData.mHpMax;

                gDefine.gDamageShow.CreateDamageShow(bloodSuck, transform.position + Vector3.up * 3, Color.green, false);

                RefreshHPUI(true);
                ShowHPUI();
            }
            int atkSound = Random.Range(0,100)<50?19:22;
            gDefine.PlaySound(atkSound);

              CGird gird = gDefine.gPlayerData.mEquipGird[(int)gDefine.eEuqipPos.MainWeapon];
            if(gird.mRefItem!=null && gird.mRefItem.mSpecialIndex == 3)
            {
                if(Random.Range(0,100)<7 && gDefine.gPlayerData.mHp< gDefine.gPlayerData.mHpMax)
                {
                    int hpadd = (int)(gDefine.gPlayerData.mHpMax*0.01f);
                    if(hpadd<1) hpadd=1;
                    gDefine.gPlayerData.mHp += hpadd;
                    if(gDefine.gPlayerData.mHp > gDefine.gPlayerData.mHpMax)
                        gDefine.gPlayerData.mHp = gDefine.gPlayerData.mHpMax;

                    mUIHp.Refresh(   (float)gDefine.gPlayerData.mHp / (float)gDefine.gPlayerData.mHpMax , true);
                
                    if( gDefine.gPlayerData.mTmpHelthSE==null)
                    {
                        GameObject se = GameObject.Instantiate(gDefine.gData.mUIHealSEPreb);
                        se_event s = se.GetComponent<se_event>();
                        s.InitLiftT(1);
                        se.transform.SetParent(mRefPointMid.transform);
                        se.transform.localPosition = Vector3.zero;
                        gDefine.gPlayerData.mTmpHelthSE = se;
                    }
                
                
                }
                
            }
        }

        //闪电链
        if (gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.LightChain) != null && Random.Range(0, 100) < 15)
        {
            CNpcInst npc = gDefine.gNpc.FindWithR(transform.position.x, 6, CNpcInst.eNpcClass.OnGround);
            if (npc != null)
            {
                GameObject obj = GameObject.Instantiate(gDefine.gData.mLightChainSEPreb);
                se_Skill_LightChain script = obj.GetComponent<se_Skill_LightChain>();
                int nextNum = Random.Range(0, 5);
                script.Init(mRefPointMid.transform, npc, nextNum, (int)(gDefine.gPlayerData.mDamage * 1.3f));
            }
            else
            {
                GameObject obj = GameObject.Instantiate(gDefine.gData.mLightChainNullPreb);
                 se_lightChainNull script = obj.GetComponent<se_lightChainNull>();
                script.Init(mRefPointMid.transform, mCurFaceRight ? Vector3.right * 8 : Vector3.left * 8,
                (int)(gDefine.gPlayerData.mDamage * 1.3f),8);   
            }
        }

    }

    /// <summary>
    /// atk1动作事件， 引发一个额外移动
    /// </summary>
    void Event_Atk1Move()
    {
        CheckTarget();
        if(mTargetObj != null)
        {
            float l = Mathf.Abs(transform.position.x - mTargetObj.GetPos().x);
            if(l>mAtk1L)
            {
                l = mAtk1L;
                mMoveTarget = transform.position + (mCurFaceRight?Vector3.right*l:Vector3.left*l);
            }
        }
        else
        {
            mMoveTarget = transform.position + (mCurFaceRight?Vector3.right*mAtk1L:Vector3.left*mAtk1L);
        }
    }

    public void ClearTarget()
    {
        mTargetObj = null;
    }
    void UpdateSkill()
    {
        if (gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.BigLun_DoubleWave) != null)
        {
            if (Time.time >= mSkillLunBoT + 5)
            {
                mSkillLunBoT = Time.time;

                GameObject o = GameObject.Instantiate(mRefSkill_LunBo_Preb);

                o.GetComponent<se_Skill_lunbo>().Init(transform.position, mCurFaceRight, (int)gDefine.gPlayerData.mDamage);

                o = GameObject.Instantiate(mRefSkill_LunBo_Preb);

                o.GetComponent<se_Skill_lunbo>().Init(transform.position, !mCurFaceRight, (int)gDefine.gPlayerData.mDamage);

                gDefine.gDamageShow.CreateDamageShow("双向波", transform.position + Vector3.up * 3, new Color(0.9f, 0.5f, 0.9f, 1));
            }
        }

        if (gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.BigLun_Saw) != null)
        {
            if (Time.time >= mSkillLunSawT + 20)
            {

                CNpcInst inst = gDefine.gNpc.FindByL(transform.position.x, mCurFaceRight, 8.0f);
                if (inst == null)
                    inst = gDefine.gNpc.FindByL(transform.position.x, !mCurFaceRight, 8.0f);

                if (inst != null)
                {
                    Vector3 epos = inst.GetHitSEPos();
                    Vector3 bpos = transform.position;
                    bpos.y += 0.8f;
                    GameObject o = GameObject.Instantiate(mRefSkill_LunSaw_Preb);
                    o.transform.position = bpos;
                    o.GetComponent<se_skill_lun_saw>().Init(epos, (int)gDefine.gPlayerData.mDamage, 5.0f, epos.x > transform.position.x ? true : false);

                    gDefine.gDamageShow.CreateDamageShow("电锯", transform.position + Vector3.up * 3, new Color(0.9f, 0.5f, 0.9f, 1));

                    mSkillLunSawT = Time.time;
                }
            }
        }

        if (gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.BigLun_Lighting) != null)
        {
            if (Time.time >= mSkillJuNengT + 20)
            {
                mSkillJuNengReady = true;
            }
        }
    }

    public void Btn_Down(bool IsRight)
    {
        if (mCurActState == eState.Skill_JieZhan_Readying ||
            mCurActState == eState.Skill_JieZhan_Ready ||
            mCurActState == eState.Skill_JuNeng)
            return;

        if (gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.BigLun_HalfKill) == null)
            return;

        mNextActState = eState.Idle;
        mNextActFaceRight = IsRight;

        if (mCurActState == eState.Idle || mCurActState == eState.JumpBack)
        {
            mCurActState = eState.Skill_JieZhan_Readying;
            mCurFaceRight = IsRight;
            mMoveTarget = transform.position;
            SetCurAct();
        }
        else
        {
            mNextActFaceRight = IsRight;
            mNextActState = eState.Skill_JieZhan_Readying;
        }
    }


    public void Btn_Up(bool IsRight)
    {
        if (mCurActState == eState.Skill_JieZhan_Readying || mCurActState == eState.Skill_JieZhan_Ready)
        {
            if (IsRight != mCurFaceRight)
                return;
        }

        if (mCurActState == eState.Skill_JieZhan_Ready)
        {
            mCurActState = eState.Skill_JieZhan_Throw;
            mAnimator.Play("lungirl_skill_throw");

            transform.rotation = new Quaternion();
            mHpRoot.transform.rotation = new Quaternion();

            mMoveTarget = transform.position;

            if (!mCurFaceRight)
            {
                transform.Rotate(0, 180, 0, Space.World);
                mHpRoot.transform.Rotate(0, 180, 0, Space.Self);
            }
        }
        else
        {
            //change to next.
            ChangeToNextActNow();
        }

    }

    public void SkillActEndCallBack()
    {
        ChangeToNextActNow();
    }

    public void Relive()
    {
        mCurActState = eState.Idle;
        mAnimator.Play("lungirl_idle");
        RefreshHPUI(true);
    }

    void Event_ActEnd(int Index)
    {
        // bool calcUseGun = false;
        // float useGunPerc = gDefine.gPlayerData.mChangeToGunPerc;
        // CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.GunMaster);
        // if (d != null)
        //     useGunPerc += d.mLearnNum * 5;
        // CNpcInst npc = gDefine.gNpc.FindByR(transform.position.x, 7, CNpcInst.eNpcClass.All);
        // if (npc != null)
        // {
        //     if (gDefine.gNpc.ThereIsOnlyAirNpc())
        //     {
        //         calcUseGun = true;
        //         useGunPerc += 100;
        //     }
        //     else if (Index == 1003 && Time.time >= mGunCoolDown)
        //     {
        //         calcUseGun = true;
        //     }

        //     if (calcUseGun)
        //     {
        //         if (Random.Range(0, 100) <= useGunPerc)
        //         {
        //             mNextActState = eState.Idle;
        //             gDefine.UseShootGirlNow(transform.position, mCurFaceRight, 2f);
        //             gameObject.SetActive(false);

        //             mGunCoolDown = Time.time+2.5f;
        //         }
        //     }

        // }

        if (mCurActState == eState.Skill_BackFlash)
        {
            mNextActState = eState.Idle;
            mNextActFaceRight = mCurFaceRight;
        }
        ChangeToNextActNow();
    }

    public bool ThrowItem(CSkill.eSkill SkillType, int Num)
    {
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

    public void EndFiLunAct()
    {
        ChangeToNextActNow();
    }

    void Event_Atk()
    {
        if (gDefine.gNpc.DoDamage(transform.position,
                     mAtkL, mCurFaceRight, (int)gDefine.gPlayerData.mDamage, false, true, CNpcInst.eNpcClass.OnGround, false))
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
            int atkSound = Random.Range(0,100)<50?19:22;
            gDefine.PlaySound(atkSound);

              CGird gird = gDefine.gPlayerData.mEquipGird[(int)gDefine.eEuqipPos.MainWeapon];
            if(gird.mRefItem!=null && gird.mRefItem.mSpecialIndex == 3)
            {
                if(Random.Range(0,100)<7 && gDefine.gPlayerData.mHp< gDefine.gPlayerData.mHpMax)
                {
                    int hpadd = (int)(gDefine.gPlayerData.mHpMax*0.01f);
                    if(hpadd<1) hpadd=1;
                    gDefine.gPlayerData.mHp += hpadd;
                    if(gDefine.gPlayerData.mHp > gDefine.gPlayerData.mHpMax)
                        gDefine.gPlayerData.mHp = gDefine.gPlayerData.mHpMax;

                    mUIHp.Refresh(   (float)gDefine.gPlayerData.mHp / (float)gDefine.gPlayerData.mHpMax , true);
                }
                
            }
        }

        //闪电链
        if (gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.LightChain) != null && Random.Range(0, 100) < 15)
        {
            CNpcInst npc = gDefine.gNpc.FindWithR(transform.position.x, 6, CNpcInst.eNpcClass.OnGround);
            if (npc != null)
            {
                GameObject obj = GameObject.Instantiate(gDefine.gData.mLightChainSEPreb);
                se_Skill_LightChain script = obj.GetComponent<se_Skill_LightChain>();
                int nextNum = Random.Range(0, 5);
                script.Init(mRefPointMid.transform, npc, nextNum, (int)(gDefine.gPlayerData.mDamage * 1.3f));
            }
            else
            {
                GameObject obj = GameObject.Instantiate(gDefine.gData.mLightChainNullPreb);
                 se_lightChainNull script = obj.GetComponent<se_lightChainNull>();
                script.Init(mRefPointMid.transform, mCurFaceRight ? Vector3.right * 8 : Vector3.left * 8,
                (int)(gDefine.gPlayerData.mDamage * 1.3f),8);   }


        }
    }

    public void RefreshHPUI(bool Show)
    {
        //mHpText.text = gDefine.gPlayerData.mHp.ToString();
        //mHpImage.transform.localScale = new Vector3(gDefine.gPlayerData.mHp / (float)gDefine.gPlayerData.mHpMax, 1, 1);
        mUIHp.Refresh(gDefine.gPlayerData.mHp / (float)gDefine.gPlayerData.mHpMax, Show);
    }


    void Event_LRAtk()
    {
        Vector3 pos = transform.position;
        CNpcInst[] npc = gDefine.gNpc.DoDamageLR(pos, mAtk2L, (int)gDefine.gPlayerData.mDamage, false, CNpcInst.eNpcClass.OnGround,
        false);
        if (npc != null)
        {
            for (int i = 0; i < npc.Length; i++)
            {
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

        }
    }

    public void UseGunGirl()
    {
        mNextActState = eState.Idle;
        gDefine.UseShootGirlNow(transform.position, mCurFaceRight, 2f);
        gameObject.SetActive(false);
        mNextActState = eState.Idle;
    }

    void Event_DieEnd()
    {
        gDefine.gGameMainUI.ShowFailUI();
    }

    void Event_Idle()
    {
        mCurActState = eState.Idle;
    }

    void Event_Throw()
    {
        CreateFeiLun((int)(gDefine.gPlayerData.mDamage * 1.2f));
    }

    void Event_Skill_JuNeng_Begin()
    {
        GameObject o = GameObject.Instantiate(mRefSkill_JuNeng_SEPreb);
        Vector3 bpos = transform.position + Vector3.up * 1.4f + Vector3.right * 2.5f;
        if (!mCurFaceRight)
            bpos = transform.position + Vector3.up * 1.4f - Vector3.right * 2.5f;
        o.transform.position = bpos;

        o.GetComponent<se_skill_lun_juneng>().Init(transform.position.x, mCurFaceRight, (int)gDefine.gPlayerData.mDamage);
    }

    void Event_Skill_JieZhan_Throw()
    {
        GameObject o = GameObject.Instantiate(mRefSkill_JieZhan_FlyPreb);
        float damage = gDefine.gPlayerData.mDamage;
        CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.BigLun_HalfKill);
        if (d != null && d.mLearnNum > 0)
            damage += (damage * 0.1f * d.mLearnNum);

        o.GetComponent<se_skill_lun_ReadyFly>().Init(
            transform.position + Vector3.up * 1.2f, mCurFaceRight, (int)damage);
    }

    public void Btn_Click(bool IsRight)
    {
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
                    //else if (mCurAct == ePCActType.JumpFly)
                    //   mNextAct = ePCActType.JumpAtk;
                    //else
                    //   mNextAct = ePCActType.JumpAtk;
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
                if (gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.BackFlash) != null && l < 7.5f)
                {
                    BeginSkillBackFlash();
                    return;
                }
                else
                if (l > mFlyMinL)
                {
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
        if (mTargetObj != null && mTargetObj.IsLive())
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
        if (mTargetObj != null && mTargetObj.IsLive())
        {
             if ((mCurActState >= eState.Atk0 && mCurActState <= eState.Atk3) || mCurActState == eState.Fly)
                return Mathf.Abs(mMoveTarget.x - mTargetObj.GetPos().x);
            else
                return Mathf.Abs(transform.position.x - mTargetObj.GetPos().x);
        }
        else
            return 10000000;
    }

    bool ChangeToNextActNow()
    {
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

        if (mSkillJuNengReady)
        {
            mCurActState = eState.Skill_JuNeng;
            mMoveTarget = transform.position;
            mSkillJuNengReady = false;
            mSkillJuNengT = Time.time;
        }
        else if (mCurActState == eState.JumpBack)
        {
            mMoveTarget = (mCurFaceRight == true) ?
                    transform.position + mJumpL * Vector3.left
                :
                    transform.position + mJumpL * Vector3.right;
        }
        else
        {
            if (mTargetObj != null)
            {
                float l = CalcTargetL();

                //再确认下位置,位置不够的情况下还是要改成攻击前进，或者是攻击
                if (mCurActState == eState.Fly && (l <= GetAtkMoveL() + mBestToNpcL || l > mJumpL))
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
                        mMoveTarget = transform.position;
                    }
                    else if (l < GetAtkMoveL() + mBestToNpcL)
                    {
                        mMoveTarget = (mCurFaceRight == true) ?
                            mTargetObj.GetPos() + mBestToNpcL * Vector3.left
                        :
                            mTargetObj.GetPos() + mBestToNpcL * Vector3.right;
                    }
                    else
                    {
                        //攻击前进
                        //若没有目标，则为当前默认的移动
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

    public void CreateFeiLun(int Damage)
    {
        GameObject o = GameObject.Instantiate(mRefFeiLunPreb);
        se_feilun script = o.GetComponent<se_feilun>();
        Vector3 pos = transform.position;
        pos.y = mRefPointMid.transform.position.y;
        script.Init(pos, 10, mCurFaceRight, Damage);
    }

    void SetCurAct()
    {
        switch (mCurActState)
        {
            case eState.Idle:
                mAnimator.Play("lungirl_idle");
                break;
            case eState.Atk0:
                mAnimator.Play("lungirl_atk0");
                break;
            case eState.Atk1:
                mAnimator.Play("lungirl_atk1");
                break;
            case eState.Atk2:
                mAnimator.Play("lungirl_atk2");
                break;
            case eState.Atk3:
                gDefine.gDamageShow.CreateDamageShow("切割", transform.position + Vector3.up * 3, new Color(0.9f, 0.5f, 0.9f, 1));
                mAnimator.Play("lungirl_atk3");
                break;
            case eState.Fly:
                mAnimator.Play("lungirl_fly");
                break;
            case eState.JumpBack:
                mAnimator.Play("lungirl_jumpback");
                break;
            case eState.Skill_JieZhan_Readying:
                mAnimator.Play("lungirl_skill_Readying");
                break;
            case eState.Skill_JuNeng:
                mAnimator.Play("lungirl_skill_juneng");
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

            case eState.Skill_BackFlash:
                return mSkillBackFalshV;
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
                    float perc = 30;
                    CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.BigLun_Cut);
                    if (d != null)
                        perc += d.mLearnNum * 10;
                    if (gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.BigLun_Cut) != null
                    && Random.Range(0, 100) < perc)
                        return eState.Atk3;
                    else
                        return eState.Atk0;
                case eState.Atk3:
                    return eState.Atk0;
                default:
                    return eState.Atk0;
            }

        }
    }

    void Update()
    {
        //if (mIsInKilledPlay)
        //   return;
        //AnimatorStateInfo info = mAnimator.GetCurrentAnimatorStateInfo(0);
        // mAnimator.getcur
        // info.

        //if(Time.time > mHpRootT)
         //   mHpRoot.SetActive(false);

        if (!gDefine.gPause)
        {
            UpdateSkill();
            UpdateLogic();
            UpdateMove(Time.deltaTime);
        }
    }

    void UpdateLogic()
    {
        //if (mCurIsPause || mIsInKilledPlay)
        //   return;

        mAtkActCoolDown -= Time.deltaTime;

        if (mTargetObj != null && !mTargetObj.IsLive())
        {
            mTargetObj = null;
        }

        //mBuff.Update();

    }


    void UpdateMove(float T)
    {
        //如果当前没到位，则移动
        if ((mCurActState >= eState.Atk0 && mCurActState <= eState.JumpBack)
            || mCurActState == eState.Fly || mCurActState == eState.Skill_BackFlash)
        {
            float moveV = GetMoveV();

            //gameObject.transform.pasrent.transform.position =
            Vector3 newPos = Vector3.MoveTowards(transform.position, mMoveTarget, moveV * Time.deltaTime);
            Vector3 deltPos = newPos - transform.position;
            transform.Translate(deltPos, Space.World);

            gDefine.CalcSceMove(deltPos);

            if (mCurActState == eState.Fly && Vector3.Distance(newPos, mMoveTarget) < 0.01f)
            {
                Event_Atk();
                ChangeToNextActNow();
            }
        }

    }

    public void DoThrowDamage(CNpcInst[] NpcArr, int Damage)
    {
        for (int i = 0; i < NpcArr.Length; i++)
        {
            int bloodSuck = gDefine.gPlayerData.GetBloodSuckHp();
            if (bloodSuck > 0)
            {
                gDefine.gPlayerData.mHp += bloodSuck;
                if (gDefine.gPlayerData.mHp > gDefine.gPlayerData.mHpMax)
                    gDefine.gPlayerData.mHp = gDefine.gPlayerData.mHpMax;

                gDefine.gDamageShow.CreateDamageShow(bloodSuck, transform.position + Vector3.up * 3,
                    Color.green, false);
                RefreshHPUI(true);
                ShowHPUI();

            }
        }
    }

    void Event_JieZhanReady()
    {
        mCurActState = eState.Skill_JieZhan_Ready;
    }

    public void EventSkill_BackFlashAtk()
    {
        CSkill skill = gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.BackFlash);

        CNpcInst[] arr = gDefine.gNpc.DoDamageLineWithDeadPrec(skill.mParamFloat, mMoveTarget.x,
                    (int)gDefine.gPlayerData.mDamage * 2, 10, CNpcInst.eNpcClass.OnGround, true);

        for (int i = 0; i < arr.Length; i++)
        {
            // GameObject mingZhongSE = GameObject.Instantiate(mRefMingzhongSe);
            //  mingZhongSE.transform.position = transform.position +
            //  ((mCurFaceRight) ? new Vector3(2.2f, 1.5f, -0.5f) : new Vector3(-2.2f, 1.5f, -0.5f));

            int bloodSuck = gDefine.gPlayerData.GetBloodSuckHp();
            if (bloodSuck > 0)
            {
                gDefine.gPlayerData.mHp += bloodSuck;
                if (gDefine.gPlayerData.mHp > gDefine.gPlayerData.mHpMax)
                    gDefine.gPlayerData.mHp = gDefine.gPlayerData.mHpMax;

                gDefine.gDamageShow.CreateDamageShow(bloodSuck, gameObject.transform.parent.transform.position + Vector3.up * 3, Color.green, false);
                RefreshHPUI(true);
                ShowHPUI();
            }

            if (Random.Range(0, 100) < 20)
            {
                arr[i].BeKilled();
            }
        }
        RefreshHPUI(true);

    }

    public void BeginSkillBackFlash()
    {
        mCurFaceRight = !mCurFaceRight;

        mCurActState = eState.Skill_BackFlash;
        mAnimator.Play("backFlash");

        transform.rotation = new Quaternion();
        mHpRoot.transform.rotation = new Quaternion();

        mMoveTarget = mCurFaceRight ? transform.position + Vector3.right * mSkillBackFlashL :
            transform.position - Vector3.right * mSkillBackFlashL;

        if (!mCurFaceRight)
        {
            transform.Rotate(0, 180, 0, Space.World);
            mHpRoot.transform.Rotate(0, 180, 0, Space.Self);
        }

        GameObject o = GameObject.Instantiate(gDefine.gData.mSkillBackFlshSEPreb);
        o.transform.position = transform.position + Vector3.up * 1.5f;
        if (!mCurFaceRight)
        {
            o.transform.rotation = new Quaternion();
            o.transform.Rotate(0, 180, 0, Space.Self);
        }

        CSkill skill = gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.BackFlash);

        skill.mParamFloat = transform.position.x;

        gDefine.gDamageShow.CreateDamageShow("后闪", transform.position + Vector3.up * 3, new Color(0.9f, 0.5f, 0.9f, 1));

    }



}
