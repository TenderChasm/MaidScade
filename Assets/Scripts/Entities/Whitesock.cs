using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whitesock : MovingActor
{

    public Spoon Spoon;
    public Spoon RedSpoon;
    public Spoon BlueSpoon;
    public Spoon PurpleSpoon;

    public AudioClip SpoonThrow;

    public int BaseSpoonVelocity;
    public bool isAttacking;

    public List<Marker> BorderStandingPoints;
    public Marker CentralStandingPoint;
    public WaypointWalker Walker { get; set; }

    public WhitesockLeave LeaveCatscene;

    private float wildCoefficient = 1F;
    private bool isCasting = false;
    private bool isPointStanding = false;
    private bool isWalking = false;
    private bool isPointReached = false;

    private int receivedDamageDuringPointStandingPhase;
    private int pointChangesNumber;

    protected override void Start()
    {
        GameObject kek = gameObject;
        base.Start();
        kek = gameObject;
        Walker = GetComponent<WaypointWalker>();
        Audio = GetComponent<AudioSource>();

        if (isAttacking)
            StartAttackLoop();
    }

    private void ShootSpoonsInArc(Vector2 VectorToCentralPoint, float arcLengthDegrees, int spoonNumber,
        Spoon spoonType, float Velocity)
    {
        float angleBetweenSpoons = arcLengthDegrees / Mathf.Max(spoonNumber - 1, 1);
        float halfArc = arcLengthDegrees / 2F;

        Vector2 startArc = Quaternion.AngleAxis(-1 * halfArc, Vector3.forward) * VectorToCentralPoint.normalized;

        Vector2 CurrentSpawnVector = startArc;

        if (arcLengthDegrees % 360 == 0)
            spoonNumber -= (int)arcLengthDegrees / 360;

        for(int i = 0; i < spoonNumber; i++)
        {
            Quaternion spoonDirection = Quaternion.LookRotation(transform.forward, CurrentSpawnVector);
            Spoon spoon = Instantiate(spoonType);
            spoon.transform.localPosition = (Vector2)transform.position + CurrentSpawnVector;

            float angleDifferenceBetweenSpoonAndNeeded = Vector2.SignedAngle(spoon.transform.up, CurrentSpawnVector);
            spoon.RotateWithoutCollider(angleDifferenceBetweenSpoonAndNeeded);

            spoon.Rigidbody.velocity = spoon.transform.up * Velocity;

            CurrentSpawnVector = 
                Quaternion.AngleAxis(-1 * halfArc + (i + 1) * angleBetweenSpoons, Vector3.forward) * VectorToCentralPoint;
        }

        Audio.PlayOneShot(SpoonThrow);

    }

    private void CreateSpoon(Vector2 point, Vector2 direction, float velocity, Spoon spoonType)
    {
        Spoon spoon = Instantiate(spoonType);
        spoon.transform.position = point;
        spoon.setDirectionWithVector(direction);
        spoon.Rigidbody.velocity = spoon.transform.up * velocity;
        Audio.PlayOneShot(SpoonThrow);
    }

    private void ShootSpoonsInLine(float lineLength, int spoonNumber,float velocity, bool isDiscrete = true)
    {
        float distanceBetweenSpoons = lineLength / Mathf.Max(spoonNumber - 1, 1);

        Vector2 shootDirection = (LookingDirection.normalized + (Vector2)PurpleSpoon.AimCorrection).normalized;
        if (isDiscrete)
            shootDirection = new Vector2(Mathf.Round(shootDirection.x), Mathf.Round(shootDirection.y));

        Vector2 leftEndpointOfLine = Vector2.Perpendicular(shootDirection).normalized * lineLength / 2F;
        Vector2 lineVector = -1 * leftEndpointOfLine.normalized;

        for(int i = 0; i < spoonNumber; i++)
        {
            Vector2 spoonPosition = leftEndpointOfLine + lineVector * i * distanceBetweenSpoons;

            Spoon spoon = Instantiate(PurpleSpoon);
            spoon.transform.localPosition = (Vector2)transform.position + spoonPosition;
            spoon.setDirectionWithVector(shootDirection);
            spoon.Rigidbody.velocity = spoon.transform.up * velocity;
        }

        Audio.PlayOneShot(SpoonThrow);

    }

    private IEnumerator ShootSpoonsMachinegun(GameObject target, int spoonNumber, float delayBetweenShots, Spoon spoonType, float velocity)
    {
        for(int i = 0; i < spoonNumber; i++)
        {
            Spoon spoon = Instantiate(spoonType);

            Vector2 aimVector = (target.transform.position + spoon.AimCorrection - transform.position).normalized;

            spoon.transform.localPosition = (Vector2)transform.position + aimVector;
            spoon.setDirectionWithVector(aimVector);
            spoon.Rigidbody.velocity = spoon.transform.up * velocity;

            Audio.PlayOneShot(SpoonThrow);

            yield return new WaitForSeconds(delayBetweenShots);
        }
    }

    IEnumerator ProceedStandingInCenterAttacks()
    {
        isWalking = false;
        WalkingController.DisableAnimation();

        WalkingController.Animator.Play("readyToCast");
        yield return new WaitUntil(() => isCasting);
        WalkingController.Animator.Play("casting");

        int numberOfRepeats = Random.Range(3, 5);

        for (int i = 0; i < numberOfRepeats; i++)
        {
            ShootSpoonsInArc(LookingDirection.normalized, 360, 18, RedSpoon, BaseSpoonVelocity / 1.5F);
            yield return new WaitForSeconds(0.5F);
            ShootSpoonsInArc(LookingDirection.normalized, 90, 10, BlueSpoon, BaseSpoonVelocity * wildCoefficient);
            yield return new WaitForSeconds(0.5F);
            ShootSpoonsInArc(LookingDirection.normalized, 90, 10, BlueSpoon, BaseSpoonVelocity * wildCoefficient);
            yield return new WaitForSeconds(1F);
        }

        numberOfRepeats = Random.Range(8, 13);
        for (int i = 0; i < numberOfRepeats; i++)
        {
            ShootSpoonsInArc(LookingDirection.normalized, 360, 18, RedSpoon, BaseSpoonVelocity);
            yield return new WaitForSeconds(0.4F);
            ShootSpoonsInArc(LookingDirection.normalized, 360, 36, RedSpoon, BaseSpoonVelocity);
            yield return new WaitForSeconds(0.4F);
        }

        numberOfRepeats = Random.Range(100, 150);
        Vector2 initialDirection = LookingDirection.normalized;
        int clockwiseDirection = 1;
        float changeClockwiseDirectionChance = 0.05F;

        for (int i = 0; i < numberOfRepeats; i++)
        {
            if (wildCoefficient >= 1.5)
                if (Random.Range(0, 100) <= 100 * changeClockwiseDirectionChance)
                    clockwiseDirection *= -1;

            initialDirection = Quaternion.AngleAxis(3 * clockwiseDirection, Vector3.forward) * initialDirection;
            ShootSpoonsInArc(initialDirection, 360, 18, RedSpoon, BaseSpoonVelocity);
            yield return new WaitForSeconds(0.2F);
        }

        isCasting = false;
        WalkingController.EnableAnimation();
    }

    private void isCastingCallback()
    {
        isCasting = true;
    }

    private IEnumerator ProceedStandingOnPointAttacks()
    {
        isWalking = false;

        List<int> availableShootingPatterns = new List<int>() { 1, 2, 3 };
        availableShootingPatterns.Shuffle();

        foreach(int attackID in availableShootingPatterns)
        {
            switch(attackID)
            {
                case 1:
                    {
                        for (int i = 0; i < Random.Range(9, 13); i++) 
                        {
                            ShootSpoonsInArc(LookingDirection.normalized, 80, (int)(9 * wildCoefficient),
                            BlueSpoon, BaseSpoonVelocity * 2);
                            yield return new WaitForSeconds(0.8F);
                        }
                        break;
                        
                    }
                case 2:
                    {
                        bool isAimDiscrete = true;
                        if (wildCoefficient >= 1.5)
                            isAimDiscrete = false;

                        for (int i = 0; i < Random.Range(4, 6); i++) 
                        {
                            ShootSpoonsInLine(11, 11, BaseSpoonVelocity * wildCoefficient, isAimDiscrete);
                            yield return new WaitForSeconds(0.75F);
                            ShootSpoonsInLine(10, 10, BaseSpoonVelocity * wildCoefficient, isAimDiscrete);
                            yield return new WaitForSeconds(0.75F);
                        }
                        break;
                    }
                case 3:
                    {
                        yield return StartCoroutine(ShootSpoonsMachinegun(GameManager.Hr.Protagonist.gameObject, 
                            Random.Range(50, 70), 0.2F, Spoon, BaseSpoonVelocity * wildCoefficient * 2));
                        break;
                    }
            }
        }

    }

    private IEnumerator InitiateAttackSequence()
    {
        Walker.ResetWaypoints();
        Walker.AddWaypoint(CentralStandingPoint.transform.position,
                () =>
                {
                    StartCoroutine(ProceedStandingInCenterAttacks());
                    isWalking = true;
                });
        Walker.StartWalking();

        yield return new WaitUntil(() => isCasting);
        yield return new WaitUntil(() => !isCasting);

        int numberOfPointInSequence = 3;
        int currentPointIndex = -1; 
        for(int i = 0; i < numberOfPointInSequence; i++)
        {
            int indexOfPointToGo = UtilStock.RandomExcept(0, BorderStandingPoints.Count, currentPointIndex);

            isPointReached = false;

            Walker.ResetWaypoints();
            Walker.AddWaypoint(BorderStandingPoints[indexOfPointToGo].transform.position,
                    () =>
                    {
                        isPointReached = true;
                    });
            Walker.StartWalking();

            yield return new WaitUntil(() => isPointReached);

            yield return StartCoroutine(ProceedStandingOnPointAttacks());

            currentPointIndex = indexOfPointToGo;
        }

    }

    private IEnumerator AttackLoop()
    {
        while(isAttacking)
        {
            yield return StartCoroutine(InitiateAttackSequence());
        }
        
    }

    public void StartAttackLoop()
    {
        StartCoroutine(AttackLoop());
    }

    protected override void OnDeath()
    {
        isAttacking = false;
        Collider.isTrigger = true;
        StopAllCoroutines();
        WalkingController.EnableAnimation();
        StartCoroutine(LeaveCatscene.PlayScene());
    }

    protected override void Update()
    {
        if (Health.Health / (float)Health.MaxHealth < 0.5)
            wildCoefficient = 1.51F;
        base.Update();
    }
}
