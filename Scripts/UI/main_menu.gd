# 璟星 - 主菜单/登录界面
# 优化版：更美观的界面，支持键盘操作，设置面板，BGM（平滑循环+交叉淡入淡出）

extends Control

# 伙伴颜色动画
var chameleon_colors: Array = GameConstants.CHAMELEON_COLORS
var color_index: int = 0
var color_timer: float = 0.0
var COLOR_INTERVAL: float = 0.8

# 伙伴引用（更新路径以适配新布局）
@onready var chameleon_sprite: Control = $BG/Companions/ChameleonCard/ChameleonContent/ChameleonSprite
@onready var topology_rect: TextureRect = $BG/Companions/TopologyCard/TopologyContent/TopologySprite/TopologySpriteImg
@onready var ginseng_rect: TextureRect = $BG/Companions/GinsengCard/GinsengContent/GinsengSprite/GinsengSpriteImg

@onready var chameleon_label: Label = $BG/Companions/ChameleonCard/ChameleonContent/AbilityLabel
@onready var topology_label: Label = $BG/Companions/TopologyCard/TopologyContent/TopologyAbilityLabel
@onready var ginseng_label: Label = $BG/Companions/GinsengCard/GinsengContent/GinsengAbilityLabel

@onready var login_button: Button = $BG/Buttons/LoginButton
@onready var exit_button: Button = $BG/Buttons/ExitButton
@onready var footer_label: Label = $BG/Footer
@onready var settings_button: Button = $SettingsButton
@onready var settings_panel: PanelContainer = $SettingsPanel
@onready var bgm_player: AudioStreamPlayer = $BGMPlayer

# BGM 双播放器交叉淡入淡出系统
var _bgm_players: Array[AudioStreamPlayer] = []
var _active_bgm_idx: int = 0  # 当前正在播放的播放器索引
var _crossfade_duration: float = 1.0  # 交叉淡入淡出时长（秒）
var _is_crossfading: bool = false
var _crossfade_timer: float = 0.0
var _crossfade_from_idx: int = 0
var _crossfade_to_idx: int = 0

# 拓扑形态循环（精灵图切换）
var topology_textures: Array = []
var topology_names: Array = ["球", "圆环", "双洞圆环", "三洞圆环", "莫比乌斯环", "克莱因瓶"]
var topology_index: int = 0

# 怪怪形态循环（精灵图切换）
var ginseng_textures: Array = []
var ginseng_names: Array = ["微积分", "泛函", "量子"]
var ginseng_index: int = 0

# 动画相关
var title_pulse_time: float = 0.0
var button_hover_scale: float = 1.0

# 伙伴浮动动画参数（不同相位和速度，避免同步）
var float_time: float = 0.0
var FLOAT_SPEED_2: float = 0.9   # 软软浮动速度
var FLOAT_SPEED_3: float = 1.5   # 怪怪浮动速度
var FLOAT_AMP: float = 6.0       # 浮动幅度（像素）
var BREATHE_SPEED: float = 0.8   # 呼吸缩放速度
var BREATHE_AMP: float = 0.03    # 呼吸缩放幅度

func _ready() -> void:
	# 确保窗口为独立模式并设置初始大小
	DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_WINDOWED)
	DisplayServer.window_set_size(Vector2i(2560, 1440))
	
	login_button.pressed.connect(_on_login_pressed)
	exit_button.pressed.connect(_on_exit_pressed)
	login_button.mouse_entered.connect(_on_button_hover.bind(true))
	login_button.mouse_exited.connect(_on_button_hover.bind(false))
	settings_button.pressed.connect(_on_settings_pressed)
	if settings_panel:
		settings_panel.settings_closed.connect(_on_settings_closed)
		settings_panel.music_changed.connect(_on_music_changed)
	else:
		push_warning("SettingsPanel node not found at $SettingsPanel")
	
	# 初始颜色
	chameleon_sprite.color = chameleon_colors[0]
	topology_textures = [
		preload("res://Assets/Sprites/topology_sphere.jpg"),
		preload("res://Assets/Sprites/topology_torus.jpg"),
		preload("res://Assets/Sprites/topology_torus2.jpg"),
		preload("res://Assets/Sprites/topology_torus3.jpg"),
		preload("res://Assets/Sprites/topology_mobius.jpg"),
		preload("res://Assets/Sprites/topology_monster.jpg"),
	]
	topology_rect.texture = topology_textures[0]
	topology_rect.modulate = Color.WHITE
	chameleon_label.text = GameConstants.CHAMELEON_COLOR_NAMES[0] + " - " + GameConstants.CHAMELEON_ABILITY_NAMES[0]
	topology_label.text = topology_names[0]
	
	# 怪怪形态
	ginseng_textures = [
		preload("res://Assets/Sprites/ginseng_monster.jpg"),
		preload("res://Assets/Sprites/ginseng_functional.jpg"),
		preload("res://Assets/Sprites/ginseng_quantum.jpg"),
	]
	ginseng_rect.texture = ginseng_textures[0]
	ginseng_rect.modulate = Color.WHITE
	ginseng_label.text = ginseng_names[0]
	
	# 初始化 BGM 双播放器系统
	_init_bgm_system()
	
	# 聚焦登录按钮
	login_button.grab_focus()

func _init_bgm_system() -> void:
	# 创建第二个 AudioStreamPlayer 用于交叉淡入淡出
	var bgm_player2 := AudioStreamPlayer.new()
	bgm_player2.name = "BGMPlayer2"
	bgm_player2.bus = "Master"
	add_child(bgm_player2)
	
	# 存入数组，索引 0 = 原始播放器，索引 1 = 新播放器
	_bgm_players = [bgm_player, bgm_player2]
	
	# 设置循环模式（WAV 文件原生无缝循环）
	var stream: AudioStream = preload("res://Assets/Audio/bgm_warm.wav")
	if stream is AudioStreamWAV:
		stream.loop_mode = AudioStreamWAV.LOOP_FORWARD
		stream.loop_begin = 0
		var channels: int = 2 if stream.stereo else 1
		stream.loop_end = stream.data.size() / channels
	
	# 播放 BGM，音量从 0 淡入
	_bgm_players[0].stream = stream
	_bgm_players[0].volume_db = -80.0
	_bgm_players[0].play()
	_fade_in(_bgm_players[0])

func _process(delta: float) -> void:
	color_timer += delta
	title_pulse_time += delta
	
	# 交叉淡入淡出进度
	if _is_crossfading:
		_crossfade_timer += delta
		var t: float = clampf(_crossfade_timer / _crossfade_duration, 0.0, 1.0)
		# 线性淡入淡出
		_bgm_players[_crossfade_from_idx].volume_db = linear_to_db(1.0 - t)
		_bgm_players[_crossfade_to_idx].volume_db = linear_to_db(t)
		if t >= 1.0:
			_is_crossfading = false
			_bgm_players[_crossfade_from_idx].stop()
			_bgm_players[_crossfade_from_idx].volume_db = 0.0
			_bgm_players[_crossfade_to_idx].volume_db = 0.0
			_active_bgm_idx = _crossfade_to_idx
	
	# 伙伴颜色循环
	if color_timer >= COLOR_INTERVAL:
		color_timer = 0.0
		
		# 龙龙颜色循环
		color_index = (color_index + 1) % chameleon_colors.size()
		chameleon_sprite.color = chameleon_colors[color_index]
		chameleon_label.text = GameConstants.CHAMELEON_COLOR_NAMES[color_index] + " - " + GameConstants.CHAMELEON_ABILITY_NAMES[color_index]

		# 软软形态循环
		topology_index = (topology_index + 1) % topology_textures.size()
		topology_rect.texture = topology_textures[topology_index]
		topology_label.text = topology_names[topology_index]

		# 怪怪形态循环
		ginseng_index = (ginseng_index + 1) % ginseng_textures.size()
		ginseng_rect.texture = ginseng_textures[ginseng_index]
		ginseng_label.text = ginseng_names[ginseng_index]
	
	# 伙伴浮动+呼吸动画（龙龙在 chameleon_sprite.gd 自行处理）
	float_time += delta
	
	# 软软浮动（只向下）
	var top_y: float = (sin(float_time * FLOAT_SPEED_2 + 1.2) * 0.5 + 0.5) * FLOAT_AMP
	var top_scale: float = 1.0 + sin(float_time * BREATHE_SPEED + 0.8) * BREATHE_AMP
	topology_rect.position.y = top_y
	topology_rect.pivot_offset = topology_rect.size / 2.0
	topology_rect.scale = Vector2(top_scale, top_scale)
	
	# 怪怪浮动（只向下）
	var gin_y: float = (sin(float_time * FLOAT_SPEED_3 + 2.5) * 0.5 + 0.5) * FLOAT_AMP
	var gin_scale: float = 1.0 + sin(float_time * BREATHE_SPEED + 1.6) * BREATHE_AMP
	ginseng_rect.position.y = gin_y
	ginseng_rect.pivot_offset = ginseng_rect.size / 2.0
	ginseng_rect.scale = Vector2(gin_scale, gin_scale)
	
	# 键盘快捷键
	if Input.is_action_just_pressed("ui_accept"):
		if not _settings_visible():
			_on_login_pressed()
	if Input.is_action_just_pressed("ui_cancel"):
		if _settings_visible():
			_on_settings_closed()
		else:
			_on_exit_pressed()

func _input(event: InputEvent) -> void:
	if event is InputEventKey and event.pressed:
		if event.keycode == KEY_ENTER or event.keycode == KEY_KP_ENTER:
			if not _settings_visible():
				_on_login_pressed()
		elif event.keycode == KEY_ESCAPE:
			if _settings_visible():
				_on_settings_closed()
			else:
				_on_exit_pressed()

func _on_login_pressed() -> void:
	# 添加点击效果
	login_button.modulate = Color(0.8, 0.8, 0.8)
	await get_tree().create_timer(0.1).timeout
	login_button.modulate = Color.WHITE
	
	# 停止 BGM（淡出后停止）
	_fade_out_and_stop()

func _on_exit_pressed() -> void:
	# 添加点击效果
	exit_button.modulate = Color(0.8, 0.8, 0.8)
	await get_tree().create_timer(0.1).timeout
	exit_button.modulate = Color.WHITE
	
	get_tree().quit()

func _on_button_hover(is_hover: bool) -> void:
	if is_hover:
		login_button.modulate = Color(1.1, 1.1, 1.1)
	else:
		login_button.modulate = Color.WHITE

func _on_settings_pressed() -> void:
	if settings_panel:
		settings_panel.visible = true

func _on_settings_closed() -> void:
	if settings_panel:
		settings_panel.visible = false
	settings_button.grab_focus()

func _on_music_changed(music_file: String) -> void:
	# 切换音乐时交叉淡入淡出
	var new_stream: AudioStream = load(music_file)
	if new_stream is AudioStreamWAV:
		new_stream.loop_mode = AudioStreamWAV.LOOP_FORWARD
		new_stream.loop_begin = 0
		var channels: int = 2 if new_stream.stereo else 1
		new_stream.loop_end = new_stream.data.size() / channels
	
	var from_idx: int = _active_bgm_idx
	var to_idx: int = 1 - _active_bgm_idx
	
	# 准备新播放器
	_bgm_players[to_idx].stream = new_stream
	_bgm_players[to_idx].volume_db = -80.0
	_bgm_players[to_idx].play()
	
	# 开始交叉淡入淡出
	_crossfade_from_idx = from_idx
	_crossfade_to_idx = to_idx
	_crossfade_timer = 0.0
	_is_crossfading = true

func _fade_in(player: AudioStreamPlayer) -> void:
	player.volume_db = -80.0
	var tween: Tween = create_tween()
	tween.tween_property(player, "volume_db", 0.0, 0.5)

func _fade_out_and_stop() -> void:
	for player in _bgm_players:
		if player.playing:
			var tween: Tween = create_tween()
			tween.tween_property(player, "volume_db", -80.0, 0.5)
			tween.tween_callback(player.stop)

func _settings_visible() -> bool:
	return settings_panel != null and settings_panel.visible
