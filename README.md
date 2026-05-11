**hanché (璟星)** - 横版过关平台游戏 Godot 版本

## 游戏简介

勇者带着三个独特随从在神秘森林中冒险的横版过关游戏。

## 角色设定

### 勇者 (Hero)
- 玩家直接控制
- 基础移动、跳跃、攻击

### 随从1: 龙龙 (Chameleon Dinosaur)
9种颜色，每种颜色有不同能力:

| 颜色 | 能力 |
|------|------|
| 红色 | 巨力 - 可破坏墙壁 |
| 橙色 | 火焰 - 烧毁障碍物 |
| 黄色 | 光 - 光剑攻击敌人 |
| 绿色 | 磁力 - 吸引收集品 |
| 浅蓝色 | 闪电 - 电击敌人 |
| 深海色 | 潜水 - 高压水炮 |
| 紫色 | 飞翔 - 带领队伍飞行 |
| 白色 | 寒冰/绝对零度 - 冻结敌人 |
| 黑色 | 黑洞 - 引力操控 |

### 随从2: 软软 (Topological Deformation Monster)
6种拓扑形态:

| 形态 | 能力 |
|------|------|
| 球 | 滚动通过狭窄通道 |
| 圆环 | 盾牌防御 |
| 双洞圆环 | 创建传送门 |
| 三洞圆环 | 三重传送门 |
| 莫比乌斯环 | 扭曲空间解谜 |
| 克莱因瓶 | 困住敌人 |

### 随从3: 怪怪 (Green Ginseng Monster)
- 缠绕控制敌人
- 弹出微积分挑战窗口
- 答对可治疗+全屏伤害

## 操作说明

| 按键 | 功能 |
|------|------|
| A/D 或 左/右箭头 | 移动 |
| W 或 空格 | 跳跃 |
| 鼠标左键 或 J | 攻击 |
| 1/2/3 | 切换激活的随从 |
| Q/E | 变色恐龙切换颜色 / 拓扑怪切换形态 |
| F | 当前随从使用能力 |
| ESC | 暂停 |

## 项目结构

```
jing-xing/
├── project.godot              # Godot 项目配置
├── icon.svg                   # 项目图标
├── Scripts/
│   ├── Core/                  # 核心系统
│   │   ├── game_constants.gd  # 游戏常量
│   │   ├── game_manager.gd    # 游戏管理器 (自动加载)
│   │   └── camera_follow.gd   # 相机跟随
│   ├── Characters/            # 角色脚本
│   │   ├── hero.gd            # 勇者 (CharacterBody2D)
│   │   ├── follower_base.gd   # 随从基类
│   │   ├── chameleon_dinosaur.gd  # 龙龙
│   │   ├── topology_monster.gd    # 软软
│   │   └── ginseng_monster.gd     # 怪怪
│   ├── Enemies/               # 敌人脚本
│   │   ├── enemy.gd           # 敌人基类 (CharacterBody2D)
│   │   ├── beetle.gd          # 甲虫
│   │   ├── bee.gd             # 蜜蜂
│   │   ├── ant.gd             # 蚂蚁
│   │   └── spider.gd          # 蜘蛛
│   ├── Level/                 # 关卡元素
│   │   ├── platform.gd        # 平台 (StaticBody2D)
│   │   ├── collectible.gd     # 收集品 (Area2D)
│   │   ├── breakable_block.gd # 可破坏方块 (StaticBody2D)
│   │   ├── obstacle.gd        # 障碍物 (Area2D)
│   │   ├── switch.gd          # 开关 (Area2D)
│   │   ├── gate.gd            # 门 (StaticBody2D)
│   │   └── forest_level_builder.gd # 森林关卡构建器
│   ├── UI/                    # UI脚本
│   │   ├── ui_manager.gd      # UI管理器 (CanvasLayer)
│   │   └── calculus_challenge.gd  # 微积分挑战
│   └── Utils/                 # 工具脚本
│       └── particle_manager.gd    # 粒子管理器 (自动加载)
├── Scenes/                    # 场景文件 (.tscn)
├── Prefabs/                   # 预制体场景
└── Sprites/                   # 精灵图资源
```

## 如何使用

### 1. 在 Godot 中打开项目
1. 打开 Godot 4.3+
2. 点击 "Import" 按钮
3. 选择 `jing-xing/project.godot` 文件
4. 等待 Godot 导入项目

### 2. 设置场景
1. 创建主场景 (`Scenes/main.tscn`)
2. 添加 Node2D 根节点
3. 添加 `ForestLevelBuilder` 脚本节点
4. 添加 Camera2D 节点并附加 `camera_follow.gd`
5. 添加 CanvasLayer 节点用于 UI

### 3. 创建预制体场景
需要为以下对象创建 `.tscn` 场景:
- Hero (CharacterBody2D + hero.gd + Sprite2D + CollisionShape2D)
- 龙龙 (CharacterBody2D + chameleon_dinosaur.gd)
- 软软 (CharacterBody2D + topology_monster.gd)
- 怪怪 (CharacterBody2D + ginseng_monster.gd)
- Beetle, Bee, Ant, Spider (CharacterBody2D + 对应脚本)
- Platform (StaticBody2D + platform.gd)
- Collectible (Area2D + collectible.gd)
- BreakableBlock (StaticBody2D + breakable_block.gd)
- Thorns (Area2D + obstacle.gd)
- Switch (Area2D + switch.gd)
- Gate (StaticBody2D + gate.gd)

### 4. 运行游戏
1. 设置主场景
2. 点击 Play 按钮 (F5)
3. 开始游戏!

## 功能特性

- 勇者移动、跳跃、攻击
- 3个随从，各有独特能力
- 龙龙9种颜色能力
- 软软6种形态
- 怪怪缠绕控制+微积分挑战
- 4种敌人类型 (甲虫、蜜蜂、蚂蚁、蜘蛛)
- 收集品系统 (宝石、心心、钥匙)
- 可破坏墙壁和荆棘障碍
- 开关和门解谜
- 粒子效果系统
- UI系统 (血条、分数、消息)
- 相机跟随系统
- 游戏状态管理 (菜单、暂停、游戏结束)

## 技术栈

- Godot 4.6+
- GDScript
- 2D Physics (CharacterBody2D, Area2D, StaticBody2D)
- Godot UI (Control, Label, ProgressBar, Button)

## 从 Unity 版本迁移

此 Godot 版本基于 Unity C# 版本移植。

主要变化:
- C# → GDScript
- MonoBehaviour → CharacterBody2D / Area2D / StaticBody2D
- 单例模式 (static Instance) → Autoload (project.godot)
- C# 事件 (Action) → Godot 信号 (Signal)
- 协程 (IEnumerator) → await / create_timer
- Physics2D.BoxCastAll → PhysicsShapeQueryParameters2D
- Input.GetKey → Input.is_action_pressed
- SceneManager → get_tree().change_scene_to_file
- Time.timeScale → Engine.time_scale

## 开发计划

- [ ] 添加精灵图资源
- [ ] 添加音效和背景音乐
- [ ] 添加更多关卡
- [ ] 添加存档系统
- [ ] 添加成就系统
- [ ] 优化移动端适配

---

**Hanché Studio** © 2026
