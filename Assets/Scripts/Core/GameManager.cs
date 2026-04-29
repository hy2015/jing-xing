// 球之积分勇者 - 游戏管理器
// 管理游戏状态、随从切换、能力使用

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace JingXing
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("游戏状态")]
        public GameConstants.GameState currentState = GameConstants.GameState.Menu;

        [Header("角色引用")]
        public Hero hero;
        public List<FollowerBase> followers = new();
        public int activeFollowerIndex = 0;

        [Header("游戏数据")]
        public int score = 0;
        public int gemsCollected = 0;

        [Header("效果状态")]
        public bool magnetActive = false;
        public float magnetTimer = 0f;
        public bool flyingActive = false;
        public float flyingTimer = 0f;

        [Header("消息系统")]
        public string currentMessage = "";
        public float messageTimer = 0f;

        // 事件
        public System.Action<string> OnShowMessage;
        public System.Action<int> OnScoreChanged;
        public System.Action<GameConstants.GameState> OnStateChanged;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            // 消息计时
            if (messageTimer > 0)
            {
                messageTimer -= Time.deltaTime;
            }

            // 磁力效果
            if (magnetActive)
            {
                magnetTimer -= Time.deltaTime;
                if (magnetTimer <= 0)
                {
                    magnetActive = false;
                }
            }

            // 飞行效果
            if (flyingActive)
            {
                flyingTimer -= Time.deltaTime;
                if (flyingTimer <= 0)
                {
                    flyingActive = false;
                }
            }
        }

        public void StartGame()
        {
            SetState(GameConstants.GameState.Playing);
            score = 0;
            gemsCollected = 0;
        }

        public void SetState(GameConstants.GameState newState)
        {
            currentState = newState;
            OnStateChanged?.Invoke(newState);

            if (newState == GameConstants.GameState.Paused)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }

        public void TogglePause()
        {
            if (currentState == GameConstants.GameState.Playing)
            {
                SetState(GameConstants.GameState.Paused);
            }
            else if (currentState == GameConstants.GameState.Paused)
            {
                SetState(GameConstants.GameState.Playing);
            }
        }

        public void GameOver()
        {
            SetState(GameConstants.GameState.GameOver);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ReturnToMenu()
        {
            SetState(GameConstants.GameState.Menu);
            SceneManager.LoadScene("MainMenu");
        }

        // 随从切换
        public void SwitchFollower(int index)
        {
            if (index >= 0 && index < followers.Count)
            {
                activeFollowerIndex = index;
                FollowerBase follower = followers[index];
                string name = follower.GetDisplayName();
                ShowMessage($"随从: {name}");
            }
        }

        public void CycleFollowerColor(bool forward)
        {
            if (followers.Count == 0) return;

            FollowerBase follower = followers[activeFollowerIndex];
            if (follower is ChameleonDinosaur chameleon)
            {
                chameleon.CycleColor(forward);
                ShowMessage($"颜色: {chameleon.GetColorName()}");
            }
            else if (follower is TopologyMonster topology)
            {
                topology.CycleForm(forward);
                ShowMessage($"形态: {topology.GetFormName()}");
            }
        }

        // 使用随从能力
        public void UseFollowerAbility()
        {
            if (followers.Count == 0) return;

            FollowerBase follower = followers[activeFollowerIndex];
            follower.UseAbility();
        }

        // 勇者攻击
        public void HeroAttack()
        {
            if (hero != null)
            {
                hero.PerformAttack();
            }
        }

        // 加分
        public void AddScore(int points)
        {
            score += points;
            OnScoreChanged?.Invoke(score);
        }

        // 收集宝石
        public void CollectGem()
        {
            gemsCollected++;
            AddScore(50);
        }

        // 显示消息
        public void ShowMessage(string message, float duration = 2f)
        {
            currentMessage = message;
            messageTimer = duration;
            OnShowMessage?.Invoke(message);
        }

        // 激活磁力效果
        public void ActivateMagnet(float duration = 3f)
        {
            magnetActive = true;
            magnetTimer = duration;
            ShowMessage("磁力激活! 吸引收集品 + 削弱敌人武器");
        }

        // 激活飞行效果
        public void ActivateFlying(float duration = 3f)
        {
            flyingActive = true;
            flyingTimer = duration;
            ShowMessage("飞行激活!");
        }
    }
}
