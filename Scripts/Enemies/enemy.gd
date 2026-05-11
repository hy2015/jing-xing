# 球之积分勇者 - 敌人基类
# 所有敌人的基类，提供巡逻、受伤、状态效果等通用逻辑

class_name EnemyBase
extends CharacterBody2D

@export var health: int = 60
@export var max_health: int = 60
@export var damage: int = 10
@export var move_speed: float = 1.5
@export var patrol_range: float = 2.0
@export var enemy_color: Color = Color.RED

var alive: bool = true
var facing_right: bool = true
var start_x: float = 0.0
var direction: int = 1

# 状态效果
var frozen_timer: float = 0.0
var trapped_timer: float = 0.0
var vine_wrapped_timer: float = 0.0
var controlled: bool = false

@onready var sprite: Sprite2D = $Sprite2D

func _ready() -> void:
	start_x = global_position.x
	if sprite:
		sprite.modulate = enemy_color
		sprite.scale = Vector2(1, 1)

func _process(delta: float) -> void:
	if not alive: return

	# 状态效果计时
	_update_status_effects(delta)

	# 如果被控制/冻结/困住，不移动
	if frozen_timer > 0 or trapped_timer > 0 or vine_wrapped_timer > 0:
		return

	# 巡逻移动
	patrol(delta)

func _update_status_effects(delta: float) -> void:
	if frozen_timer > 0:
		frozen_timer -= delta
		if sprite:
			sprite.modulate = enemy_color.lerp(Color.CYAN, 0.5)
	elif trapped_timer > 0:
		trapped_timer -= delta
	elif vine_wrapped_timer > 0:
		vine_wrapped_timer -= delta
		if vine_wrapped_timer <= 0:
			controlled = false
	else:
		if sprite:
			sprite.modulate = enemy_color

func patrol(_delta: float) -> void:
	# 移动
	var move_x := move_speed * direction * _delta
	global_position.x += move_x

	# 检查巡逻边界
	if absf(global_position.x - start_x) > patrol_range:
		direction *= -1

	# 更新朝向
	facing_right = direction > 0
	if sprite:
		sprite.flip_h = not facing_right

# 受伤
func take_damage(amount: int) -> void:
	if not alive: return

	health -= amount

	# 受伤闪烁效果
	if sprite:
		_flash_damage()

	if health <= 0:
		health = 0
		die()

func _flash_damage() -> void:
	if sprite == null: return
	var original_color := sprite.modulate
	sprite.modulate = Color.WHITE
	await get_tree().create_timer(0.1).timeout
	if sprite:
		sprite.modulate = original_color

# 死亡
func die() -> void:
	alive = false

	if ParticleManager:
		ParticleManager.spawn_death_effect(global_position)

	GameManager.add_score(20)

	await get_tree().create_timer(0.5).timeout
	queue_free()

# 状态效果方法
func freeze_enemy(duration: float) -> void:
	frozen_timer = duration

func trap_enemy(duration: float) -> void:
	trapped_timer = duration

func vine_wrap(duration: float) -> void:
	vine_wrapped_timer = duration
	controlled = true

# 碰撞检测 (伤害勇者)
func _on_body_entered(body: Node2D) -> void:
	if not alive: return
	if frozen_timer > 0 or vine_wrapped_timer > 0: return

	if body.has_method("take_damage") and body != self:
		body.take_damage(damage)

# 公共方法
func is_alive() -> bool:
	return alive

func is_frozen() -> bool:
	return frozen_timer > 0

func is_trapped() -> bool:
	return trapped_timer > 0

func is_controlled() -> bool:
	return controlled

func is_vine_wrapped() -> bool:
	return vine_wrapped_timer > 0
