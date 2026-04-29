// 球之积分勇者 - 微积分挑战系统
// 人参怪触发的微积分题目挑战

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace JingXing
{
    public class CalculusChallenge : MonoBehaviour
    {
        public static CalculusChallenge Instance { get; private set; }

        [Header("UI引用")]
        public GameObject challengePanel;
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI questionText;
        public TextMeshProUGUI hintText;
        public TMP_InputField answerInput;
        public TextMeshProUGUI resultText;
        public Button submitButton;
        public Button skipButton;

        // 挑战状态
        private bool isActive = false;
        private string correctAnswer = "";
        private string problemType = "";
        private int a, n;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // 隐藏面板
            if (challengePanel != null)
            {
                challengePanel.SetActive(false);
            }

            // 绑定按钮事件
            if (submitButton != null)
            {
                submitButton.onClick.AddListener(SubmitAnswer);
            }
            if (skipButton != null)
            {
                skipButton.onClick.AddListener(SkipChallenge);
            }
        }

        // 开始挑战
        public void StartChallenge()
        {
            isActive = true;
            GenerateProblem();

            if (challengePanel != null)
            {
                challengePanel.SetActive(true);
            }

            // 暂停游戏
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetState(GameConstants.GameState.CalculusChallenge);
            }
        }

        // 生成题目
        private void GenerateProblem()
        {
            // 随机选择题目类型
            string[] types = { "derivative", "integral", "limit" };
            problemType = types[UnityEngine.Random.Range(0, types.Length)];

            switch (problemType)
            {
                case "derivative":
                    GenerateDerivativeProblem();
                    break;
                case "integral":
                    GenerateIntegralProblem();
                    break;
                case "limit":
                    GenerateLimitProblem();
                    break;
            }
        }

        // 生成求导题目
        private void GenerateDerivativeProblem()
        {
            a = UnityEngine.Random.Range(2, 7);
            n = UnityEngine.Random.Range(2, 6);

            string question = $"d/dx [ {a} * x^{n} ]  =  ?";
            int ansA = a * n;
            int ansN = n - 1;

            if (ansN == 0)
            {
                correctAnswer = ansA.ToString();
            }
            else
            {
                correctAnswer = $"{ansA} {ansN}";
            }

            string hint = $"幂法则: d/dx [a*x^n] = a*n*x^(n-1)  ->  输入: 系数 指数";

            UpdateUI(question, hint);
        }

        // 生成积分题目
        private void GenerateIntegralProblem()
        {
            a = UnityEngine.Random.Range(2, 9);
            n = UnityEngine.Random.Range(1, 5);

            string question = $"Integral [ {a} * x^{n} ] dx  =  ?  (+C)";
            int ansN = n + 1;

            if (a % ansN == 0)
            {
                correctAnswer = $"{a / ansN} {ansN}";
            }
            else
            {
                correctAnswer = $"{a}/{ansN} {ansN}";
            }

            string hint = $"幂法则: int a*x^n dx = a/(n+1)*x^(n+1)  ->  输入: 分子/分母 指数  如 3/2 2";

            UpdateUI(question, hint);
        }

        // 生成极限题目
        private void GenerateLimitProblem()
        {
            a = UnityEngine.Random.Range(1, 7);

            string question = $"lim(x->0) sin({a}*x) / x  =  ?";
            correctAnswer = a.ToString();

            string hint = $"重要极限: lim(x->0) sin(ax)/x = a";

            UpdateUI(question, hint);
        }

        // 更新UI
        private void UpdateUI(string question, string hint)
        {
            if (titleText != null) titleText.text = "人参怪 - 微积分挑战!";
            if (questionText != null) questionText.text = question;
            if (hintText != null) hintText.text = hint;
            if (answerInput != null) answerInput.text = "";
            if (resultText != null) resultText.text = "";
        }

        // 提交答案
        public void SubmitAnswer()
        {
            if (!isActive) return;

            string userAnswer = answerInput.text.Trim();
            bool isCorrect = CheckAnswer(userAnswer);

            ShowResult(isCorrect);

            // 延迟关闭
            StartCoroutine(CloseChallengeAfterDelay(isCorrect, 2f));
        }

        // 跳过挑战
        public void SkipChallenge()
        {
            if (!isActive) return;

            ShowResult(false);
            StartCoroutine(CloseChallengeAfterDelay(false, 1f));
        }

        // 检查答案
        private bool CheckAnswer(string userAnswer)
        {
            if (string.IsNullOrEmpty(userAnswer)) return false;

            // 标准化处理
            string userNorm = userAnswer.Replace(" ", "");
            string ansNorm = correctAnswer.Replace(" ", "");

            // 直接匹配
            if (userNorm == ansNorm) return true;

            // 数字比较 (极限题)
            if (problemType == "limit")
            {
                if (float.TryParse(userAnswer, out float userNum))
                {
                    if (float.TryParse(correctAnswer, out float correctNum))
                    {
                        return Mathf.Abs(userNum - correctNum) < 0.01f;
                    }
                }
            }

            return false;
        }

        // 显示结果
        private void ShowResult(bool correct)
        {
            if (resultText == null) return;

            if (correct)
            {
                resultText.text = "正确! +HP 40  全屏伤害!";
                resultText.color = Color.green;
            }
            else
            {
                resultText.text = $"错误! 正确答案: {correctAnswer}";
                resultText.color = Color.red;
            }
        }

        // 延迟关闭挑战
        private System.Collections.IEnumerator CloseChallengeAfterDelay(bool correct, float delay)
        {
            yield return new WaitForSeconds(delay);

            // 应用效果
            if (correct)
            {
                ApplyCorrectAnswerEffect();
            }

            // 关闭挑战
            CloseChallenge();
        }

        // 应用正确答案效果
        private void ApplyCorrectAnswerEffect()
        {
            Hero hero = GameManager.Instance?.hero;
            if (hero == null) return;

            // 回血
            hero.Heal(40);

            // 全屏伤害
            Collider2D[] enemies = Physics2D.OverlapCircleAll(hero.transform.position, 6f);
            foreach (var collider in enemies)
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null && enemy.IsAlive())
                {
                    enemy.TakeDamage(80);
                    if (ParticleManager.Instance != null)
                    {
                        ParticleManager.Instance.SpawnCalculusEffect(enemy.transform.position);
                    }
                }
            }

            // 加分
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(100);
                GameManager.Instance.ShowMessage("微积分正确! 治疗+全屏伤害!");
            }
        }

        // 关闭挑战
        private void CloseChallenge()
        {
            isActive = false;

            if (challengePanel != null)
            {
                challengePanel.SetActive(false);
            }

            // 恢复游戏
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetState(GameConstants.GameState.Playing);
            }
        }
    }
}
