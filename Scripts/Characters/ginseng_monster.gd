# 球之积分勇者 - 绿色人参怪
# 缠绕控制敌人 + 微积分挑战

extends FollowerBase

@export var vine_range: float = 4.0
@export var vine_duration: float = 3.0
@export var ability_cooldown: float = 2.0

var last_ability_time: float = -1.0
var controlled_enemy = null

func _ready() -> void:
	super._ready()
	if sprite:
		sprite.modulate = Color(0.24, 0.70, 0.44)

func get_display_name() -> String:
	return "绿色人参怪"

func get_ability_description() -> String:
	return "[F] 缠绕控制+微积分挑战"

# 使用能力
func use_ability() -> void:
	var now := Time.get_ticks_msec() / 1000.0
	if now - last_ability_time < ability_cooldown:
		return
	last_ability_time = now

	var hero = GameManager.hero
	if hero == null: return

	# 寻找最近的敌人
	var closest_enemy = _find_closest_enemy(hero.global_position)

	if closest_enemy != null:
		_control_enemy(closest_enemy)
		GameManager.show_message_text("缠绕控制敌人!")

	# 触发微积分挑战
	_trigger_calculus_challenge()

func _process(delta: float) -> void:
	super._process(delta)

	if controlled_enemy != null and controlled_enemy.has_method("is_alive") and not controlled_enemy.is_alive():
		controlled_enemy = null

func _find_closest_enemy(position: Vector2):
	var space_state := get_world_2d().direct_space_state
	var query := PhysicsShapeQueryParameters2D.new()
	var shape := CircleShape2D.new()
	shape.radius = vine_range
	query.shape = shape
	query.transform = Transform2D(0, position)
	query.collision_mask = 2
	var results := space_state.intersect_shape(query)

	var closest = null
	var closest_dist := 1e10

	for result in results:
		var enemy = result["collider"]
		if enemy.has_method("is_alive") and enemy.is_alive() and not enemy.is_controlled():
			var dist: float = position.distance_to(enemy.global_position)
			if dist < closest_dist:
				closest_dist = dist
				closest = enemy

	return closest

func _control_enemy(enemy) -> void:
	enemy.vine_wrap(vine_duration)
	controlled_enemy = enemy

	if ParticleManager:
		ParticleManager.spawn_vine_effect(enemy.global_position)

func _trigger_calculus_challenge() -> void:
	# 微积分挑战通过 UIManager 处理
	pass
