using UnityEngine;
using System.Collections;

public class RepeatingController : MonoBehaviour {

    private BoxCollider2D boxCollider2DComponent;
    private float horizontalLength;

    // Use this for initialization
    void Start ()
    {
        boxCollider2DComponent = GetComponent<BoxCollider2D>();
        horizontalLength = boxCollider2DComponent.size.x;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (transform.position.x < -horizontalLength)
        {
            RepositionScroller();
        }
	}

    void RepositionScroller()
    {
        Vector2 offset = new Vector2(horizontalLength * 2f, 0);

        transform.position = (Vector2)transform.position + offset;
    }
}
