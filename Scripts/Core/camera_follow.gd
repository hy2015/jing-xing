# 球之积分勇者 - 相机跟随系统

extends Camera2D

@export var smooth_speed: float = 5.0

@export_group("边界限制")
@export var use_bounds: bool = true
@export var min_x: float = -1.0
@export var max_x: float = 50.0
@export var min_y: float = -1.0
@export var max_y: float = 15.0

var target: Node2D = null

func _ready() -> void:
	# 延迟一帧查找勇者
	call_deferred("_find_target")

func _find_target() -> void:
	var game_manager = GameManager
	if game_manager and game_manager.hero:
		target = game_manager.hero

func _physics_process(delta: float) -> void:
	if target == null:
		_find_target()
		return

	var desired_position: Vector2 = target.global_position

	# 平滑跟随
	var smoothed_position: Vector2 = global_position.lerp(desired_position, smooth_speed * delta)

	# 边界限制
	if use_bounds:
		smoothed_position.x = clampf(smoothed_position.x, min_x, max_x)
		smoothed_position.y = clampf(smoothed_position.y, min_y, max_y)

	global_position = smoothed_position

# 设置相机边界 (由关卡构建器调用)
func set_bounds(x_min: float, x_max: float, y_min: float, y_max: float) -> void:
	min_x = x_min
	max_x = x_max
	min_y = y_min
	max_y = y_max
