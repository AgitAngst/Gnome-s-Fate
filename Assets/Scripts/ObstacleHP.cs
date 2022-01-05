using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using DG.Tweening;

public class ObstacleHp : MonoBehaviour
{
    [FormerlySerializedAs("maxHP")] public int maxHp = 1;
    public bool getDistance = false;
    private bool canJump = true;
    private bool isStayingInPlace = false;
    private int currentHp;
    public float scorePerObject;
    private Character character;
    private GameManager gameManager;
    private Animator animator;
    public float distanceToReact = 3f;

    [FormerlySerializedAs("distanceToMove")]
    public float distanceToMoveMax = 10f;

    public float distanceToMoveMin = 7;
    private float distance;
    private AudioManager audioManager;
    private static readonly int Attack = Animator.StringToHash("Attack");
    public States.ObstacleType obstacleType;
    [FormerlySerializedAs("spawnedLine")] public EnemyLineEnum enemyLine;
    private int playerLineInt;
    private int nextLineToJump;
    private static readonly int IsJumping = Animator.StringToHash("isJumping");
    private int enemyLineInt;
    private bool isRigidbodyEnabled = false;
   // private RigidbodyController rigidbodyController;
    private Vector3 positionToJump;
    private static readonly int IsDancing = Animator.StringToHash("isDancing");
    private static readonly int IsFalling = Animator.StringToHash("isFalling");
    public enum EnemyLineEnum
    {
        Left,
        Center,
        Right
    }

    private void Update()
    {
        switch (obstacleType)
        {
            case States.ObstacleType.Enemy:
                EnemyAttack();
                JumpOverTheLine();
                DanceOnPlayerDeath();
                break;
            case States.ObstacleType.Obstacle:
                break;
        }
        
    }

    private void DanceOnPlayerDeath()
    {
        if (character.characterStates == States.CharacterState.Dead)
        {
            animator.SetTrigger(IsDancing);
        }
    }

    private void Start()
    {
        currentHp = maxHp;
        audioManager = FindObjectOfType<AudioManager>();
        character = FindObjectOfType<Character>();
        gameManager = FindObjectOfType<GameManager>();
        if (gameObject.GetComponent<Animator>())
        {
            animator = gameObject.GetComponent<Animator>();
        }

        //rigidbodyController = gameObject.GetComponent<RigidbodyController>();
        CheckEnemyLine();
    }

    public void Damage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            gameManager.ChangeSpeed(0.12f);
            Die();
        }
    }

    private void Die()
    {
        var score = scorePerObject * gameManager.scoreMultiplyer;
        ScoreManager.instance.SetScore(Mathf.RoundToInt(score));
        if (obstacleType == States.ObstacleType.Enemy)
        {
            var o = gameObject;
            character.WeaponLightingIntensity();
            o.transform.DOJump(DeadJump(),9f,1,2f,false)
                // .OnUpdate(() =>
                // {
                //     o.transform.DOScale(new Vector3(0, 0, 0), 2f);
                // })
                .OnStart(() =>
                {
                    animator.SetBool(IsFalling,true);
                })
                .OnComplete(() => Destroy(o));
            //rigidbodyController.EnableRigibody(true); //TODO Make nice death animation (procedural or rigidbody)
        }
        else
        {
            character.WeaponLightingIntensity();
            Destroy(gameObject);
        }

        audioManager.PlaySound(6);
    }

    Vector3 DeadJump()
    {
        if (Random.value <= 0.5f)
        {
            var position = transform.position;
            positionToJump = new Vector3(-12f, -5f, 7f);
        }
        else
        {
            var position = transform.position;
            positionToJump = new Vector3(12f, -5f, 7f);
        }

        return positionToJump;
    }

    private float GetDistanceToPlayer()
    {
        if (getDistance)
        {
            distance = Vector3.Distance(character.transform.position, gameObject.transform.position);
        }

        return distance;
    }

    private void EnemyAttack()
    {
        if (GetDistanceToPlayer() <= distanceToReact)
        {
  
            animator.SetTrigger(Attack);
        }
    }

    void CheckEnemyLine()
    {
        var lines = transform.position.x;

        if (lines <= -3f)
        {
            enemyLine = EnemyLineEnum.Left;
        }
        else if (lines >= 3f)

        {
            enemyLine = EnemyLineEnum.Right;
        }
        else
        {
            enemyLine = EnemyLineEnum.Center;
        }
    }

    void JumpOverTheLine()
    {
        switch (enemyLine)
        {
            case EnemyLineEnum.Center:
                if (GetDistanceToPlayer() <= Random.Range(distanceToMoveMin, distanceToMoveMax) && canJump)
                {
                    JumpToAnotherLine();
                }

                break;
            case EnemyLineEnum.Left:
                if (GetDistanceToPlayer() <= Random.Range(distanceToMoveMin, distanceToMoveMax) && canJump)
                {
                    JumpToAnotherLine();
                }

                break;
            case EnemyLineEnum.Right:
                if (GetDistanceToPlayer() <= Random.Range(distanceToMoveMin, distanceToMoveMax) && canJump)
                {
                    JumpToAnotherLine();
                }

                break;
        }
    }

    private void JumpToAnotherLine()
    {
        CheckEnemyLineToJump();
        var position = transform.position;
        if (enemyLineInt == playerLineInt)
        {
            canJump = false;
        }
        else
        {
            if (Random.value <= 0.5f)
            {
                animator.SetBool(IsJumping, true);
                transform.DOMoveX(nextLineToJump, .5f).SetEase(Ease.InOutExpo).OnComplete(
                    () =>
                    {
                        animator.SetBool(IsJumping, false);
                        canJump = false;
                        CheckEnemyLine();
                    });
            }
        }
    }

    private int CheckEnemyLineToJump()
    {
        CheckPlayerLine();
        isStayingInPlace = Random.value <= .5f;
        if (!isStayingInPlace)
        {
            switch (enemyLine)
            {
                // case EnemyLineEnum.Center:
                //     if (!isStayingInPlace)
                //     {
                //         if (Random.value <= 0.5f)
                //         {
                //             nextLineToJump = -3;
                //         }
                //         else nextLineToJump = 3;
                //     }
                case EnemyLineEnum.Center:
                    switch (playerLineInt)
                    {
                        case -1:
                            nextLineToJump = -3;
                            break;
                        case 0:
                            break;
                        case 1:
                            nextLineToJump = 3;
                            break;
                    }

                    break;
                case EnemyLineEnum.Right:
                    switch (playerLineInt)
                    {
                        case -1:
                            nextLineToJump = -3;
                            break;
                        case 0:
                            nextLineToJump = 0;
                            break;
                        case 1:
                            //  nextLineToJump = 3;
                            break;
                    }

                    break;
                case EnemyLineEnum.Left:
                {
                    switch (playerLineInt)
                    {
                        case -1:
                            // nextLineToJump = -3;
                            break;
                        case 0:
                            nextLineToJump = 0;
                            break;
                        case 1:
                            nextLineToJump = 3;
                            break;
                    }
                }
                    break;
            }
        }


        return nextLineToJump;
    }

    int CheckPlayerLine()
    {
        switch (character.characterLine)
        {
            case States.CharacterLine.Left:
                playerLineInt = -1;
                break;
            case States.CharacterLine.Right:
                playerLineInt = 1;
                break;
            case States.CharacterLine.Center:
                playerLineInt = 0;
                break;
        }

        return playerLineInt;
    }
}