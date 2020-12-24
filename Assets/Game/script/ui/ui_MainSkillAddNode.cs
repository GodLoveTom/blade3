using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_MainSkillAddNode : MonoBehaviour
{ 
    public Image mIcon;
    public Text mNameText;
    public Text mDesText;
    public Text mPointText;
    public Text mNeedMoneyText;
    public Text mNeedScrollText;
    public Image mBtnImage;
    public Text mBtnTip;

    public RectTransform mTipPos;


    CSkillAddData mAddData;

    public GameObject [] mUpConditionX;

    ui_MainSkillAdd mUIRoot;

    public void Init(CSkillAddData AddData, ui_MainSkillAdd UISkillAdd)
    {
        mUIRoot = UISkillAdd;
        mAddData = AddData;
        Refresh();
    }

    void Refresh()
    {
        mIcon.sprite = gDefine.gABLoad.GetSprite("icon.bytes" , mAddData.mIconName );

        string str = mAddData.GetNameLocal();
        mNameText.text = str ;//+ "(" + mAddData.mLearnNum.ToString()+"/3)";

        //str = gDefine.gMyStr.Get(mAddData.mDes, gDefine.gPlayerData.mLanguageType);
         str = mAddData.GetDesStrLocal(); // str; //+" ("+ mTalent.GetValueStr()    +")";

        int value = mAddData.GetSkillAddUIValue();

        str = str.Replace("X",value.ToString());

        mDesText.text = str;
    

        str = gDefine.gMyStr.Get("等级", gDefine.gPlayerData.mLanguageType);
        mPointText.text = str+" " + mAddData.mLearnNum.ToString();  // +  .ToString();
       
        if(mAddData.mLearnNum < 3 )
        {
            int needCrystal = mAddData.LVLUPNeedCrystal();
            int needPiece = mAddData.LVLUPNeedPiece();

            //mBtnImage.color = new Color(1,0.55f,0,1);
            mNeedMoneyText.text = needCrystal.ToString() ;

            CGird pieceGird = gDefine.gPlayerData.FindGridByItemId( mAddData.mPieceItemId);
            int pieceNum = (pieceGird==null)?0:pieceGird.mNum;

            mNeedScrollText.text =  pieceNum.ToString() +"/"+ needPiece.ToString();
            int error = 0;
            //if( mAddData.CanLvLUp(out error) )
            {
                mBtnImage.color = Color.white;
            }  
            // else
            // {
            //     mBtnImage.color = Color.gray;
            // }
            str = gDefine.gMyStr.Get("升    级", gDefine.gPlayerData.mLanguageType);
            mBtnTip.text = str;

            for(int i=0; i<mUpConditionX.Length; i++)
                mUpConditionX[i].SetActive(true);
            mNeedMoneyText.gameObject.SetActive(true);
            mNeedScrollText.gameObject.SetActive(true);
        }
        else
        {
              for(int i=0; i<mUpConditionX.Length; i++)
                mUpConditionX[i].SetActive(false);
            mNeedMoneyText.gameObject.SetActive(false);
            mNeedScrollText.gameObject.SetActive(false);

            mBtnImage.color = new Color(0.66f,0.46f,0.22f,1);
            mBtnTip.text = gDefine.GetStr(462);
        }
    }

    public void ADCallBack()
    {
        Refresh();
    }

    public void CallBackFunc(bool Finished)
    {

    }

    public void Btn_UpLvL()
    {
        int error = 0;
        if( mAddData.CanLvLUp( out error ) )
        {
            mAddData.LVLUp();
            Refresh();
            gDefine.gPlayerData.Save();
            gDefine.PlaySound(78);

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("成功", "good"); // 在注册环节的每一步完成时，以步骤名作为value传送数据
            dic.Add("名字", mAddData.GetNameLocal()); 
            TalkingDataGA.OnEvent("点击技能升级", dic);

            gDefine.gAd.PlayInterAD(CallBackFunc);

            // Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Clear();
            dic.Add("来源", "点击技能升级"); // 在注册环节的每一步完成时，以步骤名作为value传送数据
            TalkingDataGA.OnEvent("插屏广告", dic);

        }
        else
        {
            if(error == 1)
            {
                //水晶不足
                //  gDefine.ShowTip( gDefine.GetStr(159));
                gDefine.ShowLackItTip(202,100, Refresh);
                 Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("失败", "缺水晶"); // 在注册环节的每一步完成时，以步骤名作为value传送数据
                dic.Add("名字", mAddData.GetNameLocal()); // 在注册环节的每一步完成时，以步骤名作为value传送数据
                TalkingDataGA.OnEvent("点击技能升级", dic);
            }
              
            else if(error == 2)
            {
                gDefine.ShowLackItTip( mAddData.mPieceItemId ,1, Refresh);
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("失败", "缺碎片"); // 在注册环节的每一步完成时，以步骤名作为value传送数据
                dic.Add("名字", mAddData.GetNameLocal()); // 在注册环节的每一步完成时，以步骤名作为value传送数据
                TalkingDataGA.OnEvent("点击技能升级", dic);
            }
                //gDefine.ShowTip(gDefine.GetStr(302));
            gDefine.PlaySound(71);
        }
      
    }

    public void Btn_ShowTip()
    {
        CSkill SkillData = gDefine.gSkill.GetSkillByName(mAddData.mName);
        if(SkillData !=null)
            mUIRoot.ShowSkillTip(mTipPos, SkillData);
        gDefine.PlaySound(74);

    }
}
