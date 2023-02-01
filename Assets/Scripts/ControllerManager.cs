using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniSense;

public class ControllerManager : MonoBehaviour
{

    public int LeftTriggerEffectType
    {
        get => (int)leftTriggerState.EffectType;
        set => leftTriggerState.EffectType = SetTriggerEffectType(value);
    }

    public int RightTriggerEffectType
    {
        get => (int)rightTriggerState.EffectType;
        set => rightTriggerState.EffectType = SetTriggerEffectType(value);
    }


    #region Continuous Resistance Properties
    private float LeftContinuousForce
    {
        get => leftTriggerState.Continuous.Force;
        set => leftTriggerState.Continuous.Force = (byte)(value * 255);
    }

    private float LeftContinuousStartPosition
    {
        get => leftTriggerState.Continuous.StartPosition;
        set => leftTriggerState.Continuous.StartPosition = (byte)(value * 255);
    }
    #endregion

    #region Continuous Resistance Properties
    private float RightContinuousForce
    {
        get => rightTriggerState.Continuous.Force;
        set => rightTriggerState.Continuous.Force = (byte)(value * 255);
    }

    private float RightContinuousStartPosition
    {
        get => rightTriggerState.Continuous.StartPosition;
        set => rightTriggerState.Continuous.StartPosition = (byte)(value * 255);
    }
    #endregion

    private DualSenseTriggerState leftTriggerState;
    private DualSenseTriggerState rightTriggerState;


    public static DualSenseGamepadHID DualSense;

    public enum TriggerHapticStrength
    {
        off,
        low,
        medium,
        high,
        ignore
    }

    private TriggerHapticStrength _currentLeft;
    private TriggerHapticStrength _currentRight;

    public enum RumbleStrength
    {
        none,
        low,
        medium,
        high
    }

    float _lowRumble;
    float _highRumble;

    private enum Colors
    {
        r, g, b
    }

    Colors currentColor;

    // Start is called before the first frame update
    void Start()
    {
        DualSense = DualSenseGamepadHID.FindCurrent();

        if(DualSense != null)
        {
            LeftTriggerEffectType = 0;
            RightTriggerEffectType = 0;

            RightContinuousStartPosition = 0.1f;
            RightContinuousForce = 0;

            LeftContinuousStartPosition = 0.1f;
            LeftContinuousForce = 0;
        }

        _currentLeft = TriggerHapticStrength.off;
        _currentRight = TriggerHapticStrength.off;

        _lowRumble = 0;
        _highRumble = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (_lowRumble > float.Epsilon && _highRumble > float.Epsilon)
        {
            DualSense.SetMotorSpeeds(_lowRumble, _highRumble);
        }

        DualSenseGamepadState state = new DualSenseGamepadState
        {
            LeftTrigger = leftTriggerState,
            RightTrigger = rightTriggerState
        };
        DualSense.SetGamepadState(state);

    }

    public void SetTriggerHaptics(TriggerHapticStrength left, TriggerHapticStrength right)
    {
        Debug.Log("Setting Trigger Haptics");
        if (left != _currentLeft)
        {
            switch (left)
            {
                case TriggerHapticStrength.off:
                    LeftContinuousForce = 0;
                    break;
                case TriggerHapticStrength.low:
                    LeftContinuousForce = 0.333f;
                    break;
                case TriggerHapticStrength.medium:
                    LeftContinuousForce = 0.666f;
                    break;
                case TriggerHapticStrength.high:
                    LeftContinuousForce = 1f;
                    break;
                case TriggerHapticStrength.ignore:
                    break;
            }

            _currentLeft = left;
        }

        if (right != _currentRight)
        {
            switch (right)
            {
                case TriggerHapticStrength.off:
                    RightContinuousForce = 0;
                    break;
                case TriggerHapticStrength.low:
                    RightContinuousForce = 0.333f;
                    break;
                case TriggerHapticStrength.medium:
                    RightContinuousForce = 0.666f;
                    break;
                case TriggerHapticStrength.high:
                    RightContinuousForce = 1f;
                    break;
                case TriggerHapticStrength.ignore:
                    break;
            }

            _currentRight = right;
        }
    }

    public void SetRumbleStrength(RumbleStrength rumbleStrength)
    {
        switch (rumbleStrength)
        {
            case RumbleStrength.none:
                _lowRumble = 0;
                _highRumble = 0;
                break;
            case RumbleStrength.low:
                _lowRumble = 0.333f;
                _highRumble = 0.333f;
                break;
            case RumbleStrength.medium:
                _lowRumble = 0.666f;
                _highRumble = 0.666f;
                break;
            case RumbleStrength.high:
                _lowRumble = 1f;
                _highRumble = 1f;
                break;
        }
    }

    private DualSenseTriggerEffectType SetTriggerEffectType(int index)
    {
        if (index == 0) return DualSenseTriggerEffectType.ContinuousResistance;
        if (index == 1) return DualSenseTriggerEffectType.SectionResistance;
        if (index == 2) return DualSenseTriggerEffectType.EffectEx;

        return DualSenseTriggerEffectType.NoResistance;
    }

    bool pulse = false;

    public void PulseController(bool x, RumbleStrength rumbleStrength)
    {
        pulse = x;
        if (pulse)
            StartCoroutine(RumblePulse(rumbleStrength));
    }

    IEnumerator RumblePulse(RumbleStrength rumbleStrength)
    {
        while(pulse)
        {
            SetRumbleStrength(rumbleStrength);
            yield return new WaitForSecondsRealtime(0.5f);
            SetRumbleStrength(RumbleStrength.none);
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}
