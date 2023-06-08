using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Tilemaps;
using static DialogueBox;

public class Player : MovingActor
{

    public float usingRadius;
    public float InvincibilityTimeAfterDamage = 2;
    private IUsable nearestUsableObject;
    public bool isArmed = false;

    public RuntimeAnimatorController ArmedAnimator;

    public AudioClip Ouch;

    public WaypointWalker Walker;

    public Weapon Weapon { get; set; }

    protected override void Start()
    {
        base.Start();
        Walker = GetComponent<WaypointWalker>();
        Walker.enabled = false;

        Weapon = GetComponent<Weapon>();
        Audio = GetComponent<AudioSource>();

        GameManager.Hr.FuelBar.MaxValue = Weapon.MaxAmmoCount;
        GameManager.Hr.FuelBar.CurrentValue = Weapon.MaxAmmoCount;
        GameManager.Hr.FuelBar.ExternalValueChecker += () => GameManager.Hr.FuelBar.CurrentValue = Weapon.currentAmmo;

        Health.OnHealthEnd -= Room.IncreaseCurrentRoomDeadEntityCount;

        Health.OnHealthUp += (int health) => GameManager.Hr.HealthBar.SetHP(health);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Time.frameCount == 1)
        {
            /*Monologue test = new Monologue();
            test.Name = "Rebecca";
            test.Lines = new List<string>();
            test.Lines.Add("pipa vova nu kika orp");
            test.Lines.Add("ribasoma oooosdsdsads");
            test.Lines.Add("YOWANDBD");
            Monologue tes = new Monologue();
            tes.Name = "Sosya";
            tes.Lines = new List<string>();
            tes.Lines.Add("ti karp blin");
            tes.Lines.Add("bla bla car");
            tes.Lines.Add("koshkaget");
            List <Monologue> testlist = new List<Monologue>();
            testlist.Add(test);
            testlist.Add(tes);

            GameManager.Hr.Dialogue.TextToShow = testlist;
            GameManager.Hr.Dialogue.StartShowing();*/



            GetArmed();
            
        }

        if (!GameManager.Hr.IsInputLocked)
        {
            HandleInput();
            UpdateAndHighlightNearestObject();
        }

        base.Update();

    }

    private IUsable FindNearestUsableObject()
    {
        IUsable nearestUsableObject = null;
        float minDistance = 0;

        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, usingRadius))
        {
            IUsable usableObject = collider.GetComponentInParent<Actor>() as IUsable;
            if(usableObject != null && usableObject.IsAccessible)
            {
                if (nearestUsableObject == null)
                {
                    nearestUsableObject = usableObject;
                    minDistance = Vector2.Distance(transform.position, ((Actor)usableObject).transform.position);
                    continue;
                }

                float distance = Vector2.Distance(transform.position, ((Actor)usableObject).transform.position);

                if(distance < minDistance)
                {
                    minDistance = distance;
                    nearestUsableObject = usableObject;
                }
            }
        }

        return nearestUsableObject;
    }

    private void UpdateAndHighlightNearestObject()
    {
        IUsable newNearest = FindNearestUsableObject();
        if (newNearest != nearestUsableObject)
        {
            nearestUsableObject?.OutlineOff();
            newNearest?.OutlineOn();
            nearestUsableObject = newNearest;
        }
    }

    private void HandleInput()
    {
        if (isImportingMovement)
            return;

        Movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKey(KeyCode.UpArrow) ||
           Input.GetKey(KeyCode.DownArrow) ||
           Input.GetKey(KeyCode.RightArrow) ||
           Input.GetKey(KeyCode.LeftArrow))
            GazeFocused = true;

        Vector2 weaponHeight = new Vector2(0, 0.75F);
        Vector2 weaponSpecificOffset = weaponHeight;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            LookingDirection = new Vector2(0, 1);
            weaponSpecificOffset = weaponSpecificOffset + 
                new Vector2(LookingDirection.x + 0.2F, LookingDirection.y * 0.35F);
        }
        else
        if (Input.GetKey(KeyCode.DownArrow))
        {
            LookingDirection = new Vector2(0, -1);
            weaponSpecificOffset = weaponSpecificOffset +
                new Vector2(LookingDirection.x - 0.2F,LookingDirection.y * 0.35F);
        }
        else
        if (Input.GetKey(KeyCode.RightArrow))
        {
            LookingDirection = new Vector2(1, 0);
            weaponSpecificOffset = weaponSpecificOffset +
                new Vector2(LookingDirection.x * 0.7F, LookingDirection.y);
        }
        else
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            LookingDirection = new Vector2(-1, 0);
            weaponSpecificOffset = weaponSpecificOffset + 
                new Vector2(LookingDirection.x * 0.7F, LookingDirection.y);
        }

        if(GazeFocused && isArmed)
        {
            Weapon.Position = weaponSpecificOffset;
            Weapon.Shoot(LookingDirection);
        }

        if (Input.GetKeyDown(KeyCode.E))
            nearestUsableObject?.Use();

        if (Input.GetKeyDown(KeyCode.L))
            Health.Health += 1;
        if (Input.GetKeyDown(KeyCode.K))
            Health.Health -= 1;

    }

    protected override void GotDamaged(int health)
    {
        GameManager.Hr.HealthBar.SetHP(health);
        Audio.PlayOneShot(Ouch);
        StartCoroutine(Health.SetInvisibleForTime(InvincibilityTimeAfterDamage));
        StartCoroutine(SpriteFlickering(InvincibilityTimeAfterDamage));
    }

    private IEnumerator SpriteFlickering(float time)
    {
        for (int i = 0; i < time / 0.1; i++)
        {
            if(i % 2 == 0)
                Renderer.color = new Color(Renderer.color.r, Renderer.color.g, Renderer.color.b, 0.6F);
            else
                Renderer.color = new Color(Renderer.color.r, Renderer.color.g, Renderer.color.b, 0.3F);

            yield return new WaitForSeconds(0.1F);
        }

        Renderer.color = new Color(Renderer.color.r, Renderer.color.g, Renderer.color.b, 1);
    }
  
    public void GetArmed()
    {
        WalkingController.Animator.runtimeAnimatorController = ArmedAnimator; 
        isArmed = true;

    }

    protected override void OnDeath()
    {
        GameManager.Hr.Respawn();
    }
}
