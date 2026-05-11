# 球之积分勇者 - 粒子管理器
# 管理各种粒子效果

extends Node

@export_group("粒子预制体")
@export var hit_effect_scene: PackedScene
@export var break_effect_scene: PackedScene
@export var fire_effect_scene: PackedScene
@export var light_effect_scene: PackedScene
@export var lightning_effect_scene: PackedScene
@export var water_effect_scene: PackedScene
@export var ice_effect_scene: PackedScene
@export var absolute_zero_effect_scene: PackedScene
@export var black_hole_effect_scene: PackedScene
@export var portal_effect_scene: PackedScene
@export var trap_effect_scene: PackedScene
@export var vine_effect_scene: PackedScene
@export var collect_effect_scene: PackedScene
@export var death_effect_scene: PackedScene
@export var switch_effect_scene: PackedScene
@export var gate_effect_scene: PackedScene
@export var calculus_effect_scene: PackedScene

@export var default_lifetime: float = 1.0

# 生成粒子效果
func spawn_effect(scene: PackedScene, position: Vector2, lifetime: float = -1.0) -> void:
	if scene == null: return

	var effect = scene.instantiate()
	effect.global_position = position
	get_tree().current_scene.add_child(effect)

	if lifetime < 0:
		lifetime = default_lifetime
	effect.get_tree().create_timer(lifetime).timeout.connect(effect.queue_free)

# 各种效果的快捷方法
func spawn_hit_effect(position: Vector2) -> void:
	spawn_effect(hit_effect_scene, position, 0.5)

func spawn_break_effect(position: Vector2) -> void:
	spawn_effect(break_effect_scene, position, 1.0)

func spawn_fire_effect(position: Vector2) -> void:
	spawn_effect(fire_effect_scene, position, 1.5)

func spawn_light_effect(position: Vector2) -> void:
	spawn_effect(light_effect_scene, position, 0.8)

func spawn_lightning_effect(position: Vector2) -> void:
	spawn_effect(lightning_effect_scene, position, 0.6)

func spawn_water_effect(position: Vector2) -> void:
	spawn_effect(water_effect_scene, position, 1.0)

func spawn_ice_effect(position: Vector2) -> void:
	spawn_effect(ice_effect_scene, position, 1.0)

func spawn_absolute_zero_effect(position: Vector2) -> void:
	spawn_effect(absolute_zero_effect_scene, position, 2.0)

func spawn_black_hole_effect(position: Vector2) -> void:
	spawn_effect(black_hole_effect_scene, position, 1.5)

func spawn_portal_effect(position: Vector2) -> void:
	spawn_effect(portal_effect_scene, position, 2.0)

func spawn_trap_effect(position: Vector2) -> void:
	spawn_effect(trap_effect_scene, position, 1.5)

func spawn_vine_effect(position: Vector2) -> void:
	spawn_effect(vine_effect_scene, position, 1.0)

func spawn_collect_effect(position: Vector2) -> void:
	spawn_effect(collect_effect_scene, position, 0.8)

func spawn_death_effect(position: Vector2) -> void:
	spawn_effect(death_effect_scene, position, 1.0)

func spawn_switch_effect(position: Vector2) -> void:
	spawn_effect(switch_effect_scene, position, 1.0)

func spawn_gate_effect(position: Vector2) -> void:
	spawn_effect(gate_effect_scene, position, 1.5)

func spawn_calculus_effect(position: Vector2) -> void:
	spawn_effect(calculus_effect_scene, position, 1.0)
