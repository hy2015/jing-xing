# 球之积分勇者 - UI管理器

extends CanvasLayer

@export_group("HUD元素")
@export var health_bar_path: NodePath
@export var health_text_path: NodePath
@export var score_text_path: NodePath
@export var gems_text_path: NodePath
@export var follower_name_path: NodePath
@export var ability_text_path: NodePath
@export var message_text_path: NodePath

@export_group("随从指示器")
@export var follower_indicator_paths: Array[NodePath] = []

@export_group("效果指示")
@export var effect_text_path: NodePath

@export_group("菜单面板")
@export var menu_panel_path: NodePath
@export var pause_panel_path: NodePath
@export var game_over_panel_path: NodePath
@export var final_score_text_path: NodePath

var health_bar: ProgressBar
var health_text: Label
var score_text: Label
var gems_text: Label
var follower_name_text: Label
var ability_text: Label
var message_text: Label
var follower_indicators: Array[ColorRect] = []
var effect_text: Label
var menu_panel: Control
var pause_panel: Control
var game_over_panel: Control
var final_score_text: Label

func _ready() -> void:
	# 获取节点引用
	health_bar = get_node_or_null(health_bar_path)
	health_text = get_node_or_null(health_text_path)
	score_text = get_node_or_null(score_text_path)
	gems_text = get_node_or_null(gems_text_path)
	follower_name_text = get_node_or_null(follower_name_path)
	ability_text = get_node_or_null(ability_text_path)
	message_text = get_node_or_null(message_text_path)
	effect_text = get_node_or_null(effect_text_path)
	menu_panel = get_node_or_null(menu_panel_path)
	pause_panel = get_node_or_null(pause_panel_path)
	game_over_panel = get_node_or_null(game_over_panel_path)
	final_score_text = get_node_or_null(final_score_text_path)

	for path in follower_indicator_paths:
		var indicator = get_node_or_null(path)
		if indicator:
			follower_indicators.append(indicator)

	# 订阅事件
	GameManager.show_message.connect(_on_show_message)
	GameManager.score_changed.connect(_on_score_changed)
	GameManager.state_changed.connect(_on_state_changed)

func _process(_delta: float) -> void:
	# 更新消息显示
	if GameManager.message_timer > 0:
		if message_text:
			message_text.visible = true
			message_text.text = GameManager.current_message
			var alpha := minf(1.0, GameManager.message_timer * 0.5)
			message_text.modulate.a = alpha
	else:
		if message_text:
			message_text.visible = false

	# 更新效果指示
	_update_effect_indicators()

func update_health(current: int, max_val: int) -> void:
	if health_bar:
		health_bar.value = float(current) / max_val * 100.0
	if health_text:
		health_text.text = "HP: %d/%d" % [current, max_val]

func _on_score_changed(score_val: int) -> void:
	if score_text:
		score_text.text = "分数: %d" % score_val

func update_gems(gems: int) -> void:
	if gems_text:
		gems_text.text = "宝石: %d" % gems

func update_follower_info(index: int, follower_name: String, ability: String) -> void:
	if follower_name_text:
		follower_name_text.text = "随从: %s" % follower_name
	if ability_text:
		ability_text.text = ability

	for i in range(follower_indicators.size()):
		if follower_indicators[i]:
			follower_indicators[i].color = Color.GREEN if i == index else Color.GRAY

func _on_show_message(message: String) -> void:
	if message_text:
		message_text.text = message
		message_text.visible = true

func _update_effect_indicators() -> void:
	if effect_text == null: return

	var effects := ""
	if GameManager.magnet_active:
		effects += "磁力激活! "
	if GameManager.flying_active:
		effects += "飞行激活! "
	effect_text.text = effects
	effect_text.visible = not effects.is_empty()

func _on_state_changed(state: GameConstants.GameState) -> void:
	if menu_panel:
		menu_panel.visible = state == GameConstants.GameState.MENU
	if pause_panel:
		pause_panel.visible = state == GameConstants.GameState.PAUSED
	if game_over_panel:
		game_over_panel.visible = state == GameConstants.GameState.GAME_OVER
		if state == GameConstants.GameState.GAME_OVER and final_score_text:
			final_score_text.text = "最终分数: %d" % GameManager.score
