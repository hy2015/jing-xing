# 球之积分勇者 (Sphere Calcus Hero) - 游戏常量
# Jing-Xing Godot 版本

class_name GameConstants

# 物理常量
const GRAVITY: float = 20.0
const PLAYER_SPEED: float = 7.0
const PLAYER_JUMP_FORCE: float = 12.0
const MAX_FALL_SPEED: float = 15.0
const TILE_SIZE: float = 1.0

# 游戏状态
enum GameState { MENU, PLAYING, PAUSED, GAME_OVER, CALCULUS_CHALLENGE }

# 变色恐龙颜色/能力
enum ChameleonColor {
	RED,        # 巨力 - 破坏墙壁
	ORANGE,     # 火焰 - 烧毁障碍物
	YELLOW,     # 光 - 光剑攻击
	GREEN,      # 磁力 - 吸引收集品
	LIGHT_BLUE, # 闪电 - 电击敌人
	DEEP_BLUE,  # 潜水 - 水下通行/水炮
	PURPLE,     # 飞翔 - 带队飞行
	WHITE,      # 寒冰 - 冻结/绝对零度
	BLACK       # 黑洞 - 引力操控
}

# 拓扑变形怪形态
enum TopologyForm {
	SPHERE,     # 球 - 滚动通过狭窄通道
	TORUS,      # 圆环 - 盾牌防御
	TORUS2,     # 双洞圆环 - 传送门
	TORUS3,     # 三洞圆环 - 三重传送门
	MOBIUS,     # 莫比乌斯环 - 扭曲空间
	KLEIN       # 克莱因瓶 - 困住敌人
}

# 敌人类型
enum EnemyType { BEETLE, BEE, ANT, SPIDER }

# 收集品类型
enum CollectibleType { GEM, HEART, KEY }

# 颜色定义
const RED_COLOR := Color(0.86, 0.20, 0.20)
const ORANGE_COLOR := Color(1.0, 0.65, 0.0)
const YELLOW_COLOR := Color(1.0, 1.0, 0.0)
const GREEN_COLOR := Color(0.20, 0.78, 0.20)
const LIGHT_BLUE_COLOR := Color(0.39, 0.71, 1.0)
const DEEP_BLUE_COLOR := Color(0.12, 0.31, 0.63)
const PURPLE_COLOR := Color(0.63, 0.20, 0.86)
const WHITE_COLOR := Color.WHITE
const BLACK_COLOR := Color(0.24, 0.24, 0.24)

# 随从颜色名称
const CHAMELEON_COLOR_NAMES: Array = [
	"红色", "橙色", "黄色",
	"绿色", "浅蓝", "深蓝",
	"紫色", "白色", "黑色"
]

# 随从能力名称
const CHAMELEON_ABILITY_NAMES: Array = [
	"巨力", "火焰", "光",
	"磁力", "闪电", "潜水",
	"飞翔", "寒冰", "黑洞"
]

# 拓扑形态名称
const TOPOLOGY_FORM_NAMES: Array = [
	"球", "圆环", "双洞圆环", "三洞圆环", "莫比乌斯环", "克莱因瓶"
]

# 颜色数组
const CHAMELEON_COLORS: Array = [
	RED_COLOR, ORANGE_COLOR, YELLOW_COLOR, GREEN_COLOR,
	LIGHT_BLUE_COLOR, DEEP_BLUE_COLOR, PURPLE_COLOR, WHITE_COLOR, BLACK_COLOR
]
