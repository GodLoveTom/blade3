using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_lvlTip : MonoBehaviour
{
    enum eState
    {
        FlyIn = 0,
        Show,
        Rot,
        FlyOut,
        Close
    }

    eState mCurState = eState.Close;

    public enum eInsertTask
    {
        Null = 0,
        Choose31,
        Magic,
        Adv,
    }

    List<eInsertTask> mTaskList = new List<eInsertTask>();

    const float mFlyT = 0.6f;
    float mCurT = 0;
    float mFlyV = 1500;
    float mCurRot = 0;
    bool mRotIsInFirst = true;

    public Text mText;

    float mShowEndT = 0;
    /// <summary>
    /// 是否分段显示
    /// </summary>    
    public bool mCurIsShowTwice = false;

    List<string> mArr = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        mCurState = eState.Close;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 清空插播界面任务列表
    /// </summary>
    public void ClearInsertTask()
    {
        mTaskList.Clear();
    }

    public void InsertTask(eInsertTask Task)
    {
        mTaskList.Add(Task);
    }

    public bool IsShowEnd()
    {
        if (mArr.Count > 0 || Time.time <= mShowEndT || gameObject.activeSelf)
            return false;
        else
            return true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gDefine.gPause)
            return;

        switch (mCurState)
        {
            case eState.FlyIn:
                UpdateFlyIn();
                break;

            case eState.Show:
                UpdateShow();
                break;

            case eState.Rot:
                UpdateRot();
                break;

            case eState.FlyOut:
                UpdateFlyOut();
                break;
        }
    }

    public void ShowTip(string str)
    {
        mArr.Add(str);

        // if (mCurState == eState.Close)
        // {
        //     BeginFlyIn();
        //     gameObject.SetActive(true);
        // }     
    }
    /// <summary>
    /// 设置是否分段展示，中间会插播一个3选一或者其他界面
    /// </summary>
    /// <param name="Twice">true 分段显示 ，false 一次全部显示</param>
    public void NeedShowTwice(bool Twice)
    {
        mCurIsShowTwice = Twice;
    }

    /// <summary>
    /// 开始播放过关提示
    /// </summary>
    public void Play()
    {
        BeginFlyIn();
        gameObject.SetActive(true);
    }

    public void UpdateFlyIn()
    {
        Vector3 pos = gameObject.transform.position;
        pos = Vector3.MoveTowards(pos, new Vector3(Screen.width / 2, pos.y, pos.z), Time.deltaTime * mFlyV);
        gameObject.transform.position = pos;
        if (System.Math.Abs(pos.x - Screen.width / 2) < 0.01f)
        {
            mCurState = eState.Show;
            mCurT = 1.0f;
        }
    }


    void BeginFlyIn()
    {
        gameObject.transform.position = new Vector3(-1500, gameObject.transform.position.y, gameObject.transform.position.z);
        gameObject.SetActive(true);
        mCurState = eState.FlyIn;
        //mCurT = 0;
        mFlyV = (1500 + Screen.width / 2) / mFlyT;

        mText.text = mArr[0];
        gDefine.SetTextBold();
        mArr.RemoveAt(0);
    }


    void UpdateShow()
    {
        mCurT -= Time.deltaTime;
        if (mCurT < 0)
        {
            if (mArr.Count > 0 && !mCurIsShowTwice)
            {
                mCurState = eState.Rot;
                mCurRot = 0;
                mRotIsInFirst = true;

                return;
            }
            else
            {
                mCurState = eState.FlyOut;
                //mCurT = 1.0f;
                mFlyV = 1500 / mFlyT;
            }
        }
    }

    void UpdateRot()
    {
        if (mRotIsInFirst)
        {
            mCurRot -= Time.deltaTime * 270;
            if (mCurRot < -90)
            {

                mCurRot = -90;
                mRotIsInFirst = false;
                mText.text = mArr[0];
                gDefine.SetTextBold();
                mArr.RemoveAt(0);
            }
            mText.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            mText.gameObject.transform.Rotate(Vector3.left, mCurRot);
        }
        else
        {
            mCurRot += Time.deltaTime * 270;
            if (mCurRot >= 0)
            {

                mCurRot = 0;
                mCurState = eState.FlyOut;
                mCurT = 0;

            }
            mText.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            mText.gameObject.transform.Rotate(Vector3.left, mCurRot);
        }

    }

    public void UpdateFlyOut()
    {
        Vector3 pos = gameObject.transform.position;
        pos = Vector3.MoveTowards(pos, new Vector3(1500, pos.y, pos.z), Time.deltaTime * mFlyV);
        gameObject.transform.position = pos;
        if (System.Math.Abs(pos.x - 1500.0f) < 0.1f)
        {
            mCurState = eState.Close;
            gameObject.SetActive(false);
            if (mCurIsShowTwice)
            {
                ContinueTask();
            }

            mShowEndT = Time.time + 2;
        }
    }

    public void ContinueTask()
    {
        if (mCurIsShowTwice)
        {
            if (mTaskList.Count > 0)
            {
                eInsertTask Task = mTaskList[0];
                mTaskList.RemoveAt(0);
                if (Task == eInsertTask.Choose31)
                    gDefine.gGameMainUI.Show3Choose1();
                else if (Task == eInsertTask.Magic)
                    gDefine.gGameMainUI.ShowUIMagic();
                else if (Task == eInsertTask.Adv)
                    gDefine.gGameMainUI.ShowUIAdv();
            }
            else
            {
                mCurIsShowTwice = false;
                Play();
            }

        }
    }

    // public void ContinueShow()
    // {
    //     if(  mArr.Count > 0 )
    //     {
    //         mCurIsShowTwice = false;
    //         BeginFlyIn();
    //     }
    // }


}
