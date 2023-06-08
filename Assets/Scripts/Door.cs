using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Door : Actor, IUsable
{
    public bool IsAccessible { get; set; } = true;

    public enum Orientation
    {
        Horizontal,
        Vertical
    }

    public bool LockedExternal;
    public bool AlwaysAllow;

    public GameObject OpenState;
    public GameObject ClosedState;

    public Orientation EntranceOrientation;
    public bool isOpen;

    public Room LeftOrDownRoom;
    public Room RightOrUpRoom;

    public AudioClip PullHandle;
    public AudioClip OpenSound;
    public AudioClip CloseSound;

    public Action ActionToPlayOnRoomEnter;

    private Tile removedTile;
    private bool walkingCompleted;

    private AudioSource Audio { get; set; }

    protected override void Start()
    {
        Audio = GetComponent<AudioSource>();
    }

    public virtual void Use()
    {
        if (!AlwaysAllow)
        {
            if (GameManager.Hr.CurrentRoom.CanLeave() == false || LockedExternal)
            {
                Audio.PlayOneShot(PullHandle);
                return;
            }
        }

        Vector2 posOfTileNextToPlayer = new Vector2();
        Vector2 posOfTileFarToPlayer = new Vector2();

        if (EntranceOrientation == Orientation.Horizontal)
        {
            posOfTileNextToPlayer =
                new Vector2(transform.position.x - GameManager.Hr.MainGrid.cellSize.x, transform.position.y);
            posOfTileFarToPlayer =
                new Vector2(transform.position.x + GameManager.Hr.MainGrid.cellSize.x, transform.position.y);

            if (GameManager.Hr.Protagonist.transform.position.x > transform.position.x)
                (posOfTileNextToPlayer, posOfTileFarToPlayer) = (posOfTileFarToPlayer, posOfTileNextToPlayer);
        }

        if (EntranceOrientation == Orientation.Vertical)
        {
            posOfTileNextToPlayer =
                new Vector2(transform.position.x, transform.position.y - GameManager.Hr.MainGrid.cellSize.y);
            posOfTileFarToPlayer =
                new Vector2(transform.position.x, transform.position.y + GameManager.Hr.MainGrid.cellSize.y * 3);

            if (GameManager.Hr.Protagonist.transform.position.y > transform.position.y)
                (posOfTileNextToPlayer, posOfTileFarToPlayer) = (posOfTileFarToPlayer, posOfTileNextToPlayer);
        }

        Debug.Log(posOfTileNextToPlayer);

        GameManager.Hr.Protagonist.Movement = Vector2.zero;
        GameManager.Hr.Protagonist.Rigidbody.velocity = Vector2.zero;

        WaypointWalker walker = GameManager.Hr.Protagonist.Walker;
        walker.enabled = true;
        walkingCompleted = false;

        GameManager.Hr.Protagonist.GetComponent<Collider2D>().isTrigger = true;

        walker.ResetWaypoints();
        walker.AddWaypoint(posOfTileNextToPlayer,
            () =>
        {
            Toggle();
        });

        walker.AddWaypoint(transform.position,
            () =>
        {
            GameManager.Hr.CurtainManager.StartHideFromWorldPosition(posOfTileNextToPlayer);
            if (posOfTileNextToPlayer.x > transform.position.x ||
                posOfTileNextToPlayer.y > transform.position.y)
                RightOrUpRoom.OnLeave();
            else
                LeftOrDownRoom.OnLeave();

            GameManager.Hr.CurtainManager.StartRevealFromWorldPosition(posOfTileFarToPlayer);
            if (posOfTileFarToPlayer.x > transform.position.x ||
                posOfTileFarToPlayer.y > transform.position.y)
            {
                RightOrUpRoom.OnEnter();
                GameManager.Hr.CurrentRoom = RightOrUpRoom;
            }
            else
            {
                LeftOrDownRoom.OnEnter();
                GameManager.Hr.CurrentRoom = LeftOrDownRoom;
            }
        });

        walker.AddWaypoint(posOfTileFarToPlayer, 
                () => 
            { 
                Toggle(); 
                walkingCompleted = true;
                ActionToPlayOnRoomEnter?.Invoke();
            });

        walker.StartWalking();
        StartCoroutine(WaitingForWalkingToCompeteToDisableWalker());

    }

    IEnumerator WaitingForWalkingToCompeteToDisableWalker()
    {
        yield return new WaitUntil(() => walkingCompleted);
        GameManager.Hr.Protagonist.Walker.enabled = false;
        GameManager.Hr.Protagonist.GetComponent<Collider2D>().isTrigger = false;
    }

    public void OutlineOn()
    {

        OpenState.GetComponent<SpriteRenderer>().material = GameManager.Hr.Outline;
        ClosedState.GetComponent<SpriteRenderer>().material = GameManager.Hr.Outline;
    }

    public void OutlineOff()
    {

        OpenState.GetComponent<SpriteRenderer>().material = GameManager.Hr.Standart;
        ClosedState.GetComponent<SpriteRenderer>().material = GameManager.Hr.Standart;
    }

    public void Toggle()
    {
        if (isOpen)
            Close();
        else
            Open();
    }

    public void Open()
    {
        Audio.PlayOneShot(OpenSound);
        Collider = OpenState.GetComponent<Collider2D>();
        OpenState.SetActive(true);
        ClosedState.SetActive(false);
        isOpen = true;
    }

    public void Close()
    {
        Audio.PlayOneShot(CloseSound);
        Collider = OpenState.GetComponent<Collider2D>();
        OpenState.SetActive(false);
        ClosedState.SetActive(true);
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
