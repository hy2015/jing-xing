# 球之积分勇者 - 随从基类
# 所有随从的基类，提供跟随逻辑和能力接口

class_name FollowerBase
extends CharacterBody2D

@export var follow_distance: float = 2.0
@export var follow_speed: float = 5.0
@export var follow_height: float = 0.5

var sprite: Sprite2D
var facing_right: bool = true
var is_active: bool = false

func _ready() -> void:
	sprite = $Sprite2D
	if sprite:
		sprite.scale = Vector2(1, 1)
	# 注册到 GameManager
	GameManager.followers.append(self)

func _process(_delta: float) -> void:
	if GameManager.hero == null: return

	# 跟随勇者
	_follow_hero()

	# 更新朝向
	_update_facing()

func _follow_hero() -> void:
	var hero = GameManager.hero
	if hero == null: return

	var delta := get_process_delta_time()

	# 计算目标位置 (在勇者身后)
	var follower_index: int = GameManager.followers.find(self)
	var target_x: float = hero.global_position.x - (follower_index + 1) * follow_distance
	var target_y: float = hero.global_position.y + follow_height

	# 平滑移动
	var target_pos := Vector2(target_x, target_y)
	var current_pos := global_position
	var distance := current_pos.distance_to(target_pos)

	if distance > 0.1:
		var speed := minf(follow_speed, distance * 3.0)
		global_position = current_pos.lerp(target_pos, speed * delta)

func _update_facing() -> void:
	if GameManager.hero == null: return

	var dir_x: float = GameManager.hero.global_position.x - global_position.x
	if dir_x > 0.1:
		facing_right = true
	elif dir_x < -0.1:
		facing_right = false

	if sprite:
		sprite.flip_h = not facing_right

# 设置是否为当前激活的随从
func set_follower_active(active: bool) -> void:
	is_active = active
	if sprite:
		sprite.modulate = Color.WHITE if active else Color(0.8, 0.8, 0.8)

# 获取显示名称
func get_display_name() -> String:
	return "随从"

# 获取当前能力描述
func get_ability_description() -> String:
	return ""

# 使用能力
func use_ability() -> void:
	pass

# 高亮当前随从
func highlight(enabled: bool) -> void:
	if sprite:
		sprite.modulate = Color.YELLOW if enabled else Color.WHITE
