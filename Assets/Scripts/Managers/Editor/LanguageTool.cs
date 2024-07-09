using ExcelDataReader;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class LanguageTool
{
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
