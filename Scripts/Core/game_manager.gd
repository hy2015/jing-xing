# 球之积分勇者 - 游戏管理器
# 管理游戏状态、随从切换、能力使用

extends Node

# 信号
signal show_message(message: String)
signal score_changed(score: int)
signal state_changed(state: GameConstants.GameState)

# 游戏状态
var current_state: GameConstants.GameState = GameConstants.GameState.MENU

# 角色引用
var hero: Node2D = null
var followers: Array = []
var active_follower_index: int = 0

# 游戏数据
var score: int = 0
var gems_collected: int = 0

# 效果状态
var magnet_active: bool = false
var magnet_timer: float = 0.0
var flying_active: bool = false
var flying_timer: float = 0.0

# 消息系统
var current_message: String = ""
var message_timer: float = 0.0

func _ready() -> void:
	process_mode = Node.PROCESS_MODE_ALWAYS

func _process(delta: float) -> void:
	# 消息计时
	if message_timer > 0:
		message_timer -= delta

	# 磁力效果
	if magnet_active:
		magnet_timer -= delta
		if magnet_timer <= 0:
			magnet_active = false

	# 飞行效果
	if flying_active:
		flying_timer -= delta
		if flying_timer <= 0:
			flying_active = false

func start_game() -> void:
	set_state(GameConstants.GameState.PLAYING)
	score = 0
	gems_collected = 0

func set_state(new_state: GameConstants.GameState) -> void:
	current_state = new_state
	state_changed.emit(new_state)

	if new_state == GameConstants.GameState.PAUSED:
		Engine.time_scale = 0.0
	else:
		Engine.time_scale = 1.0

func toggle_pause() -> void:
	if current_state == GameConstants.GameState.PLAYING:
		set_state(GameConstants.GameState.PAUSED)
	elif current_state == GameConstants.GameState.PAUSED:
		set_state(GameConstants.GameState.PLAYING)

func game_over() -> void:
	set_state(GameConstants.GameState.GAME_OVER)

func restart_game() -> void:
	get_tree().reload_current_scene()

func return_to_menu() -> void:
	set_state(GameConstants.GameState.MENU)
	get_tree().change_scene_to_file("res://Scenes/main_menu.tscn")

# 随从切换
func switch_follower(index: int) -> void:
	if index >= 0 and index < followers.size():
		active_follower_index = index
		var follower = followers[index]
		var follower_name = follower.get_display_name()
		show_message.emit("随从: %s" % follower_name)

func cycle_follower_color(forward: bool) -> void:
	if followers.is_empty(): return

	var follower = followers[active_follower_index]
	if follower.has_method("cycle_color"):
		follower.cycle_color(forward)
		show_message.emit("颜色: %s" % follower.get_color_name())
	elif follower.has_method("cycle_form"):
		follower.cycle_form(forward)
		show_message.emit("形态: %s" % follower.get_form_name())

# 使用随从能力
func use_follower_ability() -> void:
	if followers.is_empty(): return
	followers[active_follower_index].use_ability()

# 勇者攻击
func hero_attack() -> void:
	if hero != null and hero.has_method("perform_attack"):
		hero.perform_attack()

# 加分
func add_score(points: int) -> void:
	score += points
	score_changed.emit(score)

# 收集宝石
func collect_gem() -> void:
	gems_collected += 1
	add_score(50)

# 显示消息
func show_message_text(message: String, duration: float = 2.0) -> void:
	current_message = message
	message_timer = duration
	show_message.emit(message)

# 激活磁力效果
func activate_magnet(duration: float = 3.0) -> void:
	magnet_active = true
	magnet_timer = duration
	show_message_text("磁力激活! 吸引收集品 + 削弱敌人武器")

# 激活飞行效果
func activate_flying(duration: float = 3.0) -> void:
	flying_active = true
	flying_timer = duration
	show_message_text("飞行激活!")
