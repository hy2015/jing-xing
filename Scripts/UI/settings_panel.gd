# 璟星 - 设置面板
# 声音开关 + 音量 + 音乐类型 + 分辨率

extends PanelContainer

signal settings_closed
signal music_changed(music_name: String)

# 分辨率选项
var resolutions: Array = [
	Vector2i(2560, 1440),
	Vector2i(1920, 1080),
	Vector2i(1600, 900),
	Vector2i(1280, 720),
	Vector2i(1024, 576),
]
var resolution_names: Array = [
	"2560 x 1440",
	"1920 x 1080",
	"1600 x 900",
	"1280 x 720",
	"1024 x 576",
]

# 音乐类型
var music_types: Array = [
	{"name": "温馨", "file": "res://Assets/Audio/bgm_warm.wav"},
	{"name": "轻快", "file": "res://Assets/Audio/bgm_lively.wav"},
	{"name": "慵懒", "file": "res://Assets/Audio/bgm_lazy.wav"},
	{"name": "激昂", "file": "res://Assets/Audio/bgm_epic.wav"},
	{"name": "忧伤", "file": "res://Assets/Audio/bgm_sad.wav"},
	{"name": "愤怒", "file": "res://Assets/Audio/bgm_angry.wav"},
]

# UI 引用
@onready var sound_toggle: CheckButton = $VBox/SoundRow/SoundToggle
@onready var volume_slider: HSlider = $VBox/VolumeRow/VolumeSlider
@onready var music_option: OptionButton = $VBox/MusicRow/MusicOption
@onready var resolution_option: OptionButton = $VBox/ResolutionRow/ResolutionOption
@onready var close_button: Button = $VBox/CloseButton

const BUS_NAME = "Master"

func _ready() -> void:
	# 填充音乐类型
	for mt in music_types:
		music_option.add_item(mt["name"])
	music_option.selected = 0
	
	# 填充分辨率选项
	for res_name in resolution_names:
		resolution_option.add_item(res_name)
	
	# 检测当前分辨率
	var current_size = DisplayServer.window_get_size()
	var best_idx = 0
	for i in range(resolutions.size()):
		var r = resolutions[i]
		if r.x == current_size.x and r.y == current_size.y:
			best_idx = i
			break
	resolution_option.selected = best_idx
	
	# 默认音量 50%
	var bus_idx = AudioServer.get_bus_index(BUS_NAME)
	if bus_idx >= 0:
		var vol_db = AudioServer.get_bus_volume_db(bus_idx)
		var vol_pct = db_to_linear(vol_db) * 100.0
		# 如果是默认 0dB (100%)，改成 25%
		if vol_pct > 99.0:
			vol_pct = 25.0
			AudioServer.set_bus_volume_db(bus_idx, linear_to_db(0.25))
		volume_slider.value = vol_pct
		sound_toggle.button_pressed = not AudioServer.is_bus_mute(bus_idx)
	else:
		volume_slider.value = 50.0
		sound_toggle.button_pressed = true
	
	# 连接信号
	sound_toggle.toggled.connect(_on_sound_toggled)
	volume_slider.value_changed.connect(_on_volume_changed)
	music_option.item_selected.connect(_on_music_selected)
	resolution_option.item_selected.connect(_on_resolution_selected)
	close_button.pressed.connect(_on_close_pressed)

func _on_sound_toggled(pressed: bool) -> void:
	var bus_idx = AudioServer.get_bus_index(BUS_NAME)
	if bus_idx >= 0:
		AudioServer.set_bus_mute(bus_idx, not pressed)

func _on_volume_changed(value: float) -> void:
	var bus_idx = AudioServer.get_bus_index(BUS_NAME)
	if bus_idx >= 0:
		var vol_db = linear_to_db(value / 100.0)
		AudioServer.set_bus_volume_db(bus_idx, vol_db)

func _on_music_selected(index: int) -> void:
	if index >= 0 and index < music_types.size():
		music_changed.emit(music_types[index]["file"])

func _on_resolution_selected(index: int) -> void:
	if index < 0 or index >= resolutions.size():
		return
	var res = resolutions[index]
	# 只改窗口大小，视口保持 1280x720 不变
	# canvas_items 模式会自动等比缩放 UI
	DisplayServer.window_set_size(res)
	# 居中窗口
	var screen_size = DisplayServer.screen_get_size()
	var pos = (screen_size - res) / 2
	DisplayServer.window_set_position(pos)

func _on_close_pressed() -> void:
	visible = false
	settings_closed.emit()
