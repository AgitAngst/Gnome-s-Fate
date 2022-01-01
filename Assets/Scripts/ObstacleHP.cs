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
    public float distanceToMove = 10f;
    private float distance;
    private AudioManager audioManager;
    private static readonly int Attack = Animator.StringToHash("Attack");
    public States.ObstacleType obstacleType;
    [FormerlySerializedAs("spawnedLine")] public EnemyLineEnum enemyLine;
    private int playerLine;
    private int nextLineToJump;
    private static readonly int IsJumping = Animator.StringToHash("isJumping");

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
                break;
            case States.ObstacleType.Obstacle:
                break;
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
        character.characterEvents.CashUpdate(Mathf.RoundToInt(score));
        Destroy(gameObject);
        audioManager.PlaySound(6);
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
                if (GetDistanceToPlayer() <= distanceToMove && canJump)
                {
                    JumpToAnotherLine();
                }

                break;
            case EnemyLineEnum.Left:
                break;
            case EnemyLineEnum.Right:
                break;
        }
    }

    private void JumpToAnotherLine()
    {
        CheckEnemyLineToJump();
        animator.SetBool(IsJumping, true);
        var position = transform.position;
        transform.DOMove(new Vector3(nextLineToJump, position.y, position.z), .5f).SetEase(Ease.InOutExpo).OnComplete(
            () =>
            {
                animator.SetBool(IsJumping, false);
                canJump = false;
                CheckEnemyLine();
            });
    }

    private int CheckEnemyLineToJump()
    {
        isStayingInPlace = Random.value <= .5f;
        switch (enemyLine)
        {
            case EnemyLineEnum.Center:
                if (!isStayingInPlace)
                {
                    if (Random.value <= 0.5f)
                    {
                        nextLineToJump = -3;
                    }
                    else nextLineToJump = 3;
                }
                break;
            case EnemyLineEnum.Right:
                if (!isStayingInPlace)
                {
                    if (Random.value <= 0.5f)
                    {
                        nextLineToJump = -3;
                    }
                    else nextLineToJump = 0;
                }
                break;
            case EnemyLineEnum.Left:
            {
                if (Random.value <= 0.5f)
                {
                    nextLineToJump = 0;
                }
                else nextLineToJump = 3;
            }
                break;
        }

        return nextLineToJump;
    }

    int CheckPlayerLine()
    {
        switch (character.characterLine)
        {
            case States.CharacterLine.Left:
                playerLine = -1;
                break;
            case States.CharacterLine.Right:
                playerLine = 1;
                break;
            case States.CharacterLine.Center:
                playerLine = 0;
                break;
        }

        return playerLine;
    }
}