using System.Collections;
using System.Collections.Generic;
using Script;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    private static readonly int IsTalking = Animator.StringToHash("isTalking");
    private static Chat answer = new Chat("I don't know what to say.", 0);

    [SerializeField] private List<TextAlignmentOptions> assets;
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI npcName;
    [SerializeField] private Transform chatContent;
    [SerializeField] private GameObject chatPrefab;

    private List<Talk> talks;
    private List<Chat> chats;
    private InputActionMap playerActionMap;
    private InputActionMap UIActionMap;
    private InputAction SubmitAction;
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
        SubmitAction = UIActionMap.FindAction("Submit");
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Ask();
    }

    public void Ask()
    {
        if (SubmitAction.triggered)
        {
            StartCoroutine(WaitAndPrint(1.0f));
        }
    }

    IEnumerator WaitAndPrint(float waitTime)
    {
        Chat newQ = new Chat(inputField.text, 1);
        inputField.text = "";
        if (string.IsNullOrEmpty(newQ.massage))
            yield break;
        chats.Add(newQ);
        AddChat(newQ);
        yield return new WaitForSeconds(waitTime);
        int idx = talks.FindIndex(talk => talk.question == newQ.massage);
        newQ = idx == -1 ? answer : new(talks[idx].answer, 0);
        chats.Add(newQ);
        AddChat(newQ);
    }

    private void AddChat(Chat chat)
    {
        var newChat = Instantiate(chatPrefab, chatContent);
        var text = newChat.GetComponent<TMPro.TMP_Text>();
        text.text = chat.massage;
        text.alignment = assets[chat.side];
    }

    private void ClearChat()
    {
        var all = chatContent.GetComponentsInChildren<Transform>();
        // int n = all.Length;
        // for (int i = n - 1; i >= 0; i--)
        // {
        //     Destroy(all[i].gameObject);
        // }
        foreach (Transform child in chatContent)
        {
            Destroy(child.gameObject);
        }
    }

    public static DialogManager GetInstance()
    {
        return instance;
    }

    public void OpenWindow(GameObject NPC, Animator animator, List<Chat> chats)
    {
        npcName.text = NPC.name;
        this.animator = animator;
        this.chats = chats;
        canvas.gameObject.SetActive(true);
        foreach (var chat in chats)
        {
            AddChat(chat);
        }
    }

    public void CloseWindow()
    {
        playerActionMap.Enable();
        UIActionMap.Disable();
        canvas.gameObject.SetActive(false);
        animator.SetBool(IsTalking, false);
        ClearChat();
    }
}