using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockChest : MonoBehaviour
{
    [SerializeField] GameObject chest;
    [SerializeField] Vector3 rotationEuler;
    [SerializeField] GameObject player;
    bool activateSequence;
    AnimationPlayer animationPlayer;
    private void Start()
    {
        animationPlayer = player.GetComponent<AnimationPlayer>();
    }
    private void Update()
    {
        activateSequence = animationPlayer.isBeingEaten;
        if (activateSequence)
        {
            chest.GetComponent<Animator>().Play("Take 001");
            animationPlayer.isBeingEaten = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        {
            other.transform.eulerAngles = rotationEuler;
            animationPlayer.PlayerTargetAnim("EditedOpen", true);
        }
    }
}
