using System;
using UnityEngine;

using Random = UnityEngine.Random;

public enum TreeType
{
    Easy,
    Normal,
    MeduimUp,
    MediumDown,
    Hard
}


public class TreeController : MonoBehaviour
{
    [Header("Scroll Properties")]
    [SerializeField] ScrollController.ScrollDirection scrollDirection;
    [SerializeField] float scrollSpeed;

    [Header("Tree References")]
    [SerializeField] Rigidbody2D topTree;
    [SerializeField] Rigidbody2D bottomTree;
    [SerializeField] TreeType treeType;

    
    [SerializeField] float minSwitchTime = .8f;
    [SerializeField] float maxSwitchTime = 2.5f;

    private Rigidbody2D rigidbody2DComponent;
    // Movement Speed (0 means don't move)
    private float speed = 1;
    // Switch Movement Direction every x seconds
    private float switchTime;
    private Vector2 topVelocity, bottomVelocity;

    void Awake()
    {
        rigidbody2DComponent = GetComponent<Rigidbody2D>();
    }

    public void Reset()
    {
        CancelInvoke("Switch");

        rigidbody2DComponent.velocity = Vector2.zero;
        topTree.velocity = Vector2.zero;
        bottomTree.velocity = Vector2.zero;

        transform.position = Vector3.zero;

        Vector3 tempPosition = topTree.transform.position;
        topTree.transform.localPosition = new Vector3(tempPosition.x, 10.5f, tempPosition.z);

        tempPosition = bottomTree.transform.position;
        bottomTree.transform.localPosition = new Vector3(tempPosition.x, -6.4f, tempPosition.z);
    }

    public void SetTreeMovement(TreeType type)
    {
        Vector2 scrollVelocity = scrollDirection == ScrollController.ScrollDirection.Left ? Vector2.left * scrollSpeed : Vector2.right * scrollSpeed;

        rigidbody2DComponent.velocity = scrollVelocity;
        topTree.velocity = scrollVelocity;
        bottomTree.velocity = scrollVelocity;

        treeType = type;
        switchTime = Random.Range(minSwitchTime, maxSwitchTime);

        switch (treeType)
        {
            case TreeType.Easy: return;
            case TreeType.Normal:
                {
                    // trim space in between trees
                    Vector3 tempPosition = topTree.transform.position;
                    tempPosition.y -= .25f;
                    topTree.transform.position = tempPosition;

                    tempPosition = bottomTree.transform.position;
                    tempPosition.y += .25f;
                    bottomTree.transform.position = tempPosition;
                }
                break;
            case TreeType.MeduimUp:
                {
                    // leave tree as normal but move upper layer

                    topVelocity = topTree.velocity;
                    topVelocity.y = 1 * speed;
                    topTree.velocity = topVelocity;

                    // Switch every few seconds
                    InvokeRepeating("Switch", 0, switchTime);
                }
                break;
            case TreeType.MediumDown:
                {
                    // leave tree as normal but move lower layer
                    bottomVelocity = bottomTree.velocity;
                    bottomVelocity.y = -1 * speed;
                    bottomTree.velocity = bottomVelocity;
                    // Switch every few seconds
                    InvokeRepeating("Switch", 0, switchTime);
                }
                break;
            case TreeType.Hard:
                {
                    // trim space in between trees and move both upper and lower layers
                    Vector3 tempPosition = topTree.transform.position;
                    tempPosition.y -=  .25f;
                    topTree.transform.position = tempPosition;

                    tempPosition = bottomTree.transform.position;
                    tempPosition.y += .25f;
                    bottomTree.transform.position = tempPosition;

                    topVelocity = topTree.velocity;
                    topVelocity.y = -1 * speed;
                    topTree.velocity = topVelocity;

                    bottomVelocity = bottomTree.velocity;
                    bottomVelocity.y = -1 * speed;
                    bottomTree.velocity = bottomVelocity;

                    // Switch every few seconds
                    InvokeRepeating("Switch", 0, switchTime);
                } break;
        }
    }

    void Update()
    {
        if (SceneController.Instance && !SceneController.Instance.bird.IsAlive)
        {
            rigidbody2DComponent.velocity = topTree.velocity = bottomTree.velocity = Vector2.zero;
        }
    }

    void Switch()
    {
        switch (treeType)
        {
            case TreeType.Easy:
            case TreeType.Normal:
                {
                    topTree.velocity = Vector2.zero;
                    bottomTree.velocity = Vector2.zero;
                }
                break;

            case TreeType.MeduimUp:
                {
                    topVelocity = topTree.velocity;
                    topVelocity.y *= -1;
                    topTree.velocity = topVelocity;
                } break;
            case TreeType.MediumDown:
                {
                    bottomVelocity = bottomTree.velocity;
                    bottomVelocity.y *= -1;
                    bottomTree.velocity = bottomVelocity;
                } break;

            case TreeType.Hard:
                {
                    topVelocity = topTree.velocity;
                    topVelocity.y *= -1;
                    topTree.velocity = topVelocity;

                    bottomVelocity = bottomTree.velocity;
                    bottomVelocity.y *= -1;
                    bottomTree.velocity = bottomVelocity;

                } break;
        }
    }
}
