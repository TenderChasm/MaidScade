using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Flanny : MovingActor
{
    public WaypointWalker Walker;
    public Projectile Orb;
    public Projectile Bouncy;
    public Ray Ray;
    public bool isAttacking;

    public AudioClip OrbThrow;

    public Tsundereballs Tsundereballs;
    public WhirlgigMaid Pockercutie;
    public Fly Fly;

    public Explosion Explosion;
    public AnimatedTile Lava;

    public List<Marker> ThreeLowerPoints;
    public List<Marker> ThreeUpperPoints;

    public float baseOrbVelocity = 4;
    public float SpeedOfFlyingBetweenPoints = 6;
    public float SpeedOfFlying = 4;

    public FlannyLeave LeaveCatscene;

    private List<MovingActor> Spawnables;
    private bool WalkerDone;
    private bool StartedCasting = false;

    private Ray CurrentRay;

    public enum Direction
    {
       Upper = 0,
       Lower = 1
    }


    protected override void Start()
    {
        base.Start();
        Spawnables = new List<MovingActor>() { Tsundereballs, Pockercutie, Fly };
        Walker = GetComponent<WaypointWalker>();
        Audio = GetComponent<AudioSource>();

        if (isAttacking)
            StartAttackLoop();
    }

    private void ShootInLine(float lineLength, int Number, float velocity, Projectile type, Vector2 direction = default(Vector2))
    {
        float distanceBetweenSpoons = lineLength / Mathf.Max(Number - 1, 1);

        Vector2 shootDirection;
        if (direction == default(Vector2))
            shootDirection = (LookingDirection + (Vector2)type.AimCorrection).normalized;
        else
            shootDirection = direction;

        Vector2 leftEndpointOfLine = Vector2.Perpendicular(shootDirection).normalized * lineLength / 2F;
        Vector2 lineVector = -1 * leftEndpointOfLine.normalized;

        for (int i = 0; i < Number; i++)
        {
            Vector2 Position = leftEndpointOfLine + lineVector * i * distanceBetweenSpoons;
            if (Number == 1)
                Position = Vector2.zero;

            Projectile orb = Instantiate(type);
            orb.transform.position = (Vector2)transform.position + Position;
            orb.Rigidbody.velocity = shootDirection.normalized * velocity;
        }

        Audio.PlayOneShot(OrbThrow);
    }

    IEnumerator ProceedRunAndGunAttack()
    {
        int numberOfRepeats = Random.Range(3, 6);
        Projectile orb = Random.Range(0, 1) == 1 ? Bouncy : Orb;

        for (int i = 0; i < numberOfRepeats; i++)
        {
            Walker.ChaseEntity(GameManager.Hr.Protagonist.gameObject, 5);
            yield return new WaitUntil(() => !Walker.isChasing);
            yield return new WaitForSeconds(0.5F);
            ShootInLine(3, Random.Range(5, 7), baseOrbVelocity, orb);
            yield return new WaitForSeconds(2);
        }
    }

    private IEnumerator ProceedSpinningShootingAttack()
    {
        float angleBetweenShots = 30;
        Vector2 shootingDirection = LookingDirection.normalized;

        for (float i = 0; i < 360; i+= angleBetweenShots)
        {
            shootingDirection = Quaternion.AngleAxis(angleBetweenShots, Vector3.forward) * shootingDirection;
            ShootInLine(1, 3, baseOrbVelocity, Bouncy, shootingDirection);
            yield return new WaitForSeconds(0.2F);
        }
    }

    private void SpawnRandomEnemy()
    {
        int spawnableIndex = Random.Range(0, Spawnables.Count);
        MovingActor enemy = Instantiate(Spawnables[spawnableIndex]);
        enemy.transform.position = new Vector2(-36, 30);
    }

    private IEnumerator InitiateAttackSequence()
    {
        yield return StartCoroutine(ProceedRunAndGunAttack());

        //SpawnRandomEnemy();

        yield return StartCoroutine(ProceedSpinningShootingAttack());
        yield return new WaitForSeconds(4);
    }

    private IEnumerator AttackLoop()
    {
        while(isAttacking && Health.Health > Health.MaxHealth * 0.50)
        {
            yield return StartCoroutine(InitiateAttackSequence());
        }

        ((FlannyAnimationController)WalkingController).isFlying = true;
        Speed = SpeedOfFlying;

        int prevAttack = -1;
        while(isAttacking)
        {
            int randomAttack = UtilStock.RandomExcept(0, 3, prevAttack);
            switch(randomAttack)
            {
                case 0:
                    for (int i = 0; i < Random.Range(6, 9); i++)
                        yield return StartCoroutine(ProceedPreciseRayAttack());
                    break;
                case 1:
                    for (int i = 0; i < Random.Range(3, 5); i++)
                        yield return StartCoroutine(ProceedAreaRayAttack());
                    break;
                case 2:
                    for (int i = 0; i < Random.Range(4, 7); i++)
                        yield return StartCoroutine(ProceedRandomTileAttack());
                    break;
            }

            prevAttack = randomAttack;

            SpawnRandomEnemy();

            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator CastRay(Direction upOrDown, Vector2 position = default(Vector2))
    {

        if (upOrDown == Direction.Upper)
            WalkingController.Animator.Play("readyToCast");
        else
            WalkingController.Animator.Play("readyToCastBack");

        StartedCasting = false;

        Ray ray = Instantiate(Ray, transform, false);

        if (position == default(Vector2))
            ray.transform.position = new Vector2(ray.transform.position.x, ray.transform.position.y + 0.5F);
        else
            ray.transform.position = position;

        ray.SetLength(30);
        ray.SetfireOff();
        if (upOrDown == Direction.Lower)
        {
            ray.transform.eulerAngles = new Vector3(0, 0, 180);
            ray.Renderer.sortingLayerName = "Objects";
        }

        yield return new WaitUntil(() => StartedCasting);

        if (upOrDown == Direction.Upper)
            WalkingController.Animator.Play("casting");
        else
            WalkingController.Animator.Play("castingBack");

        ray.SetfireOn();

        CurrentRay = ray;

    }

    IEnumerator ProceedAreaRayAttack()
    {
        Direction upOrDown = (Direction)Random.Range(0, 2);

        int indexOfPointToStartAttack = UtilStock.RandomExcept(0, 3, 1);
        int indexOfPointToEndAttack = indexOfPointToStartAttack == 0 ? 2 : 0;

        Vector3 pointToStartAttack;
        Vector3 pointToEndAttack;

        if (upOrDown == Direction.Upper)
        {
            pointToStartAttack = ThreeUpperPoints[indexOfPointToStartAttack].transform.position;
            pointToEndAttack = ThreeUpperPoints[indexOfPointToEndAttack].transform.position;
        }
        else
        {
            pointToStartAttack = ThreeLowerPoints[indexOfPointToStartAttack].transform.position;
            pointToEndAttack = ThreeLowerPoints[indexOfPointToEndAttack].transform.position;
        }

        Walker.Speed = SpeedOfFlyingBetweenPoints;

        Walker.ResetWaypoints();
        WalkerDone = false;
        Walker.AddWaypoint(pointToStartAttack, () => WalkerDone = true);
        Walker.StartWalking();

        yield return new WaitUntil(() => WalkerDone);

        Walker.Speed = SpeedOfFlying;

        WalkingController.DisableAnimation();

        yield return StartCoroutine(CastRay(upOrDown));

        Walker.ResetWaypoints();
        WalkerDone = false;
        Walker.AddWaypoint(pointToEndAttack, () => WalkerDone = true);
        Walker.StartWalking();

        yield return new WaitUntil(() => WalkerDone);

        Destroy(CurrentRay.gameObject);

        WalkingController.EnableAnimation();
    }

    IEnumerator ProceedPreciseRayAttack()
    {
        Vector3 positionToGo;

        Direction pointLocation = (Direction)Random.Range(0, 2);
        if (pointLocation == Direction.Upper)
            positionToGo = ThreeUpperPoints[1].transform.position;
        else
            positionToGo = ThreeLowerPoints[1].transform.position;

        Walker.Speed = SpeedOfFlyingBetweenPoints;

        Walker.ResetWaypoints();
        WalkerDone = false;
        Walker.AddWaypoint(positionToGo, () => WalkerDone = true);
        Walker.StartWalking();

        yield return new WaitUntil(() => WalkerDone);

        Walker.Speed = SpeedOfFlying;

        Vector2 targetPosition = new Vector2(GameManager.Hr.Protagonist.transform.position.x, transform.position.y);
        Walker.ResetWaypoints();
        WalkerDone = false;
        Walker.AddWaypoint(targetPosition, () => WalkerDone = true);
        Walker.StartWalking();

        yield return new WaitUntil(() => WalkerDone);

        WalkingController.DisableAnimation();

        yield return StartCoroutine(CastRay(pointLocation));

        yield return new WaitForSeconds(1.5F);

        Destroy(CurrentRay.gameObject);

        WalkingController.EnableAnimation();
    }

    IEnumerator ProceedRandomTileAttack()
    {
        WalkingController.DisableAnimation();

        Vector4 roomBoundaries = GameManager.Hr.CurrentRoom.Boundaries;
        roomBoundaries = new Vector4(roomBoundaries.x + 1, roomBoundaries.y - 1, roomBoundaries.z - 1, roomBoundaries.w + 1);

        Vector2 randomTilePos = new Vector2(
                    Random.Range((int)roomBoundaries.x, (int)roomBoundaries.z),
                    Random.Range((int)roomBoundaries.w, (int)roomBoundaries.y)
                    );

        Vector3Int tileToDestroyPosition = GameManager.Hr.Floor.WorldToCell(randomTilePos);
        TileBase tileToDestroy = GameManager.Hr.Floor.GetTile(tileToDestroyPosition);
        if (tileToDestroy != null)
            GameManager.Hr.Floor.SetTile(tileToDestroyPosition, Lava);

        Vector2 rayAndExplosionPos = GameManager.Hr.Floor.GetCellCenterWorld(tileToDestroyPosition);

        Direction randomDirection = (Direction)Random.Range(0, 2);
        yield return StartCoroutine(CastRay(randomDirection, rayAndExplosionPos));

        Explosion exp = Instantiate(Explosion);
        exp.transform.position = rayAndExplosionPos;

        yield return new WaitForSeconds(2F);

        Destroy(CurrentRay.gameObject);

        WalkingController.EnableAnimation();
    }


    private void StartedCastingCallback()
    {
        StartedCasting = true;
    }

    public void StartAttackLoop()
    {
        StartCoroutine(AttackLoop());
    }

        // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override void OnDeath()
    {
        isAttacking = false;
        WalkingController.EnableAnimation();
        ((FlannyAnimationController)WalkingController).isFlying = false;
        Collider.isTrigger = true;
        StopAllCoroutines();
        WalkingController.EnableAnimation();
        StartCoroutine(LeaveCatscene.PlayScene());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HealthComponent health = collision.gameObject.GetComponent<HealthComponent>();
        if (health != null)
            health.Health -= 1;
    }
}
