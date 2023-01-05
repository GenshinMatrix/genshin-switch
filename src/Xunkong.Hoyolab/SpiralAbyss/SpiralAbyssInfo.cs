﻿namespace Xunkong.Hoyolab.SpiralAbyss;

public class SpiralAbyssInfo
{

    [JsonIgnore]
    public int Id { get; set; }

    public int Uid { get; set; }

    [JsonPropertyName("schedule_id")]
    public int ScheduleId { get; set; }


    [JsonPropertyName("start_time"), JsonConverter(typeof(SpiralAbyssTimeJsonConverter))]
    public DateTimeOffset StartTime { get; set; }


    [JsonPropertyName("end_time"), JsonConverter(typeof(SpiralAbyssTimeJsonConverter))]
    public DateTimeOffset EndTime { get; set; }


    [JsonPropertyName("total_battle_times")]
    public int TotalBattleCount { get; set; }


    [JsonPropertyName("total_win_times")]
    public int TotalWinCount { get; set; }


    [JsonPropertyName("max_floor")]
    public string? MaxFloor { get; set; }

    /// <summary>
    /// 出战最多
    /// </summary>
    [JsonPropertyName("reveal_rank")]
    public List<SpiralAbyssRank> RevealRank { get; set; }

    /// <summary>
    /// 击破最多
    /// </summary>
    [JsonPropertyName("defeat_rank")]
    public List<SpiralAbyssRank> DefeatRank { get; set; }

    /// <summary>
    /// 伤害最高
    /// </summary>
    [JsonPropertyName("damage_rank")]
    public List<SpiralAbyssRank> DamageRank { get; set; }

    /// <summary>
    /// 承伤最高
    /// </summary>
    [JsonPropertyName("take_damage_rank")]
    public List<SpiralAbyssRank> TakeDamageRank { get; set; }

    /// <summary>
    /// 元素战技最多
    /// </summary>
    [JsonPropertyName("normal_skill_rank")]
    public List<SpiralAbyssRank> NormalSkillRank { get; set; }

    /// <summary>
    /// 元素爆发最多
    /// </summary>
    [JsonPropertyName("energy_skill_rank")]
    public List<SpiralAbyssRank> EnergySkillRank { get; set; }


    [JsonPropertyName("floors")]
    public List<SpiralAbyssFloor> Floors { get; set; }


    [JsonPropertyName("total_star")]
    public int TotalStar { get; set; }


    [JsonPropertyName("is_unlock")]
    public bool IsUnlock { get; set; }

}
