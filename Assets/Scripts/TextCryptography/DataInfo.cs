using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 武器数据模型

[Serializable]
[DataPath(FileManager.dataFolder+"weapon")]
public class  WeaponInfo
{
    public int Id;
    public int Type;
    public string Name;
    public string Description;
    public string Resources;
    public string Quality;
    public int Attack;
    public int UpgradeAttack;
    public int Magazine;
    public int Range;
    public int Speed;
    public float RateOfFire;
}

#endregion

#region 用户数据

[Serializable]
public class UserInfo
{
    public int Id;
    public string Name;
    public int Age;
    public string Sex;
}

#endregion