using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LevelChargeVisuals : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Volume globalVolume;
    private ColorAdjustments colorAdjustments;

    private void Awake()
    {
        globalVolume.profile.TryGet(out colorAdjustments);
    }

    private void Update()
    {
        colorAdjustments.postExposure.value = levelManager.GetChargeAmount();
    }
}
