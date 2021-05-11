using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CharController : MonoBehaviour
{
	[SerializeField] private float m_SprintForce = 600f; // Amount of force added when the player sprints.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement

    private float horizontalMove = 0f;
	private float verticalMove = 0f;
	private float m_SprintDuration = .5f;
	private float m_SprintCooldown = 0f;
	private bool m_Sprint;            // Whether or not the player is sprinting
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	private StaticUI staticUI;
	private WraithController wraithControl;
	private Vector2 movement; 

	public GameObject wraith;
    public float runSpeed = 60f;
	public Animator animator;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		staticUI = GetComponent<StaticUI>();
		wraithControl = wraith.GetComponent<WraithController>();
    }

    void Update()
    {
		//ensure game is active
		if(!GameStats.IsGameActive()){return;}
		//ensure game is not over
		if(GameStats.IsGameOver()){return;}
      
		 //input
    	movement.x = Input.GetAxisRaw("Horizontal");
    	movement.y = Input.GetAxisRaw("Vertical");

		animator.SetFloat("horizontal", movement.x);
    	animator.SetFloat("vertical", movement.y);
	    animator.SetFloat("speed", movement.sqrMagnitude);

		horizontalMove = movement.x * runSpeed;
		verticalMove = movement.y * runSpeed;

		 if (Input.GetKeyDown("space") && m_SprintCooldown <= 0f)
        {
            m_Sprint = true;
			staticUI.ToggleSprintText(false);
			m_SprintCooldown = 3f;
			//play sprint sound
			FindObjectOfType<AudioManager>().Play("Sprint");
        }
    }


    void FixedUpdate()
    {
		//ensure game is active
		if(!GameStats.IsGameActive())
		{
			staticUI.ToggleSprintText(false);
			return;
		}
		//check game over
		if(GameStats.IsGameOver())
		{
			staticUI.ToggleSprintText(false);
			return;
		}
        //move character
        Move(horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime, m_Sprint);

		//begin sprint countdown
		m_SprintDuration -= Time.fixedDeltaTime;
		m_SprintCooldown -= Time.fixedDeltaTime;

		if(m_SprintDuration <= 0f)
		{
			//timer reaches zero, stop sprint
			m_Sprint = false;
		}

		if(m_SprintCooldown <= 0f){
			//show sprint UI text to let user know sprint is available
			staticUI.ToggleSprintText(true);
		}
    }

	public void Move(float moveH, float moveV, bool sprint)
	{
			Vector3 targetVelocity;
			// Move the character by finding the target velocity
			if(sprint){
				targetVelocity = new Vector2(moveH * m_SprintForce, moveV * m_SprintForce);
			} else{
				targetVelocity = new Vector2(moveH * 10f, moveV * 10f);
			}
			
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			
	}

	public void ResetSprintCooldown()
	{
		m_SprintCooldown = 0f;
	}
}
