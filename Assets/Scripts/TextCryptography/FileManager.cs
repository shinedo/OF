using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
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
        if (isDataInit)
            return;
        isDataInit = true;
        InitBundle();
    }

    public static void InitBundle()
    {
        if (_initAssetBundle)
        {
            _initAssetBundle = true;
            
            ParserFromTxtFile<WeaponInfo>(_weaponInfoList);
            
        }
    }

    public static void ParserFromTxtFile<T>(List<T> list, bool refResource = false)
    {
        string asset = null;

        //获取文件路径
        string file = ((DataPathAttribute) Attribute.GetCustomAttribute(typeof(T), typeof(DataPathAttribute))).filePath;

        if (refResource)
            asset = Resources.Load<TextAsset>(file).text;
        else
            File.ReadAllText(Util.DataPath + file + ".txt");

        StringReader reader = null;
        try
        {
            bool isHeadLine = true;
            string[] headLine = null;
            string stext = string.Empty;
            reader = new StringReader(asset);
            while ((stext = reader.ReadLine()) != null)
            {
                if (isHeadLine)
                {
                    headLine = stext.Split(',');
                    isHeadLine = false;
                }
                else
                {
                    string[] data = stext.Split(',');
                    list.Add(CreateDataModule<T>(headLine.ToList(), data));
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("file:" + file + ",msg:" + e.Message);
        }
        finally
        {
            if (reader != null)
                reader.Close();
        }
    }

    private static T CreateDataModule<T>(List<string> headLine, string[] data)
    {
        T result = Activator.CreateInstance<T>();
        FieldInfo[] fis = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var fi in fis)
        {
            string column = headLine.Where(x => x == fi.Name).FirstOrDefault();
            if (!string.IsNullOrEmpty(column))
            {
                string baseValue = data[headLine.IndexOf(column)];
                object setValueObj = null;
                Type setValueType = fi.FieldType;
                if (setValueType.Equals(typeof(short)))
                    setValueObj = string.IsNullOrEmpty(baseValue.Trim()) ? (short) 0 : Convert.ToInt16(baseValue);
                else if (setValueType.Equals(typeof(int)))
                    setValueObj = string.IsNullOrEmpty(baseValue.Trim()) ? 0 : Convert.ToInt32(baseValue);
                else if (setValueType.Equals(typeof(long)))
                    setValueObj = string.IsNullOrEmpty(baseValue.Trim()) ? 0 : Convert.ToInt64(baseValue);
                else if (setValueObj.Equals(typeof(float)))
                    setValueObj = string.IsNullOrEmpty(baseValue.Trim()) ? 0 : Convert.ToSingle(baseValue);
                else if (setValueObj.Equals(typeof(double)))
                    setValueObj = string.IsNullOrEmpty(baseValue.Trim()) ? 0 : Convert.ToDouble(baseValue);
                else if (setValueObj.Equals(typeof(bool)))
                    setValueObj = string.IsNullOrEmpty(baseValue.Trim()) ? false : Convert.ToBoolean(baseValue);
                else if (setValueObj.Equals(typeof(byte)))
                    setValueObj = string.IsNullOrEmpty(baseValue.Trim()) ? 0 : Convert.ToByte(baseValue);
                else
                    setValueObj = baseValue;
                fi.SetValue(result, setValueObj);
            }
        }

        return result;
    }

    public static List<WeaponInfo> FindWeaponInfoList() => _weaponInfoList;

    private static string OBFS(string str)
    {
        int length = str.Length;
        var array = new char[length];

        for (int i = 0; i < array.Length; i++)
        {
            char c = str[i];
            var b = (byte) (c ^ length - i);
            var b2 = (byte) (c >> 8 ^ i);
            array[i] = (char) (b2 << 8 | b);
        }
        return new string(array);
    }

    public static string GetSHA512Pwd(string pwd)
    {
        byte[] bytes = Encoding.UTF7.GetBytes(pwd);
        byte[] result;
        SHA512 shaM = new SHA512Managed();
        result = shaM.ComputeHash(bytes);
        StringBuilder sb = new StringBuilder();
        foreach (var num in result)
        {
            sb.AppendFormat("{0:x2", num);
        }

        return sb.ToString();
    }

}

/// <summary>
/// 注释，各个数据对应的文件
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class DataPathAttribute : Attribute
{
    public string filePath { get; set; }

    public DataPathAttribute(string _filePath)
    {
        filePath = _filePath;
    }
}