﻿namespace Xunkong.Hoyolab.SpiralAbyss
{
    /// <summary>
    /// 深境螺旋最值统计
    /// </summary>
    public class SpiralAbyssRank
    {

        [JsonIgnore]
        public int Id { get; set; }


        [JsonPropertyName("avatar_id")]
        public int AvatarId { get; set; }


        [JsonPropertyName("avatar_icon")]
        public string AvatarIcon { get; set; }


        [JsonPropertyName("value")]
        public int Value { get; set; }


        [JsonPropertyName("rarity")]
        public int Rarity { get; set; }


    }
}
