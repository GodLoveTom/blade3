using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Char_ShortGun : MonoBehaviour
{
    Animator mAnimator;
    public Transform mRefPointL;
    public Transform mRefPointR;
    public GameObject mBulletPreb;
    bool mIsFaceR = true;
    bool mOriFaceR = true;

    float mT = 5;

    public bool mIsLongGunGirl;

    public Text mHpText;
    public Image mHpImage;
    public GameObject mHpRoot;

    [Header("射击时子弹壳位置引用")]
    public GameObject mBulletPos;
    [Header("子弹壳preb")]
    public GameObject mBulletShellPreb;

    Vector3 mCurAimPos;
    bool mIsShootAir = false;

    CNpcInst mShootAim;

    public class CAim
    {
        public CNpcInst Inst;
        public Vector3 pos;
    }
    List<CNpcInst> mShootAimArr = new List<CNpcInst>();
    //List<CAim> mAimArr = new List<CAim>();

    int mShootIndex = 0;
    int mShootCount = 7;

    //打断相关
    float mNoBreakT = 0; //不可打断时间
    float mFirstBtUpT = -1; //第一次按键抬起计时，连续两次点击，打断当前的射击

    float mFireT = 0;// 每0.25s 一枪
    //是否为影子
    bool mIsShadow = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!mIsShadow)
        {
            mAnimator = GetComponent<Animator>();
            SetStandAct(true);
            //gDefine.gPcTrans = gameObject.transform.parent.transform;

            gDefine.gPcShortGun = this;
            gameObject.SetActive(false);
        }
    }

    bool IsShootAimExist()
    {
        for (int i = 0; i < mShootAimArr.Count; i++)
        {
            if (mShootAimArr[i] == null || !mShootAimArr[i].IsLive())
            {
                mShootAimArr.RemoveAt(i);
                i--;
            }
        }

        if (mShootAimArr.Count == 0)
        {
            CNpcInst[] arr = gDefine.gNpc.OrderNpcByL(transform.position.x, 20f, CNpcInst.eNpcClass.All);
            mShootAimArr.Clear();
            for (int i = 0; i < arr.Length; i++)
                if (arr[i].mNpcType != npcdata.eNpcType.AirFlane)
                    mShootAimArr.Add(arr[i]);

            mShootIndex = 0;

        }
        return mShootAimArr.Count > 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (mShootCount <= 0)
        {
            gameObject.SetActive(false);

            if (mIsShadow)
                GameObject.Destroy(gameObject);
            else
                gDefine.EndUseGunGirl();
            //Debug.Log( "Shoot End......"+mShootCount.ToString());
            return;
        }

        IsShootAimExist();

        //查找最近的npc
        //CNpcInst npc = gDefine.gNpc.FindByR(transform.position.x , 7, CNpcInst.eNpcClass.All);


        CNpcInst npc = null;
        if (mShootAimArr.Count > 0)
        {
            mShootIndex = mShootIndex % mShootAimArr.Count;
            npc = mShootAimArr[mShootIndex];
            mIsFaceR = npc.GetPos().x > transform.position.x ? true : false;
        }

        if (npc != null)
        {
            //计算持枪角度
            string actName = CalcActNameByAirNpcAngle(npc);

            mAnimator.Play(actName);

            mShootAim = npc;

            mCurAimPos = npc.GetHitSEPos();
        }
        else
        {
            mCurAimPos = mRefPointR.transform.position + (mIsFaceR ? Vector3.right * 100 : Vector3.left * 100);
            mAnimator.Play("gun_fire");
            mIsShootAir = false;

            mShootAim = null;
        }

        transform.rotation = new Quaternion();
        mHpRoot.transform.rotation = new Quaternion();

        if (!mIsFaceR)
        {
            transform.Rotate(0, 180, 0, Space.World);
            mHpRoot.transform.Rotate(0, 180, 0, Space.Self);
        }

        UpdateFire();


    }

    public void Mouse_UpOrClick(bool IsRight)
    {
        // if (Time.time <= mNoBreakT)
        //     return;

        // if( IsRight != mOriFaceR)
        // {
        //     //break
        //     gameObject.SetActive(false);
        //     gDefine.EndUseGunGirl();
        //     gDefine.Btn_Click(IsRight);
        //     Debug.Log( "Shoot End......Opp  "+mShootCount.ToString());
        // }
    }

    public void Begin(Vector3 Pos, bool FaceR, float ExistT, bool IsShadow = false)
    {
        //建立npc列表（一人一枪）
        //动作，时间到了，按表攻击。 如果表中都不存在了。则平射。
        //子弹不依赖动作

        mAnimator = GetComponent<Animator>();

        mNoBreakT = Time.time + 0.5f;
        mFirstBtUpT = -1;

        mIsShadow = IsShadow;

        mIsFaceR = FaceR;
        mOriFaceR = FaceR;
        gameObject.SetActive(true);
        SetPosition(Pos);
        //SetAtkAct(FaceR);
        mT = ExistT;
        RefreshHPUI();

        CNpcInst[] arr = gDefine.gNpc.OrderNpcByL(transform.position.x, 7.5f, CNpcInst.eNpcClass.All);
        mShootAimArr.Clear();
        //mAimArr.Clear();
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i].mNpcType != npcdata.eNpcType.AirFlane)
            {
                mShootAimArr.Add(arr[i]);

                // CAim aim = new CAim();
                // aim.Inst = arr[i];
                // aim.pos = arr[i].GetHitSEPos();
                // mAimArr.Add(aim);
            }

        }

        mShootIndex = 0;

        CNpcInst firstAim = (arr.Length == 0) ? null : mShootAimArr[0];

        string actName = CalcActNameByAirNpcAngle(firstAim);
        mAnimator.Play(actName);

        transform.rotation = new Quaternion();
        mHpRoot.transform.rotation = new Quaternion();

        if (!FaceR)
        {
            transform.Rotate(0, 180, 0, Space.World);
            mHpRoot.transform.Rotate(0, 180, 0, Space.Self);
        }

        mShootCount = 7;
        mFireT = 0.15f;
    }

    public void Event_PlaySound(int SoundId)
    {
        //AudioClip clip = gDefine.gData.GetSoundClip(SoundId);
        //if (clip != null)
        // gDefine.gSound.Play(clip);
    }


    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    // public void SetAtkAct(bool IsFaceR)
    // {
    //     //查找最近的空中怪
    //     CNpcInst airNpc = gDefine.gNpc.FindAirByL(transform.position.x, mIsFaceR, 7);
    //     //查找最近的npc
    //     CNpcInst npc = gDefine.gNpc.FindByL(transform.position.x, mIsFaceR, 7);

    //     //如果npc，空中怪都存在，较高几率攻击空中怪，较低几率平打npc
    //     //空中怪计算夹角
    //     bool atkAir = false;

    //     if ((airNpc != null && npc != null && Random.Range(0, 100) < 85) || (airNpc != null && npc == null))
    //         atkAir = true;
    //     else
    //         atkAir = false;

    //     //设置方向
    //     if (atkAir)
    //     {

    //         string actName = CalcActNameByAirNpcAngle(airNpc);
    //         mAnimator.Play(actName);
    //         mIsShootAir = true;
    //         mCurAimPos = airNpc.GetHitSEPos();
    //     }
    //     else
    //     {
    //         mCurAimPos = mRefPointR.transform.position + (mIsFaceR ? Vector3.right * 100 : Vector3.left * 100);
    //         mAnimator.Play("gun_fire");
    //         mIsShootAir = false;
    //     }

    //     //if( IsFaceR )

    //     //else
    //     //    mAnimator.Play("gun_fireL");

    //     transform.rotation = new Quaternion();
    //     mHpRoot.transform.rotation = new Quaternion();

    //     if (!IsFaceR)
    //     {
    //         transform.Rotate(0, 180, 0, Space.World);
    //         mHpRoot.transform.Rotate(0, 180, 0, Space.Self);
    //     }
    // }

    string CalcActNameByAirNpcAngle(CNpcInst Npc)
    {
        if (Npc == null)
            return "gun_fire";

        Vector3 npcHitPos = Npc.GetHitSEPos();
        if (npcHitPos.y <= mRefPointR.transform.position.y)
            return "gun_fire";


        Vector3 dir = Npc.GetHitSEPos() - transform.position;
        float angle = Vector3.Angle(Vector3.right, dir);

        angle = angle > 90 ? 180 - angle : angle;

        if (angle <= 5)
            return "gun_fire";
        else if (angle <= 20)
            return "gun_fire15";
        else if (angle <= 40)
            return "gun_fire30";
        else if (angle <= 50)
            return "gun_fire45";
        else if (angle <= 65)
            return "gun_fire60";
        else if (angle <= 80)
            return "gun_fire75";
        else
            return "gun_fire90";


    }

    public void SetStandAct(bool IsFaceR)
    {
        mAnimator.Play("gun_stand");
        transform.rotation = new Quaternion();
        mHpRoot.transform.rotation = new Quaternion();

    }

    void BeginShoot()
    {
    }

    void UpdateFire()
    {
        if (Time.time > mFireT)
        {
            GameObject o = GameObject.Instantiate(mBulletPreb);

            Vector3 Pos = mRefPointR.position;

            bullet0 bullet = o.GetComponent<bullet0>();

            //if (mShootIndex < mShootAimArr.Count)
            if (mShootCount > 0)
            {
                if (mShootAimArr.Count > 0)
                    bullet.Init(Pos, mShootAimArr[mShootIndex], 30, (int)(gDefine.gPlayerData.mGunDamage), Vector3.zero);
                else
                {
                    Vector3 epos = mIsFaceR ? Pos + Vector3.right * 30 : Pos + Vector3.left * 30;
                    bullet.Init(Pos, null, 30, (int)(gDefine.gPlayerData.mGunDamage), epos);
                }

                // GameObject shell = GameObject.Instantiate(mBulletShellPreb);

                // float vx = Random.Range(3.0f, 5.0f) * 2;
                // float vy = Random.Range(4.0f, 8.0f) * 2;
                // if (mIsFaceR)
                //     vx = -vx;

                // shell.GetComponent<se_itemDrop>().Init(vx, vy);
                // shell.transform.position = mBulletPos.transform.position;
                mShootCount--;
                if (mShootCount > 0 && mShootAimArr.Count > 0)
                    mShootIndex = (mShootIndex + 1) % mShootAimArr.Count;


                if (Time.time - mFireT >= 0.1f)
                    gDefine.PlaySound(18);
                else
                {
                    gDefine.PlaySound(18, 0.1f - (Time.time - mFireT));
                }

                mFireT = Time.time + 0.15f;
            }
            // bullet.Init(Pos, mShootAimArr[mShootIndex].GetHitSEPos(), mIsFaceR, 30, (int)(gDefine.gPlayerData.mGunDamage*1.2f/2));
        }
    }

    public void RefreshHPUI()
    {
        mHpText.text = gDefine.gPlayerData.mHp.ToString();
        mHpImage.transform.localScale = new Vector3(gDefine.gPlayerData.mHp / (float)gDefine.gPlayerData.mHpMax, 1, 1);
    }



}
