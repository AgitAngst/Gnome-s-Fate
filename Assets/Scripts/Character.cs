using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Character : MonoBehaviour
{
    private float xPosition;
    public float waitBeforeChangeSide = 0.5f;
    public float characterSpeed = 20f;

    [Range(0, 1)] [SerializeField] private float characterSpeedDecreasePercent = .1f;

    private bool keyPressed = false;
    private Animator animator;
    private States state;

    [FormerlySerializedAs("states")] [FormerlySerializedAs("_states")]
    public States.CharacterState characterStates;

    public States.CharacterLine characterLine;
    private Ground ground;
    public GameObject groundObject;
    private float tmpSpeed;
    public float keyPressTimer = 0.5f;
    private ObjectsSpawner objectsSpawner;
    private float positionSum;
    public bool isDead = false;
    [FormerlySerializedAs("Weapon")] public GameObject weapon;
    private Light weaponLight;
    public float weaponLightIntensity = 600f;
    public float weaponLightOnDuration = 2f;
    public float weaponLightOffDuration = 7f;

    [FormerlySerializedAs("weaponEmissiveStrenght")]
    public float weaponEmissiveStrength = 10f;

    [FormerlySerializedAs("weaponGlowintTime")] [SerializeField]
    private float weaponGlowingTimer;

    private float test1;
    private float lerpTimeElapsed;
    public float lerpDuration = 0.5f;
    private static readonly int RunDirection = Animator.StringToHash("RunDirection");
    private static readonly int Die = Animator.StringToHash("Die");
    private static readonly int Attack1 = Animator.StringToHash("Attack");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private TweenerCore<float, float, FloatOptions> tweener;
    private TweenerCore<float, float, FloatOptions> tweenerWeapon;
    private TweenerCore<float, float, FloatOptions> tweenerWeaponEmission;

    public CharacterEvents characterEvents;
    private GameManager gameManager;

    public Transform attackPoint;

    public float attackRange = 0.5f;
    public Transform attackPoint1;
    public Transform attackPoint2;

    public LayerMask layerMask;

    public int attackDamage;
    [FormerlySerializedAs("maxHP")] public int maxHp;
    [FormerlySerializedAs("playerHP")] public TextMeshProUGUI playerHp;
    private int currentHp;
    private float blendDirectionValue;
    private static readonly int Hurt = Animator.StringToHash("Hurt");
    private AudioManager audioManager;
    private RigidbodyController rigidbodyController;
    private static readonly int EmissiveIntensity = Shader.PropertyToID("_EmissiveIntensity");
    public Material playerMaterial;
    private ReactionManager reactionManager;

    void Start()
    {
        characterStates = States.CharacterState.Idle;
        animator = GetComponentInChildren<Animator>();
        tmpSpeed = characterSpeed;
        ground = groundObject.GetComponent<Ground>();
        positionSum = 0f;
        currentHp = maxHp;
        playerHp.text = currentHp.ToString();
        gameManager = GameManager.instance;
        audioManager = FindObjectOfType<AudioManager>();
        rigidbodyController = GetComponent<RigidbodyController>();
        weaponLight = weapon.GetComponentInChildren<Light>();
        playerMaterial.EnableKeyword("_EmissiveIntensity");
        reactionManager = FindObjectOfType<ReactionManager>();
    }

    void Update()
    {
        CharacterControl();
        switch (characterStates)
        {
            case States.CharacterState.Idle:
                Idle();
                ground.degreesPerSecond = 0f;
                break;
            case States.CharacterState.Running:
                Run();
                break;
            case States.CharacterState.Dead:
                isDead = true;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == layerMask)
        {
            Debug.Log("Why this Character trigger is not working?");
        }

        TakeDamage(1);
    }

    private void CharacterControl()
    {
        if (!isDead)
        {
            ChangeMovementDependentOnCamera();

            if (Input.GetKeyDown(gameManager.attackKey))
            {
                animator.SetTrigger(Attack1);
                //attack is an event in attack animation
            }

            if (Input.GetKeyDown(gameManager.startRunKey))
            {
                if (characterStates == States.CharacterState.Idle)
                {
                    characterStates = States.CharacterState.Running;

                    characterSpeed = tmpSpeed += gameManager.scoreMultiplyer;
                }
            }
        }
    }


    private void ChangeMovementDependentOnCamera()
    {
        if (!gameManager.isCameraChanged)
        {
            if (Input.GetKeyDown(gameManager.strafeLeftKey))
            {
                audioManager.PlaySound(0);

                positionSum -= 3f;
                if (positionSum <= -3f)
                {
                    positionSum = -3f;
                    characterLine = States.CharacterLine.Left;
                }
                else characterLine = States.CharacterLine.Center;


                transform.DOMove(new Vector3(positionSum, 0, 0), 1f);
                AnimatorLerp(-1);
            }

            if (Input.GetKeyDown(gameManager.strafeRightKey))
            {
                audioManager.PlaySound(0);

                positionSum += 3f;
                if (positionSum >= 3f)
                {
                    positionSum = 3f;
                    characterLine = States.CharacterLine.Right;
                }
                else characterLine = States.CharacterLine.Center;


                transform.DOMove(new Vector3(positionSum, 0, 0), 1f);
                AnimatorLerp(1);
            }
        }
        else
        {
            if (Input.GetKeyDown(gameManager.strafeRightKey))
            {
                audioManager.PlaySound(0);
                positionSum -= 3f;
                if (positionSum <= -3f)
                {
                    positionSum = -3f;
                    characterLine = States.CharacterLine.Left;
                }
                else characterLine = States.CharacterLine.Center;


                transform.DOMove(new Vector3(positionSum, 0, 0), 1f);
                AnimatorLerp(-1);
            }

            if (Input.GetKeyDown(gameManager.strafeLeftKey))
            {
                audioManager.PlaySound(0);
                positionSum += 3f;
                if (positionSum >= 3f)
                {
                    positionSum = 3f;
                    characterLine = States.CharacterLine.Right;
                }
                else characterLine = States.CharacterLine.Center;


                transform.DOMove(new Vector3(positionSum, 0, 0), 1f);
                AnimatorLerp(1);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Death();
        }
    }

    private void AnimatorLerp(float direction, float duration = 0.5f)
    {
        tweener?.Kill();
        blendDirectionValue = animator.GetFloat(RunDirection);
        tweener = DOTween.To(() => blendDirectionValue, x => blendDirectionValue = x, direction, duration)
            .OnUpdate(() => { animator.SetFloat(RunDirection, blendDirectionValue); })
            .OnComplete(() =>
            {
                if (direction != 0) AnimatorLerp(0);
            });
    }

    public void Attack()
    {
        Collider[] hitenemies =
            Physics.OverlapCapsule(attackPoint1.transform.position, attackPoint2.transform.position, attackRange,layerMask);

        //      Collider[] hitenemies = Physics.OverlapSphere(attackPoint.transform.position, attackRange, layerMask);
        foreach (Collider enemy in hitenemies)
        {
            // Debug.Log("enemy" + enemy.name);
            enemy.GetComponent<ObstacleHp>().Damage(attackDamage);
        }

        audioManager.PlaySound(1);
    }

    public void WeaponLightingIntensity()
    {
        tweenerWeapon?.Kill();
        tweenerWeaponEmission?.Kill();

        tweenerWeapon = weaponLight.DOIntensity(weaponLightIntensity, weaponLightOnDuration)
            .OnComplete(() => { weaponLight.DOIntensity(0f, weaponLightOffDuration); });

        tweenerWeaponEmission = playerMaterial.DOFloat(10f, EmissiveIntensity, 10f)
            .OnComplete(() => { playerMaterial.DOFloat(0f, EmissiveIntensity, 3); });
    }

    public void Idle()
    {
        characterSpeed = 0f;
        ground.SetGroundSpeed(characterSpeed);
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        reactionManager.ShakeCameraLight();
        playerHp.text = currentHp.ToString();
        //    gameManager.ChangeSpeed(-1.5f);
        if (currentHp >= 1)
        {
            animator.SetTrigger(Hurt);
            audioManager.PlaySound(5);
            characterSpeed -= characterSpeed * characterSpeedDecreasePercent;
        }
        else
        {
            Death();
        }
    }

    public void Death()
    {
        reactionManager.ShakeCameraHard();
        characterStates = States.CharacterState.Dead;
        characterSpeed = 0f;
        ground.SetGroundSpeed(characterSpeed);
        isDead = true;
        rigidbodyController.EnableRigibody(true);
        //animator.SetTrigger(Die);
        audioManager.PlaySound(3);
        gameObject.GetComponent<Collider>().isTrigger = true;
        weapon.transform.parent = null;
        weapon.GetComponent<Rigidbody>().isKinematic = false;
        weapon.GetComponent<Collider>().isTrigger = false;
    }

    public void Run()
    {
        animator.SetBool(IsRunning, true);
        //characterSpeed = tmpSpeed;
        ground.SetGroundSpeed(characterSpeed);
    }


    void KeyPressTimer()
    {
        if (keyPressed)
        {
            keyPressTimer -= Time.deltaTime;
            if (keyPressTimer <= 0f)
            {
                keyPressed = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

     //   Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);

    }
}