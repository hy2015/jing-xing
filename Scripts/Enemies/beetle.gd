# 球之积分勇者 - 甲虫敌人
# 地面巡逻型敌人

extends EnemyBase

@export var patrol_speed: float = 1.2

func _ready() -> void:
	super._ready()
	health = 60
	max_health = 60
	move_speed = patrol_speed
	damage = 10
	patrol_range = 3.0
	enemy_color = Color(0.31, 0.24, 0.12)
	if sprite:
		sprite.modulate = enemy_color

# 甲虫特殊巡逻：地面行走，遇边缘折返
func patrol(delta: float) -> void:
	# 检查前方是否有地面
	var check_dir := Vector2.RIGHT if facing_right else Vector2.LEFT
	var check_pos: Vector2 = global_position + check_dir * 0.5

	var space_state := get_world_2d().direct_space_state
	var query := PhysicsRayQueryParameters2D.create(check_pos, check_pos + Vector2.DOWN * 1.0, 1)
	var result := space_state.intersect_ray(query)

	# 如果前方没有地面，折返
	if result.is_empty():
		direction *= -1

	super.patrol(delta)
