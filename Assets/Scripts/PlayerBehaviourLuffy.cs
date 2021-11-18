using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerBehaviourLuffy : MonoBehaviour
{

  public CharacterController2D controller;
  public Animator animator;

  public float runSpeed = 40f;

  float horizontalMove = 0f;
  bool jump = false;
  bool crouch = false;

  public Transform attackPoint;
  public float attackRange = 0.5f;
  public LayerMask enemyLayers;
  public int attackDirection;
  public int attackDamage;

  public int firstAtkPower = 10;

  // Update is called once per frame
  void Update()
  {
    AttackDamage();
    if (Input.GetKeyDown(KeyCode.Z))
    {
      Attack();
    }


    //if (!isLocalPlayer) return;


    horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

    animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

    if (Input.GetButtonDown("Jump"))
    {
      jump = true;
      animator.SetBool("IsJumping", true);
    }
    if (Input.GetButtonDown("Crouch"))
    {
      crouch = true;
      animator.SetBool("IsCrouching", true);
    }
    else if (Input.GetButtonUp("Crouch"))
    {
      crouch = false;
      animator.SetBool("IsCrouching", false);
    }
  }

  public void OnLanding()
  {
    animator.SetBool("IsJumping", false);
    jump = false;
  }
  void Attack()
  {
    animator.SetTrigger("Attack");
    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
    foreach (Collider2D enemy in hitEnemies)
    {
      if (enemy.GetComponent<Chopper>() != null)
      {
        enemy.GetComponent<Chopper>().TakeDamage(attackDamage, firstAtkPower);
        break;
      }

    }
  }
  public void AttackDamage()
  {
    if (Input.GetAxisRaw("Horizontal") != 0)
    {
      attackDirection = (int)Input.GetAxisRaw("Horizontal");
    }
    attackDamage = 1 * attackDirection;
  }
  void FixedUpdate()
  {
    //if (!isLocalPlayer) return;
    controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
  }
}