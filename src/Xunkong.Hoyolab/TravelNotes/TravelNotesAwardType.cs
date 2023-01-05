﻿using System.ComponentModel;

namespace Xunkong.Hoyolab.TravelNotes;

/// <summary>
/// 旅行札记收入类型
/// </summary>
public enum TravelNotesAwardType
{

    None = 0,

    /// <summary>
    /// 原石
    /// </summary>
    [Description("原石")]
    Primogems = 1,

    /// <summary>
    /// 摩拉
    /// </summary>
    [Description("摩拉")]
    Mora = 2,

}
