using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts.SharedOverall.Tools.Logic;
using _Scripts.SharedOverall.UI;

public class PipetteColorChangeListener : ColorChangeListener
{
    protected new void OnEnable()
    {
        ClickOnPixel.SetPipetteColor += OnColorChange;
    }
    protected new void OnDisable()
    {
        ClickOnPixel.SetPipetteColor -= OnColorChange;
    }
}
