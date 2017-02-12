using UnityEngine;
using System.Collections;

public class ScrollController : MonoBehaviour
{
    public enum ScrollDirection
    {
        Left,
        Right
    }

    [SerializeField] bool overrideValue;
    [SerializeField] ScrollDirection scrollDirection;
    [SerializeField] float scrollSpeed;
    private Rigidbody2D rigidbody2DComponent;

    // Use this for initialization
    void Start ()
    {
        rigidbody2DComponent = GetComponent<Rigidbody2D>();
        rigidbody2DComponent.velocity = scrollDirection == ScrollDirection.Left ? Vector2.left * scrollSpeed : Vector2.right * scrollSpeed;
    }

	// Update is called once per frame
	void Update ()
    {
        if (SceneController.Instance && !SceneController.Instance.bird.IsAlive)
        {
            rigidbody2DComponent.velocity = Vector2.zero;
        }
	}


}
