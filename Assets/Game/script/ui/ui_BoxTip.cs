using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_BoxTip : MonoBehaviour
{
    public GameObject mRefCoinImage;
    public Image[] mPrizeIcon;
    public Text[] mPrizeNum;
    float mT = 0;
    bool mIsMove = false;
    float mMoveT = 0;
    float mWaitT = 0;

    // Update is called once per frame
    void Update()
    {
        if (mWaitT > 0)
        {
            mWaitT -= Time.deltaTime;
            if (mWaitT > 0)
                return;
            else
            {
                Image[] imageArr = gameObject.GetComponentsInChildren<Image>();
                foreach (Image i in imageArr)
                {
                    i.color = Color.white;
                }
            }
        }

        if (Time.time >= mT)
        {
            if (mIsMove)
            {

                Image[] imageArr = gameObject.GetComponentsInChildren<Image>();
                foreach (Image i in imageArr)
                {
                    float a = i.color.a - Time.deltaTime*2f;
                    if (a < 0)
                    {
                        gameObject.SetActive(false);
                        GameObject.Destroy(gameObject);
                    }
                    else
                    {
                        i.color = new Color(1, 1, 1, Mathf.Clamp01(a));
                    }
                }

                transform.Translate(0,  1200 * gDefine.RecalcUIScale() * Time.deltaTime, 0, Space.World);
            }
            else
                gameObject.SetActive(false);
        }
    }

    public void Show(int ItemId, int ItemNum, int Item1Id, int Item1Num, bool IsMove = false, float WaitT = 0)
    {
        gameObject.SetActive(true);

        CItem it = gDefine.gData.GetItemData(ItemId);
        mPrizeIcon[0].sprite = it.GetIconSprite();
        mPrizeNum[0].text = "+" + ItemNum.ToString();
        if( Item1Id==-1)
        {
            mPrizeIcon[1].gameObject.SetActive(false);
            mPrizeNum[1].text = "" ;
        }
        else
        {
             CItem Item1 = gDefine.gData.GetItemData(Item1Id);
             mPrizeIcon[1].gameObject.SetActive(true);
            mPrizeIcon[1].sprite = Item1.GetIconSprite();
            mPrizeNum[1].text = "+" + Item1Num.ToString();
        }
       


        mWaitT = WaitT;

        if (mWaitT > 0 )
        {
            Image[] imageArr = gameObject.GetComponentsInChildren<Image>();
            foreach (Image i in imageArr)
            {
                i.color = Color.clear;
            }
        }

        mT = Time.time + (IsMove ? 1f : 2.2f) + mWaitT;

        mIsMove = IsMove;

        if (ItemId == 201)
        {
            gDefine.PlayUIGainCoinSE(gameObject);
            gDefine.PlaySound(57);
        }

        else if (ItemId == 202)
        {
            gDefine.PlayUIGainCrystalSE(gameObject);
            gDefine.PlaySound(57);
        }


        if (Item1Id == 201)
        {
            gDefine.PlayUIGainCoinSE(gameObject);
            gDefine.PlaySound(57);
        }

        else if (Item1Id == 202)
        {
            gDefine.PlayUIGainCrystalSE(gameObject);
            gDefine.PlaySound(57);
        }


        // for(int i=0;i<20; i++)
        // {
        //     GameObject o = GameObject.Instantiate(mRefCoinImage);
        //     o.transform.SetParent(transform);
        //     o.transform.localPosition = new Vector3( Random.Range(-250,250), 0,0);
        //     o.SetActive(true);

        //     ui_ItemDrop s = o.GetComponent<ui_ItemDrop>();
        //     s.Init();

        // }

         Text [] textArr = gameObject.transform.GetComponentsInChildren<Text>(true);
        foreach(Text _t in textArr)
            gDefine.ResetFontBold(_t);

    }
}
