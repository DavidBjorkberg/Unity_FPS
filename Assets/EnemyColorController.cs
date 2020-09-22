using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyColorController : MonoBehaviour
{
    public Color standardColour;
    public Color awareColour;
    public Color hostileColour;
    public MeshRenderer[] modelComponents;
    private Coroutine lerpColourCoroutine;
    public void SwitchToStandardColour()
    {
        StopColourChange();
        for (int i = 0; i < modelComponents.Length; i++)
        {
            modelComponents[i].material.color = standardColour;
        }
    }
    public void SwitchToAwareColour(bool lerpBetweenColours = false)
    {
        if (lerpBetweenColours)
        {
            StopColourChange();

            lerpColourCoroutine = StartCoroutine(LerpBetweenColours(modelComponents[0].material.color, awareColour));
        }
        else
        {
            for (int i = 0; i < modelComponents.Length; i++)
            {
                modelComponents[i].material.color = awareColour;
            }
        }
    }

    public Coroutine SwitchToHostileColour()
    {
        StopColourChange();
        lerpColourCoroutine = StartCoroutine(LerpBetweenColours(awareColour, hostileColour));
        return lerpColourCoroutine;
    }
    IEnumerator LerpBetweenColours(Color colorA, Color colorB)
    {
        print("Started colour change");
        float lerpValue = 0;
        float changeColorSpeed = 1;
        Color lerpColor;
        while (lerpValue < 1)
        {
            for (int i = 0; i < modelComponents.Length; i++)
            {
                lerpColor = Color.Lerp(colorA, colorB, lerpValue);
                modelComponents[i].material.color = lerpColor;
            }
            lerpValue += changeColorSpeed * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        print("Completed colour change");
    }
    void StopColourChange()
    {
        if (lerpColourCoroutine != null)
        {
            print("Stopped colour change");
            StopCoroutine(lerpColourCoroutine);
        }
    }
}
