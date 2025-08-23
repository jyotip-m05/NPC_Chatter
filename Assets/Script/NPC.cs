using UnityEngine;

public class NPC : MonoBehaviour
{
    private static readonly int IsTalking = Animator.StringToHash("isTalking");

    [SerializeField] private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        animator.SetBool(IsTalking, true);
    }
}
