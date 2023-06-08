using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingActor : Actor
{
    public Rigidbody2D Rigidbody { get; set; }
    public WalkingAnimationController WalkingController { get; set; }
    public HealthComponent Health { get; set; }
    protected bool GazeFocused = false;

    public float Speed = 1;
    public bool isApplyingMovement = true;
    public bool isImportingMovement = false;
    public Vector2 Movement;
    public Vector2 LookingDirection { get; set; }

    public bool IsGazeLockedOnTarget;
    public GameObject GazeLockTarget;

    protected AudioSource Audio { get; set; }


    protected override void Start()
    {
        base.Start();
        Rigidbody = GetComponent<Rigidbody2D>();
        WalkingController = GetComponent<WalkingAnimationController>();
        Health = GetComponent<HealthComponent>();

        Health.OnHealthEnd += OnDeath;
        Health.OnHealthEnd += Room.IncreaseCurrentRoomDeadEntityCount;

        Health.OnHealthDown += GotDamaged;

        Audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if(IsGazeLockedOnTarget)
        {
            LookingDirection = (GazeLockTarget.transform.position - transform.position);
            GazeFocused = true;
        }

        if(GazeFocused == false)
            updateLookingAccordingToMovement();
        WalkingController.AnimateActor();
        GazeFocused = false;
    }

    private void updateLookingAccordingToMovement()
    {
        if (Movement.x != 0)
        {
            if (Movement.x > 0.5)
                LookingDirection = new Vector2(1, 0);
            if (Movement.x < -0.5)
                LookingDirection = new Vector2(-1, 0);
        }
        if (Movement.y != 0)
        {
            if (Movement.y >= 0.5)
                LookingDirection = new Vector2(0, 1);
            if (Movement.y <= -0.5)
                LookingDirection = new Vector2(0, -1);
        }
    }

    protected void ApplyMovement()
    {
        if (Movement.x != 0 && Movement.y != 0)
            Movement /= Mathf.Sqrt(2);

        Rigidbody.velocity = Movement * Speed;
    }

    private void FixedUpdate()
    {
        if(isApplyingMovement)
            ApplyMovement();
    }

    protected virtual void OnDeath()
    {
        Destroy(gameObject);
    }

    protected virtual void GotDamaged(int health)
    {
        StopCoroutine("FadingTintOnHealthLose");
        StartCoroutine(FadingTintOnHealthLose());
    }

    private IEnumerator FadingTintOnHealthLose()
    {
        Renderer.color = GameManager.Hr.EnemyHealthLoseTint;

        float initialSValue;
        float initialHValue;
        float initialVValue;

        Color.RGBToHSV(Renderer.color, out initialHValue, out initialSValue, out initialVValue);
        for (float sValue = initialSValue; sValue > 0.01; sValue-= 0.005F) 
        {
            Renderer.color = Color.HSVToRGB(initialHValue, sValue, initialVValue);
            yield return new WaitForEndOfFrame();
        }

        Renderer.color = new Color(1, 1, 1, 1);
    }

    protected void RestoreValues()
    {
        Renderer.color = new Color(1, 1, 1, 1);
    }

    protected void SendCallbacks()
    {
        GameManager.Hr.CurrentRoom.EntitiesDestroyed++;
    }
}
