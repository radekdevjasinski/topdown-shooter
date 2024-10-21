using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    void Update()
    {
        transform.position = Player.Instance.transform.position - new Vector3(0,0,5);
    }
}
