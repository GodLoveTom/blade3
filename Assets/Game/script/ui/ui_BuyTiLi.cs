using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui_BuyTiLi : MonoBehaviour
{
    public int mFromWhere;
    public GameObject[] mBtnArr = new GameObject[2];

    ui_ClickAnim[] mClickAnim = new ui_ClickAnim[2];
    float[] mDelayT = new float[2] { 0, 0 };



    public void Show(int FromWhere)
    {
        gDefine.RecalcAutoSize(gameObject);
        gameObject.SetActive(true);
        mFromWhere = FromWhere;
    }

    public void Btn_Crystal()
    {
        if (mClickAnim[1] == null)
            mClickAnim[1] = new ui_ClickAnim();

        mClickAnim[1].Init(mBtnArr[1], 1);

        mDelayT[1] = 0.2f;
    }

    public void Btn_Close()
    {
        gameObject.SetActive(false);
        switch (mFromWhere)
        {
            case 0:
                gDefine.gMainUI.ChangeToShop();
                break;
            case 1:
                gDefine.gMainUI.ChangeToEquipent();
                break;
            case 2:
                gDefine.gMainUI.ChangeToFight();
                break;
            case 3:
                gDefine.gMainUI.ChangeToTalent();
                break;
            case 4:
                gDefine.gMainUI.ChangeToKillAdd();
                break;
        }
    }

    public void Btn_AD()
    {

        if (mClickAnim[0] == null)
            mClickAnim[0] = new ui_ClickAnim();

        mClickAnim[0].Init(mBtnArr[0], 1);

        mDelayT[0] = 0.2f;
    }

    public void Update()
    {
        for (int i = 0; i < mClickAnim.Length; i++)
            if (mClickAnim[i] != null)
                mClickAnim[i].Update();

        for (int i = 0; i < mDelayT.Length; i++)
        {
            if (mDelayT[i] > 0)
            {
                mDelayT[i] -= Time.deltaTime;
                if (mDelayT[i] < 0)
                {
                    if (i == 0)
                    {
                        //gDefine.gPlayerData.TiLI += 5;
                        gDefine.gAd.PlayADVideo(ADCallBack);
                         Dictionary<string, object> dic = new Dictionary<string, object>();
            
            dic.Add("来源", "购买体力"); 
            

            TalkingDataGA.OnEvent("激励视频广告", dic);
                    }
                    else
                    {
                        if (gDefine.gPlayerData.Crystal >= 400)
                        {
                            gDefine.gPlayerData.TiLI += 20;
                            gDefine.gPlayerData.Crystal -= 400;
                        }
                    }
                }
            }
        }
    }

    public void ADCallBack(bool Finished)
    {
        gDefine.gPlayerData.TiLI += 20;
    }
}
