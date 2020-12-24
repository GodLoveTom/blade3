using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_pause : MonoBehaviour
{
    public ui_PauseSkillTip mRefTip;
    public Text mTextTip;
    public Text mPauseTip;
    public GameObject mNodePreb;
    public RectTransform mNodeRoot;

    public RectTransform mCtlUp;
    public RectTransform mCtlDown;

    public RectTransform mCtlList;

    List<ui_pauseNode> mNodeArr = new List<ui_pauseNode>();
    public GameObject mRoot;
    public Image mSound;
    public Image mMusic;

    public Sprite [] mSoundSprite;
    public Sprite [] mMusicSprite;

    public GameObject mBtnContinue;
    public GameObject mBtnMusic;
    public GameObject mBtnSound;
    public GameObject mBtnQuit;

    public void Show()
    {
        //gDefine.RecalcAutoSize(mRoot);
        gDefine.gPause = true;
        gameObject.SetActive(true);
        Refresh();
        CalcSize();
        gDefine.gAd.PlayInterAD(null);
         
         Dictionary<string, object> dic = new Dictionary<string, object>();      
         dic.Clear();
                dic.Add("来源", "暂停");
                TalkingDataGA.OnEvent("插屏广告", dic);

                 Text [] textArr = gameObject.transform.GetComponentsInChildren<Text>(true);
        foreach(Text _t in textArr)
            gDefine.ResetFontBold(_t);

    }

   

    public void Refresh()
    {
        ClearAllNode();
        mTextTip.text = gDefine.GetStr("已学习能力");
        mPauseTip.text = gDefine.GetStr("暂停"); 
        CSkill [] skillArr = new CSkill[4];
        int skillIndex = 0;
        for(int i=0; i<(int)CSkill.eSkill.Count; i++)
        {
            CSkill skill = gDefine.gPlayerData.FindSkillInLearn((CSkill.eSkill)i);
            if( skill != null )
            {
                skillArr[skillIndex++] = skill;
                if(skillIndex == 4)
                {
                    skillIndex = 0;
                    RefreshNode(skillArr);
                    skillArr = new CSkill[4];
                }
            }  
        }
        if(skillIndex > 0 )
            RefreshNode(skillArr);

         mSound.sprite = (gDefine.gPlayerData.mSoundIsOpen) ? mSoundSprite[0] : mSoundSprite[1];
        mMusic.sprite = (gDefine.gPlayerData.mMusicIsOpen) ? mMusicSprite[0] : mMusicSprite[1];

    }

    public void CalcSize()
    {
        float perc = gDefine.RecalcUIScale();
        mRoot.GetComponent<RectTransform>().sizeDelta = new Vector2( 1080*perc, Screen.height);

        mCtlList.sizeDelta = new Vector2(0, Screen.height - mCtlDown.sizeDelta.y - mCtlUp.sizeDelta.y);
        mCtlList.localPosition = new Vector3(0, - mCtlUp.sizeDelta.y);
    }

    void ClearAllNode()
    {
        for(int i=0; i<mNodeArr.Count; i++)
        {
            mNodeArr[i].gameObject.transform.SetParent(null);
            mNodeArr[i].gameObject.SetActive(false);
            GameObject.Destroy(mNodeArr[i].gameObject);
        }
        mNodeArr.Clear();
    }

    void RefreshNode(CSkill [] SkillArr)
    {
        GameObject ob= GameObject.Instantiate(mNodePreb);
        
        ui_pauseNode script = ob.GetComponent<ui_pauseNode>();
        
        script.Refresh(SkillArr, this);

        ob.transform.SetParent(mNodeRoot);

        mNodeArr.Add(script);

    }

    public void Btn_ContinueCallBack()
    {
        gameObject.SetActive(false);
        gDefine.gPause = false;
    } 

    public void Btn_Continue()
    {
        gDefine.gBtnAnim.Init(mBtnContinue, 1, Btn_ContinueCallBack);
    }

     public void Btn_GotoMainUICallBack()
    {
         GameObject[] arr = GameObject.FindGameObjectsWithTag("it");
            for (int i = 0; i < arr.Length; i++)
                GameObject.Destroy(arr[i]);

            arr = GameObject.FindGameObjectsWithTag("it1");
            for (int i = 0; i < arr.Length; i++)
                GameObject.Destroy(arr[i]);

            arr = GameObject.FindGameObjectsWithTag("pet");
            for (int i = 0; i < arr.Length; i++)
                GameObject.Destroy(arr[i]);
        gameObject.SetActive(false);
        gDefine.GoToMainUI();

        PlayerPrefs.SetInt("continueFight",0);
    }

    public void Btn_GotoMainUI()
    {
         gDefine.gBtnAnim.Init(mBtnQuit,1, Btn_GotoMainUICallBack);
         TDGAMission.OnFailed("Chap_" + gDefine.gChapterId.ToString(), "user");
         TDGAMission.OnFailed(gDefine.gWaveStr,"user");
    }

public void Btn_SoundCallBack()
    {
        gDefine.gPlayerData.mSoundIsOpen = !gDefine.gPlayerData.mSoundIsOpen;
        gDefine.gSound.EnableSound(gDefine.gPlayerData.mSoundIsOpen);
        mMusic.sprite = (gDefine.gPlayerData.mSoundIsOpen) ? mMusicSprite[0] : mMusicSprite[1];
        gDefine.gPlayerData.Save();
    }
    public void Btn_Sound()
    {
        gDefine.gBtnAnim.Init(mBtnMusic, 1,Btn_SoundCallBack );
    }

    public void Btn_MusicCallBack()
    {
        gDefine.gPlayerData.mMusicIsOpen = !gDefine.gPlayerData.mMusicIsOpen;
        gDefine.gSound.EnableMusic(gDefine.gPlayerData.mMusicIsOpen);
        mSound.sprite = (gDefine.gPlayerData.mMusicIsOpen) ? mSoundSprite[0] : mSoundSprite[1];
        gDefine.gPlayerData.Save();
    }

    public void Btn_Music()
    {
        gDefine.gBtnAnim.Init(mBtnSound,1, Btn_MusicCallBack);
    }
}
