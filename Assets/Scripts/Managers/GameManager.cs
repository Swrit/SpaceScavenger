using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance { get; private set; }

    private ShipControllerPlayer player;
    private Vector3 lastPlayerPosition = new Vector3();


    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null) lastPlayerPosition = player.transform.position;
    }

    public void SetPlayer(ShipControllerPlayer shipControllerPlayer)
    {
        player = shipControllerPlayer;
    }

    public Vector3 GetPlayerPosition()
    {
        return lastPlayerPosition;
    }

}
