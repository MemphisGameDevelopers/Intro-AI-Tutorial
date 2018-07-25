using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogFSM : MonoBehaviour {

    enum State { Idle, Move, Jump, RandomWhatev };
    [SerializeField] State currentState;
    [SerializeField] bool FacingRight = true;
    bool moving = false,jump = false;
    float moveSpeed = 5;
    Animator enemyAnim;
    [SerializeField] float minX, maxX;
    [SerializeField] float circRadius;
    bool canSeePlayer;
    float Timer = 0;

	private void Awake()
	{
        enemyAnim = GetComponentInChildren<Animator>();
	}
	void Start () {
        currentState = State.Move;
	}
	
	
	void Update ()
    {
        SwitchStatement();
        Boundaries();

        if(CanSeePlayer() == true){
            currentState = State.Jump;
        }
        if(currentState == State.Jump && jump){
            Timer += Time.deltaTime;
            if(Timer >= .5f){
                enemyAnim.SetTrigger("Jump");
                jump = false;
                currentState = State.Idle;
                Timer = 0;
            }
        }
        if(currentState == State.Idle){
            Timer += Time.deltaTime;
            if(Timer >= 1.0f){
                currentState = State.Move;
                Timer = 0;
            }
        }

    }

    private void Boundaries()
    {
        if (gameObject.transform.position.x < minX)
        {
            FlipSprite();
        }
        else if (transform.position.x > maxX)
        {
            FlipSprite();
        }
    }

	private void LateUpdate()
	{
        if (moving)
        {
            if (FacingRight)
            {
                transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
            }
            else
            {
                transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
            }
        }
	}

	private void SwitchStatement()
    {
        switch (currentState)
        {

            case State.Idle:
                Idle();
                break;

            case State.Move:
                Move();
                break;

            case State.Jump:
                Jump();
                break;

            case State.RandomWhatev:
                RandomWhatev();
                break;

            default:
                Debug.Log("CurrentState is :" + currentState);
                break;
        }
    }

    void Idle(){
        moving = false;
        enemyAnim.SetBool("Moving", false);
        jump = false;
    }
    void  Move(){
        moving = true;
        enemyAnim.SetBool("Moving", true);
        jump = false;

    }
    void Jump(){
        moving = false;
        enemyAnim.SetBool("Moving", false);
        jump = true;

    }
    void RandomWhatev(){
        moving = false; 
        enemyAnim.SetBool("Moving", false);
        jump = false;

    }
    void FlipSprite(){
        FacingRight = !FacingRight;
        Vector3 enemyScale = this.transform.localScale;
        enemyScale.x = enemyScale.x * -1;
        this.transform.localScale = enemyScale;
    }
    bool CanSeePlayer(){
        canSeePlayer = Physics2D.OverlapCircle(this.transform.position, circRadius, 1 << LayerMask.NameToLayer("Player"));

        if(canSeePlayer){
            return true;
        }
        else{
            return false;
        }
    }
}
