# 球之积分勇者 - 勇者角色控制器
# 玩家直接控制的角色，支持移动、跳跃、攻击

extends CharacterBody2D

@export var health: int = 100
@export var max_health: int = 100
@export var attack_damage: int = 30
@export var attack_range: float = 1.5
@export var attack_cooldown: float = 0.3
@export var invincible_duration: float = 1.0
@export var hero_color: Color = Color(1.0, 0.78, 0.59)

@export_group("地面检测")
@export var ground_check_radius: float = 0.1

var is_grounded: bool = false
var facing_right: bool = true
var last_attack_time: float = -1.0
var invincible_timer: float = 0.0
var is_alive: bool = true

@onready var sprite: Sprite2D = $Sprite2D
@onready var ground_check: RayCast2D = $GroundCheck

func _ready() -> void:
	if sprite:
		sprite.modulate = hero_color
		sprite.scale = Vector2(1, 1)
	# 注册到 GameManager
	GameManager.hero = self

func _process(delta: float) -> void:
	if not is_alive: return
	if GameManager.current_state != GameConstants.GameState.PLAYING:
		return

	# 地面检测
	is_grounded = is_on_floor()

	# 无敌时间
	if invincible_timer > 0:
		invincible_timer -= delta
		if sprite:
			var alpha := 0.3 if int(invincible_timer * 10) % 2 == 0 else 1.0
			sprite.modulate.a = alpha
	elif sprite:
		sprite.modulate.a = 1.0

	# 移动输入
	_handle_movement()

	# 跳跃输入
	_handle_jump()

	# 攻击输入
	if Input.is_action_just_pressed("attack"):
		perform_attack()

	# 掉出世界检测
	if global_position.y > 10:
		take_damage(100)

func _handle_movement() -> void:
	var move_input := 0.0

	if Input.is_action_pressed("move_left"):
		move_input = -1.0
		facing_right = false
	elif Input.is_action_pressed("move_right"):
		move_input = 1.0
		facing_right = true

	velocity.x = move_input * GameConstants.PLAYER_SPEED

	# 重力
	if not is_on_floor():
		velocity.y += GameConstants.GRAVITY * get_physics_process_delta_time()
		velocity.y = minf(velocity.y, GameConstants.MAX_FALL_SPEED)

	move_and_slide()

	# 翻转精灵
	if sprite:
		sprite.flip_h = not facing_right

func _handle_jump() -> void:
	# 飞行模式
	if GameManager.flying_active:
		if Input.is_action_pressed("jump"):
			velocity.y = -GameConstants.PLAYER_JUMP_FORCE * 0.7
		return

	# 普通跳跃
	if Input.is_action_just_pressed("jump") and is_grounded:
		velocity.y = -GameConstants.PLAYER_JUMP_FORCE
		is_grounded = false

func perform_attack() -> void:
	if Time.get_ticks_msec() / 1000.0 - last_attack_time < attack_cooldown:
		return

	last_attack_time = Time.get_ticks_msec() / 1000.0

	# 攻击方向
	var attack_dir := Vector2.RIGHT if facing_right else Vector2.LEFT
	var attack_pos: Vector2 = global_position + attack_dir * 0.5

	# 检测攻击范围内的敌人
	var space_state := get_world_2d().direct_space_state
	var query := PhysicsShapeQueryParameters2D.new()
	var shape := RectangleShape2D.new()
	shape.size = Vector2(attack_range, 1.0)
	query.shape = shape
	query.transform = Transform2D(0, attack_pos)
	query.collision_mask = 2  # enemies layer
	var results := space_state.intersect_shape(query)

	for result in results:
		var enemy = result["collider"]
		if enemy.has_method("is_alive") and enemy.is_alive():
			enemy.take_damage(attack_damage)
			if ParticleManager:
				ParticleManager.spawn_hit_effect(enemy.global_position)
			GameManager.add_score(10)

func take_damage(amount: int) -> void:
	if invincible_timer > 0: return

	health -= amount
	invincible_timer = invincible_duration

	if health <= 0:
		health = 0
		is_alive = false
		_die()

func heal(amount: int) -> void:
	health = mini(health + amount, max_health)

func _die() -> void:
	GameManager.game_over()

func is_facing_right() -> bool:
	return facing_right

func is_character_alive() -> bool:
	return is_alive

func is_character_grounded() -> bool:
	return is_grounded
