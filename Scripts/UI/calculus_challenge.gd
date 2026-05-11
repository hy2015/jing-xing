# 球之积分勇者 - 微积分挑战系统
# 人参怪触发的微积分题目挑战

extends Control

@export_group("UI引用")
@export var challenge_panel_path: NodePath
@export var title_text_path: NodePath
@export var question_text_path: NodePath
@export var hint_text_path: NodePath
@export var answer_input_path: NodePath
@export var result_text_path: NodePath
@export var submit_button_path: NodePath
@export var skip_button_path: NodePath

var challenge_panel: Control
var title_text: Label
var question_text: Label
var hint_text: Label
var answer_input: LineEdit
var result_text: Label
var submit_button: Button
var skip_button: Button

var is_active: bool = false
var correct_answer: String = ""
var problem_type: String = ""
var a: int = 0
var n: int = 0

func _ready() -> void:
	challenge_panel = get_node_or_null(challenge_panel_path)
	title_text = get_node_or_null(title_text_path)
	question_text = get_node_or_null(question_text_path)
	hint_text = get_node_or_null(hint_text_path)
	answer_input = get_node_or_null(answer_input_path)
	result_text = get_node_or_null(result_text_path)
	submit_button = get_node_or_null(submit_button_path)
	skip_button = get_node_or_null(skip_button_path)

	if challenge_panel:
		challenge_panel.visible = false

	if submit_button:
		submit_button.pressed.connect(_submit_answer)
	if skip_button:
		skip_button.pressed.connect(_skip_challenge)

# 开始挑战
func start_challenge() -> void:
	is_active = true
	_generate_problem()

	if challenge_panel:
		challenge_panel.visible = true

	GameManager.set_state(GameConstants.GameState.CALCULUS_CHALLENGE)

# 生成题目
func _generate_problem() -> void:
	var types := ["derivative", "integral", "limit"]
	problem_type = types[randi() % types.size()]

	match problem_type:
		"derivative":
			_generate_derivative_problem()
		"integral":
			_generate_integral_problem()
		"limit":
			_generate_limit_problem()

# 生成求导题目
func _generate_derivative_problem() -> void:
	a = randi_range(2, 6)
	n = randi_range(2, 5)

	var question := "d/dx [ %d * x^%d ]  =  ?" % [a, n]
	var ans_a := a * n
	var ans_n := n - 1

	if ans_n == 0:
		correct_answer = str(ans_a)
	else:
		correct_answer = "%d %d" % [ans_a, ans_n]

	var hint := "幂法则: d/dx [a*x^n] = a*n*x^(n-1)  ->  输入: 系数 指数"
	_update_ui(question, hint)

# 生成积分题目
func _generate_integral_problem() -> void:
	a = randi_range(2, 8)
	n = randi_range(1, 4)

	var question := "Integral [ %d * x^%d ] dx  =  ?  (+C)" % [a, n]
	var ans_n := n + 1

	if a % ans_n == 0:
		correct_answer = "%d %d" % [a / ans_n, ans_n]
	else:
		correct_answer = "%d/%d %d" % [a, ans_n, ans_n]

	var hint := "幂法则: int a*x^n dx = a/(n+1)*x^(n+1)  ->  输入: 分子/分母 指数  如 3/2 2"
	_update_ui(question, hint)

# 生成极限题目
func _generate_limit_problem() -> void:
	a = randi_range(1, 6)

	var question := "lim(x->0) sin(%d*x) / x  =  ?" % a
	correct_answer = str(a)

	var hint := "重要极限: lim(x->0) sin(ax)/x = a"
	_update_ui(question, hint)

func _update_ui(question: String, hint: String) -> void:
	if title_text:
		title_text.text = "人参怪 - 微积分挑战!"
	if question_text:
		question_text.text = question
	if hint_text:
		hint_text.text = hint
	if answer_input:
		answer_input.text = ""
	if result_text:
		result_text.text = ""

# 提交答案
func _submit_answer() -> void:
	if not is_active: return

	var user_answer: String = answer_input.text.strip_edges() if answer_input else ""
	var is_correct := _check_answer(user_answer)

	_show_result(is_correct)

	# 延迟关闭
	await get_tree().create_timer(2.0).timeout
	_close_challenge(is_correct)

# 跳过挑战
func _skip_challenge() -> void:
	if not is_active: return

	_show_result(false)
	await get_tree().create_timer(1.0).timeout
	_close_challenge(false)

# 检查答案
func _check_answer(user_answer: String) -> bool:
	if user_answer.is_empty(): return false

	# 标准化处理
	var user_norm := user_answer.replace(" ", "")
	var ans_norm := correct_answer.replace(" ", "")

	# 直接匹配
	if user_norm == ans_norm: return true

	# 数字比较 (极限题)
	if problem_type == "limit":
		var user_num := user_answer.to_float()
		var correct_num := correct_answer.to_float()
		if absf(user_num - correct_num) < 0.01:
			return true

	return false

# 显示结果
func _show_result(correct: bool) -> void:
	if result_text == null: return

	if correct:
		result_text.text = "正确! +HP 40  全屏伤害!"
		result_text.modulate = Color.GREEN
	else:
		result_text.text = "错误! 正确答案: %s" % correct_answer
		result_text.modulate = Color.RED

# 关闭挑战
func _close_challenge(correct: bool) -> void:
	if correct:
		_apply_correct_answer_effect()

	is_active = false

	if challenge_panel:
		challenge_panel.visible = false

	GameManager.set_state(GameConstants.GameState.PLAYING)

# 应用正确答案效果
func _apply_correct_answer_effect() -> void:
	var hero = GameManager.hero
	if hero == null: return

	# 回血
	hero.heal(40)

	# 全屏伤害
	var space_state := get_world_2d().direct_space_state
	var query := PhysicsShapeQueryParameters2D.new()
	var shape := CircleShape2D.new()
	shape.radius = 6.0
	query.shape = shape
	query.transform = Transform2D(0, hero.global_position)
	query.collision_mask = 2
	var results := space_state.intersect_shape(query)

	for result in results:
		var enemy = result["collider"]
		if enemy.has_method("is_alive") and enemy.is_alive():
			enemy.take_damage(80)
			if ParticleManager:
				ParticleManager.spawn_calculus_effect(enemy.global_position)

	GameManager.add_score(100)
	GameManager.show_message_text("微积分正确! 治疗+全屏伤害!")
