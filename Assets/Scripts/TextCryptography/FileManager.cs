using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    private static bool _initAssetBundle;
    
    //静态数据
    private static List<WeaponInfo> _weaponInfoList = new List<WeaponInfo>();
    //数据文件位置
    public const string dataFolder = "/data/";
    //玩家数据
    public static UserInfo mUserInfo;

    private static bool isDataInit;

    public static void Init()
    {
        if(isDataInit)
            return;
        isDataInit = true;
        InitBundle();
    }

    public static void InitBundle()
    {
        if (_initAssetBundle)
        {
            _initAssetBundle = true;
            
            
        }
    }

    public static void ParserFromTxtFile<T>(List<T> list, bool refResource = false)
    {
        string asset = null;
        
        //获取文件路径
        string file = ((DataPathAttribute) Attribute.GetCustomAttribute(typeof(T), typeof(DataPathAttribute))).filePath;
        
        
    }
    
}

/// <summary>
/// 注释，各个数据对应的文件
/// </summary>
[AttributeUsage(AttributeTargets.Class,AllowMultiple = false,Inherited = false)]
public class DataPathAttribute : Attribute
{
    public string filePath { get; set; }

    public DataPathAttribute(string _filePath)
    {
        filePath = _filePath;
    }
}
