# 球之积分勇者 - 蜘蛛敌人
# 蛛网摆动型敌人

extends EnemyBase

@export var web_length: float = 3.0
@export var swing_speed: float = 1.0
@export var swing_amplitude: float = 1.5

var swing_timer: float = 0.0
var anchor_position: Vector2
var web_line: Line2D

func _ready() -> void:
	super._ready()
	health = 50
	max_health = 50
	move_speed = 0.0
	damage = 15
	patrol_range = 0.0
	enemy_color = Color(0.24, 0.24, 0.24)
	if sprite:
		sprite.modulate = enemy_color

	# 设置锚点
	anchor_position = global_position + Vector2.UP * web_length

	# 初始化蛛网线条
	web_line = Line2D.new()
	web_line.width = 2.0
	web_line.default_color = Color.WHITE
	web_line.add_point(anchor_position)
	web_line.add_point(global_position)
	add_child(web_line)

func patrol(delta: float) -> void:
	# 摆动运动
	swing_timer += delta * swing_speed

	var swing_x := sin(swing_timer) * swing_amplitude
	var swing_y := cos(swing_timer * 0.5) * web_length * 0.5

	global_position = Vector2(
		anchor_position.x + swing_x,
		anchor_position.y - web_length + swing_y
	)

	# 更新朝向
	facing_right = sin(swing_timer) > 0
	if sprite:
		sprite.flip_h = not facing_right

	# 更新蛛网线条
	if web_line and web_line.get_point_count() >= 2:
		web_line.set_point_position(0, anchor_position)
		web_line.set_point_position(1, global_position)
