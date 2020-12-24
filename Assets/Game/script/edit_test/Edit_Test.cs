using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Edit_Test : MonoBehaviour
{
    public InputField mDying2UpV;
    public InputField mDying2DownAcc;
    public InputField mDying2XV;
    // Start is called before the first frame update
    void Start()
    {
        gDefine.gUIEditer = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetEdit_Dying2AUpV()
    {
        return float.Parse(mDying2UpV.text);
    }

    public float GetEdit_Dying2DownAcc()
    {
        return float.Parse(mDying2DownAcc.text);
    }

    public float GetEdit_Dying2_XV()
    {
        return float.Parse(mDying2XV.text);
    }
}
