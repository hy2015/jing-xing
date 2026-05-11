# 球之积分勇者 - 变色恐龙
# 9种颜色，每种颜色有不同能力

extends FollowerBase

@export var current_color_index: int = 0
@export var ability_range: float = 2.0
@export var ability_cooldown: float = 1.0

var last_ability_time: float = -1.0

func _ready() -> void:
	super._ready()
	_update_color_visual()

func get_display_name() -> String:
	return "变色恐龙 (%s)" % GameConstants.CHAMELEON_COLOR_NAMES[current_color_index]

func get_ability_description() -> String:
	return GameConstants.CHAMELEON_COLOR_NAMES[current_color_index]

func get_color_name() -> String:
	return GameConstants.CHAMELEON_COLOR_NAMES[current_color_index]

# 切换颜色
func cycle_color(forward: bool) -> void:
	if forward:
		current_color_index = (current_color_index + 1) % 9
	else:
		current_color_index = (current_color_index - 1 + 9) % 9
	_update_color_visual()

func _update_color_visual() -> void:
	if sprite:
		sprite.modulate = GameConstants.CHAMELEON_COLORS[current_color_index]

# 使用能力
func use_ability() -> void:
	var now := Time.get_ticks_msec() / 1000.0
	if now - last_ability_time < ability_cooldown:
		return
	last_ability_time = now

	var hero = GameManager.hero
	if hero == null: return

	if current_color_index == 0:
		_use_strength_ability(hero)
	elif current_color_index == 1:
		_use_fire_ability(hero)
	elif current_color_index == 2:
		_use_light_ability(hero)
	elif current_color_index == 3:
		_use_magnet_ability()
	elif current_color_index == 4:
		_use_lightning_ability(hero)
	elif current_color_index == 5:
		_use_diving_ability(hero)
	elif current_color_index == 6:
		_use_flying_ability()
	elif current_color_index == 7:
		_use_ice_ability(hero)
	elif current_color_index == 8:
		_use_black_hole_ability(hero)

# 红色 - 巨力破墙
func _use_strength_ability(hero: Node2D) -> void:
	var attack_dir := Vector2.RIGHT if hero.is_facing_right() else Vector2.LEFT
	var attack_pos: Vector2 = hero.global_position + attack_dir * 1.0

	var space_state := get_world_2d().direct_space_state
	var query := PhysicsShapeQueryParameters2D.new()
	var shape := RectangleShape2D.new()
	shape.size = Vector2(2.0, 1.0)
	query.shape = shape
	query.transform = Transform2D(0, attack_pos)
	query.collision_mask = 3  # platforms + obstacles
	var results := space_state.intersect_shape(query)

	for result in results:
		var collider = result["collider"]
		if collider.has_method("destroy_block"):
			collider.destroy_block()
			if ParticleManager:
				ParticleManager.spawn_break_effect(collider.global_position)

	GameManager.show_message_text("巨力破墙!")

# 橙色 - 火焰烧毁障碍物
func _use_fire_ability(hero: Node2D) -> void:
	var attack_dir := Vector2.RIGHT if hero.is_facing_right() else Vector2.LEFT
	var attack_pos: Vector2 = hero.global_position + attack_dir * 1.2

	var space_state := get_world_2d().direct_space_state
	var query := PhysicsShapeQueryParameters2D.new()
	var shape := RectangleShape2D.new()
	shape.size = Vector2(2.5, 1.0)
	query.shape = shape
	query.transform = Transform2D(0, attack_pos)
	query.collision_mask = 16  # obstacles layer
	var results := space_state.intersect_shape(query)

	for result in results:
		var collider = result["collider"]
		if collider.has_method("is_thorns") and collider.is_thorns():
			collider.destroy_obstacle()
			if ParticleManager:
				ParticleManager.spawn_fire_effect(collider.global_position)

	GameManager.show_message_text("火焰燃烧!")

# 黄色 - 光剑攻击
func _use_light_ability(hero: Node2D) -> void:
	var space_state := get_world_2d().direct_space_state
	var query := PhysicsShapeQueryParameters2D.new()
	var shape := CircleShape2D.new()
	shape.radius = 3.0
	query.shape = shape
	query.transform = Transform2D(0, hero.global_position)
	query.collision_mask = 2  # enemies layer
	var results := space_state.intersect_shape(query)

	for result in results:
		var enemy = result["collider"]
		if enemy.has_method("is_alive") and enemy.is_alive():
			enemy.take_damage(60)
			if ParticleManager:
				ParticleManager.spawn_light_effect(enemy.global_position)

	GameManager.show_message_text("光剑斩击! 照亮+攻击!")

# 绿色 - 磁力吸引
func _use_magnet_ability() -> void:
	GameManager.activate_magnet(3.0)

# 浅蓝 - 闪电
func _use_lightning_ability(hero: Node2D) -> void:
	var space_state := get_world_2d().direct_space_state
	var query := PhysicsShapeQueryParameters2D.new()
	var shape := CircleShape2D.new()
	shape.radius = 4.0
	query.shape = shape
	query.transform = Transform2D(0, hero.global_position)
	query.collision_mask = 2
	var results := space_state.intersect_shape(query)

	for result in results:
		var enemy = result["collider"]
		if enemy.has_method("is_alive") and enemy.is_alive():
			enemy.take_damage(50)
			if ParticleManager:
				ParticleManager.spawn_lightning_effect(enemy.global_position)

	GameManager.show_message_text("闪电链! 电击敌人!")

# 深海色 - 高压水炮
func _use_diving_ability(hero: Node2D) -> void:
	var attack_dir := Vector2.RIGHT if hero.is_facing_right() else Vector2.LEFT
	var attack_pos: Vector2 = hero.global_position + attack_dir * 1.5

	var space_state := get_world_2d().direct_space_state
	var query := PhysicsShapeQueryParameters2D.new()
	var shape := RectangleShape2D.new()
	shape.size = Vector2(4.0, 0.5)
	query.shape = shape
	query.transform = Transform2D(0, attack_pos)
	query.collision_mask = 2
	var results := space_state.intersect_shape(query)

	for result in results:
		var enemy = result["collider"]
		if enemy.has_method("is_alive") and enemy.is_alive():
			enemy.take_damage(40)
			# 击退效果
			if enemy is CharacterBody2D:
				enemy.velocity += attack_dir * 10.0
			if ParticleManager:
				ParticleManager.spawn_water_effect(enemy.global_position)

	GameManager.show_message_text("高压水炮! 击退敌人!")

# 紫色 - 飞翔
func _use_flying_ability() -> void:
	GameManager.activate_flying(3.0)

# 白色 - 寒冰/绝对零度
func _use_ice_ability(hero: Node2D) -> void:
	var absolute_zero := Input.is_action_pressed("shift")
	var range_val := 6.0 if absolute_zero else 3.5

	var space_state := get_world_2d().direct_space_state
	var query := PhysicsShapeQueryParameters2D.new()
	var shape := CircleShape2D.new()
	shape.radius = range_val
	query.shape = shape
	query.transform = Transform2D(0, hero.global_position)
	query.collision_mask = 2
	var results := space_state.intersect_shape(query)

	for result in results:
		var enemy = result["collider"]
		if enemy.has_method("is_alive") and enemy.is_alive():
			if absolute_zero:
				enemy.take_damage(9999)
				if ParticleManager:
					ParticleManager.spawn_absolute_zero_effect(enemy.global_position)
			else:
				enemy.freeze_enemy(3.0)
				if ParticleManager:
					ParticleManager.spawn_ice_effect(enemy.global_position)

	GameManager.show_message_text("绝对零度! 万物冻结秒杀!" if absolute_zero else "寒冰冻结!")

# 黑色 - 黑洞引力
func _use_black_hole_ability(hero: Node2D) -> void:
	var space_state := get_world_2d().direct_space_state
	var query := PhysicsShapeQueryParameters2D.new()
	var shape := CircleShape2D.new()
	shape.radius = 5.0
	query.shape = shape
	query.transform = Transform2D(0, hero.global_position)
	query.collision_mask = 2
	var results := space_state.intersect_shape(query)

	for result in results:
		var enemy = result["collider"]
		if enemy.has_method("is_alive") and enemy.is_alive():
			# 向勇者方向吸引
			var pull_dir: Vector2 = (hero.global_position - enemy.global_position).normalized()
			if enemy is CharacterBody2D:
				enemy.velocity += pull_dir * 8.0
			enemy.take_damage(15)
			if ParticleManager:
				ParticleManager.spawn_black_hole_effect(enemy.global_position)

	GameManager.show_message_text("黑洞引力! 吸引敌人!")
