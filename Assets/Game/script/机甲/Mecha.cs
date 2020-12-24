using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mecha : MonoBehaviour
{
    #region  状态机
    enum eState
    {
        Null,
        BornDrop,
        BornDropWait,
        BornOpen,
        Normal,
        Bomb,
        DieBomb,
    }

    eState mState = eState.Null;
    Animator mAnim;
    float mStateDropWaitT = 0;

    public int mDieBombNum;
    float mStateDieBombT;
    [Header("死亡爆炸特效")]
    public GameObject mDieBombSEPreb;
    [Header("机甲中心位置索引")]
    public GameObject mRefCenPos; 

    public void Init(Vector3 Pos, float LiveT)
    {
        mLiveT = LiveT;
        mLiveMaxT = LiveT;
        Pos.y = gDefine.gGrounY;
        mState = eState.BornDrop;
        transform.position = Pos + Vector3.up * 30;
        PlayAct("box");
    }

    #endregion
    #region  生命值
    //int mHp = 0;
    float mLiveT = 0;
    float mLiveMaxT = 1;
    public Image mHp; 

    public void UpdateLife()
    {
        float perc = mLiveT / mLiveMaxT;
        perc = Mathf.Clamp01(perc);
        mHp.fillAmount = perc;

        if(mLiveT>0)
        {
            mLiveT -= Time.deltaTime;
            if (mLiveT < 0)
            {
                mState = eState.DieBomb;
                mStateDieBombT = 0;
                mDieBombNum = 8;
                //gameObject.SetActive(false);
                //gDefine.EndUseMacheGirl();
            }
        }
    }

    public void BeAtk()
    {
        if (mLiveT > 0)
        {
            mLiveT -= 1;
            if (mLiveT <= 0)
            {
                mState = eState.DieBomb;
                mStateDieBombT = 0;
                mDieBombNum = 8;
                //gameObject.SetActive(false);
                //gDefine.EndUseMacheGirl();
            }
        }

    }


    #endregion

    #region  开火特效
    [Header("----开火特效----")]

    public SpriteRenderer mSEFireSprite;
    public Sprite[] mSEFireArr;
    int mSEFireIndex = 0;
    float mSEFireT = 0;
    bool mSEFireIsOn = false;

    void OpenFireSE()
    {
        mSEFireIsOn = true;
        mSEFireSprite.gameObject.SetActive(true);
    }

    void CloseFireSE()
    {
        mSEFireIsOn = false;
        mSEFireSprite.gameObject.SetActive(false);
    }

    void UpdateSEFire()
    {
        if (mSEFireIsOn)
        {
            if (Time.time > mSEFireT)
            {
                mSEFireT = Time.time + 0.05f;
                mSEFireIndex = (mSEFireIndex + 1) % mSEFireArr.Length;
                mSEFireSprite.sprite = mSEFireArr[mSEFireIndex];
            }
        }
    }

    #endregion
    #region  操控移动
    [Header("---操控移动---")]
    bool mIsFaceRight = true;
    public float mMoveV;
    public bool mRightIsDown = false;
    //float mRightDownT = 0;
    public bool mLeftIsDown = false;
    [Header("血条根")]
    public GameObject mHpRoot;


    //float mLeftIsDownT = 0;

    public void UpdateMove()
    {
        //if( mRightIsDown && Time.time > mRightDownT )
        //  mRightIsDown = false;
        //if( mRightIsDown && Time.time > mRightDownT )
        //mRightIsDown = false;

        bool isMove = false;
        if (mRightIsDown && !mLeftIsDown)
        {
            mIsFaceRight = true;
            isMove = true;
        }
        else if (!mRightIsDown && mLeftIsDown)
        {
            mIsFaceRight = false;
            isMove = true;
        }

        if (mIsFaceRight)
        {
            transform.rotation = new Quaternion();
            mHpRoot.transform.rotation = new Quaternion();
            PlayAct("move");
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            mHpRoot.transform.rotation = new Quaternion();
            mHpRoot.transform.Rotate(0, 180, 0, Space.Self);
            PlayAct("moveleft");
        }


        if (isMove)
        {
            float deltL = Time.deltaTime * mMoveV;
            Vector3 pos = transform.position;
            pos.x += mIsFaceRight ? deltL : -deltL;
            pos.y = gDefine.gGrounY;
            transform.position = pos;

            gDefine.PcSetPos(pos);

            mAnim.speed = 1;
        }
        else
        {
            mAnim.speed = 0;
        }
    }

    #endregion
    #region  自动开火管控
    [Header("---自动开火管控---")]
    public float mFireIntervalT = 0.1f;
    float mFireT = 0;
    public GameObject mSEFireHitPreb;
    public GameObject mBulletPreb;
    public GameObject mFireBulletRefPos;


    void UpdateAutoFire()
    {
        //寻找区域内地面敌人
        CNpcInst[] npcArr = gDefine.gNpc.FindByLine(transform.position.x, transform.position.x + (mIsFaceRight ? 8f : -8f), CNpcInst.eNpcClass.OnGround);
        if (npcArr.Length > 0)
        {
            OpenFireSE();
            //如果有自动开火并造成伤害
            if (Time.time > mFireT)
            {
                GameObject se = GameObject.Instantiate(mSEFireHitPreb);
                se.transform.position = npcArr[0].GetHitSEPos();

                se = GameObject.Instantiate(mSEFireHitPreb);
                se.transform.position = npcArr[0].GetHitSEPos() + Vector3.up * Random.Range(0.5f, 1.0f);
                se.transform.localScale = Vector3.one * 0.6f;

                se = GameObject.Instantiate(mSEFireHitPreb);
                se.transform.position = npcArr[0].GetHitSEPos() - Vector3.up * Random.Range(0.5f, 1.0f); ;
                se.transform.localScale = Vector3.one * 0.6f;

                npcArr[0].BeDamage((int)gDefine.gPlayerData.mGunDamage, false, false, false);

                GameObject bullet = GameObject.Instantiate(mBulletPreb);
                bullet.transform.position = mFireBulletRefPos.transform.position;

                mFireT = Time.time + mFireIntervalT;
            }
        }
        else
        {
            CloseFireSE();
        }
    }

    #endregion
    #region  火箭
    [Header("火箭初始位置索引")]
    public GameObject[] mMissleBPos;
    int mMissileBPosIndex = 0;
    CNpcInst[] mMissileAimList;
    int mMissileAimIndex = 0;
    int mMissleNum = 0;
    [Header("导弹发射间隔时间s")]
    public float mMissileIntervalT = 0.2f;
    float mMissileT = 0;
    [Header("机甲导弹preb")]
    public GameObject mMissilePreb;

    void UpdateFireMissile()
    {
        if (mMissleNum > 0 && Time.time > mMissileT)
        {
            GameObject m = GameObject.Instantiate(mMissilePreb);
            mMissileBPosIndex = mMissileBPosIndex % mMissleBPos.Length;
            m.transform.position = mMissleBPos[mMissileBPosIndex++].transform.position;

            MechaMissile s = m.GetComponent<MechaMissile>();

            s.Init((mMissileAimIndex < mMissileAimList.Length) ? mMissileAimList[mMissileAimIndex++] : null);

            mMissileT = Time.time + mMissileIntervalT;

            if (mMissleNum > 0 && mMissleNum - 1 == 0)
            {
                gDefine.gGameMainUI.mRefGunUI.ReSetGunCoolDown();
            }
            mMissleNum--;
        }
    }
    #endregion
    // Update is called once per frame
    void Update()
    {
        if (gDefine.gPause)
            return;

        if (mState == eState.BornDrop)
        {
            float y = transform.position.y - Time.deltaTime * 40f;
            Vector3 pos = transform.position;
            pos.y = y;
            transform.position = pos;
            if (y < gDefine.gGrounY)
            {
                y = gDefine.gGrounY;
                mState = eState.BornDropWait;
                //PlayAct("open");
                gDefine.PlayVibrate();
                mStateDropWaitT = 0.3f + Time.time;
            }
        }
        else if (mState == eState.BornDropWait)
        {
            if (Time.time > mStateDropWaitT)
            {
                mState = eState.BornOpen;
                PlayAct("open");
            }
        }
        else if (mState == eState.Normal)
        {
            this.UpdateMove();
            this.UpdateAutoFire();
            this.UpdateSEFire();
            this.UpdateFireMissile();
            this.UpdateLife();
        }
        else if(mState == eState.DieBomb)
        {
            if(mDieBombNum<0)
            {
                gameObject.SetActive(false);
                GameObject.Destroy(gameObject);
                gDefine.gMecha = null;
                gDefine.EndUseMacheGirl();
            }
            else if( Time.time > mStateDieBombT)
            {
                GameObject o = GameObject.Instantiate(mDieBombSEPreb);
                Vector3 pos = mRefCenPos.transform.position;
                pos.x += Random.Range(-1f,1f);
                pos.y += Random.Range(-2f,2f);
                o.transform.position  = pos;
                o.transform.localScale = Vector3.one *2;
                mStateDieBombT = Time.time + 0.15f;
                mDieBombNum --;
            }
        }
    }

    public void Event_OpenEnd()
    {
        mState = eState.Normal;
        PlayAct("move");
        mHpRoot.SetActive(true);
    }

    public void Event_BombEnd()
    {
        gameObject.SetActive(false);
        GameObject.Destroy(gameObject);
    }

    public void Event_FootDamage()
    {
        CNpcInst[] arr = gDefine.gNpc.FindHalfUpWithR(transform.position.x, 1.5f, CNpcInst.eNpcClass.All);
        foreach (var item in arr)
        {
            item.BeEndKill1();
        }
    }

    void PlayAct(string Act)
    {
        if (mAnim == null)
            mAnim = gameObject.GetComponent<Animator>();
        mAnim.Play(Act, 0);
    }

    public void UseMissile()
    {
        //find the targe....
        mMissileAimList = gDefine.gNpc.FindAllWithR(transform.position.x, 7, CNpcInst.eNpcClass.Air);
        //这个值大于0， 于是自动发射
        mMissleNum = Random.Range(4, 7);
        //mMissleNum = 1;
        mMissileT = 0;

        mMissileBPosIndex = 0;
        mMissileAimIndex = 0;
    }
}
