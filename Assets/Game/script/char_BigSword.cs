using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class char_BigSword : MonoBehaviour
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
        SkillReadyKill_Readying = 7, //蓄力
        SkillReadyKill_Ready = 8, //蓄力完毕
        SkillReadyKill_Atk = 9,// 蓄力斩
        SkillSword_Flush = 10, //剑气斩
        SkillSword_Lun = 11,   //大车轮
        SkillBackFlash = 12, //后闪
    }

    eState mNextActState = eState.Idle;
    bool mNextActFaceRight = true;

    eState mCurActState = eState.Idle;
    public bool mCurFaceRight = true;

    Vector3 mMoveTarget; //当前要移动的目标

    Animator mAnimator;

    public CNpcInst mTargetObj;

    [Header("跳跃距离")]
    public float mJumpL = 3;
    [Tooltip("跳跃速度")]
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


    public float mCirleAtkL = 4;  //旋转攻击距离

    [Tooltip("突击距离 - 最近")]
    public float mFlyMinL = 6;
    [Tooltip("突击距离 - 最远")]
    public float mFlyMaxL = 10; //突击距离

    [Tooltip("突击速度")]
    public float mFlyV = 20;

    [Header("技能 大车轮 L距离 V速度")]
    [Tooltip("冲击距离")]
    public float mSkillLunMoveL = 10;
    [Tooltip("冲击速度")]
    public float mSkillLunMoveV = 10;

    [Tooltip("特效人物中心参考点")]
    public GameObject mRefPointMid;

    float mAtkActCoolDown = 0;   //攻击动作冷却
    eState mOldAtkAct = eState.Idle; //上一次的动作是什么

    public GameObject mRefMingzhongSe;

    public GameObject mRefReadyKill_KillAll_Preb;
    public GameObject mRefSkill_Rain_Preb;
    public GameObject mRefSkill_Flush_Preb;

    public Text mHpText;

    public Image mHpImage;

    public GameObject mHpRoot;

    float mSkillRainT = 20;

    float mSkillFlushT = 20;
    bool mHaveSkillFlush = true;
    bool mSkillFlushIsReady = false;

    float mSkillBigLunT = 20;
    bool mHaveSkillBigLun = true;
    bool mSkillBigLunIsReady = false;
    [Header("技能后闪距离")]
    public float mSkillBackFlashL = 10;
    [Header("技能后闪速度")]
    public float mSkillBackFalshV = 80;

    // Start is called before the first frame update
    void Start()
    {
        mAnimator = GetComponent<Animator>();
        gDefine.gPcBigSword = this;
        RefreshHPUI();

        mSkillFlushT = Random.Range(10.0f, 20.0f);
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        mNextActState = eState.Idle;
        mNextActFaceRight = mCurFaceRight;
        ChangeToNextActNow();
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

    public void BeAtk(int Damage, float DelayT = 0)
    {
        if (gDefine.gPlayerData.IsCurIgonrDamage())
            return;

        if (Damage <= 0)
            return;

        gDefine.gPlayerData.mHp -= Damage;
        gDefine.gDamageShow.CreateDamageShow(Damage, gameObject.transform.parent.transform.position + Vector3.up * 3, Color.red, false);

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


            }
            else
            {
                gDefine.gGameMainUI.ShowFailUI();
                gDefine.gPause = true;

                Event_PlaySound(27);
            }
        }

        RefreshHPUI();

    }


    public void ClearTarget()
    {
        mTargetObj = null;
    }

    public void Event_PlaySound(int SoundId)
    {
        AudioClip clip = gDefine.gData.GetSoundClip(SoundId);
        if (clip != null)
            gDefine.gSound.Play(clip);
    }

    bool SetNextActState(eState state)
    {
        if (mNextActState >= eState.SkillReadyKill_Readying)
            return false;

        mNextActState = state;

        return true;
    }

    public void Relive()
    {
        mCurActState = eState.Idle;
        mAnimator.Play("BigSword_idle");
        RefreshHPUI();
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


    void Event_ActEnd(int Index)
    {
        bool calcUseGun = false;
        float useGunPerc = gDefine.gPlayerData.mChangeToGunPerc;
        CNpcInst npc = gDefine.gNpc.FindByR(transform.position.x, 7, CNpcInst.eNpcClass.All);
        if (npc != null)
        {
            if (gDefine.gNpc.ThereIsOnlyAirNpc())
            {
                calcUseGun = true;
                useGunPerc += 100;
            }
            else if (Index == 1003)
                calcUseGun = true;

            if (calcUseGun)
            {
                if (Random.Range(0, 100) <= useGunPerc)
                {
                    mNextActState = eState.Idle;
                    gDefine.UseShootGirlNow(transform.position, mCurFaceRight, 1.5f);
                    gameObject.SetActive(false);
                }
            }
        }


        if (mCurActState == eState.SkillBackFlash)
        {
            mNextActState = eState.Idle;
            mNextActFaceRight = mCurFaceRight;
        }
        ChangeToNextActNow();
    }

    public void EndFiLunAct()
    {
        ChangeToNextActNow();
    }

    void Event_Atk()
    {
        // if (gDefine.gNpc.DoDamage(transform.position,
        //              mAtkL, mCurFaceRight, (int)gDefine.gPlayerData.mDamage, false, false, CNpcInst.eNpcClass.OnGround))
        // {
        //     GameObject mingZhongSE = GameObject.Instantiate(mRefMingzhongSe);
        //     mingZhongSE.transform.position = transform.position +
        //        ((mCurFaceRight) ? new Vector3(2.2f, 1.5f, -0.5f) : new Vector3(-2.2f, 1.5f, -0.5f));

        //     int bloodSuck = gDefine.gPlayerData.GetBloodSuckHp();
        //     if (bloodSuck > 0)
        //     {
        //         gDefine.gPlayerData.mHp += bloodSuck;
        //         if (gDefine.gPlayerData.mHp > gDefine.gPlayerData.mHpMax)
        //             gDefine.gPlayerData.mHp = gDefine.gPlayerData.mHpMax;

        //         gDefine.gDamageShow.CreateDamageShow(bloodSuck, transform.position + Vector3.up * 3, Color.green, false);
        //         RefreshHPUI();
        //     }


        //     Event_PlaySound(3);

        // }
        // RefreshHPUI();
    }

    void Event_HAtk()
    {
        bool kickback = mCurActState == eState.Atk3 ? true : false;
        // if (gDefine.gNpc.DoDamage(transform.position,
        //              mHAtkL, mCurFaceRight, (int)gDefine.gPlayerData.mDamage * 2, true, kickback , CNpcInst.eNpcClass.OnGround))
        // {
        //     GameObject mingZhongSE = GameObject.Instantiate(mRefMingzhongSe);
        //     mingZhongSE.transform.position = transform.position +
        //        ((mCurFaceRight) ? new Vector3(2.2f, 1.5f, -0.5f) : new Vector3(-2.2f, 1.5f, -0.5f));

        //     int bloodSuck = gDefine.gPlayerData.GetBloodSuckHp();
        //     if (bloodSuck > 0)
        //     {
        //         gDefine.gPlayerData.mHp += bloodSuck;
        //         if (gDefine.gPlayerData.mHp > gDefine.gPlayerData.mHpMax)
        //             gDefine.gPlayerData.mHp = gDefine.gPlayerData.mHpMax;

        //         gDefine.gDamageShow.CreateDamageShow(bloodSuck, transform.position + Vector3.up * 3, Color.green, false);
        //         RefreshHPUI();
        //     }
        // }
        RefreshHPUI();
    }

    void Event_LRAtk()
    {
        // Vector3 pos = transform.position;
        // pos.x -= mCirleAtkL * 0.5f;
        // CNpcInst[] npc = gDefine.gNpc.DoDamageLR(pos, mCirleAtkL, (int)gDefine.gPlayerData.mDamage, false, CNpcInst.eNpcClass.OnGround);
        // if (npc != null)
        // {
        //     for (int i = 0; i < npc.Length; i++)
        //     {
        //         int bloodSuck = gDefine.gPlayerData.GetBloodSuckHp();
        //         if (bloodSuck > 0)
        //         {
        //             gDefine.gPlayerData.mHp += bloodSuck;
        //             if (gDefine.gPlayerData.mHp > gDefine.gPlayerData.mHpMax)
        //                 gDefine.gPlayerData.mHp = gDefine.gPlayerData.mHpMax;

        //             gDefine.gDamageShow.CreateDamageShow(bloodSuck, gameObject.transform.parent.transform.position + Vector3.up * 3, Color.green, false);
        //             RefreshHPUI();
        //         }
        //     }

        // }
        // RefreshHPUI();
    }

    public void RefreshHPUI()
    {
        mHpText.text = gDefine.gPlayerData.mHp.ToString();
        mHpImage.transform.localScale = new Vector3(gDefine.gPlayerData.mHp / (float)gDefine.gPlayerData.mHpMax, 1, 1);
    }

    public void Btn_Down(bool IsRight)
    {
        if (mCurActState == eState.SkillReadyKill_Readying ||
            mCurActState == eState.SkillReadyKill_Ready)
            return;

        if (gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.BigSword_KillAll) == null)
            return;

        mNextActState = eState.Idle;
        mNextActFaceRight = IsRight;

        if (mCurActState == eState.Idle || mCurActState == eState.JumpBack)
        {
            mCurActState = eState.SkillReadyKill_Readying;
            mCurFaceRight = IsRight;
            mMoveTarget = transform.position;
            SetCurAct();
        }
        else
        {
            mNextActFaceRight = IsRight;
            SetNextActState(eState.SkillReadyKill_Readying);

        }
    }




    public void Btn_Click(bool IsRight)
    {
        if (mCurActState == eState.SkillReadyKill_Readying)
            return;

        if (mNextActState >= eState.SkillReadyKill_Readying)
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
            else if (mTargetObj != null)
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
        if (mTargetObj == null)
            return 10000000;
        else
        {
            if ((mCurActState >= eState.Atk0 && mCurActState <= eState.Atk3) || mCurActState == eState.Fly)
                return Mathf.Abs(mMoveTarget.x - mTargetObj.GetPos().x);
            else
                return Mathf.Abs(transform.position.x - mTargetObj.GetPos().x);
        }
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

        if (mCurActState == eState.SkillReadyKill_Readying)
        {
            mMoveTarget = transform.position;
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

                    if (mCurActState == eState.SkillSword_Lun)
                    {
                        mMoveTarget = transform.position;
                    }
                    else if (l < mAtkL)
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

                    if (mCurActState == eState.SkillSword_Lun)
                    {
                        mMoveTarget = transform.position;
                    }
                    else
                    {
                        mMoveTarget = (mCurFaceRight == true) ?
                       transform.position + GetAtkMoveL() * Vector3.right
                    :
                        transform.position + GetAtkMoveL() * Vector3.left;
                    }


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
        switch (mCurActState)
        {
            case eState.Idle:
                mAnimator.Play("BigSword_idle");
                break;
            case eState.Atk0:
                mAnimator.Play("atk0");
                break;
            case eState.Atk1:
                mAnimator.Play("atk1");
                break;
            case eState.Atk2:
                mAnimator.Play("atk2");
                break;
            case eState.Atk3:
                mAnimator.Play("atk3");
                break;
            case eState.Fly:
                mAnimator.Play("BigSword_fly");
                break;
            case eState.JumpBack:
                mAnimator.Play("BigSword_JumpBack");
                break;
            case eState.SkillReadyKill_Readying:
                mAnimator.Play("BigSword_Skill_ReadyKill");
                break;
            case eState.SkillSword_Flush:
                mAnimator.Play("BigSword_Skill_Flush");
                break;

            case eState.SkillSword_Lun:
                mAnimator.Play("BigSword_Skill_BigLun");
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

            case eState.SkillSword_Lun:
                return mSkillLunMoveL;


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

            case eState.SkillSword_Lun:
                return mSkillLunMoveV;

            case eState.SkillBackFlash:
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
        if (mSkillFlushIsReady)
        {
            mSkillFlushIsReady = false;
            return eState.SkillSword_Flush;
        }

        if (mSkillBigLunIsReady)
        {
            mSkillBigLunIsReady = false;
            return eState.SkillSword_Lun;
        }



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

        if (!gDefine.gPause)
        {
            UpdateSkill();
            UpdateLogic();
            UpdateMove(Time.deltaTime);
        }


    }

    void UpdateSkill()
    {
        if (gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.BigSword_SwordRain) != null)
        {
            mSkillRainT -= Time.deltaTime;
            if (mSkillRainT < 0)
            {
                mSkillRainT = 20;

                GameObject o = GameObject.Instantiate(mRefSkill_Rain_Preb);

                o.GetComponent<se_skillRain>().Init((int)gDefine.gPlayerData.mDamage);

                gDefine.gDamageShow.CreateDamageShow("剑雨", transform.position + Vector3.up * 3, new Color(0.9f, 0.5f, 0.9f, 1));

            }
        }

        if (gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.BigSword_SwordFlush) != null)
        {
            mSkillFlushT -= Time.deltaTime;
            if (mSkillFlushT < 0)
            {
                mSkillFlushT = 20;
                mSkillFlushIsReady = true;
            }
        }

        if (gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.BigSword_BigLun) != null)
        {
            mSkillBigLunT -= Time.deltaTime;
            if (mSkillBigLunT < 0)
            {
                mSkillBigLunT = 20;
                mSkillBigLunIsReady = true;
            }
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

    }


    void UpdateMove(float T)
    {
        //如果当前没到位，则移动
        if ((mCurActState >= eState.Atk0 && mCurActState <= eState.JumpBack)
            || mCurActState == eState.Fly || mCurActState == eState.SkillSword_Lun
            || mCurActState == eState.SkillBackFlash
           )
        {
            float moveV = GetMoveV();

            //gameObject.transform.pasrent.transform.position =
            Vector3 newPos = Vector3.MoveTowards(transform.position, mMoveTarget, moveV * Time.deltaTime);
            Vector3 deltPos = newPos - transform.position;
            transform.Translate(deltPos, Space.World);

            gDefine.CalcSceMove(deltPos);

            if (mCurActState == eState.Fly && Mathf.Abs(transform.position.x - mMoveTarget.x) < 0.001f)
            {
                mNextActState = eState.Atk0;
                mNextActFaceRight = mCurFaceRight;
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
                RefreshHPUI();
            }
        }
    }

    public void EventSkill_ReadyKillReady()
    {
        mCurActState = eState.SkillReadyKill_Ready;
    }

    public void Btn_Up(bool IsRight)
    {
        if (mCurActState == eState.SkillReadyKill_Readying || mCurActState == eState.SkillReadyKill_Ready)
        {
            if (IsRight != mCurFaceRight)
                return;
        }

        if (mCurActState == eState.SkillReadyKill_Ready)
        {
            mCurActState = eState.SkillReadyKill_Atk;
            mAnimator.Play("BigSword_ReadyKill_Atk");

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

    public void EventSkill_ReadyKill_KillEveryThing()
    {
        GameObject o = GameObject.Instantiate(mRefReadyKill_KillAll_Preb);
        o.transform.position = transform.position + Vector3.up * 2;
        o.GetComponent<se_readyKill>().Init((int)gDefine.gPlayerData.mDamage);
    }

    public void EventSkill_Rain()
    {

    }

    public void EventSkill_Flush()
    {
        GameObject o = GameObject.Instantiate(mRefSkill_Flush_Preb);
        //o.transform.position = transform.position + Vector3.up * 2;
        o.GetComponent<se_skill_Sword_Flush>().Init(transform.position, mCurFaceRight,
            15, (int)gDefine.gPlayerData.mDamage);

        gDefine.gDamageShow.CreateDamageShow("剑气斩", transform.position + Vector3.up * 3, new Color(0.9f, 0.5f, 0.9f, 1));

    }

    public void EventSkill_BigLunAtk()
    {
        Vector3 pos = transform.position;
        pos.x -= 1.5f;
        CNpcInst[] npc = gDefine.gNpc.DoDamageLR(pos, 3, (int)gDefine.gPlayerData.mDamage, false, CNpcInst.eNpcClass.OnGround, true);
        if (npc != null)
        {
            for (int i = 0; i < npc.Length; i++)
            {
                int bloodSuck = gDefine.gPlayerData.GetBloodSuckHp();
                if (bloodSuck > 0)
                {
                    gDefine.gPlayerData.mHp += bloodSuck;
                    if (gDefine.gPlayerData.mHp > gDefine.gPlayerData.mHpMax)
                        gDefine.gPlayerData.mHp = gDefine.gPlayerData.mHpMax;

                    gDefine.gDamageShow.CreateDamageShow(bloodSuck, gameObject.transform.parent.transform.position + Vector3.up * 3, Color.green, false);
                    RefreshHPUI();
                }
            }

        }
        RefreshHPUI();
    }

    public void EventSkill_Lun_BeginMove()
    {
        mMoveTarget = mCurFaceRight ? transform.position + mSkillLunMoveL * Vector3.right :
            transform.position + mSkillLunMoveL * Vector3.left;
    }

    public void EventSkill_BackFlashAtk()
    {
        // CSkill skill = gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.BackFlash);

        // CNpcInst[] arr = gDefine.gNpc.DoDamageLineWithDeadPrec(skill.mParamFloat, mMoveTarget.x,
        //             (int)gDefine.gPlayerData.mDamage * 2, 10, CNpcInst.eNpcClass.OnGround);

        // for (int i = 0; i < arr.Length; i++)
        // {
        //     // GameObject mingZhongSE = GameObject.Instantiate(mRefMingzhongSe);
        //     //  mingZhongSE.transform.position = transform.position +
        //     //  ((mCurFaceRight) ? new Vector3(2.2f, 1.5f, -0.5f) : new Vector3(-2.2f, 1.5f, -0.5f));

        //     int bloodSuck = gDefine.gPlayerData.GetBloodSuckHp();
        //     if (bloodSuck > 0)
        //     {
        //         gDefine.gPlayerData.mHp += bloodSuck;
        //         if (gDefine.gPlayerData.mHp > gDefine.gPlayerData.mHpMax)
        //             gDefine.gPlayerData.mHp = gDefine.gPlayerData.mHpMax;

        //         gDefine.gDamageShow.CreateDamageShow(bloodSuck, gameObject.transform.parent.transform.position + Vector3.up * 3, Color.green, false);

        //     }

        //     if (Random.Range(0, 100) < 20)
        //     {
        //         arr[i].BeKilled();
        //     }
        // }
        // RefreshHPUI();

    }

    public void BeginSkillBackFlash()
    {
        mCurFaceRight = !mCurFaceRight;

        mCurActState = eState.SkillBackFlash;
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
