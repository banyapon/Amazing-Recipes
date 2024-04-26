using System.Collections.Generic;
using UnityEngine;

namespace KitchenChaos.Interactions.Visual
{
    public class PlatesCounterVisual : MonoBehaviour
    {
        [SerializeField] PlatesCounter _platesCounter;
        [SerializeField] Transform _counterSpawnPoint;
        [SerializeField] Transform _plateVisualPrefab;
        [SerializeField] float _spawnOffsetY = .1f;
        List<GameObject> _plateVisualsList = new List<GameObject>();

        private void Start()
        {
            _platesCounter.OnPlateSpawned += _platesCounter_OnPlateSpawned;
            _platesCounter.OnPlateRemoved += _platesCounter_OnPlateRemoved;
        }

        private void _platesCounter_OnPlateSpawned()
        {
            Transform plateVisualTransform = Instantiate(_plateVisualPrefab, _counterSpawnPoint);

            plateVisualTransform.localPosition = new Vector3(0, _spawnOffsetY * _plateVisualsList.Count, 0);

            _plateVisualsList.Add(plateVisualTransform.gameObject);
        }

        private void _platesCounter_OnPlateRemoved()
        {
            GameObject plate = _plateVisualsList[_plateVisualsList.Count - 1];
            _plateVisualsList.Remove(plate);
            Destroy(plate); //prototype, implement pooling
        }

        void OnDestroy()
        {
            _platesCounter.OnPlateSpawned -= _platesCounter_OnPlateSpawned;
            _platesCounter.OnPlateRemoved -= _platesCounter_OnPlateRemoved;
        }
    }
}
