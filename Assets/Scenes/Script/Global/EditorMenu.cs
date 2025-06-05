using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class EditorMenu : Editor
{
    private static string dataFolder = DataManager.dataFolder;

    [MenuItem("DataView/DataView")]
    public static void DataView(){
        Debug.Log(dataFolder);
        if (Directory.Exists(dataFolder)){
            EditorUtility.RevealInFinder(dataFolder);
        } else {
            Debug.LogError("不存在存档文件夹！");
        }
    }
}
#endif