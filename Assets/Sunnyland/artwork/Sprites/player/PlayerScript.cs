using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {
    Animator playerAnim;
    [SerializeField] bool FacingRight = true;
    [SerializeField] float moveSpeed = 1;
    [SerializeField] bool grounded;
    [SerializeField] Transform groundcheck;
    bool playerJumped, playerJumping,enemyGrounded;
    float jumpTimer = 0;
    [SerializeField] float maxJumpTime,initialForce = 1000f;
    [SerializeField] AudioClip jumpSound,death;
    [SerializeField] Slider health;
    int maxhealth = 100,currentHealth;

	private void Awake()
	{
        playerAnim = GetComponentInChildren<Animator>();

        health.maxValue = maxhealth;

	}

	void Start () {
        currentHealth = 100;
	}
	private void Update()
	{
        UpdateHealth();
        JumpLogic();
        if(currentHealth <= 0 && gameObject.activeInHierarchy){
            AudioSource.PlayClipAtPoint(death, Camera.main.transform.position,5f);
            this.gameObject.SetActive(false);

        }
        //if player pressed jump key
        if (playerJumped)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, initialForce));
            playerJumped = false;


        }
        if (Groundchecker() == true || EnemyGroundChecker() == true)
        {
            playerAnim.SetBool("Jump", false);
        }
        else
        {
            if (Groundchecker() == false || EnemyGroundChecker() == false)
            {
                playerAnim.SetBool("Jump", true);
            }
        }

	}


	void LateUpdate () {
        float horizMove = Input.GetAxisRaw("Horizontal");
        playerAnim.SetFloat("VelocityX", Mathf.Abs(horizMove));

        if(FacingRight && horizMove > 0){
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
        }
        else if(FacingRight && horizMove < 0){
            FlipSprite();
        }
        if(!FacingRight && horizMove < 0){
            transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
        }
        else if(!FacingRight && horizMove > 0){
            FlipSprite();
        }

	}

    void FlipSprite(){
        FacingRight = !FacingRight;
        Vector3 playerScale = this.transform.localScale;
        playerScale.x = playerScale.x * -1;
        this.transform.localScale = playerScale;
    }
    public bool Groundchecker()
    {
        //Check for ground using Linecast
        grounded = Physics2D.Linecast(transform.position, groundcheck.position, 1 << LayerMask.NameToLayer("Ground"));
        Debug.DrawLine(transform.position, groundcheck.position, Color.red);
        if (grounded)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    void JumpLogic()
    {

        if (Input.GetButtonDown("Jump") && Groundchecker() == true || Input.GetButtonDown("Jump") && EnemyGroundChecker() == true)
        {
            playerJumped = true;//player jumped
            playerJumping = true; //player is Jumping
            jumpTimer = Time.time;//set time at which we jumped
            AudioSource.PlayClipAtPoint(jumpSound, Camera.main.transform.position, .5f);

        }
        if (Input.GetButtonUp("Jump") || Time.time - jumpTimer > maxJumpTime)
        {
            playerJumping = false; //set player jumping to false
        }
       



    }
    public bool EnemyGroundChecker()
    {
        enemyGrounded = Physics2D.Linecast(transform.position, groundcheck.position, 1 << LayerMask.NameToLayer("Enemy"));
        if (enemyGrounded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void UpdateHealth(){
        health.value = currentHealth;
    }
    public void DecreaseHealth(int deduct){
        currentHealth = currentHealth - deduct;

    }
	private void OnCollisionEnter2D(Collision2D collision)
	{
        if(collision.gameObject.tag == "Spikes"){
            DecreaseHealth(100);

        }
        if (collision.gameObject.tag == "Enemy")
        {
            DecreaseHealth(5);
            playerAnim.SetTrigger("Hurt");
        }
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
        if(other.tag == "Spikes"){
            DecreaseHealth(100);

        }
        if(other.tag == "Enemy"){
            DecreaseHealth(5);
            playerAnim.SetTrigger("Hurt");
        }
	}
}
