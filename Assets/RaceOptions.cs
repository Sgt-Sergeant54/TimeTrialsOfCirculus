using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaceOptions : MonoBehaviour
{
    [SerializeField] private Slider lapsSlider;
    [SerializeField] private Slider timeAfterFinishSlider;

    [SerializeField] private TextMeshProUGUI currentLaps;
    [SerializeField] private TextMeshProUGUI currentTimeAfter;

    public void changeCurrentLaps()
    {
        currentLaps.text = lapsSlider.value.ToString();
    }

    public void changeCurrentTimeAfter()
    {
        currentTimeAfter.text = timeAfterFinishSlider.value.ToString();
    }
}
