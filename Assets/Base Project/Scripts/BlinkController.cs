using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class BlinkController : MonoBehaviour
{
    public uint blinkRate = 1;

    private CanvasGroup canavsGroup;

    private float rate;

	// Use this for initialization
	void Awake ()
    {
        rate = 0f;

        canavsGroup = GetComponent<CanvasGroup>();
        canavsGroup.interactable = false;
        canavsGroup.blocksRaycasts = false;
        canavsGroup.alpha = 1f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        rate = Mathf.Repeat(Time.realtimeSinceStartup, blinkRate);

        if (rate < blinkRate * .5f)
        {
            canavsGroup.alpha = 1f;
        }
        else
        {
            canavsGroup.alpha = 0f;
        }
    }
}
