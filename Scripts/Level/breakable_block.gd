# 球之积分勇者 - 可破坏方块
# 被红色巨力能力破坏

extends StaticBody2D

@onready var sprite: Sprite2D = $Sprite2D

func _ready() -> void:
	if sprite:
		sprite.modulate = Color(0.59, 0.47, 0.31)
		sprite.scale = Vector2(1, 1)

# 被破坏
func destroy_block() -> void:
	if ParticleManager:
		ParticleManager.spawn_break_effect(global_position)
	queue_free()
