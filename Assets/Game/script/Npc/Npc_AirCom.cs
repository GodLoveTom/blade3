using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_AirCom : MonoBehaviour
{
    public GameObject mLightObj;
    public GameObject mAtkObj;
    public GameObject mHeadObj;

    public GameObject mGunObj;
    public GameObject mGunPoint;

    public LineRenderer[] mAimArr = new LineRenderer[7];
    // Start is called before the first frame update
    public void Show(Vector3 Epos)
    {
         for (int i = 0; i < mAimArr.Length; i++)
            mAimArr[i].gameObject.SetActive(false);

        mLightObj.SetActive(true);
        mAtkObj.SetActive(true);

        mHeadObj.SetActive(false);

        mAtkObj.transform.position = Epos;

        Vector3 dir = Epos - mGunObj.transform.position;
        dir.Normalize();
        mGunObj.transform.up = -dir;

        LineRenderer mLine = mLightObj.GetComponent<LineRenderer>();

        mLine.positionCount = 2;

        Vector3[] arr = new Vector3[2];
        arr[0].x = mGunPoint.transform.position.x;
        arr[0].y = mGunPoint.transform.position.y;
        arr[0].z = mGunPoint.transform.position.z;

        arr[1].x = Epos.x;
        arr[1].y = Epos.y;
        arr[1].z = Epos.z;

        mLine.SetPositions(arr);
    }

    public void ShowHead(float perc)
    {
         mLightObj.SetActive(true);
        mAtkObj.SetActive(false);

        for (int i = 0; i < mAimArr.Length; i++)
            mAimArr[i].gameObject.SetActive(false);
        mHeadObj.SetActive(true);

         mGunObj.transform.up = Vector3.up;

         float YL = Mathf.Abs( mGunPoint.transform.position.y - gDefine.gGrounY);
         Vector3 pos =  mGunPoint.transform.position;
         pos.y -= YL*perc;
         if(pos.y<gDefine.gGrounY)
            pos.y = gDefine.gGrounY;
        mHeadObj.transform.position = pos;

           LineRenderer mLine = mLightObj.GetComponent<LineRenderer>();

        mLine.positionCount = 2;

        Vector3[] arr = new Vector3[2];
        arr[0].x = mGunPoint.transform.position.x;
        arr[0].y = mGunPoint.transform.position.y;
        arr[0].z = mGunPoint.transform.position.z;

        arr[1].x = mGunPoint.transform.position.x;
        arr[1].y = pos.y;
        arr[1].z = mGunPoint.transform.position.z;

        mLine.SetPositions(arr);
    }

    public void ShowAim(float X, float L)
    {
        mLightObj.SetActive(false);
        mAtkObj.SetActive(false);

        for (int i = 0; i < mAimArr.Length; i++)
            mAimArr[i].gameObject.SetActive(true);

        mHeadObj.SetActive(false);

        mGunObj.transform.up = Vector3.up;

        float l = L / 3;

        for (int i = 0; i < mAimArr.Length; i++)
        {
            float x = X - L + l * i;

            mAimArr[i].positionCount = 2;

            Vector3[] arr = new Vector3[2];
            arr[0].x = mGunPoint.transform.position.x;
            arr[0].y = mGunPoint.transform.position.y;
            arr[0].z = mGunPoint.transform.position.z;

            arr[1].x = x;
            arr[1].y = gDefine.gGrounY;
            arr[1].z = mAimArr[i].gameObject.transform.position.z;

            mAimArr[i].SetPositions(arr);
        }
    }

    public void Close()
    {
        mLightObj.SetActive(false);
        mAtkObj.SetActive(false);

        for (int i = 0; i < mAimArr.Length; i++)
            mAimArr[i].gameObject.SetActive(false);
    }


}
