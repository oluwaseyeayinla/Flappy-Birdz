using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchTest : MonoBehaviour {

    public enum Raycaster
    {
        Graphic,
        Physics,
        Physics2D
    }

    public Raycaster raycaster;

    float touchPressure = 0f;
    bool countingPressure = false;
    bool couroutineIsRunning = false;

    
    bool fireRay = false;
	// Use this for initialization
	void Start ()
    {
        couroutineIsRunning = true;
        StartCoroutine(UpdatePressure());
	}

    IEnumerator UpdatePressure()
    {
        while (couroutineIsRunning)
        {
            if (countingPressure)
            {
                touchPressure += Time.deltaTime;
            }

            yield return null;
        }

        yield return null;
    }

	// Update is called once per frame
	void Update ()
    {
        Vector2 position = Vector2.zero;

        if (Input.GetButtonDown("Cancel"))
        {
            couroutineIsRunning = false;
            return;
        }

        if (Application.isMobilePlatform)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    if (touch.fingerId == 0 && Input.touchPressureSupported)
                    {
                        countingPressure = false;
                        Debug.Log("Pressure: " + touch.pressure);
                        return;
                    }
                    else if (touch.fingerId == 0)
                    {
                        touchPressure = 0f;
                        countingPressure = true;
                        position = touch.position;
                        return;
                    }
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    if (touch.fingerId == 0 && countingPressure)
                    {
                        Debug.Log("Pressure: " + touch.pressure);
                        touchPressure = 0f;
                        countingPressure = false;
                        fireRay = true;
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                if (!countingPressure)
                {
                    touchPressure = 0f;
                    countingPressure = true;
                }
            }
            else
            {
                if (countingPressure)
                {
                    Debug.Log("Pressure: " + touchPressure);
                    touchPressure = 0f;
                    countingPressure = false;
                    position = Input.mousePosition;
                    fireRay = true;
                }
            }
        }

        if (!fireRay) return;

        switch (raycaster)
        {
            case Raycaster.Graphic:
            {
                Debug.Log("Debug");
                //Code to be place in a MonoBehaviour with a GraphicRaycaster component
                GraphicRaycaster gr = this.GetComponentInParent<GraphicRaycaster>();
                //Create the PointerEventData with null for the EventSystem
                PointerEventData ped = new PointerEventData(null);
                //Set required parameters, in this case, mouse position
                ped.position = position;
                //Create list to receive all results
                List<RaycastResult> results = new List<RaycastResult>();
                //Raycast it
                gr.Raycast(ped, results);

                for (int i = 0; i < results.Count; i++)
                {
                    if(results[i].gameObject.CompareTag("Tap"))
                    {
                        SceneController.Instance.bird.FlapBird();
                        break;
                    }
                }

                fireRay = false;
                
            } break;

            case Raycaster.Physics:
            {
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(position), out hit, Mathf.Infinity))
                {
                    if (hit.collider.gameObject.CompareTag("Tap"))
                    {
                        Debug.Log("Tap");
                        fireRay = false;
                    }
                }
            } break;
        }
    }
}
