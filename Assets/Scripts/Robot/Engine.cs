using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniSense;

namespace HapticFeedback
{
    public class Engine : MonoBehaviour
    {

        public static DualSenseGamepadHID DualSense;
        [SerializeField] public Robot robot;

        private enum Colors
        {
            r, g, b
        }

        Colors currentColor;

        // Start is called before the first frame update
        void Start()
        {
            DualSense = UniSense.DualSenseGamepadHID.FindCurrent();
            currentColor = Colors.r;
            SetColor(currentColor);
            DualSense.ResumeHaptics();
            Rumble(0.5f, 0.5f);
        }

        // Update is called once per frame
        void Update()
        {
            if (DualSense.crossButton.wasPressedThisFrame)
                NextColor();
        }

        void NextColor()
        {
            if (currentColor != Colors.b)
                currentColor = (currentColor + 1);
            else
                currentColor = Colors.r;
            SetColor(currentColor);
        }

        void SetColor(Colors color)
        {
            if (color == Colors.r)
                DualSense.SetLightBarColor(Color.red);
            else if (color == Colors.g)
                DualSense.SetLightBarColor(Color.green);
            else
                DualSense.SetLightBarColor(Color.blue);
        }

        void Rumble(float left, float right)
        {
            DualSense.SetMotorSpeeds(left, right);
        }

    }
}
