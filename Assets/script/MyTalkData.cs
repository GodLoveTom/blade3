using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTalkData : MonoBehaviour
{
    TDGAAccount account;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Unity SDK  init begin ");
        TalkingDataGA.OnStart("2B827C6E6D75498BBB6D29908E18F678", "TalkingData");
        Debug.Log("Unity SDK  init completed ");

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void btn_LoginIn()
    {
        account = TDGAAccount.SetAccount(TalkingDataGA.GetDeviceId());
        account.SetAccountType(AccountType.ANONYMOUS);

    }


    public void Btn_FinishLVL()
    {
        // 玩家进入名称为“蓝色龙之领地”的关卡。
        TDGAMission.OnBegin("LVL1");
        // 玩家成功打过了关卡
        TDGAMission.OnCompleted("LVL1");
    }
}
