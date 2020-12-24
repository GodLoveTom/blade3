using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui_AdvBox : MonoBehaviour
{
    public ui_Adv mUIAdv;
    // Start is called before the first frame update

    public void Event_Finish()
    {
        mUIAdv.ShowTreasure();
    }
}
