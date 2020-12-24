using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sce : MonoBehaviour
{
    //---场景----
    // public GameObject mRefSceBackObj;
    // public GameObject mRefSceFarFarObj;
    // public GameObject mRefSceFarObj;
    // public GameObject mRefSceFrontObj;

    public GameObject[] mRefSceObj;
    GameObject[] mRefSce1Obj;

    // GameObject mRefSceBack1Obj;
    // GameObject mRefSceFarFar1Obj;
    // GameObject mRefSceFar1Obj;
    // GameObject mRefSceFront1Obj;

    public float mSceSizeW;
    public float mIphone8CamOffY;
    public float mIphone8CamSize;
    public float mIphoneXCamOffY;
    public float mIphoneXCamSize;

    // public Vector3 mRefSceBackObjOriPos;
    // public Vector3 mRefSceFarFarObjOriPos;
    // public Vector3 mRefSceFarObjOriPos;
    [Header("女主出发位置")]
    public GameObject mRefBeginPos;

    float[] mOffsetX;

    // Start is called before the first frame update
    void Start()
    {
        // mRefSceBackObjOriPos = mRefSceBackObj.transform.position;
        // mRefSceFarFarObjOriPos = mRefSceFarFarObj.transform.position;
        // mRefSceFarObjOriPos = mRefSceFarObj.transform.position;

        gDefine.gMySce = this;
        mRefSce1Obj = new GameObject[mRefSceObj.Length];
        mOffsetX = new float[mRefSceObj.Length];
        for (int i = 0; i < mRefSceObj.Length; i++)
            mOffsetX[i] = 0;

    }

    public void ReInitBeforeGame()
    {
        if (mOffsetX == null)
            mOffsetX = new float[mRefSceObj.Length];
        if (mRefSce1Obj == null)
            mRefSce1Obj = new GameObject[mRefSceObj.Length];

        for (int i = 0; i < mRefSceObj.Length; i++)
            mOffsetX[i] = 0;

        for (int i = 0; i < mRefSceObj.Length; i++)
        {
            mRefSceObj[i].transform.localPosition = Vector3.zero;
            if (mRefSce1Obj[i] != null)
                mRefSce1Obj[i].SetActive(false);
        }

        //  mRefSceBack1Obj?.SetActive(false);
        //  mRefSceFarFar1Obj?.SetActive(false);
        //  mRefSceFar1Obj?.SetActive(false);
        //  mRefSceFront1Obj?.SetActive(false);

        //  mRefSceBackObj.transform.localPosition = Vector3.zero;
        //  mRefSceFarFarObj.transform.localPosition = Vector3.zero;
        //  mRefSceFarObj.transform.localPosition = Vector3.zero;
        //  mRefSceFrontObj.transform.localPosition = Vector3.zero;

        // mRefSceBackObj.transform.localPosition = Vector3.zero;
        // mRefSceFarFarObj.transform.localPosition = Vector3.zero;
        // mRefSceFarObj.transform.localPosition = Vector3.zero;
        // mRefSceFrontObj.transform.localPosition = Vector3.zero;

    }

    public void Move(Vector3 Delt)
    {
        //for(int i=0; i<mOffsetX.Length; i++)

        mOffsetX[0] += Delt.x * 0.75f;
        mOffsetX[1] += Delt.x * 0.38f;
        mOffsetX[2] += Delt.x * 0.3f;
        //mOffsetX[3] += Delt.x * 0.0f;
        // Vector3 backPos = Delt.x * 0.75f;
        // mRefSceBackObj.transform.Translate(backPos);
        // Vector3 farfarPos = Delt * 0.38f;
        // mRefSceFarFarObj.transform.Translate(farfarPos);
        // Vector3 farPos = Delt * 0.3f;
        // mRefSceFarObj.transform.Translate(farPos);
        //  Vector3 frontPos = Delt ;
        // mRefSceFarObj.transform.Translate(farPos);

        Calc();
    }

    void Calc()
    {
        //计算左边
        float xleft = Camera.main.transform.position.x - mRefBeginPos.transform.position.x - 16;
        float xRight = Camera.main.transform.position.x - mRefBeginPos.transform.position.x + 16;
        for (int i = 0; i < mRefSceObj.Length; i++)
        {
            int index0 = (int)((xleft - mOffsetX[i] + mSceSizeW * 0.5f) / mSceSizeW);
            int index1 = (int)((xRight - mOffsetX[i] + mSceSizeW * 0.5f) / mSceSizeW);

            if( Camera.main.transform.position.x < mRefBeginPos.transform.position.x )
            {
                 index0 = (int)((xleft - mOffsetX[i] - mSceSizeW * 0.5f) / mSceSizeW);
                 index1 = (int)((xRight - mOffsetX[i] - mSceSizeW * 0.5f) / mSceSizeW);
            }
           
            mRefSceObj[i].transform.localPosition = new Vector3(index0 * mSceSizeW + mOffsetX[i], 0, 0);
            if (index0 != index1)
            {
                if (mRefSce1Obj[i] == null)
                    mRefSce1Obj[i] = GameObject.Instantiate(mRefSceObj[i]);
                mRefSce1Obj[i].transform.position = mRefSceObj[i].transform.position + Vector3.right * mSceSizeW;
                mRefSce1Obj[i].SetActive(true);
            }
            else if (mRefSce1Obj[i] != null)
                mRefSce1Obj[i].SetActive(false);
        }

    }


}
