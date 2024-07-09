using ExcelDataReader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;



public class LanguageScriptobject : ScriptableObject
{
    [SerializeField]
    public List<LanguageData> datas = new List<LanguageData>();
    public string GetLanguageStr(LanguageType type, LanguageKey key, params object[] para)
    {
        int typeInt = (int)type;
        int keyInt = (int)key;
        if (datas.Count > typeInt)
        {
            if (datas[typeInt].datas.Count > keyInt)
            {
                return string.Format(datas[typeInt].datas[keyInt], para);
            }
            else
            {
                return $"[No found key {key}]";
            }
        }
        return $"[no found type {type}]";
    }
}

