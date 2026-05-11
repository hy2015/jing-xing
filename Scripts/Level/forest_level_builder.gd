# 球之积分勇者 - 森林关卡构建器
# 构建第一关: 森林

extends Node2D

@export_group("预制体场景")
@export var platform_scene: PackedScene
@export var hero_scene: PackedScene
@export var chameleon_scene: PackedScene
@export var topology_scene: PackedScene
@export var ginseng_scene: PackedScene
@export var beetle_scene: PackedScene
@export var bee_scene: PackedScene
@export var ant_scene: PackedScene
@export var spider_scene: PackedScene
@export var gem_scene: PackedScene
@export var heart_scene: PackedScene
@export var key_scene: PackedScene
@export var breakable_block_scene: PackedScene
@export var thorns_scene: PackedScene
@export var switch_scene: PackedScene
@export var gate_scene: PackedScene

@export_group("关卡设置")
@export var ground_y: float = -2.0
@export var tile_size: float = 1.0

func _ready() -> void:
	_build_level()

func _build_level() -> void:
	_create_ground()
	_create_platforms()
	_create_breakable_blocks()
	_create_thorns()
	_create_enemies()
	_create_collectibles()
	_create_switches_and_gates()
	_create_player_characters()
	_setup_camera_bounds()

func _create_ground() -> void:
	# 主地面 (分段)
	_create_platform_segment(0, ground_y, 16, 1, 0)  # Ground
	_create_platform_segment(17, ground_y, 8, 1, 0)
	_create_platform_segment(26, ground_y, 22, 1, 0)

	# 地下层
	_create_platform_segment(0, ground_y - 1, 48, 1, 2)  # Dirt

func _create_platforms() -> void:
	# 区域1: 起始区
	_create_platform_segment(3, ground_y + 2, 2, 0.3, 1)    # Stone
	_create_platform_segment(5.5, ground_y + 3, 1.5, 0.3, 1)
	_create_platform_segment(4, ground_y + 4, 1.2, 0.3, 1)

	# 区域2: 需要破墙
	_create_platform_segment(7.5, ground_y + 2.5, 1, 0.3, 1)
	_create_platform_segment(9, ground_y + 3.5, 1, 0.3, 1)
	_create_platform_segment(10.5, ground_y + 4.5, 1.8, 0.3, 1)

	# 区域3: 高平台区
	_create_platform_segment(13, ground_y + 2, 1.2, 0.3, 1)
	_create_platform_segment(13, ground_y + 4, 1.2, 0.3, 1)
	_create_platform_segment(14.5, ground_y + 3, 1, 0.3, 1)

	# 区域4: 坑洞后
	_create_platform_segment(18, ground_y + 2.5, 1.5, 0.3, 1)
	_create_platform_segment(20, ground_y + 3.5, 1.2, 0.3, 1)
	_create_platform_segment(22, ground_y + 4.5, 2, 0.3, 1)

	# 区域5: 最终区
	_create_platform_segment(27, ground_y + 2, 1.5, 0.3, 1)
	_create_platform_segment(29.5, ground_y + 3, 1, 0.3, 1)
	_create_platform_segment(31, ground_y + 4, 1.5, 0.3, 1)
	_create_platform_segment(33.5, ground_y + 2.5, 1.2, 0.3, 1)
	_create_platform_segment(35.5, ground_y + 3.5, 2, 0.3, 1)

func _create_breakable_blocks() -> void:
	_create_breakable_block(7, ground_y + 1)
	_create_breakable_block(7, ground_y + 2)
	_create_breakable_block(25, ground_y + 1)
	_create_breakable_block(25, ground_y + 2)
	_create_breakable_block(25, ground_y + 3)

func _create_thorns() -> void:
	_create_thorns_at(6, ground_y + 1)
	_create_thorns_at(12, ground_y + 1)
	_create_thorns_at(24, ground_y + 1)
	_create_thorns_at(24.5, ground_y + 1)

func _create_enemies() -> void:
	# 甲虫 (地面巡逻)
	_create_enemy(beetle_scene, 3.5, ground_y + 1)
	_create_enemy(beetle_scene, 8.5, ground_y + 1)
	_create_enemy(beetle_scene, 18, ground_y + 1)
	_create_enemy(beetle_scene, 28, ground_y + 1)
	_create_enemy(beetle_scene, 32, ground_y + 1)

	# 蜜蜂 (空中飞行)
	_create_enemy(bee_scene, 5, ground_y + 5)
	_create_enemy(bee_scene, 11, ground_y + 6)
	_create_enemy(bee_scene, 19, ground_y + 5.5)
	_create_enemy(bee_scene, 30, ground_y + 6.5)
	_create_enemy(bee_scene, 34, ground_y + 5)

	# 蚂蚁 (群体行动)
	_create_enemy(ant_scene, 10, ground_y + 1)
	_create_enemy(ant_scene, 10.3, ground_y + 1)
	_create_enemy(ant_scene, 10.6, ground_y + 1)
	_create_enemy(ant_scene, 29, ground_y + 1)
	_create_enemy(ant_scene, 29.3, ground_y + 1)

	# 蜘蛛 (蛛网摆动)
	_create_enemy(spider_scene, 14, ground_y + 5)
	_create_enemy(spider_scene, 21.5, ground_y + 6)
	_create_enemy(spider_scene, 33, ground_y + 6.5)

func _create_collectibles() -> void:
	# 宝石
	_create_collectible(gem_scene, 3.7, ground_y + 3)
	_create_collectible(gem_scene, 4.4, ground_y + 5)
	_create_collectible(gem_scene, 9.5, ground_y + 4)
	_create_collectible(gem_scene, 11, ground_y + 5.5)
	_create_collectible(gem_scene, 15, ground_y + 4)
	_create_collectible(gem_scene, 18.5, ground_y + 3.5)
	_create_collectible(gem_scene, 20.5, ground_y + 4.5)
	_create_collectible(gem_scene, 22.5, ground_y + 5.5)
	_create_collectible(gem_scene, 27.5, ground_y + 3)
	_create_collectible(gem_scene, 30, ground_y + 4)
	_create_collectible(gem_scene, 31.5, ground_y + 5)
	_create_collectible(gem_scene, 34, ground_y + 3.5)
	_create_collectible(gem_scene, 36, ground_y + 4.5)

	# 心心
	_create_collectible(heart_scene, 6, ground_y + 3)
	_create_collectible(heart_scene, 13.5, ground_y + 5)
	_create_collectible(heart_scene, 18.5, ground_y + 5)

	# 钥匙
	_create_collectible(key_scene, 13.5, ground_y + 6)

func _create_switches_and_gates() -> void:
	_create_switch_at(38, ground_y + 1, 0)
	_create_gate_at(40, ground_y + 1, 0)

func _create_player_characters() -> void:
	if hero_scene:
		var hero = hero_scene.instantiate()
		hero.global_position = Vector2(1, ground_y + 2)
		hero.name = "Hero"
		add_child(hero)

	if chameleon_scene:
		var chameleon = chameleon_scene.instantiate()
		chameleon.global_position = Vector2(-0.5, ground_y + 2)
		chameleon.name = "ChameleonDinosaur"
		add_child(chameleon)

	if topology_scene:
		var topology = topology_scene.instantiate()
		topology.global_position = Vector2(-2, ground_y + 2)
		topology.name = "TopologyMonster"
		add_child(topology)

	if ginseng_scene:
		var ginseng = ginseng_scene.instantiate()
		ginseng.global_position = Vector2(-3.5, ground_y + 2)
		ginseng.name = "GinsengMonster"
		add_child(ginseng)

func _setup_camera_bounds() -> void:
	var camera = get_viewport().get_camera_2d()
	if camera and camera.has_method("set_bounds"):
		camera.set_bounds(-1, 48, -5, 15)

# 辅助方法
func _create_platform_segment(x: float, y: float, width: float, height: float, type: int) -> void:
	if platform_scene == null: return

	var platform = platform_scene.instantiate()
	platform.global_position = Vector2(x + width / 2.0, y + height / 2.0)
	platform.scale = Vector2(width, height)
	platform.name = "Platform_%d_%s_%s" % [type, x, y]
	if platform.has_method("set_platform_type"):
		platform.platform_type = type
	add_child(platform)

func _create_breakable_block(x: float, y: float) -> void:
	if breakable_block_scene == null: return

	var block = breakable_block_scene.instantiate()
	block.global_position = Vector2(x, y)
	block.name = "BreakableBlock_%s_%s" % [x, y]
	add_child(block)

func _create_thorns_at(x: float, y: float) -> void:
	if thorns_scene == null: return

	var thorns = thorns_scene.instantiate()
	thorns.global_position = Vector2(x, y)
	thorns.name = "Thorns_%s_%s" % [x, y]
	add_child(thorns)

func _create_enemy(scene: PackedScene, x: float, y: float) -> void:
	if scene == null: return

	var enemy = scene.instantiate()
	enemy.global_position = Vector2(x, y)
	enemy.name = "Enemy_%s_%s_%s" % [scene.resource_path.get_file().get_basename(), x, y]
	add_child(enemy)

func _create_collectible(scene: PackedScene, x: float, y: float) -> void:
	if scene == null: return

	var collectible = scene.instantiate()
	collectible.global_position = Vector2(x, y)
	collectible.name = "Collectible_%s_%s_%s" % [scene.resource_path.get_file().get_basename(), x, y]
	add_child(collectible)

func _create_switch_at(x: float, y: float, id: int) -> void:
	if switch_scene == null: return

	var switch_obj = switch_scene.instantiate()
	switch_obj.global_position = Vector2(x, y)
	switch_obj.name = "Switch_%d" % id
	if switch_obj.has_method("set_switch_id"):
		switch_obj.switch_id = id
	add_child(switch_obj)

func _create_gate_at(x: float, y: float, id: int) -> void:
	if gate_scene == null: return

	var gate_obj = gate_scene.instantiate()
	gate_obj.global_position = Vector2(x, y)
	gate_obj.name = "Gate_%d" % id
	if gate_obj.has_method("set_gate_id"):
		gate_obj.gate_id = id
	add_child(gate_obj)
