using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerMovement : MonoBehaviour
{

    //can walk jump 
    //stop move when rang attack
    //melee attack
    //fire attack

    public bool isDead = false;
    public bool canMove = true;


    [SerializeField]
    private float moveSpeed = 10f;
    [SerializeField]
    private float maxJumpHight = 20;
    [SerializeField]
    private float jumpFoce = 50;
    private float y = 0;
    [SerializeField]
    private float meleeAttackRadius = 3;
    [SerializeField]
    private float meleeAttackDamage = 20;
    [SerializeField]
    private PlayerStatus status;
    [SerializeField]
    private Vector3 CameraPosition;
    [SerializeField]
    private float rotetionSpeed;
    [SerializeField]
    private CharacterController controller;
    [SerializeField]
    private GameObject avata;
    [SerializeField]
    private Animator avataAnimator;
    
    
    [SerializeField]
    private GameObject sampleBulet;
    [SerializeField]
    private Transform shootPosition;
    [SerializeField]
    private VisualEffect slashEffec;
    [SerializeField]
    private CampMonster inCampMonster;

    //sound
    [SerializeField]
    private AudioSource shootSound;
    [SerializeField]
    private AudioSource jumpSound;
    [SerializeField]
    private AudioSource slashSound;


    private bool isShoot = false;
    private bool shootReady = true;
    private bool slashReady = true;
    //private bool isRun;
    private Vector3 rotDirection;
    private Vector3 moveDirection;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isDead)
        {

        }else
        {
            if(status.PlayerCurrenHP() <= 0)
            {
                isDead = true;
            }
            if(canMove)
            {
                //gravity direction
                float velosity = 5 * -9.8f * Time.deltaTime;
                y += velosity;
                controller.Move(new Vector3(0, y * Time.deltaTime, 0));

                //melee attack
                if(Input.GetMouseButton(1))
                {
                    if(slashReady)
                    {
                        StartCoroutine("SlashAttack");
                    }
                    
                }


                if (isShoot)
                {
                    if(shootReady)
                    {
                        StartCoroutine("SampleShoot");

                        //rotate avata to shoot target
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit target;
                        if (Physics.Raycast(ray, out target))
                        {
                            Vector3 targetRoteation = target.point - avata.transform.position;
                            targetRoteation.y = 0;
                            avata.transform.rotation = Quaternion.LookRotation(targetRoteation);
                        }
                    }
                }else
                {
                    moveDirection = MoveDirection();
                    controller.Move(moveDirection);
                
                   //rotate avata to movedirection
                    if(rotDirection != Vector3.zero)
                    {
                        avata.transform.rotation = Quaternion.LookRotation(rotDirection * rotetionSpeed * Time.deltaTime,Vector3.up);

                    }
                    
                }

                //ranged attack
                //left click
                if (Input.GetMouseButton(0))
                {
                    isShoot = true;
                }else
                {
                    isShoot = false;
                }

                //camera fallow player
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, transform.position + CameraPosition, 10 * Time.deltaTime);
                Camera.main.transform.LookAt(transform.position);


            }
        }

        SetAvataAnimetor();
    }

    Vector3 MoveDirection()
    {
        //walk direction
        float x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        rotDirection = new Vector3 (x, 0, z);
        



        
        //jump
        
            if (controller.isGrounded)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    y = -2;
                    y += jumpFoce;
                    jumpSound.Play();
                }
            }
       
        y = Mathf.Clamp(y, -10, maxJumpHight);


        return new Vector3(x,0,z);
    }

    IEnumerator SampleShoot()
    {
        shootReady = false;
        yield return new WaitForSeconds(0.15f);
        //do somtiog
        Instantiate(sampleBulet,shootPosition.position,shootPosition.rotation);
        shootSound.Play();

        
        shootReady = true;
    }

    IEnumerator SlashAttack()
    {
        slashReady = false;
        slashEffec.Play();

        //target take damage
        if(inCampMonster != null)
        {
            if(inCampMonster.monster.Count > 0)
            {
                for (int i = 0; i < inCampMonster.monster.Count; i++)
                {
                    //distance between monster to player
                    float dis = Vector3.Distance(inCampMonster.monster[i].transform.position,transform.position);
                    if (dis <= meleeAttackRadius)
                    {
                        inCampMonster.monster[i].GetComponent<DragonBehaviour>().TakeDamage(meleeAttackDamage);
                        slashSound.Play();                        
                        if (inCampMonster.monster[i].GetComponent<DragonBehaviour>().isDead)
                        {
                            inCampMonster.monster.Remove(inCampMonster.monster[i]);
                        }
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.7f);
        slashReady = true;
    }

    void SetAvataAnimetor()
    {
        //run
        if(moveDirection.magnitude > 0.01f)
        {
            avataAnimator.SetBool("isRun",true);
        }else if (isShoot)
        {
            avataAnimator.SetBool("isRun", false);
        }else
        {
            avataAnimator.SetBool("isRun", false);
        }

        //shoot animation
        avataAnimator.SetBool("isShoot", isShoot);

        avataAnimator.SetBool("isDead", isDead);

    }

    public void SetCampMonster(CampMonster camp)
    {
        inCampMonster = camp;
    }

}


    