using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Hr { get; set; }
    public Camera MainCamera;
    public SmartCamera Camera;
    public Grid MainGrid;

    public Tilemap Floor;
    public Tilemap Objects;
    public Tilemap Ceiling;
    public Tilemap CurtainsOfUnseen;

    public Player Protagonist;

    public Material Outline;
    public Material Standart;
    public Material Blind;

    public HealthBar HealthBar;
    public ProgressBar FuelBar;
    public DialogueBox Dialogue;

    public CurtainManager CurtainManager { get; set; }
    public MusicManager MusicManager { get; set; }

    public Color EnemyHealthLoseTint;

    public Room CurrentRoom;

    public Vector2 CurrentRespawnPoint = new Vector2();
    public Room CurrentRespawnRoom;

    public bool IsInputLocked = false;

    public Catscene InitialCatscene;

    public ParticleSystem RespawnParticles;

    public bool ChambersOpened { get; set; }


    void Awake()
    {
        if (Hr != null)
            DestroyImmediate(gameObject);
        else
            Hr = this;
    }

    void Start()
    {
        CurtainManager = GetComponent<CurtainManager>();
        MusicManager = GetComponent<MusicManager>();
        WorkaroundFirstDialogueBoxActivationLag();
    }

    public void WorkaroundFirstDialogueBoxActivationLag()
    {
        Dialogue.gameObject.SetActive(true);
        Dialogue.gameObject.SetActive(false);
    }

    public void Respawn()
    {
        if (!ChambersOpened)
            MusicManager.Play(MusicManager.MainroomsAfterGun);
        else if (Protagonist.isArmed)
            MusicManager.Play(MusicManager.MainroomsChambers);
        else
            MusicManager.Play(MusicManager.MainroomsBeforeGun);

        CurtainManager.StartHideFromWorldPosition(CurrentRoom.transform.position);

        Protagonist.transform.position = CurrentRespawnPoint;
        Protagonist.Health.Health = Protagonist.Health.MaxHealth;
        Protagonist.Weapon.currentAmmo = Protagonist.Weapon.MaxAmmoCount;

        CurrentRoom.OnLeave();
        CurrentRoom = CurrentRespawnRoom;

        CurtainManager.StartRevealFromWorldPosition(CurrentRoom.transform.position);

        Camera.hardFollow = true;

        Instantiate(RespawnParticles).transform.position = Protagonist.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount == 1)
        {
            CurtainManager.StartRevealFromWorldPosition(Vector2.zero);
            /*BlindURPEffect.IsOn = false;
            Camera.Target = Protagonist.transform;
            Protagonist.GetArmed();*/
            StartCoroutine(InitialCatscene.PlayScene());
        }
    }
}
