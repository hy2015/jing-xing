# 球之积分勇者 - 障碍物
# 荆棘等障碍物，可被火焰烧毁

extends Area2D

enum ObstacleType { THORNS, FIRE, WATER }

@export var obstacle_type: ObstacleType = ObstacleType.THORNS
@export var damage: int = 15

@onready var sprite: Sprite2D = $Sprite2D

func _ready() -> void:
	body_entered.connect(_on_body_entered)
	if sprite:
		sprite.scale = Vector2(1, 1)
	_update_visual()

func _update_visual() -> void:
	if sprite == null: return

	match obstacle_type:
		ObstacleType.THORNS:
			sprite.modulate = Color(0.0, 0.39, 0.0)
		ObstacleType.FIRE:
			sprite.modulate = Color(1.0, 0.4, 0.0)
		ObstacleType.WATER:
			sprite.modulate = Color(0.0, 0.4, 0.8)

func _on_body_entered(body: Node2D) -> void:
	if body.has_method("take_damage"):
		body.take_damage(damage)

func _on_body_exited(_body: Node2D) -> void:
	pass

func is_thorns() -> bool:
	return obstacle_type == ObstacleType.THORNS

# 被烧毁
func destroy_obstacle() -> void:
	if obstacle_type == ObstacleType.THORNS:
		if ParticleManager:
			ParticleManager.spawn_fire_effect(global_position)
		queue_free()
