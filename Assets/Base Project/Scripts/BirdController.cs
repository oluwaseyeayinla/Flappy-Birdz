using UnityEngine;

public class BirdController : MonoBehaviour
{
    // Flap force
    public float flapForce = 200;
    [Header("Sound References")]
    public AudioClip deathClip;
    public AudioClip flapClip;
    public AudioClip pointClip;

    private Rigidbody2D rigidbody2DComponent;
    private Animator animatorComponent;
    private Vector3 birdRotation = Vector3.zero;
    private float RotateUpSpeed = 1, RotateDownSpeed = 1;
    private bool isAlive = true;

    BirdState birdState;

    enum BirdState
    {
        Rising, Falling
    }

    // Use this for initialization
    void Awake()
    {
        isAlive = true;
        rigidbody2DComponent = GetComponent<Rigidbody2D>();
        rigidbody2DComponent.velocity = Vector2.zero;

        animatorComponent = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneController.Instance.IsGameRunning && isAlive)
        { 
            // Flap
            //if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                rigidbody2DComponent.velocity = Vector2.zero;
                rigidbody2DComponent.AddForce(Vector2.up * flapForce);
                //AudioSource.PlayClipAtPoint(flapClip, Vector3.zero);
            }
        }
    }

    /// <summary>
    /// When the bird goes up, it'll rotate up to 45 degrees. when it falls, rotation will be -90 degrees min
    /// </summary>
    //void FixedUpdate()
    //{
    //    if (SceneController.Instance.IsGameRunning || SceneController.Instance.IsGameOver)
    //    {
    //        RotateBird();
    //    }
    //}

    public void StartFlapping()
    {
        animatorComponent.SetTrigger("Flap");
    }

    void RotateBird()
    {
        if (GetComponent<Rigidbody2D>().velocity.y > 0) birdState = BirdState.Rising;
        else birdState = BirdState.Falling;

        float degreesToAdd = 0;

        switch (birdState)
        {
            case BirdState.Rising:
                degreesToAdd = 6 * RotateUpSpeed;
                break;
            case BirdState.Falling:
                degreesToAdd = -3 * RotateDownSpeed;
                break;
            default:
                break;
        }

        // clamp the values so that -90 < rotation < 45 *always*
        birdRotation = new Vector3(0, 0, Mathf.Clamp(birdRotation.z + degreesToAdd, -90, 45));
        transform.eulerAngles = birdRotation;
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (!SceneController.Instance) return;

        if (SceneController.Instance.IsGameRunning && isAlive)
        {
            KillBird();
        }
    }
	
	void OnTriggerEnter2D(Collider2D collider)
    {	
		if(collider.gameObject.tag == "Gate")
		{
            if (!SceneController.Instance) return;

            SceneController.Instance.IncrementScore();

            //AudioSource.PlayClipAtPoint(pointClip, Vector3.zero);
        }
        else if(collider.gameObject.tag == "Pipe")
		{
			KillBird();
		}
    }
	
	public void KillBird()
	{
		if(!isAlive || !SceneController.Instance) return;
		
		isAlive = false;
		rigidbody2DComponent.velocity = Vector2.zero;
		animatorComponent.SetTrigger("Die");
        //AudioSource.PlayClipAtPoint(deathClip, Vector3.zero);
		SceneController.Instance.GameOver();
	}
}
