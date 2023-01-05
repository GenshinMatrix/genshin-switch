﻿namespace Xunkong.Hoyolab.SpiralAbyss
{
    /// <summary>
    /// 深境螺旋角色
    /// </summary>
    public class SpiralAbyssAvatar
    {

        [JsonIgnore]
        public int Id { get; set; }


        [JsonPropertyName("id")]
        public int AvatarId { get; set; }


        [JsonPropertyName("icon")]
        public string Icon { get; set; }


        [JsonPropertyName("level")]
        public int Level { get; set; }


        [JsonPropertyName("rarity")]
        public int Rarity { get; set; }


    }
}
