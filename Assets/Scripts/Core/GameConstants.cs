// 球之积分勇者 (Sphere Calcus Hero) - 游戏常量
// Jing-Xing Unity 版本

namespace JingXing
{
    public static class GameConstants
    {
        // 物理常量
        public const float Gravity = 20f;
        public const float PlayerSpeed = 7f;
        public const float PlayerJumpForce = 12f;
        public const float MaxFallSpeed = 15f;
        public const float TileSize = 1f;

        // 游戏状态
        public enum GameState
        {
            Menu,
            Playing,
            Paused,
            GameOver,
            CalculusChallenge
        }

        // 变色恐龙颜色/能力
        public enum ChameleonColor
        {
            Red,        // 巨力 - 破坏墙壁
            Orange,     // 火焰 - 烧毁障碍物
            Yellow,     // 光 - 光剑攻击
            Green,      // 磁力 - 吸引收集品
            LightBlue,  // 闪电 - 电击敌人
            DeepBlue,   // 潜水 - 水下通行/水炮
            Purple,     // 飞翔 - 带队飞行
            White,      // 寒冰 - 冻结/绝对零度
            Black       // 黑洞 - 引力操控
        }

        // 拓扑变形怪形态
        public enum TopologyForm
        {
            Sphere,     // 球 - 滚动通过狭窄通道
            Torus,      // 圆环 - 盾牌防御
            Torus2,     // 双洞圆环 - 传送门
            Torus3,     // 三洞圆环 - 三重传送门
            Mobius,     // 莫比乌斯环 - 扭曲空间
            Klein       // 克莱因瓶 - 困住敌人
        }

        // 敌人类型
        public enum EnemyType
        {
            Beetle,     // 甲虫 - 地面巡逻
            Bee,        // 蜜蜂 - 空中飞行
            Ant,        // 蚂蚁 - 群体行动
            Spider      // 蜘蛛 - 蛛网摆动
        }

        // 收集品类型
        public enum CollectibleType
        {
            Gem,    // 宝石 +50分
            Heart,  // 心心 +30HP
            Key     // 钥匙 +100分
        }

        // 颜色定义 (Unity Color)
        public static readonly UnityEngine.Color RedColor = new(0.86f, 0.20f, 0.20f);
        public static readonly UnityEngine.Color OrangeColor = new(1f, 0.65f, 0f);
        public static readonly UnityEngine.Color YellowColor = new(1f, 1f, 0f);
        public static readonly UnityEngine.Color GreenColor = new(0.20f, 0.78f, 0.20f);
        public static readonly UnityEngine.Color LightBlueColor = new(0.39f, 0.71f, 1f);
        public static readonly UnityEngine.Color DeepBlueColor = new(0.12f, 0.31f, 0.63f);
        public static readonly UnityEngine.Color PurpleColor = new(0.63f, 0.20f, 0.86f);
        public static readonly UnityEngine.Color WhiteColor = UnityEngine.Color.white;
        public static readonly UnityEngine.Color BlackColor = new(0.24f, 0.24f, 0.24f);

        // 随从颜色名称
        public static readonly string[] ChameleonColorNames = {
            "红色 - 巨力", "橙色 - 火焰", "黄色 - 光",
            "绿色 - 磁力", "浅蓝 - 闪电", "深海 - 潜水",
            "紫色 - 飞翔", "白色 - 寒冰", "黑色 - 黑洞"
        };

        // 拓扑形态名称
        public static readonly string[] TopologyFormNames = {
            "球", "圆环", "双洞圆环", "三洞圆环", "莫比乌斯环", "克莱因瓶"
        };

        // 颜色数组
        public static readonly UnityEngine.Color[] ChameleonColors = {
            RedColor, OrangeColor, YellowColor, GreenColor,
            LightBlueColor, DeepBlueColor, PurpleColor, WhiteColor, BlackColor
        };
    }
}
