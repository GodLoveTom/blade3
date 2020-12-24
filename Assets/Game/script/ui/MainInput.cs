using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainInput : MonoBehaviour
{
    public ui_Win mRefWinUI;
    public ui_fail mRefFailUI;
    public ui_pause mRefPauseUI;
    public ui_lvlTip mRefLVLTipUI;
    public ui_3Choose1InFight mRef3Choose1InFight;
    public ui_Magic mRefMagicUI;
    public ui_Adv mRefAdvUI;

    public ui_Teach mRefTeachUI;

    public ui_LVLChange mRefLvLChangeUI;
    public ui_LVLChange mRefLvLChangeUIFirst;

    public ui_Gun mRefGunUI;
    public se_beAtkFlashOnScreen mBeAtkUI;

    public GameObject mAutoObj;
    public GameObject mLvLObj;

    public Text mAutoText;  //自动
    public Text mLvLText;
    public Text mRemaindText;
    public Text mBeginText;

    public Button mLeftBtn;
    public Button mRightBtn;
    public Button mBeginTipBtn;

    public GameObject mRemainCtl;
    public GameObject mPauseBtn;

    float mRBtnDownBT = -1;
    float mLBtnDownBT = -1;
    [Header("点击消息的间隔时间")]
    public float mClickT = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        //mAutoText.text = gDefine.gIsAuto==true ? "自动\n开启" : "自动\n关闭";
        mLeftBtn.gameObject.SetActive(false);
        mRightBtn.gameObject.SetActive(false);

        gDefine.gGameMainUI = this;
        gDefine.gUIFail = mRefFailUI;

        ShowStartUI();
    }

    public void ShowTipTeachBtnClick()
    {
        mRefTeachUI.ShowRightBtn();
        mPauseBtn.SetActive(false);
        mRefGunUI.Show(false);
        mBeginTipBtn.gameObject.SetActive(false);
        mLeftBtn.gameObject.SetActive(true);
        mRightBtn.gameObject.SetActive(true);
    }

    public void ShowStoryUI()
    {
        mRefTeachUI.gameObject.SetActive(false);
        mPauseBtn.SetActive(false);
        mRefGunUI.Show(false);
        mBeginTipBtn.gameObject.SetActive(false);
        mLeftBtn.gameObject.SetActive(false);
        mRightBtn.gameObject.SetActive(false);
    }

    public void ShowTipTeachFireClick()
    {
        mRefTeachUI.ShowUpBtn();
        mPauseBtn.SetActive(false);
        mRefGunUI.Show(true);
        mBeginTipBtn.gameObject.SetActive(false);
    }

    public void EndTeach()
    {
        mBeginTipBtn.gameObject.SetActive(true);
        mLeftBtn.gameObject.SetActive(false);
        mRightBtn.gameObject.SetActive(false);
        mPauseBtn.SetActive(false);
        mRefTeachUI.gameObject.SetActive(false);
        mRefGunUI.Show(false);
    }

    public void Btn_UseGun()
    {
        gDefine.UseGunGirlNow();
    }

    public void ShowTeachBtnNull()
    {
        mRefTeachUI.ShowNullBtn();
    }

    public void CloseTeachBtn()
    {
        mRefTeachUI.gameObject.SetActive(false);
    }

    public void ShowTipTeachDodgeClick()
    {
        mRefTeachUI.ShowLeftBtn();
    }

    public void TeachNext()
    {
        gDefine.gLogic.mTeach.PlayerClick_AndNext();
    }

    private void Update()
    {
        if(mRBtnDownBT > 0 && Time.time > mRBtnDownBT+ mClickT)
        {
            if (!gDefine.IsUseGunGirl())
            {
                 gDefine.Btn_Down(true);
            }
               
        }

        if (mLBtnDownBT > 0 && Time.time > mLBtnDownBT + mClickT)
        {
            if (!gDefine.IsUseGunGirl())
                gDefine.Btn_Down(false);
        }
    }

    public void Show3Choose1()
    {
        mRef3Choose1InFight.Show();
    }

    public void ShowUIMagic()
    {
        mRefMagicUI.Show(gDefine.gLogic.mWaveLvL);
    }

    public void ShowUIAdv()
    {
        mRefAdvUI.Show();
    }

    public void ShowLvLUpTip(string Str)
    {
        mRefLVLTipUI.ShowTip(Str);
    }

    public void PlayLvLUpTip(bool IsNeedInsertOtherUI)
    {
        mRefLVLTipUI.NeedShowTwice(IsNeedInsertOtherUI);
        mRefLVLTipUI.Play();
    }

    public bool IsShowLvLUpTipFinish()
    {
        return !mRef3Choose1InFight.gameObject.activeSelf && 
        !mRefAdvUI.gameObject.activeSelf && !mRefMagicUI.gameObject.activeSelf;
    }

    public void BtnRight_Down()
    {
        mRBtnDownBT = Time.time;
    }

    public void BtnRight_Up()
    {   
        gDefine.gBtnRightDown = false;
        if (mRBtnDownBT > 0 )
        {
            if(Time.time > mRBtnDownBT + mClickT)
            {
                if (!gDefine.IsUseGunGirl())
                    gDefine.Btn_Up(true);
            }
            else
            {
                gDefine.Btn_Click(true);
            }
            
        }
        mRBtnDownBT = -1;
    }

    public void BtnRight_Click()
    {
       // if(!gDefine.IsUseGunGirl())
         //   gDefine.Btn_Click(true);
    }

    public void BtnLeft_Click()
    {
        //if (!gDefine.IsUseGunGirl())
         //   gDefine.Btn_Click(false);
    }

    public void BtnLeft_Up()
    {
        gDefine.gBtnLeftDown = false;
        if (mLBtnDownBT > 0 )
        {
            if(Time.time > mLBtnDownBT +mClickT )
            {
                if (!gDefine.IsUseGunGirl())
                    gDefine.Btn_Up(false);
            }
            else
            {
                gDefine.Btn_Click(false);
            }
        }

        mLBtnDownBT = -1;
    }

    public void BtnLeft_Down()
    {
        mLBtnDownBT = Time.time;
    }

    public void Btn_Auto()
    {
        //gDefine.gIsAuto = !gDefine.gIsAuto;
        //mAutoText.text = gDefine.gIsAuto==true ? "自动\n开启" : "自动\n关闭";

    }

    public void RefreshTipText(int LVL, int RemineNpc)
    {
        //mLvLText.text = "第"+LVL.ToString()+"关";
        mRemaindText.text = RemineNpc.ToString();
    }

    public void Btn_BeginGame()
    {
        mBeginTipBtn.gameObject.SetActive(false);
        //mRemainCtl.SetActive(true);
        mPauseBtn.SetActive(true);
        mRefGunUI.Show(true);
        mLeftBtn.gameObject.SetActive(true);
        mRightBtn.gameObject.SetActive(true);

        gDefine.BeginGame();

        PlayerPrefs.SetInt("continueFight",1);

        TDGAMission.OnBegin("Chap_" + gDefine.gChapterId.ToString());

        gDefine.gBoxData.FightActive();
    }

    public void ShowStartUI()
    {
        mAutoObj.SetActive(false);
        mLvLObj.SetActive(false);

        mLeftBtn.gameObject.SetActive(false);
        mRightBtn.gameObject.SetActive(false);
        mBeginTipBtn.gameObject.SetActive(true);
        mBeginText.text = gDefine.GetStr(357);
        mBeginText.fontSize = (int)(gDefine.RecalcUIScale() * 50);
        //gDefine.SetTextBold(mBeginText);

        
       // mRemainCtl.SetActive(false);
        mPauseBtn.SetActive(false);
        mRefGunUI.Show(false);
        mRefLvLChangeUI.gameObject.SetActive(false);
        mRefLvLChangeUIFirst.gameObject.SetActive(false);
        mRef3Choose1InFight.gameObject.SetActive(false);
        mRefAdvUI.gameObject.SetActive(false);
        mRefMagicUI.gameObject.SetActive(false);
        
    }

    public void ShowInGameUI()
    {
       // mAutoObj.SetActive(true);
       mLvLObj.SetActive(true);

        mLeftBtn.gameObject.SetActive(true);
        mRightBtn.gameObject.SetActive(true);
        mBeginTipBtn.gameObject.SetActive(false);
        //mRemainCtl.SetActive(true);
        mPauseBtn.SetActive(true);
        mRefGunUI.Show(true);

    }

    public void ShowFailUI()
    {
        mRefFailUI.Show();
    }





}
