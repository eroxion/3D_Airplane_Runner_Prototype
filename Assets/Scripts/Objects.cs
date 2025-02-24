using System.Collections;
using UnityEngine;

public class Objects : MonoBehaviour
{
    [SerializeField] private GameObject[] _obstacles;
    [SerializeField] private GameObject[] _powerUps;
    [SerializeField] private float _obstaclesDistance;
    [SerializeField] private int _obstaclesFrequency;
    [SerializeField] private float _secondsToGenerateObstacles;
    [SerializeField] private float _secondsToGeneratePowerUps;
    private IEnumerator _obstaclesGenerator;
    private IEnumerator _powerUpsGenerator;
    [SerializeField] private Transform _obstaclesParent;
    [SerializeField] private Transform _powerUpsParent;
    private LaneControl _laneControl;
    private Transform _player;

    private void Start() {
        _laneControl = FindAnyObjectByType<LaneControl>();
        _player = FindFirstObjectByType<Player>().transform;
        _obstaclesGenerator = _obstaclesInstantiate(_secondsToGenerateObstacles);
        _powerUpsGenerator = _powerUpsInstantiate(_secondsToGeneratePowerUps);
        StartCoroutine(_obstaclesGenerator);
        StartCoroutine(_powerUpsGenerator);
    }
    private IEnumerator _obstaclesInstantiate(float _secondsToGenerateObstacles) {
        float _generateDistance = _obstaclesDistance;
        while (true) {
            for (int i = 0; i < _obstaclesFrequency; i++) {
                int _randomIndex = Random.Range(0, _obstacles.Length);
                int _randomLaneX = Random.Range(0, _laneControl.LaneCount());
                float _zPosition = _obstacles[_randomIndex].transform.position.z + _generateDistance;
                float _xPosition = _laneControl.GetLaneX(_randomLaneX);
                Vector3 _obstacleVector3 = new Vector3(_xPosition, _obstacles[_randomIndex].transform.position.y, _zPosition);
                Instantiate(_obstacles[_randomIndex], _obstacleVector3, _obstacles[_randomIndex].transform.rotation, _obstaclesParent);
                _generateDistance += _obstaclesDistance;
            }
            
            int _lastChildCount = _obstaclesParent.childCount - 1;
            _generateDistance = _obstaclesParent.GetChild(_lastChildCount).transform.position.z - _obstaclesDistance;
            yield return new WaitForSeconds(_secondsToGenerateObstacles);
        }
    }

    private IEnumerator _powerUpsInstantiate(float _secondsToGeneratePowerUps) {
        float _generateDistance = _obstaclesDistance;
        while (true) {
            int _randomIndex = Random.Range(0, _powerUps.Length);
            int _randomLaneX = Random.Range(0, _laneControl.LaneCount());
            float _zPosition = _powerUps[_randomIndex].transform.position.z + _generateDistance;
            float _xPosition = _laneControl.GetLaneX(_randomLaneX);
            Vector3 _powerUpsVector3 = new Vector3(_xPosition, _powerUps[_randomIndex].transform.position.y, _zPosition);
            Instantiate(_powerUps[_randomIndex], _powerUpsVector3, _powerUps[_randomIndex].transform.rotation, _powerUpsParent);
            _generateDistance += _obstaclesDistance;
            yield return new WaitForSeconds(_secondsToGeneratePowerUps);
        }
    }

}
