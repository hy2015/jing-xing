# 球之积分勇者 - 蚂蚁敌人
# 群体行动型敌人

extends EnemyBase

@export var is_leader: bool = false
@export var group_follow_range: float = 3.0

var leader = null
var group: Array = []

func _ready() -> void:
	super._ready()

	if is_leader:
		health = 40
		max_health = 40
		move_speed = 1.8
		damage = 12
		enemy_color = Color(0.47, 0.24, 0.10)
	else:
		health = 20
		max_health = 20
		move_speed = 2.5
		damage = 8
		enemy_color = Color(0.39, 0.20, 0.08)

	patrol_range = 2.0
	if sprite:
		sprite.modulate = enemy_color

	# 寻找附近的领袖
	if not is_leader:
		_find_leader()

func _find_leader() -> void:
	var space_state := get_world_2d().direct_space_state
	var query := PhysicsShapeQueryParameters2D.new()
	var shape := CircleShape2D.new()
	shape.radius = group_follow_range
	query.shape = shape
	query.transform = Transform2D(0, global_position)
	query.collision_mask = 2
	var results := space_state.intersect_shape(query)

	for result in results:
		var collider = result["collider"]
		if collider is EnemyBase and collider != self and collider.has_method("is_ant_leader") and collider.is_ant_leader():
			leader = collider
			leader.add_ant_to_group(self)
			break

func is_ant_leader() -> bool:
	return is_leader

func add_ant_to_group(ant) -> void:
	if ant not in group:
		group.append(ant)

func patrol(delta: float) -> void:
	if not is_leader and leader != null and leader.is_alive():
		# 跟随领袖
		var leader_pos: Vector2 = leader.global_position
		var idx: int = group.find(self) if self in group else 0
		var follow_x: float = leader_pos.x + (idx + 1) * 0.8 * (-1 if leader.facing_right else 1)

		var target_pos := Vector2(follow_x, global_position.y)
		global_position = global_position.lerp(target_pos, move_speed * delta)

		# 更新朝向
		facing_right = leader.facing_right
		if sprite:
			sprite.flip_h = not facing_right
	else:
		# 独立巡逻
		super.patrol(delta)

# 领袖死亡时，群体成员变为独立
func die() -> void:
	if is_leader:
		for ant in group:
			if ant != null:
				ant.leader = null
	super.die()
