using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CMyStr
{
    class CMyStrNode
    {
        public string[] mStr = new string[(int)eType.Count];
    }

    public enum eType
    {
        Simple = 0,//简体
        Old=1,//繁体
        English, //英文
        Japanese,//日语
        Count,
    }

    Dictionary<string , CMyStrNode> mDict = new Dictionary<string, CMyStrNode>();
    CMyStrNode [] mStrArr = new CMyStrNode[500];

    bool mInit = false;


    public void Read(TextAsset Text)
    {
        string str = Text.text;
        string[] sepStr = new string[] { "^##" };
        string[] sepStr1 = new string[] { "^@@" };
        string[] itArr = str.Split(sepStr, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < itArr.Length; i++)
        {
           
            string[] valueArr = itArr[i].Split(sepStr1, StringSplitOptions.RemoveEmptyEntries);
            CMyStrNode n = new CMyStrNode();
            for(int j=0 ; j<(int)eType.Count; j++)
            {
                valueArr[j+1] = valueArr[j+1].Replace("\t",string.Empty);
                valueArr[j+1] = valueArr[j+1].Replace(" -","-");
                n.mStr[j] = valueArr[j+1];
            }
               
            if(!mDict.ContainsKey(n.mStr[0]))
                mDict.Add( n.mStr[0], n);

            valueArr[0] = valueArr[0].Replace("/n",string.Empty);
            int id = int.Parse(valueArr[0]);
            if( mStrArr.Length > id )
                mStrArr[id] = n;
            else
            {
                Debug.LogError("字符串数组越界！！！！！" + id.ToString());
            }

        }
        mInit = true;
    }

    public string Get(string SimpleStr, eType TextType )
    {
        CMyStrNode node;
        if( mDict.TryGetValue(SimpleStr, out node))
            return node.mStr[(int)TextType];
        else
            return "";
    }

    public string Get(int StrId, eType TextType)
    {
        if(!mInit)
            return "";
        CMyStrNode node = mStrArr[StrId];
        return node.mStr[(int)TextType];
    }

}
