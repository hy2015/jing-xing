# 球之积分勇者 - 平台
# 支持多种类型：地面、石台、泥土

extends StaticBody2D

enum PlatformType { GROUND, STONE, DIRT }

@export var platform_type: PlatformType = PlatformType.GROUND

@onready var sprite: Sprite2D = $Sprite2D

func _ready() -> void:
	if sprite:
		sprite.scale = Vector2(1, 1)
	_update_visual()

func _update_visual() -> void:
	if sprite == null: return

	match platform_type:
		PlatformType.GROUND:
			sprite.modulate = Color(0.55, 0.27, 0.07)
		PlatformType.STONE:
			sprite.modulate = Color(0.5, 0.5, 0.5)
		PlatformType.DIRT:
			sprite.modulate = Color(0.31, 0.16, 0.04)
