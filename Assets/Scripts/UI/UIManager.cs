// 球之积分勇者 - UI管理器

using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace JingXing
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("HUD元素")]
        public Slider healthBar;
        public TextMeshProUGUI healthText;
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI gemsText;
        public TextMeshProUGUI followerNameText;
        public TextMeshProUGUI abilityText;
        public TextMeshProUGUI messageText;

        [Header("随从指示器")]
        public Image[] followerIndicators = new Image[3];

        [Header("效果指示")]
        public TextMeshProUGUI effectText;

        [Header("菜单面板")]
        public GameObject menuPanel;
        public GameObject pausePanel;
        public GameObject gameOverPanel;
        public TextMeshProUGUI finalScoreText;

        [Header("微积分挑战面板")]
        public GameObject calculusPanel;
        public TextMeshProUGUI calculusTitle;
        public TextMeshProUGUI calculusQuestion;
        public TextMeshProUGUI calculusHint;
        public TMP_InputField calculusInput;
        public TextMeshProUGUI calculusResult;

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
            // 订阅事件
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnShowMessage += ShowMessage;
                GameManager.Instance.OnScoreChanged += UpdateScore;
                GameManager.Instance.OnStateChanged += OnGameStateChanged;
            }
        }

        private void Update()
        {
            if (GameManager.Instance == null) return;

            // 更新消息显示
            if (GameManager.Instance.messageTimer > 0)
            {
                messageText.gameObject.SetActive(true);
                messageText.text = GameManager.Instance.currentMessage;
                float alpha = Mathf.Min(1f, GameManager.Instance.messageTimer * 0.5f);
                messageText.alpha = alpha;
            }
            else
            {
                messageText.gameObject.SetActive(false);
            }

            // 更新效果指示
            UpdateEffectIndicators();
        }

        public void UpdateHealth(int current, int max)
        {
            if (healthBar != null)
            {
                healthBar.value = (float)current / max;
            }
            if (healthText != null)
            {
                healthText.text = $"HP: {current}/{max}";
            }
        }

        public void UpdateScore(int score)
        {
            if (scoreText != null)
            {
                scoreText.text = $"分数: {score}";
            }
        }

        public void UpdateGems(int gems)
        {
            if (gemsText != null)
            {
                gemsText.text = $"宝石: {gems}";
            }
        }

        public void UpdateFollowerInfo(int index, string name, string ability)
        {
            if (followerNameText != null)
            {
                followerNameText.text = $"随从: {name}";
            }
            if (abilityText != null)
            {
                abilityText.text = ability;
            }

            // 更新指示器
            for (int i = 0; i < followerIndicators.Length; i++)
            {
                if (followerIndicators[i] != null)
                {
                    followerIndicators[i].color = (i == index) ? Color.green : Color.gray;
                }
            }
        }

        public void ShowMessage(string message)
        {
            if (messageText != null)
            {
                messageText.text = message;
                messageText.gameObject.SetActive(true);
            }
        }

        private void UpdateEffectIndicators()
        {
            if (effectText == null) return;

            string effects = "";
            if (GameManager.Instance.magnetActive)
            {
                effects += "磁力激活! ";
            }
            if (GameManager.Instance.flyingActive)
            {
                effects += "飞行激活! ";
            }
            effectText.text = effects;
            effectText.gameObject.SetActive(!string.IsNullOrEmpty(effects));
        }

        private void OnGameStateChanged(GameConstants.GameState state)
        {
            // 显示/隐藏面板
            if (menuPanel != null) menuPanel.SetActive(state == GameConstants.GameState.Menu);
            if (pausePanel != null) pausePanel.SetActive(state == GameConstants.GameState.Paused);
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(state == GameConstants.GameState.GameOver);
                if (state == GameConstants.GameState.GameOver && finalScoreText != null)
                {
                    finalScoreText.text = $"最终分数: {GameManager.Instance.score}";
                }
            }
        }

        // 微积分挑战UI
        public void ShowCalculusChallenge(string question, string hint)
        {
            if (calculusPanel != null)
            {
                calculusPanel.SetActive(true);
                if (calculusTitle != null) calculusTitle.text = "人参怪 - 微积分挑战!";
                if (calculusQuestion != null) calculusQuestion.text = question;
                if (calculusHint != null) calculusHint.text = hint;
                if (calculusInput != null) calculusInput.text = "";
                if (calculusResult != null) calculusResult.text = "";
            }
        }

        public void HideCalculusChallenge()
        {
            if (calculusPanel != null)
            {
                calculusPanel.SetActive(false);
            }
        }

        public void ShowCalculusResult(bool correct, string answer = "")
        {
            if (calculusResult != null)
            {
                if (correct)
                {
                    calculusResult.text = "正确! +HP 40  全屏伤害!";
                    calculusResult.color = Color.green;
                }
                else
                {
                    calculusResult.text = $"错误! 正确答案: {answer}";
                    calculusResult.color = Color.red;
                }
            }
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnShowMessage -= ShowMessage;
                GameManager.Instance.OnScoreChanged -= UpdateScore;
                GameManager.Instance.OnStateChanged -= OnGameStateChanged;
            }
        }
    }
}
