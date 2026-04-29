// 球之积分勇者 - 粒子管理器
// 管理各种粒子效果

using UnityEngine;
using System.Collections.Generic;

namespace JingXing
{
    public class ParticleManager : MonoBehaviour
    {
        public static ParticleManager Instance { get; private set; }

        [Header("粒子预制体")]
        public GameObject hitEffectPrefab;
        public GameObject breakEffectPrefab;
        public GameObject fireEffectPrefab;
        public GameObject lightEffectPrefab;
        public GameObject lightningEffectPrefab;
        public GameObject waterEffectPrefab;
        public GameObject iceEffectPrefab;
        public GameObject absoluteZeroEffectPrefab;
        public GameObject blackHoleEffectPrefab;
        public GameObject portalEffectPrefab;
        public GameObject trapEffectPrefab;
        public GameObject vineEffectPrefab;
        public GameObject collectEffectPrefab;
        public GameObject deathEffectPrefab;
        public GameObject switchEffectPrefab;
        public GameObject gateEffectPrefab;
        public GameObject calculusEffectPrefab;

        [Header("通用设置")]
        public int poolSize = 50;
        public float defaultLifetime = 1f;

        private Dictionary<string, Queue<GameObject>> particlePool = new();

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
            // 初始化粒子池
            InitializePool();
        }

        private void InitializePool()
        {
            // 可以在这里预生成粒子对象
        }

        // 生成粒子效果
        public void SpawnEffect(GameObject prefab, Vector3 position, float lifetime = -1)
        {
            if (prefab == null) return;

            GameObject effect = Instantiate(prefab, position, Quaternion.identity);
            if (lifetime < 0) lifetime = defaultLifetime;
            Destroy(effect, lifetime);
        }

        // 各种效果的快捷方法
        public void SpawnHitEffect(Vector3 position)
        {
            SpawnEffect(hitEffectPrefab, position, 0.5f);
        }

        public void SpawnBreakEffect(Vector3 position)
        {
            SpawnEffect(breakEffectPrefab, position, 1f);
        }

        public void SpawnFireEffect(Vector3 position)
        {
            SpawnEffect(fireEffectPrefab, position, 1.5f);
        }

        public void SpawnLightEffect(Vector3 position)
        {
            SpawnEffect(lightEffectPrefab, position, 0.8f);
        }

        public void SpawnLightningEffect(Vector3 position)
        {
            SpawnEffect(lightningEffectPrefab, position, 0.6f);
        }

        public void SpawnWaterEffect(Vector3 position)
        {
            SpawnEffect(waterEffectPrefab, position, 1f);
        }

        public void SpawnIceEffect(Vector3 position)
        {
            SpawnEffect(iceEffectPrefab, position, 1f);
        }

        public void SpawnAbsoluteZeroEffect(Vector3 position)
        {
            SpawnEffect(absoluteZeroEffectPrefab, position, 2f);
        }

        public void SpawnBlackHoleEffect(Vector3 position)
        {
            SpawnEffect(blackHoleEffectPrefab, position, 1.5f);
        }

        public void SpawnPortalEffect(Vector3 position)
        {
            SpawnEffect(portalEffectPrefab, position, 2f);
        }

        public void SpawnTrapEffect(Vector3 position)
        {
            SpawnEffect(trapEffectPrefab, position, 1.5f);
        }

        public void SpawnVineEffect(Vector3 position)
        {
            SpawnEffect(vineEffectPrefab, position, 1f);
        }

        public void SpawnCollectEffect(Vector3 position)
        {
            SpawnEffect(collectEffectPrefab, position, 0.8f);
        }

        public void SpawnDeathEffect(Vector3 position)
        {
            SpawnEffect(deathEffectPrefab, position, 1f);
        }

        public void SpawnSwitchEffect(Vector3 position)
        {
            SpawnEffect(switchEffectPrefab, position, 1f);
        }

        public void SpawnGateEffect(Vector3 position)
        {
            SpawnEffect(gateEffectPrefab, position, 1.5f);
        }

        public void SpawnCalculusEffect(Vector3 position)
        {
            SpawnEffect(calculusEffectPrefab, position, 1f);
        }
    }
}
