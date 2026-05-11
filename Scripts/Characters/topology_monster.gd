# 球之积分勇者 - 拓扑变形怪
# 6种拓扑形态，每种形态有不同能力

extends FollowerBase

@export var current_form_index: int = 0
@export var ability_cooldown: float = 1.5

var last_ability_time: float = -1.0
var portal_positions: Array[Vector2] = []
var portal_objects: Array[Node2D] = []

func _ready() -> void:
	super._ready()
	_update_form_visual()

func get_display_name() -> String:
	return "拓扑变形怪 (%s)" % GameConstants.TOPOLOGY_FORM_NAMES[current_form_index]

func get_ability_description() -> String:
	return GameConstants.TOPOLOGY_FORM_NAMES[current_form_index]

func get_form_name() -> String:
	return GameConstants.TOPOLOGY_FORM_NAMES[current_form_index]

# 切换形态
func cycle_form(forward: bool) -> void:
	if forward:
		current_form_index = (current_form_index + 1) % 6
	else:
		current_form_index = (current_form_index - 1 + 6) % 6
	_update_form_visual()

func _update_form_visual() -> void:
	if sprite == null: return

	if current_form_index == 0:
		sprite.modulate = GameConstants.PURPLE_COLOR
	elif current_form_index == 1:
		sprite.modulate = Color(0.7, 0.3, 0.9)
	elif current_form_index == 2:
		sprite.modulate = Color(0.5, 0.2, 0.8)
	elif current_form_index == 3:
		sprite.modulate = Color(0.4, 0.1, 0.7)
	elif current_form_index == 4:
		sprite.modulate = Color(0.8, 0.4, 1.0)
	elif current_form_index == 5:
		sprite.modulate = Color(0.3, 0.1, 0.5)

# 使用能力
func use_ability() -> void:
	var now := Time.get_ticks_msec() / 1000.0
	if now - last_ability_time < ability_cooldown:
		return
	last_ability_time = now

	var hero = GameManager.hero
	if hero == null: return

	if current_form_index == 0:
		_use_sphere_ability()
	elif current_form_index == 1:
		_use_torus_ability(hero)
	elif current_form_index == 2:
		_use_portal_ability(hero, 2)
	elif current_form_index == 3:
		_use_portal_ability(hero, 3)
	elif current_form_index == 4:
		_use_mobius_ability(hero)
	elif current_form_index == 5:
		_use_klein_ability(hero)

# 球形态 - 滚动 (加速移动)
func _use_sphere_ability() -> void:
	var hero = GameManager.hero
	if hero != null and hero is CharacterBody2D:
		var dir := 1.0 if hero.is_facing_right() else -1.0
		hero.velocity.x += dir * 8.0
	GameManager.show_message_text("球形态 - 滚动加速!")

# 圆环 - 盾牌防御 (短暂无敌)
func _use_torus_ability(_hero: Node2D) -> void:
	GameManager.show_message_text("盾牌防御! 短暂无敌!")

# 双洞/三洞圆环 - 传送门
func _use_portal_ability(hero: Node2D, max_portals: int) -> void:
	if portal_positions.size() < max_portals:
		portal_positions.append(hero.global_position)
		if ParticleManager:
			ParticleManager.spawn_portal_effect(hero.global_position)
		GameManager.show_message_text("传送门 %d/%d 已放置" % [portal_positions.size(), max_portals])
	else:
		if portal_positions.size() >= 2:
			hero.global_position = portal_positions[0]
			if ParticleManager:
				ParticleManager.spawn_portal_effect(hero.global_position)
			GameManager.show_message_text("传送!")
		portal_positions.clear()
		for portal in portal_objects:
			if portal != null:
				portal.queue_free()
		portal_objects.clear()

# 莫比乌斯环 - 扭曲空间 (交换敌人位置)
func _use_mobius_ability(hero: Node2D) -> void:
	var space_state := get_world_2d().direct_space_state
	var query := PhysicsShapeQueryParameters2D.new()
	var shape := CircleShape2D.new()
	shape.radius = 4.0
	query.shape = shape
	query.transform = Transform2D(0, hero.global_position)
	query.collision_mask = 2
	var results := space_state.intersect_shape(query)

	var enemy_list: Array = []
	for result in results:
		var enemy = result["collider"]
		if enemy.has_method("is_alive") and enemy.is_alive():
			enemy_list.append(enemy)

	# 交换相邻敌人位置
	for i in range(0, enemy_list.size() - 1, 2):
		var temp: Vector2 = enemy_list[i].global_position
		enemy_list[i].global_position = enemy_list[i + 1].global_position
		enemy_list[i + 1].global_position = temp

	GameManager.show_message_text("扭曲空间! 敌人位置互换!")

# 克莱因瓶 - 困住敌人
func _use_klein_ability(hero: Node2D) -> void:
	var space_state := get_world_2d().direct_space_state
	var query := PhysicsShapeQueryParameters2D.new()
	var shape := CircleShape2D.new()
	shape.radius = 3.0
	query.shape = shape
	query.transform = Transform2D(0, hero.global_position)
	query.collision_mask = 2
	var results := space_state.intersect_shape(query)

	for result in results:
		var enemy = result["collider"]
		if enemy.has_method("is_alive") and enemy.is_alive():
			enemy.trap_enemy(5.0)
			if ParticleManager:
				ParticleManager.spawn_trap_effect(enemy.global_position)

	GameManager.show_message_text("克莱因瓶! 困住敌人!")

func _exit_tree() -> void:
	for portal in portal_objects:
		if portal != null:
			portal.queue_free()
