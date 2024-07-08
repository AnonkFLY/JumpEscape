using ExcelDataReader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;



public enum LanguageKey
{
    QuickPress,
    LongPress,
    Revieve,
    LevelComplate,
    CrazyMode,
    NormalMode,
    MotivationValue_1,
    MotivationValue_2,
    MotivationValue_3,
    MotivationValue_4,
    MotivationValue_5,
    MotivationValue_6,
    MotivationValue_7,
    MotivationValue_8,
    MotivationValue_9,
    MotivationValue_10,
    MotivationValue_11,
    MotivationValue_12,
    MotivationValue_13,
    MotivationValue_14,
    MotivationValue_15,
}
public enum LanguageType
{
    English,
    Chinese,
}
[Serializable]
public struct LanguageData
{
    public List<string> datas;
    public LanguageType type;
}
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
    [MenuItem("Tools/Create Language File")]
    public static void CreateLanguageObject()
    {
        string path = EditorUtility.OpenFilePanel("Select Excel File", "", "xls,xlsx");
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("No Excel file selected.");
            return;
        }
        using (var fileStream = File.Open(path, FileMode.Open))
        {
            using (var reader = ExcelReaderFactory.CreateReader(fileStream))
            {
                var languageDataObj = ScriptableObject.CreateInstance<LanguageScriptobject>();

                //Dictionary<LanguageKey, string> tempDic;

                for (int i = 0; i < reader.RowCount; ++i)
                {
                    reader.Read();
                    for (int j = 0; j < reader.FieldCount; j++)
                    {
                        //if (!languageDataObj.languageData.TryGetValue((LanguageType)j, out tempDic))
                        //{
                        //    tempDic = new Dictionary<LanguageKey, string>();
                        //    languageDataObj.languageData.Add((LanguageType)j, tempDic);
                        //}
                        //tempDic.Add((LanguageKey)i, reader.GetString(j));
                        if (languageDataObj.datas.Count <= j)
                        {
                            languageDataObj.datas.Add(new LanguageData() { datas = new List<string>(), type = (LanguageType)j }); ;
                        }
                        languageDataObj.datas[j].datas.Add(reader.GetString(j));
                        Debug.Log($"Add {(LanguageType)j}[{(LanguageKey)i}]:{languageDataObj.datas[j].datas[i]}");
                    }
                }
                string savePath = "Assets/ResourcesData/OtherData/LanguageData.asset";
                AssetDatabase.CreateAsset(languageDataObj, savePath);
                AssetDatabase.SaveAssets();
            }

        }
    }

}

