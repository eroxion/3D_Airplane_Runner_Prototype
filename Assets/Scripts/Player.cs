using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Inputs _inputs;
    [SerializeField] private LaneControl _laneControl;
    [SerializeField] private float _speed;
    [SerializeField] private float _slowmodeFactor;
    [SerializeField] private float _slowModeTime;
    private IEnumerator _slowMode;
    private bool[] _lanesStatus;
    private const string _obstacleTag = "Obstacle";
    private const string _powerUpYellowTag = "PowerUp_Yellow";
    private const string _powerUpTag = "PowerUp";

    private void Start() {
        StartCoroutine(_slowMode);
    }

    private void Update() {
        _slowMode = _startSlowMode(_slowModeTime);
        int _currentLane = _laneControl.GetCurrentLane();
        MovePlayer(_currentLane);
    }

    private void MovePlayer(int _currentLane) {
        _lanesStatus = _laneControl.GetLanesStatus();
        int _moveMagnitude = (_inputs.GetButtonPressed() == false) ? _inputs.Get1DAxis() : 0;
        if(_moveMagnitude != 0) _inputs.SetButtonPressed(true);

        if (_currentLane <= 0) _moveMagnitude = _moveMagnitude > 0 ? _moveMagnitude : 0;
        else if (_currentLane >= _lanesStatus.Length - 1) _moveMagnitude = _moveMagnitude > 0 ? 0 : _moveMagnitude;

        Vector3 _moveToVector = new Vector3(_laneControl.GetLaneX(_currentLane + _moveMagnitude), this.transform.position.y, this.transform.position.z);
        this.transform.position = Vector3.Lerp(this.transform.position, _moveToVector, Time.deltaTime * _speed);
        _laneControl.SetCurrentLane(_currentLane, _currentLane + _moveMagnitude);
    }
    private void OnCollisionEnter(Collision collision) {
        if(collision.transform.parent.tag == _obstacleTag) {
            Debug.Log("Game Over!");
            Time.timeScale = 0;
        }
        else if(collision.transform.parent.tag == _powerUpTag) {
            if(collision.transform.tag == _powerUpYellowTag) {
                StartCoroutine(_slowMode);
            }
        }
    }
    private IEnumerator _startSlowMode(float _slowModeTime) {
        Time.timeScale *= _slowmodeFactor;
        yield return new WaitForSeconds(_slowModeTime);
        Time.timeScale = 1;
    }
}
