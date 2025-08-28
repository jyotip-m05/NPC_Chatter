using System.Collections.Generic;
using Script;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogManager : MonoBehaviour
{
    private static readonly int IsTalking = Animator.StringToHash("isTalking");
    
    [SerializeField] private List<TextAlignmentOptions> assets;
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI npcName;

    private List<Talk> talks;
    private InputActionMap playerActionMap;
    private InputActionMap UIActionMap;
    private GameObject player;
    private Animator animator;
    private static DialogManager instance;

    public List<Talk> Talks
    {
        get => talks;
        set => talks = value;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerActionMap = actionAsset.FindActionMap("Player");
        UIActionMap = actionAsset.FindActionMap("UI");
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public static DialogManager GetInstance()
    {
        return instance;
    }

    public void OpenWindow(GameObject NPC, Animator animator)
    {
        npcName.text = NPC.name;
        this.animator = animator;
        canvas.gameObject.SetActive(true);
    }

    public void CloseWindow()
    {
        playerActionMap.Enable();
        UIActionMap.Disable();
        canvas.gameObject.SetActive(false);
        animator.SetBool(IsTalking, false);
    }
}