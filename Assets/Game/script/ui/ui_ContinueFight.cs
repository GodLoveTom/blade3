using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ui_ContinueFight : MonoBehaviour
{
    public GameObject mTipObj;
    public Text mTipText;
    public Text mOKBtnText;
    public Text mCancelText;

    void Start()
    {
        Refresh();
        //PlayerPrefs.SetInt("firstInGame",0);
        if (PlayerPrefs.GetInt("firstInGame", 0) == 0)
        {
            gDefine.gIsFirstInGame = true;
            PlayerPrefs.SetInt("firstInGame", 1);
        }
        else
        {
            gDefine.gIsFirstInGame = false;

            if (PlayerPrefs.GetInt("continueFight", 0) == 0)
            {
                 PlayerPrefs.GetInt("Relive",0);
                  gameObject.SetActive(false);
            }
            else
            {
                mTipObj.SetActive(true);
            }

            //push
            DoPush();
        }
    }

    void Update()
    {
        if (gDefine.gIsFirstInGame)
        {
            gDefine.gMainUI.Btn_ContinueFight();
            gameObject.SetActive(false);
        }
    }

    public void Refresh()
    {
        mTipText.text = gDefine.GetStr(387);
        gDefine.SetTextBold();
        mOKBtnText.text = gDefine.GetStr(272);
        gDefine.SetTextBold();
        mCancelText.text = gDefine.GetStr(384);
        gDefine.SetTextBold();
        gDefine.gForbidLvL = 0;

         Text [] textArr = gameObject.transform.GetComponentsInChildren<Text>(true);
        foreach(Text _t in textArr)
            gDefine.ResetFontBold(_t);
    }

    public void Btn_Go()
    {
        gDefine.gMainUI.Btn_ContinueFight();
        gameObject.SetActive(false);
    }

    public void Btn_Close()
    {
        gameObject.SetActive(false);
        PlayerPrefs.GetInt("Relive",0);
    }

    public void DoPush()
    {
         UnityEngine.iOS.NotificationServices.RegisterForNotifications(
           UnityEngine.iOS.  NotificationType.Badge | UnityEngine.iOS.  NotificationType.Alert | 
           UnityEngine.iOS.  NotificationType.Sound);
        //清空所有推送
        CleanNotification();
        //设置新推送
        int year = System.DateTime.Now.Year;
        int month = System.DateTime.Now.Month;
        int day = System.DateTime.Now.Day;
      
        System.DateTime newDate = new System.DateTime(year, month, day, 21, 30, 0);
        System .DateTime dt = newDate.AddDays( 6 - (int)System.DateTime.Now.DayOfWeek );

        string str = gDefine.GetStr(407);

        NotificationMessage(str, dt);
        
    }

	//清空所有本地消息
    void CleanNotification()
    {
        UnityEngine.iOS.LocalNotification l = new UnityEngine.iOS.LocalNotification();
        l.applicationIconBadgeNumber = -1;
        UnityEngine.iOS.NotificationServices.PresentLocalNotificationNow(l);
        //Invoke("WaitOneFrameClear",0);

        UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
        UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
    }

    public void NotificationMessage(string message, System.DateTime newDate)
    {
        //推送时间需要大于当前时间
        if (newDate > System.DateTime.Now)
        {
            UnityEngine.iOS.LocalNotification localNotification = new UnityEngine.iOS.LocalNotification();
            localNotification.fireDate = newDate;
            localNotification.alertBody = message;
            localNotification.applicationIconBadgeNumber = 1;
            localNotification.hasAction = true;
            
            localNotification.soundName = UnityEngine.iOS.LocalNotification.defaultSoundName;
            UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(localNotification);
        }
    }
}
