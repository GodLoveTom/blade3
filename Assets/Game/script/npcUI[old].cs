using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class npcUI : MonoBehaviour
{
    public Text mHpText;
    public Image mHpImage;
    public SpriteRenderer mSprite;
    public SpriteRenderer mSESprite;
    public SpriteRenderer mBeKilledSprite;
    public GameObject mRoot;
    public GameObject mLeftPos;
    public GameObject mRightPos;
    [Header("中心参考点")]
    public GameObject mRefMidPoint;

    public void Refresh(float Hp, float MaxHp)
    {
        if( Hp<100000)
            mHpText.text = ((int)Hp).ToString();
        else
        {
            int v = (int)(Hp )/ 1000;
            mHpText.text = v.ToString()+"k";
        }

        if (Hp <= 0)
            mHpImage.transform.localScale = new Vector3(0, 1, 1);
        else
            mHpImage.transform.localScale = new Vector3(Hp/MaxHp, 1, 1);

    }

    public void Close()
    {
        mRoot.SetActive(false);
    }

    public Vector3 GetDamageShowPos()
    {
        return gameObject.transform.position + Vector3.up * 3;
    }
}
