# 球之积分勇者 - 门/闸门
# 被开关激活后打开

extends StaticBody2D

@export var gate_id: int = 0
@export var is_opened: bool = false
@export var open_speed: float = 2.0
@export var open_height: float = 3.0

@export var closed_color: Color = Color(0.31, 0.31, 0.31)
@export var opened_color: Color = Color(0.31, 0.31, 0.31, 0.0)

var closed_position: Vector2
var opened_position: Vector2
var is_opening: bool = false

@onready var sprite: Sprite2D = $Sprite2D
@onready var collision: CollisionShape2D = $CollisionShape2D

func _ready() -> void:
	if sprite:
		sprite.scale = Vector2(1, 1)
	closed_position = global_position
	opened_position = closed_position + Vector2.UP * open_height
	_update_visual()

func _process(delta: float) -> void:
	if is_opening and not is_opened:
		global_position = global_position.lerp(opened_position, open_speed * delta)

		if global_position.distance_to(opened_position) < 0.1:
			is_opened = true
			is_opening = false
			if collision:
				collision.disabled = true

func open_gate() -> void:
	if is_opened or is_opening: return

	is_opening = true

	if ParticleManager:
		ParticleManager.spawn_gate_effect(global_position)

func _update_visual() -> void:
	if sprite:
		sprite.modulate = opened_color if is_opened else closed_color
