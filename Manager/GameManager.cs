using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform respawnPoint;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private float respawnTime;

    private float respawnTimeStart;

    private bool respawn;

    private CinemachineVirtualCamera cvc;

    private void Start()
    {
        cvc = GameObject.Find("player_camera").GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        checkRespawn();
    }

    public void Respawn()
    {
        respawnTimeStart = Time.time;
        respawn = true;
    }

    private void checkRespawn()
    {
        if(Time .time >= respawnTimeStart + respawnTime && respawn)
        {
            var playerTemp = Instantiate(player, respawnPoint);

            cvc.m_Follow = playerTemp.transform;
            respawn = false;
        }
    }
}
