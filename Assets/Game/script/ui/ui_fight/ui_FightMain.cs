using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_FightMain : MonoBehaviour
{
    public Text mCoinText;
    public Text mLVLText;
    public Text mEXPText;

    // Start is called before the first frame update
    void Start()
    {
        gDefine.gUITopInFight = this;
        Refresh();
    }

    public void Refresh()
    {
        mCoinText.text = gDefine.gPlayerData.Coin.ToString();
        //mLVLText.text = "LVL:" + gDefine.gPlayerData.mLVLInGame.ToString();
        //mEXPText.text = "EXP:" + gDefine.gPlayerData.mEXPInGame.ToString();
    }

    public void Pause()
    {
        gDefine.gPause = true;
        gDefine.gGameMainUI.mRefPauseUI.Show();
        gDefine.PlayUIClickSound();
    }
}
