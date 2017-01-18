using UnityEngine;
using System.Collections;

public class ScrollController : MonoBehaviour
{
    private Rigidbody2D rigidbody2DComponent;

    // Use this for initialization
    void Start ()
    {
        rigidbody2DComponent = GetComponent<Rigidbody2D>();
        float scrollSpeed = (SceneController.Instance == null) ? -1.5f : SceneController.Instance.scrollSpeed;
        rigidbody2DComponent.velocity = Vector2.right * scrollSpeed;
    }

	// Update is called once per frame
	void Update ()
    {
        if (SceneController.Instance && SceneController.Instance.IsGameOver)
        {
            rigidbody2DComponent.velocity = Vector2.zero;
        }
	}


}
