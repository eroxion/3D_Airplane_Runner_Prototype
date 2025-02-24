using System.Collections;
using UnityEngine;

public class LaneControl : MonoBehaviour
{
    [SerializeField] private float[] _laneX;
    private bool[] _laneStatus;
    [SerializeField] private int _currentLane;
    [SerializeField] private GameObject _floor;
    [SerializeField] private Transform _floorParent;
    [SerializeField] private float _floorSize;
    private IEnumerator _floorGenerator;
    [SerializeField] private float _secondsToGenerate;
    [SerializeField] private float _floorMoveSpeed;
    private Transform _player;


    private void Start() {
        _player = FindFirstObjectByType<Player>().transform;
        if (_laneX.Length <= 1) {
            Debug.LogWarning("Lane X: Size should be greater than 1");
            Time.timeScale = 0;
        }
        _laneStatus = new bool[_laneX.Length];
        SetDefaultLane();
        _floorGenerator = _floorInstantiate(_secondsToGenerate);
        StartCoroutine(_floorGenerator);
    }

    private void Update() {
        this.transform.Translate(Vector3.back * Time.deltaTime * _floorMoveSpeed);
        if (_floorParent.GetChild(0).transform.position.z < _player.position.z - _floorSize) Destroy(_floorParent.GetChild(0).gameObject);
    }

    private void SetDefaultLane() {
        for (int i = 0; i < _laneStatus.Length; i++) {
            if (i == _currentLane) _laneStatus[i] = true;
            else _laneStatus[i] = false;
        }
    }

    internal void SetCurrentLane(int _oldLane, int _targetLane) {
        _laneStatus[_oldLane] = false;
        _laneStatus[_targetLane] = true;
        _currentLane = _targetLane;
    }

    internal int GetCurrentLane() {
        return _currentLane;
    }

    internal float GetLaneX(int _index) {
        return _laneX[_index];
    }

    internal bool[] GetLanesStatus() {
        return _laneStatus;
    }

    internal int LaneCount() {
        return _laneX.Length;
    }

    private IEnumerator _floorInstantiate(float _secondsToGenerate) {
        float _generateDistance = _floor.transform.position.z;
        while (true) {
            float _zPosition = _generateDistance + _floorParent.transform.position.z;
            Vector3 _floorVector3 = new Vector3(_floor.transform.position.x, _floor.transform.position.y, _zPosition);
            Instantiate(_floor, _floorVector3, _floor.transform.rotation, _floorParent);
            _generateDistance += _floorSize;
            yield return new WaitForSeconds(_secondsToGenerate);
        }
    }
}
