# 球之积分勇者 - 蜜蜂敌人
# 空中飞行型敌人

extends EnemyBase

@export var fly_speed: float = 2.0
@export var float_amplitude: float = 1.0
@export var float_frequency: float = 1.0

var float_timer: float = 0.0
var start_position: Vector2

func _ready() -> void:
	super._ready()
	health = 30
	max_health = 30
	move_speed = fly_speed
	damage = 8
	patrol_range = 4.0
	enemy_color = Color(1.0, 0.78, 0.0)
	start_position = global_position
	if sprite:
		sprite.modulate = enemy_color

func patrol(delta: float) -> void:
	# 上下浮动
	float_timer += delta * float_frequency
	var y_offset := sin(float_timer) * float_amplitude

	# 水平移动
	var move_x := move_speed * direction * delta
	global_position = Vector2(
		global_position.x + move_x,
		start_position.y + y_offset
	)

	# 检查巡逻边界
	if absf(global_position.x - start_x) > patrol_range:
		direction *= -1

	# 更新朝向
	facing_right = direction > 0
	if sprite:
		sprite.flip_h = not facing_right
