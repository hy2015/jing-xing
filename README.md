# 球之积分勇者 (Sphere Calcus Hero) - Unity 版本

**Jing-Xing** - 横版过关平台游戏

## 游戏简介

勇者带着三个独特随从在神秘森林中冒险的横版过关游戏。

## 角色设定

### 勇者 (Hero)
- 玩家直接控制
- 基础移动、跳跃、攻击

### 随从1: 变色恐龙 (Chameleon Dinosaur)
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

### 随从2: 拓扑变形怪 (Topological Deformation Monster)
6种拓扑形态:

| 形态 | 能力 |
|------|------|
| 球 | 滚动通过狭窄通道 |
| 圆环 | 盾牌防御 |
| 双洞圆环 | 创建传送门 |
| 三洞圆环 | 三重传送门 |
| 莫比乌斯环 | 扭曲空间解谜 |
| 克莱因瓶 | 困住敌人 |

### 随从3: 绿色人参怪 (Green Ginseng Monster)
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
├── Assets/
│   ├── Scripts/
│   │   ├── Core/           # 核心系统
│   │   │   ├── GameConstants.cs    # 游戏常量
│   │   │   ├── GameManager.cs      # 游戏管理器
│   │   │   └── CameraFollow.cs     # 相机跟随
│   │   ├── Characters/     # 角色脚本
│   │   │   ├── Hero.cs             # 勇者
│   │   │   ├── FollowerBase.cs     # 随从基类
│   │   │   ├── ChameleonDinosaur.cs # 变色恐龙
│   │   │   ├── TopologyMonster.cs  # 拓扑变形怪
│   │   │   └── GinsengMonster.cs   # 人参怪
│   │   ├── Enemies/        # 敌人脚本
│   │   │   ├── Enemy.cs            # 敌人基类
│   │   │   ├── Beetle.cs           # 甲虫
│   │   │   ├── Bee.cs              # 蜜蜂
│   │   │   ├── Ant.cs              # 蚂蚁
│   │   │   └── Spider.cs           # 蜘蛛
│   │   ├── Level/          # 关卡元素
│   │   │   ├── Platform.cs         # 平台
│   │   │   ├── Collectible.cs      # 收集品
│   │   │   ├── BreakableBlock.cs   # 可破坏方块
│   │   │   ├── Obstacle.cs         # 障碍物
│   │   │   ├── Switch.cs           # 开关
│   │   │   ├── Gate.cs             # 门
│   │   │   └── ForestLevelBuilder.cs # 森林关卡构建器
│   │   ├── UI/             # UI脚本
│   │   │   ├── UIManager.cs        # UI管理器
│   │   │   └── CalculusChallenge.cs # 微积分挑战
│   │   └── Utils/          # 工具脚本
│   │       └── ParticleManager.cs  # 粒子管理器
│   ├── Scenes/             # 场景文件
│   ├── Prefabs/            # 预制体
│   ├── Materials/          # 材质
│   └── Sprites/            # 精灵图
├── ProjectSettings/        # Unity项目设置
└── Packages/               # Unity包管理
```

## 如何使用

### 1. 在 Unity 中打开项目
1. 打开 Unity Hub
2. 点击 "Open" 按钮
3. 选择 `jing-xing` 文件夹
4. 等待 Unity 导入项目

### 2. 设置场景
1. 创建新场景或打开现有场景
2. 创建空对象，命名为 "GameManager"，添加 `GameManager` 脚本
3. 创建空对象，命名为 "ParticleManager"，添加 `ParticleManager` 脚本
4. 创建空对象，命名为 "ForestLevelBuilder"，添加 `ForestLevelBuilder` 脚本
5. 创建 Canvas，添加 `UIManager` 和 `CalculusChallenge` 脚本

### 3. 创建预制体
需要为以下对象创建预制体:
- Hero (带 Hero 脚本、Rigidbody2D、BoxCollider2D、SpriteRenderer)
- ChameleonDinosaur (带 ChameleonDinosaur 脚本)
- TopologyMonster (带 TopologyMonster 脚本)
- GinsengMonster (带 GinsengMonster 脚本)
- Beetle, Bee, Ant, Spider (各带对应脚本)
- Platform (带 Platform 脚本)
- Collectible (带 Collectible 脚本)
- BreakableBlock (带 BreakableBlock 脚本)
- Thorns (带 Obstacle 脚本)
- Switch (带 Switch 脚本)
- Gate (带 Gate 脚本)

### 4. 运行游戏
1. 打开场景
2. 点击 Play 按钮
3. 开始游戏!

## 功能特性

- ✅ 勇者移动、跳跃、攻击
- ✅ 3个随从，各有独特能力
- ✅ 变色恐龙9种颜色能力
- ✅ 拓扑变形怪6种形态
- ✅ 人参怪缠绕控制+微积分挑战
- ✅ 4种敌人类型 (甲虫、蜜蜂、蚂蚁、蜘蛛)
- ✅ 收集品系统 (宝石、心心、钥匙)
- ✅ 可破坏墙壁和荆棘障碍
- ✅ 开关和门解谜
- ✅ 粒子效果系统
- ✅ UI系统 (血条、分数、消息)
- ✅ 相机跟随系统
- ✅ 游戏状态管理 (菜单、暂停、游戏结束)

## 技术栈

- Unity 2022.3+ LTS
- C# 
- 2D Physics
- TextMeshPro (UI文本)
- Unity UI (UGUI)

## 从 Python 版本迁移

此 Unity 版本基于原始 Python + Pygame 版本 (`sphere-calcus/`) 移植。

主要变化:
- 从 Pygame Surface 绘制 → Unity SpriteRenderer
- 从 Pygame 碰撞检测 → Unity Physics2D
- 从 Pygame 事件循环 → Unity Update/Input 系统
- 从单文件结构 → 组件化架构

## 开发计划

- [ ] 添加精灵图资源
- [ ] 添加音效和背景音乐
- [ ] 添加更多关卡
- [ ] 添加存档系统
- [ ] 添加成就系统
- [ ] 优化移动端适配

---

**JingXing Studio** © 2026
