using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_Adv : MonoBehaviour
{
    public enum eAdvType
    {
        Hp15,
        Atk10,
        Hp15Del,
        RandomSkill,
        Atk30Del,
        Boss,
        Coin,
        Crystal,
        BigGun,
        Pet0,
        Pet1,
        Mecha,
        Count,
    }

    eAdvType mCurAdvType;

    public GameObject mAdvCtl;
    public GameObject mConfirmCtl;

    public Text mTip;
    public Text mDes;
    public Animator mAnim;

    bool mBoxIsOpen = false;

    public GameObject[] mBtnArr;
    public Text[] mBtnTextArr;
    //public GameObject mTreasurePlane;
    public Image mTreasureIcon;
    public GameObject mRoot;
    public GameObject mNameObj;

    int mItemNum = 0; //用来寄存金币，水晶数量

    int mLoopNum = 0;
    float mLoopT = 0;
    bool mIsLoop = false;
    bool mIsX10 = false;
    public void Show()
    {
        mBoxIsOpen = false;
        mConfirmCtl.SetActive(false);
        mAdvCtl.SetActive(true);
        gameObject.SetActive(true);
        RefreshClose();
        gDefine.RecalcAutoSize(mRoot);
        mNameObj.SetActive(true);
        mIsLoop = false;
        gDefine.gPause = true;

         Text [] textArr = gameObject.transform.GetComponentsInChildren<Text>(true);
        foreach(Text _t in textArr)
            gDefine.ResetFontBold(_t);
    }

    public void Btn_CloseCallBack()
    {
        gDefine.gPause = false;
        gameObject.SetActive(false);
        gDefine.gGameMainUI.mRefLVLTipUI.ContinueTask();
        
    }

    public void Btn_Close()
    {
        gDefine.gBtnAnim.Init(mBtnArr[1], 1, Btn_CloseCallBack);
        
    }

    public void Btn_OpenCallBack()
    {
        mAnim.Play("open", 0);
    }

    public void Btn_Open()
    {
        gDefine.gBtnAnim.Init(mBtnArr[0], 1, Btn_OpenCallBack);
        // mAnim.Play("open", 0);
        //mConfirmCtl.SetActive(true);
        //mAdvCtl.SetActive(false);
        //Refresh();
    }

    public void RefreshClose()
    {

        mAnim.Play("close", 0);
        mBtnArr[0].SetActive(true);
        mBtnArr[1].SetActive(true);
        mBtnArr[2].SetActive(false);
        mBtnArr[3].SetActive(false);
        mBtnArr[4].SetActive(false);
        mBtnArr[5].SetActive(false);
        //mTreasurePlane.SetActive(false);
        mBtnTextArr[0].text = gDefine.GetStr("打    开");
        mBtnTextArr[1].text = gDefine.GetStr("离    开");
        mTip.text = gDefine.GetStr("您获得了一个神秘宝箱");

    }

    void Refresh()
    {
        if (Random.Range(0, 100) < 33.3333f)
            mIsX10 = true;
        else
            mIsX10 = false;

        mIsX10 = true;

        mNameObj.SetActive(false);
        mConfirmCtl.SetActive(true);
        //  mAdvCtl.SetActive(false);

        mBtnArr[0].SetActive(false);
        mBtnArr[1].SetActive(false);

        mBtnArr[3].SetActive(false);
        mBtnArr[4].SetActive(false);
        mBtnArr[5].SetActive(false);
        if (mIsLoop)
        {
            mBtnArr[2].SetActive(false);
            mBtnArr[3].SetActive(false);
            mBtnArr[4].SetActive(false);
            mBtnArr[5].SetActive(false);

            mDes.gameObject.SetActive(false);
        }
        else
        {
            mBtnArr[2].SetActive(true);
            mDes.gameObject.SetActive(true);
            //mBtnArr[3].SetActive(true);
        }

        gDefine.PlaySound(71);

        mBtnTextArr[2].text = gDefine.GetStr("获    得");

        mTip.text = gDefine.GetStr("请选择一项附魔");

        List<int> tmpArr = new List<int>();

        if (PlayerPrefs.GetInt("IsFirstADV", 0) == 0 && !mIsLoop)
        {
            tmpArr.Add(9);
            tmpArr.Add(10);
            PlayerPrefs.SetInt("IsFirstADV", 1);
            PlayerPrefs.Save();
        }
        if (PlayerPrefs.GetInt("IsFirstMecha", 0) == 0 && !mIsLoop)
        {
            tmpArr.Add((int)eAdvType.Mecha);
            //tmpArr.Add(10);
            PlayerPrefs.SetInt("IsFirstMecha", 1);
            PlayerPrefs.Save();
        }
        else
        {
            for (int i = 0; i < 11; i++)
            {
                if (i == 3)
                    continue;
                else if (i == 8)
                {
                    CGird gird = gDefine.gPlayerData.mEquipGird[(int)gDefine.eEuqipPos.Cloak];
                    if (gird.mRefItem != null && gird.mRefItem.mSpecialIndex == 4)
                        continue;
                }
                else if (i == 9 || i == 10)
                {
                    GameObject[] arr = GameObject.FindGameObjectsWithTag("pet");
                    if (arr != null && arr.Length > 0)
                        continue;
                }
                tmpArr.Add(i);
            }
        }
        //tmpArr.Remove((int)eAdvType.Mecha); //先删掉还在测试的机甲

        int index = Random.Range(0, tmpArr.Count);

        mCurAdvType = (eAdvType)tmpArr[index];

        if (!mIsLoop && mCurAdvType == eAdvType.Boss)
            gDefine.PlaySound(29);

        //mCurAdvType = eAdvType.Mecha;

        switch (mCurAdvType)
        {
            case eAdvType.Hp15:
                mDes.text = gDefine.GetStr("血量上限和当前血量加15%");
                mBtnTextArr[3].text = gDefine.GetStr(360);// 双倍效果
                mBtnTextArr[4].text = gDefine.GetStr(360);// 双倍效果
                if (!mIsLoop)
                {
                    if (gDefine.gPlayerData.mLanguageType == CMyStr.eType.Simple ||
                   gDefine.gPlayerData.mLanguageType == CMyStr.eType.Old)
                        mBtnArr[4].SetActive(true);
                    else
                    {
                        mBtnArr[3].SetActive(true);
                    }
                }
                mTreasureIcon.sprite = gDefine.gABLoad.GetSprite("icon.bytes", "血量加");
                break;
            case eAdvType.Atk10:
                mDes.text = gDefine.GetStr("攻击力增加10%");
                mBtnTextArr[3].text = gDefine.GetStr(360);// 双倍效果
                mBtnTextArr[4].text = gDefine.GetStr(360);// 双倍效果
                if (!mIsLoop)
                {
                    if (gDefine.gPlayerData.mLanguageType == CMyStr.eType.Simple ||
                                        gDefine.gPlayerData.mLanguageType == CMyStr.eType.Old)
                        mBtnArr[4].SetActive(true);
                    else
                    {
                        mBtnArr[3].SetActive(true);
                    }
                }

                mTreasureIcon.sprite = gDefine.gABLoad.GetSprite("icon.bytes", "攻击力增加10");
                break;
            case eAdvType.Hp15Del:
                mDes.text = gDefine.GetStr("血量减50%");
                mBtnTextArr[3].text = gDefine.GetStr(386);
                mBtnTextArr[4].text = gDefine.GetStr(386);// 取    消
                if (!mIsLoop)
                    mBtnArr[4].SetActive(true);

                mTreasureIcon.sprite = gDefine.gABLoad.GetSprite("icon.bytes", "血量减50");
                mBtnTextArr[2].text = gDefine.GetStr(272);//=确定
                break;
            case eAdvType.RandomSkill:
                mDes.text = gDefine.GetStr("血量减50%");
                mBtnTextArr[2].text = gDefine.GetStr(272);//=确定
                mBtnTextArr[3].text = gDefine.GetStr(386);
                mBtnTextArr[4].text = gDefine.GetStr(386);//"取    消"
                if (!mIsLoop)
                    mBtnArr[4].SetActive(true);
                mTreasureIcon.sprite = gDefine.gABLoad.GetSprite("icon.bytes", "血量减50");
                break;
            case eAdvType.Atk30Del:
                mDes.text = gDefine.GetStr(284);//"攻击减50%"
                mBtnTextArr[3].text = gDefine.GetStr(386);
                mBtnTextArr[4].text = gDefine.GetStr(386);//"取    消"
                if (!mIsLoop)
                    mBtnArr[4].SetActive(true);


                mBtnTextArr[2].text = gDefine.GetStr(272);//=确定
                mTreasureIcon.sprite = gDefine.gABLoad.GetSprite("icon.bytes", "攻击减30");
                break;
            case eAdvType.Boss:
                mDes.text = "";//gDefine.GetStr("召唤出一个暗黑追击者");
                mBtnTextArr[3].text = gDefine.GetStr(386);
                mBtnTextArr[4].text = gDefine.GetStr(386);//"取    消"
                if (!mIsLoop)
                    mBtnArr[4].SetActive(true);
                mBtnTextArr[2].text = gDefine.GetStr(272);//=确定
                mTreasureIcon.sprite = gDefine.gABLoad.GetSprite("icon.bytes", "暗黑追击者");
                break;
            case eAdvType.Coin:
                mItemNum = Random.Range(100, 301);
                mDes.text = gDefine.GetStr("金币") + " " + mItemNum.ToString();
                if (mIsX10)
                {
                    mBtnTextArr[3].text = gDefine.GetStr(398);// 4倍获得
                    mBtnTextArr[4].text = gDefine.GetStr(398);// 4倍获得
                    mBtnTextArr[5].text = gDefine.GetStr(398);// 4倍获得
                }
                else
                {
                    mBtnTextArr[3].text = gDefine.GetStr(360);// 2倍获得
                    mBtnTextArr[4].text = gDefine.GetStr(360);// 2倍获得
                    mBtnTextArr[5].text = gDefine.GetStr(360);// 2倍获得
                }

                if (!mIsLoop)
                {
                    if (gDefine.gPlayerData.mLanguageType == CMyStr.eType.Simple ||
                   gDefine.gPlayerData.mLanguageType == CMyStr.eType.Old)
                        mBtnArr[4].SetActive(true);
                    else
                    {
                        mBtnArr[5].SetActive(true);
                    }
                }
                mTreasureIcon.sprite = gDefine.gABLoad.GetSprite("icon.bytes", "金币");


                break;
            case eAdvType.Crystal:
                mItemNum = Random.Range(100, 301);
                mDes.text = gDefine.GetStr("钻石") + " " + mItemNum.ToString();
                if (mIsX10)
                {
                    mBtnTextArr[3].text = gDefine.GetStr(398);// 4倍获得
                    mBtnTextArr[4].text = gDefine.GetStr(398);// 4倍获得
                    mBtnTextArr[5].text = gDefine.GetStr(398);// 4倍获得
                }
                else
                {
                    mBtnTextArr[3].text = gDefine.GetStr(360);// 2倍获得
                    mBtnTextArr[4].text = gDefine.GetStr(360);// 2倍获得
                    mBtnTextArr[5].text = gDefine.GetStr(360);// 2倍获得
                }


                if (!mIsLoop)
                {
                    if (gDefine.gPlayerData.mLanguageType == CMyStr.eType.Simple ||
                   gDefine.gPlayerData.mLanguageType == CMyStr.eType.Old)
                        mBtnArr[4].SetActive(true);
                    else
                    {
                        mBtnArr[5].SetActive(true);
                    }
                }

                mTreasureIcon.sprite = gDefine.gABLoad.GetSprite("icon.bytes", "钻石");

                break;

            case eAdvType.BigGun:
                //mItemNum = Random.Range(100, 301);
                mDes.text = gDefine.GetStr(395);//获得一个持枪幻像
                mBtnTextArr[2].text = gDefine.GetStr(272);//=确定
                mBtnTextArr[3].text = gDefine.GetStr(397);
                mBtnTextArr[4].text = gDefine.GetStr(397);//"双倍时间"

                if (!mIsLoop)
                {
                      if (gDefine.gPlayerData.mLanguageType == CMyStr.eType.Simple ||
                     gDefine.gPlayerData.mLanguageType == CMyStr.eType.Old 
                     || gDefine.gPlayerData.mLanguageType == CMyStr.eType.Japanese)
                        mBtnArr[4].SetActive(true);
                    else
                    {
                        mBtnArr[3].SetActive(true);
                    }
                }

                mTreasureIcon.sprite = gDefine.gABLoad.GetSprite("icon.bytes", "幻象大枪");

                break;
            case eAdvType.Pet0:
                //mItemNum = Random.Range(100, 301);
                mDes.text = gDefine.GetStr(393);//获得宠物0
                mBtnTextArr[2].text = gDefine.GetStr(306);// 离开
                mBtnTextArr[3].text = gDefine.GetStr(394);// 修理
                mBtnTextArr[4].text = gDefine.GetStr(394);// 修理
                mBtnTextArr[5].text = gDefine.GetStr(394);// 修理

                if (!mIsLoop)
                {
                    //  if (gDefine.gPlayerData.mLanguageType == CMyStr.eType.Simple ||
                    // gDefine.gPlayerData.mLanguageType == CMyStr.eType.Old)
                    mBtnArr[4].SetActive(true);
                    // else
                    // {
                    //    mBtnArr[5].SetActive(true);
                    //}
                }

                mTreasureIcon.sprite = gDefine.gABLoad.GetSprite("icon.bytes", "宠物");

                break;
            case eAdvType.Pet1:
                //mItemNum = Random.Range(100, 301);
                mDes.text = gDefine.GetStr(393);//获得宠物1

                mBtnTextArr[2].text = gDefine.GetStr(306); //离开

                mBtnTextArr[3].text = gDefine.GetStr(394);// 双倍伤害
                mBtnTextArr[4].text = gDefine.GetStr(394);// 双倍伤害
                mBtnTextArr[5].text = gDefine.GetStr(394);// 双倍伤害

                if (!mIsLoop)
                {
                    //  if (gDefine.gPlayerData.mLanguageType == CMyStr.eType.Simple ||
                    // gDefine.gPlayerData.mLanguageType == CMyStr.eType.Old)
                    mBtnArr[4].SetActive(true);
                    // else
                    // {
                    //    mBtnArr[5].SetActive(true);
                    //}
                }

                mTreasureIcon.sprite = gDefine.gABLoad.GetSprite("icon.bytes", "宠物1");

                break;

            case eAdvType.Mecha:
                
                mDes.text = gDefine.GetStr(474);//获得一台机甲
                mBtnTextArr[2].text = gDefine.GetStr(272);//=确定
                mBtnTextArr[3].text = gDefine.GetStr(397);
                mBtnTextArr[4].text = gDefine.GetStr(397);//"双倍时间"

                if (!mIsLoop)
                {
                      if (gDefine.gPlayerData.mLanguageType == CMyStr.eType.Simple ||
                     gDefine.gPlayerData.mLanguageType == CMyStr.eType.Old 
                     || gDefine.gPlayerData.mLanguageType == CMyStr.eType.Japanese)
                    mBtnArr[4].SetActive(true);
                    else
                    {
                        mBtnArr[3].SetActive(true);
                    }
                }

                mTreasureIcon.sprite = gDefine.gABLoad.GetSprite("icon.bytes", "合金机甲");

                break;
        }
    }

    public void CreateSomeCoinInGame(int Num)
    {
        Vector3 pcPos = gDefine.GetPCTrans().position;
        pcPos.y = gDefine.gGrounY + 1;
        for (int i = 0; i < Num; i++)
        {
            Vector3 pos = pcPos;
            pos.x += Random.Range(-2f, 2f);
            GameObject coin = GameObject.Instantiate(gDefine.gData.mCoinPreb);
            coin.transform.position = pos;
            se_itemDrop script = coin.GetComponent<se_itemDrop>();
            script.mEndFlyToPc = true;
            script.Init();
        }

    }

    public void CreateSomeCrystalInGame(int Num)
    {
        Vector3 pcPos = gDefine.GetPCTrans().position;
        pcPos.y = gDefine.gGrounY + 1;
        for (int i = 0; i < Num; i++)
        {
            Vector3 pos = pcPos;
            pos.x += Random.Range(-2f, 2f);
            GameObject coin = GameObject.Instantiate(gDefine.gData.mCrystalPreb);
            coin.transform.position = pos;
            se_itemDrop script = coin.GetComponent<se_itemDrop>();
            script.mEndFlyToPc = true;
            script.Init();
        }
    }

    public void GetPrize(int Times)
    {
        switch (mCurAdvType)
        {
            case eAdvType.Hp15:
                float oldprec = (float)gDefine.gPlayerData.mHp / (float)gDefine.gPlayerData.mHpMax;
                int hpadd = (int)(gDefine.gPlayerData.mHp * 0.15f);
                gDefine.gPlayerData.mHp += hpadd * Times;
                gDefine.gPlayerData.mHpMax += hpadd * Times;
                float newPrec = oldprec + 0.15f;
                if (newPrec > 1f)
                {
                    oldprec = 0.7f;
                    newPrec = 1f;
                }
                float realPrec = (float)gDefine.gPlayerData.mHp / (float)gDefine.gPlayerData.mHpMax;
                gDefine.RefreshHPUI(oldprec, newPrec, realPrec, 0.25f);

                GameObject o = GameObject.Instantiate(gDefine.gData.mUIHealSEPreb);
                se_event script = o.GetComponent<se_event>();
                script.InitLiftT(2);
                o.transform.SetParent(gDefine.GetPcRefMid());
                o.transform.localPosition = Vector3.zero;

                PlayerPrefs.SetInt("Adv_" + gDefine.gPlayerData.mAdvCount.ToString(), (int)mCurAdvType);
                PlayerPrefs.SetInt("Adv_num_" + gDefine.gPlayerData.mAdvCount.ToString(), Times);
                gDefine.gPlayerData.mAdvCount++;
                PlayerPrefs.SetInt("AdvCount", gDefine.gPlayerData.mAdvCount); break;
            case eAdvType.Atk10:
                int damageadd = (int)(gDefine.gPlayerData.mDamage * 0.1f);
                gDefine.gPlayerData.mDamage += damageadd * Times;

                PlayerPrefs.SetInt("Adv_" + gDefine.gPlayerData.mAdvCount.ToString(), (int)mCurAdvType);
                PlayerPrefs.SetInt("Adv_num_" + gDefine.gPlayerData.mAdvCount.ToString(), Times);
                gDefine.gPlayerData.mAdvCount++;
                PlayerPrefs.SetInt("AdvCount", gDefine.gPlayerData.mAdvCount);
                GameObject se = GameObject.Instantiate(gDefine.gData.mUIAtkAddSEPreb);
                se.transform.SetParent(gDefine.GetPcRefMid());
                se.transform.localPosition = Vector3.zero;

                break;
            case eAdvType.Hp15Del:

                if (Times == 1)
                {
                    float operc = (float)gDefine.gPlayerData.mHp / (float)gDefine.gPlayerData.mHpMax;
                    gDefine.gPlayerData.mHp = (int)(gDefine.gPlayerData.mHp * 0.5f);
                    PlayerPrefs.SetInt("Adv_" + gDefine.gPlayerData.mAdvCount.ToString(), (int)mCurAdvType);
                    PlayerPrefs.SetInt("Adv_num_" + gDefine.gPlayerData.mAdvCount.ToString(), Times);
                    gDefine.gPlayerData.mAdvCount++;
                    PlayerPrefs.SetInt("AdvCount", gDefine.gPlayerData.mAdvCount);
                    float nperc = (float)gDefine.gPlayerData.mHp / (float)gDefine.gPlayerData.mHpMax;


                    gDefine.RefreshHPUI(operc, nperc, nperc);

                }
                break;
            case eAdvType.RandomSkill:
                //mDes.text = "血量减50%";
                //mOptionTip.text = "看视频取消";
                break;
            case eAdvType.Atk30Del:
                if (Times == 1)
                {
                    gDefine.gPlayerData.mDamage = (int)(gDefine.gPlayerData.mDamage * 0.5f);
                    PlayerPrefs.SetInt("AdvCount", gDefine.gPlayerData.mAdvCount + 1);
                    PlayerPrefs.SetInt("Adv_" + gDefine.gPlayerData.mAdvCount.ToString(), (int)mCurAdvType);
                    PlayerPrefs.SetInt("Adv_num_" + gDefine.gPlayerData.mAdvCount.ToString(), Times);
                }
                break;
            case eAdvType.Boss:
                {
                    if (Times == 1)
                    {
                        npcdata.eNpcType[] arr = new npcdata.eNpcType[]{npcdata.eNpcType.RagePursuer,
                                    npcdata.eNpcType.PosionPursuer,
                                    npcdata.eNpcType.DefPursuer,
                                    npcdata.eNpcType.RebackPursuer,
                                    npcdata.eNpcType.JumpPursuer};

                        npcdata.eNpcType npcType = arr[Random.Range(0, arr.Length)];

                        if (gDefine.gChapterId == 1)
                        {
                            npcType = npcdata.eNpcType.RagePursuer;
                        }

                        GameObject npc = GameObject.Instantiate(gDefine.gNpcData.GetNpcPreb(npcType));

                        Vector3 pos = gDefine.GetPCTrans().position + Vector3.right * 3;

                        pos.y = gDefine.gGrounY;

                        CNpcInst npcInst = gDefine.gNpc.CreateNpcInst(npcType);

                        npcInst.Init(npc, npcType, pos, gDefine.gLogic.mWaveLvL);

                        gDefine.gNpc.AddNpc(npcInst);

                        gDefine.PlaySound(1);

                    }
                }
                //mDes.text = "召唤出一个暗黑追击者";
                //mOptionTip.text = "看视频取消";
                break;
            case eAdvType.Coin:

                if (Times > 1)
                {
                    if (mIsX10)
                    {
                        gDefine.gPlayerData.Coin += mItemNum * 4;
                        CreateSomeCoinInGame(35);
                        gDefine.gGainInFight.AddCoins(mItemNum * 4, "ADV");
                    }
                    else
                    {
                        gDefine.gPlayerData.Coin += mItemNum * 2;
                        CreateSomeCoinInGame(25);
                        gDefine.gGainInFight.AddCoins(mItemNum * 2, "ADV");
                    }
                }

                else
                {
                    gDefine.gPlayerData.Coin += mItemNum;
                    CreateSomeCoinInGame(15);
                    gDefine.gGainInFight.AddCoins(mItemNum, "ADV");
                }



                gDefine.PlaySound(57);

                //mDes.text = "100-300金币";
                //mOptionTip.text = "看视频双倍获取";
                break;
            case eAdvType.Crystal:

                if (Times > 1)
                {
                    if (mIsX10)
                    {
                        gDefine.gPlayerData.Crystal += mItemNum * 4;
                        CreateSomeCrystalInGame(35);
                        gDefine.gGainInFight.AddCrystals(mItemNum * 4, "ADV");
                    }
                    else
                    {
                        gDefine.gPlayerData.Crystal += mItemNum * 2;
                        CreateSomeCrystalInGame(25);
                        gDefine.gGainInFight.AddCrystals(mItemNum * 2, "ADV");
                    }
                }
                else
                {
                    gDefine.gPlayerData.Crystal += mItemNum;
                    CreateSomeCrystalInGame(15);
                    gDefine.gGainInFight.AddCrystals(mItemNum, "ADV");
                }

                gDefine.PlaySound(57);
                //mDes.text = "100-300钻石";
                //mOptionTip.text = "看视频双倍获取";
                break;
            case eAdvType.BigGun:
                {
                    Vector3 pcPos = gDefine.GetPCTrans().position;
                    pcPos.y = gDefine.gGrounY;
                    pcPos.x += Random.Range(0, 100) < 50 ? 2 : -2;
                    gDefine.UseCloneShootGirlNow(pcPos, Random.Range(0, 100) < 50 ? true : false, 5f * Times);
                }
                break;
            case eAdvType.Pet0:
                {
                    if (Times > 1)
                    {
                        Vector3 pcPos = gDefine.GetPCTrans().position;
                        //pcPos.y = gDefine.gGrounY;
                        GameObject pet = gDefine.gABLoad.GetPreb("obj.bytes", "Pet0");
                        pet = GameObject.Instantiate(pet);
                        Pet0 s = pet.GetComponent<Pet0>();
                        //s.Init(pcPos, Times > 1 ? true : false);
                        s.Init(pcPos, false);

                        PlayerPrefs.SetInt("Adv_" + gDefine.gPlayerData.mAdvCount.ToString(), (int)mCurAdvType);
                        PlayerPrefs.SetInt("Adv_num_" + gDefine.gPlayerData.mAdvCount.ToString(), Times);
                        gDefine.gPlayerData.mAdvCount++;
                        PlayerPrefs.SetInt("AdvCount", gDefine.gPlayerData.mAdvCount);
                    }

                    //gDefine.UseCloneShootGirlNow(pcPos, Random.Range(0,100)<50?true:false, 5f*Times);
                }
                break;
            case eAdvType.Pet1:
                {
                    if (Times > 1)
                    {
                        Vector3 pcPos = gDefine.GetPCTrans().position;

                        GameObject pet = gDefine.gABLoad.GetPreb("obj.bytes", "Pet1");
                        pet = GameObject.Instantiate(pet);
                        //GameObject pet = GameObject.Instantiate(mPet1);
                        Pet1 s = pet.GetComponent<Pet1>();
                        //s.Init(pcPos, Times > 1 ? true : false);
                        s.Init(pcPos, false);

                        PlayerPrefs.SetInt("Adv_" + gDefine.gPlayerData.mAdvCount.ToString(), (int)mCurAdvType);
                        PlayerPrefs.SetInt("Adv_num_" + gDefine.gPlayerData.mAdvCount.ToString(), Times);
                        gDefine.gPlayerData.mAdvCount++;
                        PlayerPrefs.SetInt("AdvCount", gDefine.gPlayerData.mAdvCount);
                    }
                }
                break;
             case eAdvType.Mecha:
                {
                    //if (Times > 1)
                    {
                        gDefine.CreateMecha(Times>1? 20:35);
                        // Vector3 pcPos = gDefine.GetPCTrans().position;

                        // GameObject pet = gDefine.gABLoad.GetPreb("obj.bytes", "Pet1");
                        // pet = GameObject.Instantiate(pet);
                        // //GameObject pet = GameObject.Instantiate(mPet1);
                        // Pet1 s = pet.GetComponent<Pet1>();
                        // //s.Init(pcPos, Times > 1 ? true : false);
                        // s.Init(pcPos, false);

                        PlayerPrefs.SetInt("Adv_" + gDefine.gPlayerData.mAdvCount.ToString(), (int)mCurAdvType);
                        PlayerPrefs.SetInt("Adv_num_" + gDefine.gPlayerData.mAdvCount.ToString(), Times);
                        gDefine.gPlayerData.mAdvCount++;
                        PlayerPrefs.SetInt("AdvCount", gDefine.gPlayerData.mAdvCount);
                    }
                }
                break;
        }
        gDefine.gPause = false;
        gDefine.gGameMainUI.mRefLVLTipUI.ContinueTask();
        gameObject.SetActive(false);

    }

    public void ADCallBack(bool Finished)
    {
        if (Finished)
        {
            GetPrize(2);
        }
    }

    public void Btn_GetCallBack()
    {
        GetPrize(1);
    }

    public void Btn_Get()
    {
        gDefine.gBtnAnim.Init(mBtnArr[2], 1, Btn_GetCallBack);
    }

    public void Btn_GetDoubleCallBack()
    {
        gDefine.gAd.PlayADVideo(ADCallBack);
        Dictionary<string, object> dic = new Dictionary<string, object>();

        dic.Add("来源", "附魔双倍");
        dic.Add("名称", mCurAdvType.ToString());

        TalkingDataGA.OnEvent("激励视频广告", dic);
    }

    public void Btn_GetDouble(int Index)
    {
        if (Index == 0)
            gDefine.gBtnAnim.Init(mBtnArr[3], 1, Btn_GetDoubleCallBack);
        else if (Index == 1)
            gDefine.gBtnAnim.Init(mBtnArr[4], 1, Btn_GetDoubleCallBack);
        else if (Index == 2)
            gDefine.gBtnAnim.Init(mBtnArr[5], 1, Btn_GetDoubleCallBack);
    }

    public void Pre_Load()
    {
        gDefine.gPlayerData.mAdvCount = PlayerPrefs.GetInt("AdvCount", 0);
        for (int i = 0; i < gDefine.gPlayerData.mAdvCount; i++)
        {
            eAdvType e = (eAdvType)PlayerPrefs.GetInt("Adv_" + i.ToString(), 0);
            int num = PlayerPrefs.GetInt("Adv_num_" + i.ToString(), 0);
            Pre_load(e, num);
        }
    }

    void Pre_load(eAdvType AdvType, int Times)
    {
        switch (AdvType)
        {
            case eAdvType.Hp15:
                int hpadd = (int)(gDefine.gPlayerData.mHp * 0.15f);
                gDefine.gPlayerData.mHp += hpadd * Times;
                gDefine.gPlayerData.mHpMax += hpadd * Times;
                break;
            case eAdvType.Atk10:
                int damageadd = (int)(gDefine.gPlayerData.mDamage * 0.1f);
                gDefine.gPlayerData.mDamage += damageadd * Times;
                break;
            case eAdvType.Hp15Del:
                if (Times == 1)
                {
                    gDefine.gPlayerData.mHp = (int)(gDefine.gPlayerData.mHp * 0.5f);
                }
                break;
            case eAdvType.RandomSkill:
                //mDes.text = "血量减50%";
                //mOptionTip.text = "看视频取消";
                break;
            case eAdvType.Atk30Del:
                if (Times == 1)
                {
                    gDefine.gPlayerData.mDamage = (int)(gDefine.gPlayerData.mDamage * 0.7f);
                }
                break;
            case eAdvType.Pet0:
                {
                    {
                        Vector3 pcPos = gDefine.GetPCTrans().position;
                        //pcPos.y = gDefine.gGrounY;
                        GameObject pet = gDefine.gABLoad.GetPreb("obj.bytes", "Pet0");
                        pet = GameObject.Instantiate(pet);
                        Pet0 s = pet.GetComponent<Pet0>();
                        s.Init(pcPos, Times > 1 ? true : false);
                    }

                    //gDefine.UseCloneShootGirlNow(pcPos, Random.Range(0,100)<50?true:false, 5f*Times);
                }
                break;
            case eAdvType.Pet1:
                {
                    {
                        Vector3 pcPos = gDefine.GetPCTrans().position;
                        GameObject pet = gDefine.gABLoad.GetPreb("obj.bytes", "Pet1");
                        pet = GameObject.Instantiate(pet);
                        Pet1 s = pet.GetComponent<Pet1>();
                        s.Init(pcPos, Times > 1 ? true : false);
                    }
                }
                break;
        }
    }


    public void ShowTreasure()
    {
        mIsLoop = true;
        mLoopT = 0.04f;
        mLoopNum = 12;
        Refresh();
    }

    void Update()
    {
        if (mIsLoop)
        {
            mLoopT -= Time.deltaTime;
            if (mLoopT < 0)
            {
                mLoopT += 0.15f;
                mLoopNum--;
                if (mLoopNum <= 0)
                    mIsLoop = false;
                Refresh();
            }
        }



    }
}
