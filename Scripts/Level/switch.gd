# 球之积分勇者 - 开关
# 触发机关，激活对应的门

extends Area2D

@export var switch_id: int = 0
@export var is_activated: bool = false

@export var inactive_color: Color = Color.RED
@export var active_color: Color = Color.GREEN

@export var linked_gate: NodePath

@onready var sprite: Sprite2D = $Sprite2D

func _ready() -> void:
	body_entered.connect(_on_body_entered)
	if sprite:
		sprite.scale = Vector2(1, 1)
	_update_visual()

func _on_body_entered(body: Node2D) -> void:
	if is_activated: return

	if body.has_method("take_damage"):  # Hero
		activate()

func activate() -> void:
	if is_activated: return

	is_activated = true
	_update_visual()

	# 激活关联的门
	if not linked_gate.is_empty():
		var gate = get_node_or_null(linked_gate)
		if gate and gate.has_method("open_gate"):
			gate.open_gate()

	# 加分
	GameManager.add_score(200)
	GameManager.show_message_text("开关激活! 门已打开")

	if ParticleManager:
		ParticleManager.spawn_switch_effect(global_position)

func _update_visual() -> void:
	if sprite:
		sprite.modulate = active_color if is_activated else inactive_color
