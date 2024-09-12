using UnityEngine;

namespace Varto.FPSTests
{
    public class Varto_InstantiatePrefabInUpdateTest : MonoBehaviour
    {
        [SerializeField] GameObject _prefab;
        [SerializeField] private float _toggleEverySeconds;

        float _timer = 0;
        bool _isInstantiatedLastTime;
        private GameObject instance;

        private void Awake()
        {
            _timer = _toggleEverySeconds;
            instance = Instantiate(_prefab, transform);
        }

        private void Update()
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
                return;
            }
            instance.SetActive(!instance.activeSelf);
        }
    }
}
