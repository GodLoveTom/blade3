using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class SpriteActNode
{
    public enum eEvent
    {
        Null=0,
        Atk,  //攻击
        DAtk, //双击
        HAtk, //重击
        LRAtk, //左右同时攻击
        Throw, //投掷
    }
    public Sprite mS; //精灵图片索引
    public Sprite mSE; //特效精灵图片索引
    public float mlastT=0.2f; //持续时间
    public eEvent mEvent;
}

[System.Serializable]
public class SpriteActNodeInst : SpriteActNode
{
    float mBT; //该帧的开始时间

    public float GetBT() { return mBT;  }

    public float Init(float T)
    {
        mBT = T;
        return mBT + mlastT;
    }
}


public class CSpriteActionEvent
{
    public SpriteActNode.eEvent mEvent;
    public float mT;
}

[System.Serializable]
public class SpriteAction
{
    public SpriteActNodeInst[] mArr; //动作的序列帧集合
    public bool mLoop = true; //该动作是否为循环动作
    float mActSumT = 0;
    List<CSpriteActionEvent> mEventArr = new List<CSpriteActionEvent>();
    List<CSpriteActionEvent> mDamageEventArr = new List<CSpriteActionEvent>();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<CSpriteActionEvent> GetDelayDamageEventArr()
    {
        return mDamageEventArr;
    }

    /// <summary>
    /// 初始化，为了更加便利计算，而对数据进行了优化计算
    /// </summary>
    public void Init()
    {
        float t = 0;
        if (mArr!=null)
        {
            for (int i = 0; i < mArr.Length; i++)
            {
                if (mArr[i].mEvent != SpriteActNode.eEvent.Null)
                {
                    CSpriteActionEvent e = new CSpriteActionEvent();
                    e.mEvent = mArr[i].mEvent;
                    e.mT = t;
                    mEventArr.Add(e);

                    if ( mArr[i].mEvent == SpriteActNode.eEvent.Atk  ||
                         mArr[i].mEvent == SpriteActNode.eEvent.DAtk ||
                         mArr[i].mEvent == SpriteActNode.eEvent.HAtk ||
                         mArr[i].mEvent == SpriteActNode.eEvent.LRAtk ||
                         mArr[i].mEvent == SpriteActNode.eEvent.Throw 
                         )
                        mDamageEventArr.Add(e);

                    
                }

                t = mArr[i].Init(t);
                mActSumT += mArr[i].mlastT;
               
            }
        }
    }


    /// <summary>
    /// 获取总的帧数
    /// </summary>
    /// <returns></returns>
    public int GetSumFrameNum()
    {
        return mArr.Length;
    }

    public float GetSumT()
    {
        return mActSumT;
    }

    /// <summary>
    /// 通过帧索引获得该动画帧
    /// </summary>
    /// <param name="FrameIndex"></param>
    /// <returns></returns>
    public SpriteActNodeInst GetFrame(int FrameIndex)
    {
        if (FrameIndex >= 0 && FrameIndex < mArr.Length)
            return mArr[FrameIndex];
        else
            return null;
    }
    /// <summary>
    /// 更新当前帧
    /// </summary>
    /// <param name="T">该动作开始后流逝的时间</param>
    /// <param name="FrameIndex">当前的帧索引</param>
    /// <param name="NextUpdateT">下一次回来更新的预估时间</param>
    /// <returns>true：更新成功，false 动作已经完成，需要切换到下一动作</returns>
    public bool UpdateT( ref float T, ref int FrameIndex, ref float NextUpdateT)
    {
        if(false==mLoop)
        {
            if(T >= mActSumT)
            {
                T -= mActSumT;
                return false;
            }
        }
      


        if (T < mArr[FrameIndex].GetBT() + mArr[FrameIndex].mlastT)
        {
            NextUpdateT = mArr[FrameIndex].GetBT() + mArr[FrameIndex].mlastT;
            return true;
        }
        //检查最快的两种情况，当前帧简单+1即可 
        else if ( FrameIndex + 1 < mArr.Length && T < mArr[FrameIndex+1].GetBT() + mArr[FrameIndex+1].mlastT)
        {
            
            NextUpdateT = mArr[FrameIndex + 1].GetBT() + mArr[FrameIndex + 1].mlastT;
            FrameIndex += 1;
            return true;
        }
        else //其他情况干脆重算
        {
            T = T % mActSumT;
            for(int i=0; i<mArr.Length; i++)
            {
                if (T < mArr[i].GetBT() + mArr[i].mlastT)
                {
                    FrameIndex = i;
                    NextUpdateT = mArr[i].GetBT() + mArr[i].mlastT;
                    break;
                }
            }
            return true;
        }
    }
}

public class CDelayDamage
{
    public bool mIsHeavy = false; //重击
    public bool mIsRL = false; //背后也会攻击
    public bool mIsThrow = false; //投掷
    public int mDamage; //伤害
    public long mDelayT; // 延迟
}


public class SpriteAct : MonoBehaviour
{
    //---渲染等逻辑----
    public enum ePCActType
    {
        Idle = 0, //发呆
        Atk0 = 1, //普攻1
        Atk1 = 2, //普攻2
        Atk2 = 3,//普攻3
        Atk3 = 4,//普攻4
        BackJump = 5,//后跳
        JumpFly=6,//跳起滑行
        JumpAtk=7, //跳起滑行后攻击
    }
    //0 idle
    //1 atk0
    //2 atk1
    //3 atk2
    //4 atk3
    //5 backjump
    public SpriteAction[] mAct; //所有的序列帧动画

    public float mCurActT=0; //当前动画时间

    public float mActSpeed = 1.5f;

    public ePCActType mCurAct = ePCActType.Idle; //当前动作

    public int mFrameIndex=0; //当前动作的帧数

    public ePCActType mNextAct = ePCActType.Idle;
    public bool mNextActFaceRight = true;

    public SpriteRenderer mRefSprite; //精灵图
    //public GameObject mRefSpriteObj;

    public SpriteRenderer mRefSESprite; //精灵特效图

    public bool mCurIsPause; //当前是否为暂停状态
    public float mNextActUpdateT;

    public bool mIsDamgeDone = false; //伤害是否触发

    //----操作逻辑----
    public bool mCurFaceRight = true;
    public Vector3 mAtkMoveTarget;
    public float mMoveV = 5;
    public float mDefaultAtkMoveL = 2.5f;

    public CNpcInst mTargetObj;

    //---场景----
    public GameObject mRefSceBackObj;
    public GameObject mRefSceFarFarObj;
    public GameObject mRefSceFarObj;

    public Vector3 mRefSceBackObjOriPos;
    public Vector3 mRefSceFarFarObjOriPos;
    public Vector3 mRefSceFarObjOriPos;

    //---属性----
    //public float mHp = 5000;
    //public float mHpMax = 5000;
    //public float mDamage = 3400;

    public Vector3 mBornPos;

    //--Hp UI---
    public Text mHpText;
    public Image mHpImage;
    public GameObject mUIRoot;

    //delay damage
    List<CDelayDamage> mDelayDamage = new List<CDelayDamage>();

    //命中特效
    public GameObject mRefMingzhongSe;

    //终结演出
    bool mIsInKilledPlay = false;

    public bool mIsPlayer3 = false;

    float mAtkActCoolDown = 0;
    ePCActType mOldAtkAct= ePCActType.Idle;

    public GameObject mRefFeiLunPreb; //飞轮perb

    public enum ePcWeapon
    {
        DSword=0,
        BigSword,
        FeiLun,
    }
    public ePcWeapon mCurWeapon = ePcWeapon.FeiLun;




    // Start is called before the first frame update
    void Start()
    {
        gDefine.gPlayerData. mHp = gDefine.gPlayerData. mHpMax = 20000;

        Init();
        mCurActT = 0;
        mCurIsPause = false;
        mFrameIndex = 0;
        mNextActUpdateT = 0;
        RefreshSprite();
        

        gDefine.gPcTrans = gameObject.transform.parent.transform;

        gDefine.gPc = this;

        mBornPos = gameObject.transform.parent.transform.position;

        mRefSceBackObjOriPos = mRefSceBackObj.transform.position;
        mRefSceFarFarObjOriPos = mRefSceFarFarObj.transform.position; 
        mRefSceFarObjOriPos = mRefSceFarObj.transform.position;

        mUIRoot.SetActive(false);
    }

    void Init()
    {
        if(mAct!=null)
        for(int i=0; i<mAct.Length; i++)
        {
            mAct[i].Init();
        }
        RefreshHPUI();
        CloseHPUI();
    }

    public void ShowHPUI()
    {
        mUIRoot.SetActive(true);
    }

    public void CloseHPUI()
    {
        mUIRoot.SetActive(false);
    }

    public void Show()
    {
        mRefSprite.gameObject.transform.parent.gameObject.SetActive(true);
    }

    public void BeAtk(int Damage, float DelayT=0)
    {
        /*if( DelayT > 0.001f  )
        {
            CDelayDamage d = new CDelayDamage();
            d.mDamage = Damage;
            d.mDelayT = DelayT;
            mDelayDamage.Add(d);
        }
        else
        {*/
        gDefine.gPlayerData. mHp -= Damage;
        gDefine.gDamageShow.CreateDamageShow(Damage, gameObject.transform.parent.transform.position + Vector3.up * 3, Color.red, false);
        
        if (gDefine.gPlayerData.mHp <= 0)
        {
            gDefine.gGameMainUI.ShowFailUI();
            gDefine.gPause = true;
        }

        RefreshHPUI();

    }

    //public void UpdateDelayDamage(float DeltT)
    //{
    //    for(int i=0; i<mDelayDamage.Count; i++)
    //    {
    //        mDelayDamage[i].mDelayT -= DeltT;
    //        if( mDelayDamage[i].mDelayT < 0 )
    //        {
    //            BeAtk(mDelayDamage[i].mDamage, 0);
    //            mDelayDamage.RemoveAt(i);
    //            i--;
    //        }
    //    }
    //}

    /// <summary>
    /// 恢复血量，当前的动作
    /// </summary>
    public void Relive()
    {
        gDefine.gPlayerData. mHp = gDefine.gPlayerData. mHpMax;
        mCurActT = 0;
        mCurIsPause = false;
        mFrameIndex = 0;
        mNextActUpdateT = 0;
        mCurAct = ePCActType.Idle;
        
        RefreshSprite();
        RefreshHPUI();
    }

    public void RefreshHPUI()
    {
        mHpText.text = gDefine.gPlayerData. mHp.ToString();
        mHpImage.transform.localScale= new Vector3(gDefine.gPlayerData. mHp / (float)gDefine.gPlayerData. mHpMax , 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (mIsInKilledPlay)
            return;

        if(!gDefine.gPause)
        {
            UpdateLogic();
            UpdateMove(Time.deltaTime);
            RefreshSprite();
            UpdateDelayDamage();
        }
    }

    void UpdateMove(float T)
    {
        //如果当前没到位，则移动
        if( ( mCurAct >= ePCActType.Atk0 && mCurAct <= ePCActType.BackJump )
            || mCurAct == ePCActType.JumpAtk || mCurAct ==ePCActType.JumpFly)
        {
            float moveV = CalcMoveV(T);

            //gameObject.transform.pasrent.transform.position =
            Vector3 newPos = Vector3.MoveTowards(gameObject.transform.parent.transform.position, mAtkMoveTarget, moveV * Time.deltaTime);
            newPos = newPos - gameObject.transform.parent.transform.position;
            gameObject.transform.parent.transform.Translate(newPos);

            gDefine.CalcSceMove(newPos);

            //Vector3 backPos = newPos * 0.75f;
            //mRefSceBackObj.transform.Translate(backPos);
            //Vector3 farfarPos = newPos * 0.38f;
            //mRefSceFarFarObj.transform.Translate(farfarPos);
            //Vector3 farPos = newPos * 0.3f;
            //mRefSceFarObj.transform.Translate(farPos);

        }
        
    }

    float CalcMoveV(float T)
    {
        //if (mCurAct == ePCActType.JumpFly)
        //{
        //    gDefine.gPlayerData.mTmpJumpFlyV += gDefine.gPlayerData.mJumpFlyAcc * T;
        //    return gDefine.gPlayerData.mTmpJumpFlyV;
        //} 
        //else if (mCurAct >= ePCActType.Atk0 && mCurAct <= ePCActType.Atk3)
        //    return gDefine.gPlayerData.mAtkMoveV;
        //else if (mCurAct == ePCActType.JumpAtk)
        //    return gDefine.gPlayerData.mAtkMoveV;
        //else if (mCurAct == ePCActType.BackJump)
        //    return gDefine.gPlayerData.mJumpBackV;
        //else
            return 0;

    }


    void UpdateDelayDamage()
    {
        //如果位置到位，则计算伤害的触发。
        //如果没到位，等待。
        if( Mathf.Abs( gameObject.transform.parent.transform.position.x - mAtkMoveTarget.x) < 0.001f )
        {
            for( int i=0; i<mDelayDamage.Count; i++ )
            {
                if( System.DateTime.Now.Ticks >= mDelayDamage[i].mDelayT  )
                {
                    if( mDelayDamage[i].mIsThrow )
                    {
                        CreateFeiLun(mDelayDamage[i].mDamage);
                    }
                    else
                    {
                        //do damage..
                        bool faceRight = mCurFaceRight;
                       // float l = 2.5f;
                        if (mDelayDamage[i].mIsRL)
                        {
                            faceRight = !faceRight;
                            //l = 5;
                        }

                        // if (gDefine.gNpc.DoDamage(mRefSprite.transform.parent.transform.position,
                        //     l, faceRight, mDelayDamage[i].mDamage, mDelayDamage[i].mIsHeavy,false,
                        //     CNpcInst.eNpcClass.OnGround))
                        // {
                        //     GameObject mingZhongSE = GameObject.Instantiate(mRefMingzhongSe);
                        //     mingZhongSE.transform.position = gameObject.transform.parent.transform.position +
                        //        ((mCurFaceRight) ? new Vector3(2.2f, 1.5f, -0.5f) : new Vector3(-2.2f, 1.5f, -0.5f));

                        //     int bloodSuck = gDefine.gPlayerData.GetBloodSuckHp();
                        //     if (bloodSuck > 0)
                        //     {
                        //         gDefine.gPlayerData.mHp += bloodSuck;
                        //         if (gDefine.gPlayerData.mHp > gDefine.gPlayerData.mHpMax)
                        //             gDefine.gPlayerData.mHp = gDefine.gPlayerData.mHpMax;

                        //         gDefine.gDamageShow.CreateDamageShow(bloodSuck, gameObject.transform.parent.transform.position + Vector3.up * 3, Color.green, false);
                        //         RefreshHPUI();
                        //     }
                        // }
                    }

                    mDelayDamage.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    public void CreateFeiLun(int Damage)
    {
        GameObject o = GameObject.Instantiate(mRefFeiLunPreb);
        se_feilun script = o.GetComponent<se_feilun>();
        script.Init(mRefSprite.gameObject.transform.position, 6,  mCurFaceRight, Damage);
    }

    float CalcFeiLunDamageL()
    {
        return 4;
    }

    public void DoFeilunDamage(Vector3 Pos, int Damage)
    {
        // Vector3 pos = Pos;
        // pos.x -= CalcFeiLunDamageL() * 0.5f;
        // CNpcInst[] npc = gDefine.gNpc.DoDamageLR(Pos, CalcFeiLunDamageL(), Damage, false);
        // if(npc != null)
        // {
        //     for(int i=0; i<npc.Length; i++)
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
    }


    void UpdateLogic()
    {
        if (mCurIsPause || mIsInKilledPlay )
            return;

        mAtkActCoolDown -= Time.deltaTime;

        if ( mTargetObj != null && !mTargetObj.IsLive())
        {
            mTargetObj = null;
        }

        if (mCurActT + Time.deltaTime* mActSpeed < mNextActUpdateT)
        {
            mCurActT += Time.deltaTime* mActSpeed;
            return;
        }
        else
            mCurActT += Time.deltaTime* mActSpeed; 
          
        //先算第几个循环
        if ( !mAct[(int)mCurAct].UpdateT(ref mCurActT,ref mFrameIndex, ref mNextActUpdateT) )
        {
            mNextActUpdateT = 0.003f;
            //到位置才能切换下一个动作
            if ( Mathf.Abs( mAtkMoveTarget.x - gameObject.transform.parent.transform.position.x)<0.001f)
            {
                if( mTargetObj != null && mTargetObj.IsLive() )
                {

                    mNextAct = ePCActType.Idle;
                    ChangeToNextActNow();

                    //gDefine.UseLongGunGirlNow( mRefSprite.gameObject.transform.parent.transform.position,
                    //  mCurFaceRight,1);

                    gDefine.UseShootGirlNow( mRefSprite.gameObject.transform.parent.transform.position,
                      mCurFaceRight,1);

                    mRefSprite.gameObject.transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    //该动作执行完毕需要切换到下一个接续动作
                    if (!ChangeToNextActNow())
                    {
                        //切换向下一个接续动作 失败或者时间过长，跳过了这个动作
                        mNextAct = ePCActType.Idle;
                        ChangeToNextActNow();
                    }

                }




            }

            //动作做完了。如果是攻击动作，计算伤害
            /*if(mCurAct >= ePCActType.Atk0 && mCurAct <= ePCActType.Atk3 )
            {
                //
                if(!mIsDamgeDone)
                {
                    if (gDefine.gNpc.DoDamage(mRefSprite.transform.parent.transform.position,
                    3, mCurFaceRight, (int)mDamage))
                    {
                        GameObject mingZhongSE = GameObject.Instantiate(mRefMingzhongSe);
                        mingZhongSE.transform.position = gameObject.transform.parent.transform.position +
                           ((mCurFaceRight) ? new Vector3(2.2f, 1.5f, -0.5f) : new Vector3(-2.2f, 1.5f, -0.5f));
                    }
                    mIsDamgeDone = true;
                    //
                
                }
                
            }*/

                 //if( !BeginKilledPlay() )
                 //{
                 //    //该动作执行完毕需要切换到下一个接续动作
                 //    if (!ChangeToNextActNow())
                 //    {
                 //        //切换向下一个接续动作 失败或者时间过长，跳过了这个动作
                 //        mNextAct = ePCActType.Idle;
                 //        ChangeToNextActNow();
                 //    }
                 //}


        }

    }

    void RefreshSprite()
    {
        SpriteActNodeInst node = mAct[(int)mCurAct].GetFrame(mFrameIndex);
        mRefSprite.sprite = node.mS;
        mRefSprite.flipX = mCurFaceRight ? false : true;


        if (node.mSE == null)
            mRefSESprite.gameObject.SetActive(false);
        else
        {
            mRefSESprite.sprite = node.mSE;
            mRefSESprite.gameObject.SetActive(true);
            mRefSESprite.flipX = mCurFaceRight ? false : true;
        }


       // if( mCurAct == ePCActType.BackJump)
       // {
           mRefSprite.gameObject.transform.localScale = new Vector3(5.529f, 5.529f, 1);
           mRefSESprite.gameObject.transform.localScale = new Vector3(5.529f, 5.529f, 1);
       // }
       // else
       // {
       //    mRefSprite.gameObject.transform.localScale = Vector3.one;
        //    mRefSESprite.gameObject.transform.localScale = Vector3.one;
       // }
            
    }

    /// <summary>
    /// 立即处理下一个动作跳转
    /// </summary>
    /// <returns></returns>
    // bool ChangeToNextActNow_old()
    // {
    // //     mCurActT = 0;
    // //     mNextActUpdateT = 0;
    // //     mFrameIndex = 0;
    // //    /* if (!mAct[(int)mNextAct].UpdateT(ref mCurActT, ref mFrameIndex, ref mNextActUpdateT))
    // //     {
    // //         //已经超过了新动作时间
    // //         mNextAct = ePCActType.Idle;
    // //         return false;
    // //     }
    // //     else*/
    // //     {
    // //         mCurAct = mNextAct;
    // //         mNextAct = ePCActType.Idle;

    // //         mCurFaceRight = mNextActFaceRight; //设置朝向

    // //         if (mCurAct>=ePCActType.Atk0 && mCurAct<=ePCActType.Atk3)
    // //         {
    // //             mIsDamgeDone = false;
    // //             //先查有攻击目标否，有目标，则选取目标为移动位置。
    // //             CNpcInst targetObj = gDefine.gNpc.FindByL(gameObject.transform.position.x, mCurFaceRight, 8); 
    // //             if( targetObj != null )
    // //             {
    // //                 mTargetObj = targetObj;

    // //                 float l = Mathf.Abs( targetObj.mRefNpcUI.transform.position.x - gameObject.transform.parent.transform.position.x );
    // //                 if( l > 1 )
    // //                 {

    // //                     mAtkMoveTarget = (mCurFaceRight == true) ?
    // //                          targetObj.mRefNpcUI.transform.position - Vector3.right*2
    // //                :
    // //                          targetObj.mRefNpcUI.transform.position + Vector3.right*2;
    // //                 }
    // //             }
    // //             else
    // //             {
    // //                 //若没有目标，则为当前默认的移动
    // //                 mAtkMoveTarget = (mCurFaceRight == true) ?
    // //                     gameObject.transform.parent.transform.position + mDefaultAtkMoveL * Vector3.right
    // //                 :
    // //                     gameObject.transform.parent.transform.position - mDefaultAtkMoveL * Vector3.right;
    // //             }
    // //         }
    // //         else if(mCurAct == ePCActType.BackJump)
    // //         {
    // //             mAtkMoveTarget = (mCurFaceRight == true) ?
    // //                     gameObject.transform.parent.transform.position - mDefaultAtkMoveL * Vector3.right*1.5f
    // //                 :
    // //                     gameObject.transform.parent.transform.position + mDefaultAtkMoveL * Vector3.right*1.5f;

    // //         }
    // //         return true;
    // //     }
            
    // }


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


        //mCurActT = 0;
        //mNextActUpdateT = 0;
        //mFrameIndex = 0;

        //ePCActType oldAct = mCurAct; //

        //mCurAct = mNextAct;
        //mNextAct = (mCurAct == ePCActType.JumpFly)? ePCActType.JumpAtk: ePCActType.Idle;
        //mCurFaceRight = mNextActFaceRight; //设置朝向

       

        //if (mCurAct == ePCActType.BackJump)
        //{
        //    mAtkMoveTarget = (mCurFaceRight == true) ?
        //            gameObject.transform.parent.transform.position + gDefine.gPlayerData.GetPcJumpL() * Vector3.left
        //        :
        //            gameObject.transform.parent.transform.position + gDefine.gPlayerData.GetPcJumpL() * Vector3.right;

        //}
        //else
        //{
            

        //    if( mTargetObj != null )
        //    {
        //        float l = CalcTargetL();

        //        //再确认下位置,位置不够的情况下还是要改成攻击前进，或者是攻击
        //        if (mCurAct == ePCActType.JumpFly && (l <= gDefine.gPlayerData.GetPcMoveAtkL() || l > gDefine.gPlayerData.GetPcJumpAtkL()))
        //            mCurAct = ePCActType.Atk0;

        //        if (mCurAct == ePCActType.JumpFly)
        //        {

        //            gDefine.gPlayerData.ResetJumpFlyV();
        //            mAtkMoveTarget =( (mCurFaceRight == true) ?
        //                        mTargetObj.mRefNpcUI.transform.position + gDefine.gPlayerData.GetPcToNpcL() * Vector3.left
        //            :
        //                        mTargetObj.mRefNpcUI.transform.position + gDefine.gPlayerData.GetPcToNpcL() * Vector3.right);
        //        }

        //        if ((mCurAct >= ePCActType.Atk0 && mCurAct <= ePCActType.Atk3) || mCurAct == ePCActType.JumpAtk)
        //        {
        //            //此时重算一个攻击动作
        //            mCurAct = GetNextAtkAct();

        //            if (l < gDefine.gPlayerData.GetPcAtkL())
        //            {
        //                //mAtkMoveTarget = mTargetObj.mRefNpcUI.transform.position;
        //                CalcCurActDelayDamage();
        //            }
        //            else if (l < gDefine.gPlayerData.GetPcMoveAtkL())
        //            {
        //                mAtkMoveTarget = (mCurFaceRight == true) ?
        //                    mTargetObj.mRefNpcUI.transform.position + gDefine.gPlayerData.GetPcToNpcL() * Vector3.left
        //                :
        //                    mTargetObj.mRefNpcUI.transform.position + gDefine.gPlayerData.GetPcToNpcL() * Vector3.right;
        //                CalcCurActDelayDamage();
        //            }
        //            else
        //            {
        //                //攻击前进
        //                //若没有目标，则为当前默认的移动
        //                mCurAct = GetNextAtkAct();
        //                float actT = mAct[(int)mCurAct].GetSumT();
        //                actT -= 0.1f;
        //                actT = actT / mActSpeed;

        //                mAtkMoveTarget = (mCurFaceRight == true) ?
        //                    gameObject.transform.parent.transform.position + gDefine.gPlayerData.GetDefaultAtkMoveL(actT) * Vector3.right
        //                :
        //                    gameObject.transform.parent.transform.position + gDefine.gPlayerData.GetDefaultAtkMoveL(actT) * Vector3.left;
        //            }
        //        }


        //    }
        //    else
        //    {
        //        if (mCurAct == ePCActType.JumpFly)
        //                mCurAct = ePCActType.Atk0;

        //        if ((mCurAct >= ePCActType.Atk0 && mCurAct <= ePCActType.Atk3) || mCurAct == ePCActType.JumpAtk)
        //        {

        //            mCurAct = GetNextAtkAct();

                  
        //            float actT = mAct[(int)mCurAct].GetSumT();
                   
        //            actT -= 0.1f;

        //            actT = actT / mActSpeed;

        //            mAtkMoveTarget = (mCurFaceRight == true) ?
        //             gameObject.transform.parent.transform.position + gDefine.gPlayerData.GetDefaultAtkMoveL(actT) * Vector3.right
        //         :
        //             gameObject.transform.parent.transform.position + gDefine.gPlayerData.GetDefaultAtkMoveL(actT) * Vector3.left;

        //            CalcCurActDelayDamage();
        //        }
        //    }

        //}

        //if (mCurAct == ePCActType.Idle)
        //    mAtkActCoolDown = 0.3f;
        //else
        //    mAtkActCoolDown = 10000;

        return true;
    }

    ePCActType GetNextAtkAct()
    {
        if (mAtkActCoolDown <= 0)
            mOldAtkAct = ePCActType.Atk0;
        else
        {
            mOldAtkAct += 1;
            if (mOldAtkAct == ePCActType.BackJump)
                mOldAtkAct = ePCActType.Atk0;
        }

        return mOldAtkAct;
    }

    void CalcCurActDelayDamage()
    { 

        List<CSpriteActionEvent> e = mAct[(int)mCurAct].GetDelayDamageEventArr(); 
        for(int i=0; i<e.Count; i++)
        {
            CDelayDamage d = new CDelayDamage();

            d.mDelayT = (long)(e[i].mT / mActSpeed * 10000000) + System.DateTime.Now.Ticks;

           

            if (e[i].mEvent == SpriteActNode.eEvent.HAtk)
            {
                d.mDamage = (int)(gDefine.gPlayerData.mDamage * 2);
                d.mIsHeavy = true;
            }
            else if(e[i].mEvent == SpriteActNode.eEvent.Throw)
            {
                d.mIsThrow = true;
                d.mDamage = (int)gDefine.gPlayerData.mDamage;
            }
            else
                d.mDamage = (int)gDefine.gPlayerData.mDamage;

            mDelayDamage.Add(d);

            if( e[i].mEvent == SpriteActNode.eEvent.DAtk)
            {
                CDelayDamage d1 = new CDelayDamage();
                d1.mDelayT = (long)(e[i].mT / mActSpeed * 10000000) + System.DateTime.Now.Ticks;
                if (e[i].mEvent == SpriteActNode.eEvent.HAtk)
                {
                    d1.mDamage = (int)(gDefine.gPlayerData.mDamage * 2);
                    d1.mIsHeavy = true;
                } 
                else
                {
                    d1.mDamage = (int)gDefine.gPlayerData.mDamage;
                }
                   
                mDelayDamage.Add(d1);
            }
            else if (e[i].mEvent == SpriteActNode.eEvent.LRAtk)
            {
                CDelayDamage d1 = new CDelayDamage();
                d1.mDelayT = (long)(e[i].mT / mActSpeed * 10000000) + System.DateTime.Now.Ticks;
                d1.mIsRL = true;
                d1.mDamage = (int)gDefine.gPlayerData.mDamage;
                mDelayDamage.Add(d1);
            }
        }
    }


    public void BtnUp(bool IsRight)
    {
        if (mIsInKilledPlay)
            return;

        if(mIsPlayer3)
        {
            BtnUp3(IsRight);
        }
        else
        {
            if (IsRight == mCurFaceRight)
            {
                //按钮与当前方向同，则继续攻击
                if (mCurAct == ePCActType.Idle || mCurAct == ePCActType.BackJump)
                {
                    //此时立刻切换攻击动作
                    mNextAct = ePCActType.Atk0;
                    //mCurActT = 0;
                    ChangeToNextActNow();
                }
                else if (mCurAct >= ePCActType.Atk0 && mCurAct < ePCActType.Atk3)
                {
                    mNextAct = (ePCActType)((int)mCurAct + 1);
                    if (mNextAct >= ePCActType.BackJump)
                        mNextAct = ePCActType.Atk0;
                    //攻击动作中，不打断当前的攻击动作，等待攻击动作结束的时候自然的跳转
                    //ChangeToNextAct();
                }
            }
            else
            {
                //if (mCurAct == ePCActType.Idle)
                if (mTargetObj == null)
                {
                    //此时立刻切换攻击动作,并换方向
                    mNextAct = ePCActType.Atk0;
                    mNextActFaceRight = IsRight;
                    //mCurActT = 0;
                    ChangeToNextActNow();
                    //mCurFaceRight = IsRight;
                }
                else //if (mCurAct >= ePCActType.Atk0 && mCurAct <= ePCActType.Atk3)
                {
                    if (!IsEnamyBehind())
                    {
                        //身后没人
                        //攻击动作中，打断当前的攻击动作，切换成后跳
                        //此时并不改变朝向
                        mNextAct = ePCActType.BackJump;
                        mNextActFaceRight = mCurFaceRight;

                        if (mCurAct != ePCActType.BackJump)
                        {
                            ChangeToNextActNow();
                        }

                    }
                    else
                    {
                        //当前攻击完毕后，转向并跳向后方，攻击后面的敌人
                        mNextAct = (ePCActType)((int)mCurAct + 1);
                        mNextActFaceRight = IsRight;
                        if (mNextAct >= ePCActType.BackJump)
                            mNextAct = ePCActType.Atk0;

                        if (mCurAct == ePCActType.Idle || mCurAct == ePCActType.BackJump)
                        {
                            //此时立刻切换攻击动作
                            ChangeToNextActNow();
                        }
                    }
                }
            }
        }   
    }

    public void BtnUp3(bool IsRight)
    {
        //if (mIsInKilledPlay)
        //    return;

        //if (IsRight == mCurFaceRight)
        //{
        //    //按钮与当前方向同，则继续攻击

        //    //查看攻击范围内是否有目标
        //    //1
        //    //如果有目标，判断目标的远近。
        //    //--如果远，则跃飞过去攻击
        //    //-------
        //    //--如果近，则判断是否在可攻击范围内
        //    //------不在攻击范围内，于是1，2段攻击前行
        //    //------在范围内，则1，2，3，4交替攻击
        //    //2
        //    //如果目标不存在，则1，2攻击前进

        //    CheckTarget();
        //    if(mTargetObj != null)
        //    {
        //        float l = CalcTargetL();
               
        //        if(l > gDefine.gPlayerData.GetPcMoveAtkL() && l <= gDefine.gPlayerData.GetPcJumpAtkL())
        //        {
        //            mNextAct = ePCActType.JumpFly;
        //            if (mCurAct == ePCActType.Idle || mCurAct == ePCActType.BackJump)
        //            {
        //                ChangeToNextActNow();
        //            }
        //            //else if (mCurAct == ePCActType.JumpFly)
        //             //   mNextAct = ePCActType.JumpAtk;
        //            //else
        //             //   mNextAct = ePCActType.JumpAtk;
        //        }
        //        else if (l > gDefine.gPlayerData.GetPcAtkL() && l<= gDefine.gPlayerData.GetPcMoveAtkL())
        //        {
        //            if (mCurAct == ePCActType.Atk0)
        //                mNextAct = ePCActType.Atk1;
        //            else if (mCurAct == ePCActType.Atk1)
        //                mNextAct = ePCActType.Atk0;
        //            else
        //                mNextAct = ePCActType.Atk0;

        //            if (mCurAct == ePCActType.Idle || mCurAct == ePCActType.BackJump)
        //            {
        //                ChangeToNextActNow();
        //            }
        //        }
        //        else
        //        {
        //            if (mCurAct == ePCActType.Atk0)
        //                mNextAct = ePCActType.Atk1;
        //            else if (mCurAct == ePCActType.Atk1)
        //                mNextAct = ePCActType.Atk2;
        //            else if (mCurAct == ePCActType.Atk2)
        //                mNextAct = ePCActType.Atk3;
        //            else
        //                mNextAct = ePCActType.Atk0;

        //            if (mCurAct == ePCActType.Idle || mCurAct == ePCActType.BackJump)
        //            {
                        
        //                ChangeToNextActNow();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (mCurAct == ePCActType.Atk0)
        //            mNextAct = ePCActType.Atk1;
        //        else if (mCurAct == ePCActType.Atk1)
        //            mNextAct = ePCActType.Atk0;
        //        else
        //            mNextAct = ePCActType.Atk0;

        //        if (mCurAct == ePCActType.Idle || mCurAct == ePCActType.BackJump)
        //        {
        //            ChangeToNextActNow();
        //        }
        //    }
        //}
        //else
        //{
        //    //按钮与当前方向反，则攻击或后退
        //    //背后有人么？
        //    //---有，那么距离远么
        //    //------远，飞跃过去攻击
        //    //------近，开启atk0
        //    //---如果背后没有人，那么我当前方向上有目标么
        //    //-------有，向后跳跃
        //    //-------没有，调转方向，atk0，攻击前行

        //    CNpcInst inst = CheckBackTarget();

        //    if (  inst != null )
        //    {
        //        mTargetObj = inst;
        //        float l = CalcTargetL();
        //        if (l > gDefine.gPlayerData.GetPcMoveAtkL())
        //        {
        //            mNextAct = ePCActType.JumpFly;
        //            mNextActFaceRight = IsRight;
                   
        //        }
        //        else
        //        {
        //            mNextAct = ePCActType.Atk0;
        //            mNextActFaceRight = IsRight;
        //        }
        //        if (mCurAct == ePCActType.Idle || mCurAct == ePCActType.BackJump)
        //            ChangeToNextActNow();
        //    }
        //    else if(mTargetObj != null )
        //    {
        //        mNextAct = ePCActType.BackJump;
        //        mNextActFaceRight = mCurFaceRight;

        //        if (mCurAct != ePCActType.BackJump)
        //        {
        //            ChangeToNextActNow();
        //        }
        //    }
        //    else
        //    {
        //        mNextAct = ePCActType.Atk0;
        //        mNextActFaceRight = IsRight;
        //        if (mCurAct == ePCActType.Idle)
        //            ChangeToNextActNow();
        //    }
        //}    
    }


    public void BtnUp4(bool IsRight)
    {
        //if (mIsInKilledPlay)
        //    return;

        //if (IsRight == mCurFaceRight)
        //{
        //    //按钮与当前方向同，则继续攻击

        //    //查看攻击范围内是否有目标
        //    //1
        //    //如果有目标，判断目标的远近。
        //    //--如果远，则跃飞过去攻击
        //    //-------
        //    //--如果近，则判断是否在可攻击范围内
        //    //------不在攻击范围内，于是1，2段攻击前行
        //    //------在范围内，则1，2，3，4交替攻击
        //    //2
        //    //如果目标不存在，则1，2攻击前进

        //    CheckTarget();
        //    if (mTargetObj != null)
        //    {
        //        float l = CalcTargetL();

        //        if (l > gDefine.gPlayerData.GetPcMoveAtkL() && l <= gDefine.gPlayerData.GetPcJumpAtkL())
        //        {
        //            mNextAct = ePCActType.JumpFly;
        //            if (mCurAct == ePCActType.Idle || mCurAct == ePCActType.BackJump)
        //            {
        //                ChangeToNextActNow();
        //            }
        //            //else if (mCurAct == ePCActType.JumpFly)
        //            //   mNextAct = ePCActType.JumpAtk;
        //            //else
        //            //   mNextAct = ePCActType.JumpAtk;
        //        }
        //        else if (l > gDefine.gPlayerData.GetPcAtkL() && l <= gDefine.gPlayerData.GetPcMoveAtkL())
        //        {

        //            mNextAct = ePCActType.Atk0;
                    

        //            if (mCurAct == ePCActType.Idle || mCurAct == ePCActType.BackJump)
        //            {
        //                ChangeToNextActNow();
        //            }
        //        }
        //        else
        //        {
        //            mNextAct = ePCActType.Atk0;

        //            if (mCurAct == ePCActType.Idle || mCurAct == ePCActType.BackJump)
        //            {

        //                ChangeToNextActNow();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        mNextAct = ePCActType.Atk0;

        //        if (mCurAct == ePCActType.Idle || mCurAct == ePCActType.BackJump)
        //        {
        //            ChangeToNextActNow();
        //        }
        //    }
        //}
        //else
        //{
        //    //按钮与当前方向反，则攻击或后退
        //    //背后有人么？
        //    //---有，那么距离远么
        //    //------远，飞跃过去攻击
        //    //------近，开启atk0
        //    //---如果背后没有人，那么我当前方向上有目标么
        //    //-------有，向后跳跃
        //    //-------没有，调转方向，atk0，攻击前行

        //    CNpcInst inst = CheckBackTarget();

        //    if (inst != null)
        //    {
        //        mTargetObj = inst;
        //        float l = CalcTargetL();
        //        if (l > gDefine.gPlayerData.GetPcMoveAtkL())
        //        {
        //            mNextAct = ePCActType.JumpFly;
        //            mNextActFaceRight = IsRight;

        //        }
        //        else
        //        {
        //            mNextAct = ePCActType.Atk0;
        //            mNextActFaceRight = IsRight;
        //        }

        //        ChangeToNextActNow();
        //    }
        //    else if (mTargetObj != null)
        //    {
        //        mNextAct = ePCActType.BackJump;
        //        mNextActFaceRight = mCurFaceRight;

        //        if (mCurAct != ePCActType.BackJump)
        //        {
        //            ChangeToNextActNow();
        //        }
        //    }
        //    else
        //    {
        //        mNextAct = ePCActType.Atk0;
        //        mNextActFaceRight = IsRight;
        //        ChangeToNextActNow();
        //    }
        //}
    }

    public bool BeginKilledPlay()
    {
        Vector3 endPos = Vector3.zero;
        if (gDefine.gNpc.BeginKilledPlay(mRefSprite.transform.parent.transform.position, 3, mCurFaceRight, ref endPos))
        {
            mIsInKilledPlay = true;
            mRefSprite.color = new Color(1, 1, 1, 0);
            mRefSprite.transform.parent.transform.position = endPos;
            return true;
        }
        else
            return false;
    }

    public void EndKilledPlay()
    {
        mIsInKilledPlay = false;
        mRefSprite.color = new Color(1, 1, 1, 1);
        mCurAct = ePCActType.Idle;
        mCurActT = 0;
        mNextActUpdateT = 0;
        mFrameIndex = 0;
    }

    public void EndFiLunAct()
    {
        if (mCurAct == ePCActType.Atk3)
            ChangeToNextActNow();
    }

    bool IsEnamyBehind()
    {
        CNpcInst obj = gDefine.gNpc.FindByL(gameObject.transform.position.x, !mCurFaceRight, 8);
        if (obj != null)
            return true;
        else
            return false;
    }

    /// <summary>
    /// 查找面对的敌人
    /// </summary>
    /// <returns></returns>
    CNpcInst CheckTarget()
    {
        //if (mTargetObj != null)
            return mTargetObj;
        //else
        //{
        //    float l = gDefine.gPlayerData.GetPcJumpAtkL();
        //    CNpcInst obj = gDefine.gNpc.FindByL(gameObject.transform.parent.transform.position.x, mCurFaceRight, l);
        //    mTargetObj = obj;
        //    return mTargetObj;
        //}
        
    }


    /// <summary>
    /// 查身后的敌人
    /// </summary>
    /// <returns></returns>
    CNpcInst CheckBackTarget()
    {
        //float l = gDefine.gPlayerData.GetPcJumpAtkL();
        // CNpcInst obj = gDefine.gNpc.FindByL(gameObject.transform.position.x, !mCurFaceRight, l);
        // return obj;
        return null;
    }

    // float CalcTargetL()
    // {
    //     if (mTargetObj == null)
    //         return 10000000;
    //     else
    //     {
    //         if( (mCurAct >= ePCActType.Atk0 && mCurAct <= ePCActType.Atk3)|| mCurAct == ePCActType.JumpAtk )
    //             return Mathf.Abs(mAtkMoveTarget.x - mTargetObj.mRefNpcUI.gameObject.transform.position.x);
    //         else
    //             return Mathf.Abs(gameObject.transform.position.x - mTargetObj.mRefNpcUI.gameObject.transform.position.x);
    //     }
    // }

    public void ReStartGame()
    {
        Relive();
        gameObject.transform.parent.transform.position = mBornPos;
        mRefSceBackObj.transform.position = mRefSceBackObjOriPos;
        mRefSceFarFarObj.transform.position = mRefSceFarFarObjOriPos;
        mRefSceFarObj.transform.position = mRefSceFarObjOriPos;
        CloseHPUI();
    }

   



}
