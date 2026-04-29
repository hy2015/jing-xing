// 球之积分勇者 - 森林关卡构建器
// 构建第一关: 森林

using UnityEngine;

namespace JingXing
{
    public class ForestLevelBuilder : MonoBehaviour
    {
        [Header("预制体引用")]
        public GameObject platformPrefab;
        public GameObject heroPrefab;
        public GameObject chameleonPrefab;
        public GameObject topologyPrefab;
        public GameObject ginsengPrefab;
        public GameObject beetlePrefab;
        public GameObject beePrefab;
        public GameObject antPrefab;
        public GameObject spiderPrefab;
        public GameObject gemPrefab;
        public GameObject heartPrefab;
        public GameObject keyPrefab;
        public GameObject breakableBlockPrefab;
        public GameObject thornsPrefab;
        public GameObject switchPrefab;
        public GameObject gatePrefab;

        [Header("关卡设置")]
        public float groundY = -2f;
        public float tileSize = 1f;

        private void Start()
        {
            BuildLevel();
        }

        private void BuildLevel()
        {
            // 创建地面
            CreateGround();

            // 创建平台
            CreatePlatforms();

            // 创建可破坏墙壁
            CreateBreakableBlocks();

            // 创建荆棘障碍
            CreateThorns();

            // 创建敌人
            CreateEnemies();

            // 创建收集品
            CreateCollectibles();

            // 创建开关和门
            CreateSwitchesAndGates();

            // 创建玩家角色
            CreatePlayerCharacters();

            // 设置相机边界
            SetupCameraBounds();
        }

        private void CreateGround()
        {
            // 主地面 (分段)
            CreatePlatformSegment(0, groundY, 16, 1, Platform.PlatformType.Ground);
            CreatePlatformSegment(17, groundY, 8, 1, Platform.PlatformType.Ground);
            CreatePlatformSegment(26, groundY, 22, 1, Platform.PlatformType.Ground);

            // 坑洞区 (16-17之间有间隙)

            // 地下层
            CreatePlatformSegment(0, groundY - 1, 48, 1, Platform.PlatformType.Dirt);
        }

        private void CreatePlatforms()
        {
            // 区域1: 起始区
            CreatePlatformSegment(3, groundY + 2, 2, 0.3f, Platform.PlatformType.Stone);
            CreatePlatformSegment(5.5f, groundY + 3, 1.5f, 0.3f, Platform.PlatformType.Stone);
            CreatePlatformSegment(4, groundY + 4, 1.2f, 0.3f, Platform.PlatformType.Stone);

            // 区域2: 需要破墙
            CreatePlatformSegment(7.5f, groundY + 2.5f, 1, 0.3f, Platform.PlatformType.Stone);
            CreatePlatformSegment(9, groundY + 3.5f, 1, 0.3f, Platform.PlatformType.Stone);
            CreatePlatformSegment(10.5f, groundY + 4.5f, 1.8f, 0.3f, Platform.PlatformType.Stone);

            // 区域3: 高平台区
            CreatePlatformSegment(13, groundY + 2, 1.2f, 0.3f, Platform.PlatformType.Stone);
            CreatePlatformSegment(13, groundY + 4, 1.2f, 0.3f, Platform.PlatformType.Stone);
            CreatePlatformSegment(14.5f, groundY + 3, 1, 0.3f, Platform.PlatformType.Stone);

            // 区域4: 坑洞后
            CreatePlatformSegment(18, groundY + 2.5f, 1.5f, 0.3f, Platform.PlatformType.Stone);
            CreatePlatformSegment(20, groundY + 3.5f, 1.2f, 0.3f, Platform.PlatformType.Stone);
            CreatePlatformSegment(22, groundY + 4.5f, 2, 0.3f, Platform.PlatformType.Stone);

            // 区域5: 最终区
            CreatePlatformSegment(27, groundY + 2, 1.5f, 0.3f, Platform.PlatformType.Stone);
            CreatePlatformSegment(29.5f, groundY + 3, 1, 0.3f, Platform.PlatformType.Stone);
            CreatePlatformSegment(31, groundY + 4, 1.5f, 0.3f, Platform.PlatformType.Stone);
            CreatePlatformSegment(33.5f, groundY + 2.5f, 1.2f, 0.3f, Platform.PlatformType.Stone);
            CreatePlatformSegment(35.5f, groundY + 3.5f, 2, 0.3f, Platform.PlatformType.Stone);
        }

        private void CreateBreakableBlocks()
        {
            // 需要红色巨力破坏的墙壁
            CreateBreakableBlock(7, groundY + 1);
            CreateBreakableBlock(7, groundY + 2);
            CreateBreakableBlock(25, groundY + 1);
            CreateBreakableBlock(25, groundY + 2);
            CreateBreakableBlock(25, groundY + 3);
        }

        private void CreateThorns()
        {
            // 荆棘障碍
            CreateThorns(6, groundY + 1);
            CreateThorns(12, groundY + 1);
            CreateThorns(24, groundY + 1);
            CreateThorns(24.5f, groundY + 1);
        }

        private void CreateEnemies()
        {
            // 甲虫 (地面巡逻)
            CreateEnemy(beetlePrefab, 3.5f, groundY + 1);
            CreateEnemy(beetlePrefab, 8.5f, groundY + 1);
            CreateEnemy(beetlePrefab, 18, groundY + 1);
            CreateEnemy(beetlePrefab, 28, groundY + 1);
            CreateEnemy(beetlePrefab, 32, groundY + 1);

            // 蜜蜂 (空中飞行)
            CreateEnemy(beePrefab, 5, groundY + 5);
            CreateEnemy(beePrefab, 11, groundY + 6);
            CreateEnemy(beePrefab, 19, groundY + 5.5f);
            CreateEnemy(beePrefab, 30, groundY + 6.5f);
            CreateEnemy(beePrefab, 34, groundY + 5);

            // 蚂蚁 (群体行动)
            CreateEnemy(antPrefab, 10, groundY + 1);
            CreateEnemy(antPrefab, 10.3f, groundY + 1);
            CreateEnemy(antPrefab, 10.6f, groundY + 1);
            CreateEnemy(antPrefab, 29, groundY + 1);
            CreateEnemy(antPrefab, 29.3f, groundY + 1);

            // 蜘蛛 (蛛网摆动)
            CreateEnemy(spiderPrefab, 14, groundY + 5);
            CreateEnemy(spiderPrefab, 21.5f, groundY + 6);
            CreateEnemy(spiderPrefab, 33, groundY + 6.5f);
        }

        private void CreateCollectibles()
        {
            // 宝石
            CreateCollectible(gemPrefab, 3.7f, groundY + 3);
            CreateCollectible(gemPrefab, 4.4f, groundY + 5);
            CreateCollectible(gemPrefab, 9.5f, groundY + 4);
            CreateCollectible(gemPrefab, 11, groundY + 5.5f);
            CreateCollectible(gemPrefab, 15, groundY + 4);
            CreateCollectible(gemPrefab, 18.5f, groundY + 3.5f);
            CreateCollectible(gemPrefab, 20.5f, groundY + 4.5f);
            CreateCollectible(gemPrefab, 22.5f, groundY + 5.5f);
            CreateCollectible(gemPrefab, 27.5f, groundY + 3);
            CreateCollectible(gemPrefab, 30, groundY + 4);
            CreateCollectible(gemPrefab, 31.5f, groundY + 5);
            CreateCollectible(gemPrefab, 34, groundY + 3.5f);
            CreateCollectible(gemPrefab, 36, groundY + 4.5f);

            // 心心
            CreateCollectible(heartPrefab, 6, groundY + 3);
            CreateCollectible(heartPrefab, 13.5f, groundY + 5);
            CreateCollectible(heartPrefab, 18.5f, groundY + 5);

            // 钥匙
            CreateCollectible(keyPrefab, 13.5f, groundY + 6);
        }

        private void CreateSwitchesAndGates()
        {
            // 开关和门
            CreateSwitch(38, groundY + 1, 0);
            CreateGate(40, groundY + 1, 0);
        }

        private void CreatePlayerCharacters()
        {
            // 创建勇者
            if (heroPrefab != null)
            {
                GameObject hero = Instantiate(heroPrefab, new Vector3(1, groundY + 2, 0), Quaternion.identity);
                hero.name = "Hero";
            }

            // 创建随从
            if (chameleonPrefab != null)
            {
                GameObject chameleon = Instantiate(chameleonPrefab, new Vector3(-0.5f, groundY + 2, 0), Quaternion.identity);
                chameleon.name = "ChameleonDinosaur";
            }

            if (topologyPrefab != null)
            {
                GameObject topology = Instantiate(topologyPrefab, new Vector3(-2, groundY + 2, 0), Quaternion.identity);
                topology.name = "TopologyMonster";
            }

            if (ginsengPrefab != null)
            {
                GameObject ginseng = Instantiate(ginsengPrefab, new Vector3(-3.5f, groundY + 2, 0), Quaternion.identity);
                ginseng.name = "GinsengMonster";
            }
        }

        private void SetupCameraBounds()
        {
            CameraFollow camera = Camera.main?.GetComponent<CameraFollow>();
            if (camera != null)
            {
                camera.SetBounds(-1, 48, -5, 15);
            }
        }

        // 辅助方法
        private void CreatePlatformSegment(float x, float y, float width, float height, Platform.PlatformType type)
        {
            if (platformPrefab == null) return;

            GameObject platform = Instantiate(platformPrefab, new Vector3(x + width / 2, y + height / 2, 0), Quaternion.identity);
            platform.transform.localScale = new Vector3(width, height, 1);
            platform.name = $"Platform_{type}_{x}_{y}";

            Platform platformScript = platform.GetComponent<Platform>();
            if (platformScript != null)
            {
                platformScript.platformType = type;
            }
        }

        private void CreateBreakableBlock(float x, float y)
        {
            if (breakableBlockPrefab == null) return;

            GameObject block = Instantiate(breakableBlockPrefab, new Vector3(x, y, 0), Quaternion.identity);
            block.name = $"BreakableBlock_{x}_{y}";
        }

        private void CreateThorns(float x, float y)
        {
            if (thornsPrefab == null) return;

            GameObject thorns = Instantiate(thornsPrefab, new Vector3(x, y, 0), Quaternion.identity);
            thorns.name = $"Thorns_{x}_{y}";
        }

        private void CreateEnemy(GameObject prefab, float x, float y)
        {
            if (prefab == null) return;

            GameObject enemy = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
            enemy.name = $"Enemy_{prefab.name}_{x}_{y}";
        }

        private void CreateCollectible(GameObject prefab, float x, float y)
        {
            if (prefab == null) return;

            GameObject collectible = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
            collectible.name = $"Collectible_{prefab.name}_{x}_{y}";
        }

        private void CreateSwitch(float x, float y, int id)
        {
            if (switchPrefab == null) return;

            GameObject switchObj = Instantiate(switchPrefab, new Vector3(x, y, 0), Quaternion.identity);
            switchObj.name = $"Switch_{id}";

            Switch switchScript = switchObj.GetComponent<Switch>();
            if (switchScript != null)
            {
                switchScript.switchId = id;
            }
        }

        private void CreateGate(float x, float y, int id)
        {
            if (gatePrefab == null) return;

            GameObject gateObj = Instantiate(gatePrefab, new Vector3(x, y, 0), Quaternion.identity);
            gateObj.name = $"Gate_{id}";

            Gate gateScript = gateObj.GetComponent<Gate>();
            if (gateScript != null)
            {
                gateScript.gateId = id;
            }
        }
    }
}
