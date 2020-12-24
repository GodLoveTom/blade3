using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcMono : MonoBehaviour
{
    public GameObject mQuitDestoryObj;
    public GameObject mSELockObj;
    public NpcFlashSE mRefFlash;
    public se_CurseMask mRefMask;
    Animator mAnimator;
    public Text mHpText;

    public Image mHpImage;
    public GameObject mRoot;
    public GameObject mLeftPos;
    public GameObject mRightPos;
    public GameObject mThrowPoint;
    [Header("中心参考点")]
    public GameObject mRefMidPoint;
    [Header("伤害参考点")]
    public GameObject mRefDamagePoint;
    [Header("半死下身位置参考点")]
    public GameObject mRefDHDPoint;
    [Header("半死上身位置参考点")]
    public GameObject mRefDHUPoint;

    [Header("最远攻击距离")]

    [Header("--------数据--------")]
    public float mAtkL = 2.0f;
    [Header("最佳攻击距离")]
    public float mBestAtkL = 1.8f;
    [Header("最近攻击距离")]
    public float mMinAtkL = 1.0f;
    [Header("移动速度")]
    public float mV = 4;

    [Header("攻击速度--多少秒攻击一次")]
    public float mAtkV = 4;
    float mLastAtkT = 0; //上次发动攻击时间，npc的动作动画回调函数来负责更新攻击时间

    [Header("死亡动画数量")]
    public int mDieNum = 1;

    [Header("投掷物品/死亡投掷物品")]
    public GameObject mThrowItemPreb;

    [Header("是否为空中飞行怪物 true 飞行怪物")]
    public bool mIsInAir = false;

    [Header("是否为地面排队")]
    public bool mIsGroundTeam = false;

    [Header("是否为空中排队")]
    public bool mIsAirTeam = false;

    [Header("是否可被击退击飞")]
    public bool mCanKickBackAndKickFly = true;

    [Header("是否为Boss")]
    public bool mIsBoss = false;

    [Header("是否有攻击前提示")]
    public bool mHasAtkTip = false;
    [Header("攻击提示时间")]
    public float mShowAtkTipT = 2;

    [Header("攻击提示是否显示")]
    public bool mShowAtkTipSE = true;

    [Header("攻击提示是否追随玩家位置")]
    public bool mIsAtkTipFollowPc = false;
    [Header("攻击位置位置参考点")]
    public GameObject mRefAtkPoint;

    [Header("不攻击时是否前后游荡")]
    public bool mHasIdleMove = false;

    [Header("是否有被斩杀一半的死亡")]
    public bool mHasHalfDie = false;

    [Header("死亡延迟时间")]
    public float mDyingDelayT = 3f;
    [Header("坠落死亡脚本")]
    public NpcAirDropAndBomb mAirDropDie;

    [Header("空中怪死亡剥落组件")]
    public GameObject[] mDiedPartArr;

    public CNpcInst mNpcInst;
    [Header("是否美术资源为朝右的")]
    public bool mOriDirIsRight = false;

    public enum eActEvent
    {
        Null = 0,
        Destory,
    }

    public delegate void ActEventCallBack(eActEvent E);

    public ActEventCallBack mEventCallBackFunc;

    public void Event_CallBack(eActEvent E)
    {
        if (mEventCallBackFunc != null)
            mEventCallBackFunc(E);
    }

    public void BeAtk()
    {
        if (mRefMask != null)
            mRefMask.Cancel();
    }

    void Start()
    {

    }
    /// <summary>
    /// 当前时间是否已经可以进行攻击
    /// </summary>
    /// <returns></returns>
    public bool IsTimeReadyToAtk()
    {
        if (Time.time > mLastAtkT + 5)
            return true;
        else
            return false;
    }

    public void Event_PlaySound(int SoundId)
    {
        if (SoundId == 16 || SoundId == 15)
        {
            int[] s = new int[] { 15, 16, 32, 33, 34 };
            SoundId = s[Random.Range(0, s.Length)];
        }

        AudioClip clip = gDefine.gData.GetSoundClip(SoundId);
        if (clip != null)
            gDefine.gSound.Play(clip);
    }

    public void CreateDiedPart()
    {
        if (mDiedPartArr != null)
        {
            for (int i = 0; i < mDiedPartArr.Length; i++)
            {
                GameObject o = GameObject.Instantiate(mDiedPartArr[i]);
                o.transform.position = transform.position;
                NpcAirFlowerDropAndBomb s = o.GetComponent<NpcAirFlowerDropAndBomb>();
                s.Init();
            }
        }
    }

    void Event_GoldDrop()
    {
        int num = Random.Range(3, 5);
        for (int i = 0; i < num; i++)
        {
            GameObject obj = GameObject.Instantiate(gDefine.gData.mGlodCaoCoinDropSEPreb);
            se_itemDrop script = obj.GetComponent<se_itemDrop>();
            script.Init();
            obj.transform.position = mRefMidPoint.transform.position;
        }
    }

    public void Event_Vibrate()
    {
        gDefine.PlayVibrate();
    }

    public void SetNpcInst(CNpcInst Inst)
    {
        mNpcInst = Inst;
        mAnimator = GetComponent<Animator>();
    }

    public void Refresh(float Hp, float MaxHp)
    {
        if (mHpText == null)
            return;
        if (Hp < 100000)
            mHpText.text = ((int)Hp).ToString();
        else
        {
            int v = (int)(Hp) / 1000;
            mHpText.text = v.ToString() + "k";
        }

        if (Hp <= 0)
            mHpImage.transform.localScale = new Vector3(0, 1, 1);
        else
            mHpImage.transform.localScale = new Vector3(Hp / MaxHp, 1, 1);

    }

    public void CloseUI()
    {
        //mRoot?.SetActive(false);
    }

    public Vector3 GetDamageShowPos()
    {
        if (mRefDamagePoint != null)
            return mRefDamagePoint.transform.position;
        else
            return Vector3.zero;
    }

    public void Play(string ActName, bool RandomT = false)
    {
       // Debug.Log("ActName:" + ActName);
        if (mNpcInst.mNpcType == npcdata.eNpcType.HalfDown && ActName == "die")
        {
            int kkke = 1;
        }

        if (mNpcInst.mHp <= 0 && ActName == "idle")
        {
            int yyy = 1;
        }
        //Debug.Log("----" + ActName);
        mAnimator.Play(ActName, 0, RandomT ? Random.Range(0.0f, 1.0f) : 0);
        if (mRefFlash != null)
            mRefFlash.SetColor(Color.black);
    }

    /// <summary>
    /// npc的动作动画回调函数来负责更新攻击时间
    /// </summary>
    public void Event_RefreshAtkT()
    {
        mLastAtkT = Time.time;
    }
    public void Pause()
    {
        //mAnimator.speed = 0;
    }

    public void Continue()
    {
        mAnimator.speed = 1;
    }

    public void SetPos(Vector3 Pos)
    {
        transform.position = Pos;
    }

    public void Destory()
    {
        gameObject.SetActive(false);
        GameObject.Destroy(gameObject);

        mNpcInst.mCurState = CNpcInst.eState.Died;

        if (mQuitDestoryObj != null)
            GameObject.Destroy(mQuitDestoryObj);
    }

    public void Event_TeachPause()
    {
        if (gDefine.gLogic.mTeach.mBossShouldPause && gDefine.gLogic.mTeach.mIsInTeach)
        {
            mAnimator.speed = 0;
            gDefine.gLogic.mTeach.PlayerClick_AndNext();
        }

    }

    public void DestoryAndCreateHalf()
    {
        mNpcInst.mCanBeCount = false;

        if (Random.Range(0, 100) < 15 && Time.time > gDefine.gNpcHalfUPBronT)
        {
            gDefine.CreateHalfUp(this);
            gDefine.gNpcHalfUPBronT = Time.time + 10;
        }
        else
            gDefine.CreateHalfDown(this);

        gameObject.SetActive(false);
        GameObject.Destroy(gameObject);



    }

    public void Event_Skill(int CustomEventValue)
    {
        //if( mNpcInst.IsLive())
        mNpcInst.DoSkillEvent(CustomEventValue);
    }

    void Event_ActEnd(int Index)
    {
        if (Index == 1000)
        {
            mNpcInst.CloseAtkTip();
            mNpcInst.CloseBaBa();
            //攻击结束
            if (mHasIdleMove && mNpcInst.IsLive())
            {
                mNpcInst.ChangeToRandomMoveState();
                return;
            }
        }
        if (mNpcInst.IsLive())
        {
            mNpcInst.ChangeToStateIdle(mAtkV);
            Play("idle");
        }


    }

    void Event_Atk()
    {
        if (mNpcInst.IsLive())
            mNpcInst.AtkPc();
    }


    void Event_Throw()
    {
        GameObject o = GameObject.Instantiate(mThrowItemPreb);
        se_NpcSkill_Throw s = o.GetComponent<se_NpcSkill_Throw>();
        s.Init(mThrowPoint.transform.position, mNpcInst.GetThrowPos(), mNpcInst.GetDamage());
    }

    public void Event_AddBuff(int BuffType)
    {
        gDefine.PcAddBuff((CBuff.eBuff)BuffType, 2, 0);
    }

    void Event_DieThrow()
    {
        int num = Random.Range(3, 5);
        for (int i = 0; i < num; i++)
        {
            GameObject o = GameObject.Instantiate(gDefine.gData.mNpcRagePursuerBombPreb);
            Npc_SE_RagePursuerBomb s = o.GetComponent<Npc_SE_RagePursuerBomb>();
            Vector3 bPos = mRefMidPoint.transform.position;
            Vector3 ePos = bPos + Random.Range(-5.0f, 5.0f) * Vector3.right;
            ePos.y = gDefine.gGrounY;
            s.Init(bPos, ePos, 1.5f, mNpcInst.GetDamage());
        }
    }

    void Event_DieSE()
    {
        GameObject o = GameObject.Instantiate(gDefine.gData.mNpcRagePursuerBombFirePreb);
        o.transform.position = mRefMidPoint.transform.position;
    }

    public void FaceRight(bool FaceRight)
    {
        if (mOriDirIsRight == FaceRight)
        {
            transform.rotation = new Quaternion();
           // if (mRoot != null)
               // mRoot.transform.rotation = new Quaternion();
        }
        else
        //if (FaceRight)
        {
            transform.rotation = new Quaternion();
          //  if (mRoot != null)
              //  mRoot.transform.rotation = new Quaternion();
            //transform.Rotate(0, 180, 0, Space.World);
            // mRoot.transform.Rotate(0, 180, 0, Space.Self);


            //transform.rotation = Quaternion.Euler(0,Mathf.PI,0);
            //mRoot.transform.Rotate(0, 180, 0, Space.Self);
            //mRoot.transform.rotation = Quaternion.Euler(0,Mathf.PI,0);
            transform.Rotate(0, 180, 0, Space.World);
            //if (mRoot != null)
                //mRoot.transform.Rotate(0, 180, 0, Space.Self);
        }
    }
}
