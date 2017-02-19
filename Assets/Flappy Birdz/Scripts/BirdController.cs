using UnityEngine;

public class BirdController : MonoBehaviour
{
    // Flap force
    [SerializeField] float flapForce = 200;

    [Header("Sound References")]
    [SerializeField] AudioClip deathClip;
    [SerializeField] AudioClip flapClip;
    [SerializeField] AudioClip pointClip;

    
    private PaletteSwapper paletteSwapperComponent = null;
    private Rigidbody2D rigidbody2DComponent = null;
    private Animator animatorComponent = null;
    private Vector3 birdPosition = new Vector3(-0.914f, 2.075f, 0);
    private Vector3 birdRotation = Vector3.zero;
    private int birdPalette = 0;
    private float RotateUpSpeed = 1, RotateDownSpeed = 1;
    private bool isAlive = true;

    BirdState birdState;

    enum BirdState
    {
        Rising, Falling
    }

    public bool IsAlive
    {
        get { return isAlive; }
    }


    void Awake()
    {
        rigidbody2DComponent = GetComponent<Rigidbody2D>();
        animatorComponent = GetComponent<Animator>();
        paletteSwapperComponent = GetComponent<PaletteSwapper>();

        transform.position = birdPosition;
        transform.eulerAngles = birdRotation = Vector3.zero;

        animatorComponent.Play("Flying");
    }

    public void Restart()
    {
        Awake();
        Start();
    }

    // Use this for initialization
    void Start()
    {
        isAlive = true;

        rigidbody2DComponent.velocity = Vector2.zero;

        birdPalette = GameData.LoadIntValue("Bird Palette", 0) % GameData.LoadIntValue("Birdz Unlocked");

        if (birdPalette > 0 && birdPalette <= paletteSwapperComponent.colorPalettes.Length)
        {
            paletteSwapperComponent.SwapColors(paletteSwapperComponent.colorPalettes[birdPalette - 1]);
        }
    }

    /// <summary>
    /// When the bird goes up, it'll rotate up to 45 degrees. when it falls, rotation will be -90 degrees min
    /// </summary>
    void FixedUpdate()
    {
        if (SceneController.Instance && SceneController.Instance.IsGameRunning)
        {
            RotateBird();
        }
    }

    public void StartFlapping()
    {
        animatorComponent.SetTrigger("Flap");
    }

    public void FlapBird()
    {
        if (SceneController.Instance.IsGameRunning && isAlive)
        {
            // Flap
            rigidbody2DComponent.velocity = Vector2.zero;
            rigidbody2DComponent.AddForce(Vector2.up * flapForce);

            AudioSource.PlayClipAtPoint(flapClip, Vector3.zero, SceneController.Instance.IsSoundEnabled ? 1 : 0);
        }
    }

    void RotateBird()
    {
        if (GetComponent<Rigidbody2D>().velocity.y > 0) birdState = BirdState.Rising;
        else birdState = BirdState.Falling;

        float degreesToAdd = 0;

        switch (birdState)
        {
            case BirdState.Rising:
                degreesToAdd = 1f * RotateUpSpeed;
                break;
            case BirdState.Falling:
                degreesToAdd = -1f * RotateDownSpeed;
                break;
            default:
                break;
        }

        // clamp the values so that -90 < rotation < 45 *always*
        birdRotation = new Vector3(0, 0, Mathf.Clamp(birdRotation.z + degreesToAdd, -45, 20));
        transform.eulerAngles = birdRotation;
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Ground"))
        {
            if (isAlive)
            {
                KillBird();
            }

            if (SceneController.Instance)
            {
                SceneController.Instance.GameOver();
            }
        }
    }
	
	void OnTriggerEnter2D(Collider2D collider)
    {	
		if(collider.gameObject.CompareTag("Gate"))
		{
            if (SceneController.Instance)
            {
                SceneController.Instance.IncrementScore();

                AudioSource.PlayClipAtPoint(pointClip, Vector3.zero, SceneController.Instance.IsSoundEnabled ? 1 : 0);
            }
        }
        else if(collider.gameObject.CompareTag("Pipe"))
		{
			KillBird();
		}
    }
	
	public void KillBird()
	{
        if (!isAlive) return;

        animatorComponent.SetTrigger("Die");
        isAlive = false;
		rigidbody2DComponent.velocity = Vector2.zero;
        AudioSource.PlayClipAtPoint(deathClip, Vector3.zero, SceneController.Instance.IsSoundEnabled ? 1 : 0);

        int unlocked = GameData.LoadIntValue("Birdz Unlocked");
        birdPalette += 1;
        GameData.SaveIntValue("Bird Palette", birdPalette % unlocked);
    }
}
