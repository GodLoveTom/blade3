using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagerShowUI : MonoBehaviour
{
    public Text mText;

    public void Refresh(int Damage,Color C)
    {
        mText.text = Damage.ToString();
        mText.color = C;
    }

    public void Refresh(string Str, Color C)
    {
        mText.text = Str;
        mText.color = C;
    }
}
