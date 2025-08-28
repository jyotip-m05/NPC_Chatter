using System.Collections.Generic;
using Script;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPC : MonoBehaviour
{
    private static readonly int IsTalking = Animator.StringToHash("isTalking");

    [SerializeField] private Animator animator;
    [SerializeField] private List<Talk> talks;
    [SerializeField] private InputActionAsset inputAction;
    [SerializeField] private GameObject player;
    
    private InputActionMap playerActionMap;
    private InputActionMap UIActionMap;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerActionMap = inputAction.FindActionMap("Player");
        UIActionMap = inputAction.FindActionMap("UI");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        Debug.Log("Interact in NPC " + gameObject.name);
        animator.SetBool(IsTalking, true);
        Vector3 dir = player.transform.position - transform.position;
        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir);
        playerActionMap.Disable();
        UIActionMap.Enable();
        var insatnce = DialogManager.GetInstance();
        insatnce.Talks = talks;
        insatnce.OpenWindow(this.gameObject, animator);
    }
}
