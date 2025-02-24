using UnityEngine;

public class Inputs : MonoBehaviour
{
    private ControlsMapping _controlsMapping;
    private bool _isButtonPressed;
    private int _buttonValue;

    private void Start() {
        _controlsMapping = new ControlsMapping();
        _controlsMapping.Controls.Enable();
        _buttonValue = 0;
        _isButtonPressed = false;
    }

    private void Update() {
        _buttonValue = (int)_controlsMapping.Controls.Movement.ReadValue<float>();
        if (_buttonValue == 0) _isButtonPressed = false;
    }

    internal int Get1DAxis() {
        return _buttonValue;
    }

    internal bool GetButtonPressed() {
        return _isButtonPressed;
    }

    internal void SetButtonPressed(bool _value) {
        _isButtonPressed = _value;
    }
}
