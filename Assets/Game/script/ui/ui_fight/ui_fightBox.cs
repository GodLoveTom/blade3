using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_fightBox : MonoBehaviour
{
    public Sprite []mBoxSprite;
    public Image mImage;
    int Index = -1;
    float mT = 0;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetImage( int Index )
    {
        mImage.sprite = mBoxSprite[Index*7];
    }

    public void PlayOpenAnim( int Difficult)
    {

    }


}
