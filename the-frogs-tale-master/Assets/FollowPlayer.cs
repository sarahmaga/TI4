using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    [SerializeField] Transform player;
    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, 1, -10);
    }
}