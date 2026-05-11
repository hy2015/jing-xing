# 龙龙精灵 - 分层显示
# 背景图层 + 龙龙图层（染色）

extends Control

var color: Color = Color(0.86, 0.20, 0.20)
var time: float = 0.0

# 图层引用
@onready var bg_layer: TextureRect = $Background
@onready var dragon_layer: TextureRect = $DragonBody

# Shader 材料
var bg_material: ShaderMaterial
var dragon_material: ShaderMaterial

func _ready() -> void:
	# 缩放动画居中
	pivot_offset = custom_minimum_size / 2.0
	# 防止子节点缩放时溢出边界盖住名字标签
	clip_children = CLIP_CHILDREN_ONLY
	
	# 加载素材图片
	var texture = load("res://Assets/Sprites/chameleon_dinosaur.jpg")
	
	# 设置背景图层
	bg_layer.texture = texture
	bg_layer.expand_mode = TextureRect.EXPAND_IGNORE_SIZE
	bg_layer.stretch_mode = TextureRect.STRETCH_KEEP_ASPECT_CENTERED
	
	# 设置龙龙图层
	dragon_layer.texture = texture
	dragon_layer.expand_mode = TextureRect.EXPAND_IGNORE_SIZE
	dragon_layer.stretch_mode = TextureRect.STRETCH_KEEP_ASPECT_CENTERED
	
	# 加载 Shader
	var shader = load("res://Assets/Shaders/chameleon_tint.gdshader")
	
	# 创建背景材质（只显示背景，不染色）
	bg_material = ShaderMaterial.new()
	bg_material.shader = shader
	bg_material.set_shader_parameter("layer_mode", 1)
	bg_layer.material = bg_material
	
	# 创建龙龙材质（只显示龙龙，染色）
	dragon_material = ShaderMaterial.new()
	dragon_material.shader = shader
	dragon_material.set_shader_parameter("layer_mode", 2)
	dragon_material.set_shader_parameter("tint_color", color)
	dragon_material.set_shader_parameter("tint_strength", 0.6)
	dragon_material.set_shader_parameter("color_levels", 8)
	dragon_material.set_shader_parameter("detail_reduce", 0.2)
	dragon_layer.material = dragon_material

func _process(delta: float) -> void:
	time += delta
	
	# 更新龙龙颜色到 Shader
	if dragon_material:
		dragon_material.set_shader_parameter("tint_color", color)
	
	# 呼吸缩放动画（不设置 position.y，避免和容器布局冲突）
	var breathe = sin(time * 0.8) * 0.03
	scale = Vector2(1.0 + breathe, 1.0 + breathe)
