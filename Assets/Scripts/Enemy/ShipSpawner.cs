using Meta.XR.MRUtilityKit;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spaceShip;
    [SerializeField] private float normalOffset;


    void Start()
    {
        //MRUK.Instance.SceneLoadedEvent.AddListener(SpawnSpaceShip);
        SpawnSpaceShip();
    }

    private void SpawnSpaceShip()
    {
        MRUKRoom room = MRUK.Instance.GetCurrentRoom();

        Vector3 spawnPos = room.CeilingAnchor.transform.position * normalOffset;

        Instantiate(spaceShip, spawnPos, Quaternion.identity);
    }
}
