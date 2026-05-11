# 球之积分勇者 - 收集品
# 宝石、心心、钥匙

extends Area2D

@export var collectible_type: GameConstants.CollectibleType = GameConstants.CollectibleType.GEM
@export var float_amplitude: float = 0.2
@export var float_frequency: float = 1.0

var start_position: Vector2
var float_timer: float = 0.0
var collected: bool = false

@onready var sprite: Sprite2D = $Sprite2D

func _ready() -> void:
	start_position = global_position
	body_entered.connect(_on_body_entered)
	if sprite:
		sprite.scale = Vector2(1, 1)
	_update_visual()

func _process(delta: float) -> void:
	if collected: return

	# 上下浮动效果
	float_timer += delta * float_frequency
	var y_offset := sin(float_timer) * float_amplitude
	global_position = Vector2(start_position.x, start_position.y + y_offset)

func _update_visual() -> void:
	if sprite == null: return

	if collectible_type == 0:
		sprite.modulate = Color(1.0, 0.84, 0.0)
	elif collectible_type == 1:
		sprite.modulate = Color.RED
	elif collectible_type == 2:
		sprite.modulate = Color(1.0, 0.84, 0.0)

func _on_body_entered(body: Node2D) -> void:
	if collected: return

	if body.has_method("take_damage"):  # Hero
		_collect(body)

func _collect(hero: Node2D) -> void:
	collected = true

	if collectible_type == 0:
		GameManager.collect_gem()
	elif collectible_type == 1:
		hero.heal(30)
	elif collectible_type == 2:
		GameManager.add_score(100)
		GameManager.show_message_text("获得钥匙!")

	if ParticleManager:
		ParticleManager.spawn_collect_effect(global_position)

	queue_free()
