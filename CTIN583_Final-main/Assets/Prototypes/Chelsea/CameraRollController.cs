using UnityEngine;

namespace Unrealtor.Player
{
    public class CameraRollController : MonoBehaviour
    {
        [Header("Roll Settings")]
        [SerializeField] private Transform CameraTransform;
        [SerializeField] private float RollSpeed = 90f;
        [SerializeField] private float RollReturnSpeed = 180f;

        private float TargetRoll = 0f;
        private float CurrentRoll = 0f;

        public void UpdateRoll(float xRotation)
        {
            float rollInput = 0f;
            if (Input.GetKey(KeyCode.Q)) rollInput += 1f;
            if (Input.GetKey(KeyCode.E)) rollInput -= 1f;

            if (rollInput != 0f)
            {
                TargetRoll += rollInput * RollSpeed * Time.deltaTime;
                TargetRoll = Mathf.Clamp(TargetRoll, -60f, 60f);
            }
            else
            {
                TargetRoll = Mathf.Lerp(TargetRoll, 0f, Time.deltaTime * (RollReturnSpeed / 90f));
            }

            CurrentRoll = Mathf.Lerp(CurrentRoll, TargetRoll, Time.deltaTime * 10f);

            Vector3 currentEuler = CameraTransform.localEulerAngles;
            if (currentEuler.x > 180) currentEuler.x -= 360;

            currentEuler.x = xRotation;
            currentEuler.z = CurrentRoll;

            CameraTransform.localEulerAngles = currentEuler;
        }
    }
}