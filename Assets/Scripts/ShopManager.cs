using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Animator))]
public class ShopManager : MonoBehaviour
{
    [HideInInspector] public static ShopManager instance { private set; get; }

    [HideInInspector] public UnityEvent OnOpenShop;
    [HideInInspector] public UnityEvent OnCloseShop;

    [HideInInspector] public Queue<Wildchar> wildchars;
    [HideInInspector] public Queue<Autocomplete> autocompletes;

    [SerializeField] private KeyCode toggleKey = KeyCode.Tab;

    [Space(5)]

    [Header("Upgrade Settings")]
    [Range(0, 100)]
    [SerializeField] private int startingWildcharPrice = 10;
    [Range(0, 1000)]
    [SerializeField] private int startingAutocompletePrice = 100;

    [Space(10)]

    [Header("References")]
    [SerializeField] private Transform shopPanel;
    [SerializeField] private UpgradeReferences wildcharReferences;
    [SerializeField] private UpgradeReferences autocompleteReferences;

    private int wildcharPrice;
    private int autocompletePrice;

    private Animator toggleAnimator;
    private bool isOpen;
    private bool isAnimating;

    public void PurchaseWildchar()
    {
        if (GameManager.instance.balance >= wildcharPrice)
            GameManager.instance.AddBalance(-wildcharPrice);
        else
            return;

        Wildchar wildchar = Instantiate(wildcharReferences.prefab, wildcharReferences.parent)
            .GetComponent<Wildchar>();
        wildchars.Enqueue(wildchar);

        wildcharPrice *= 2;
    }

    public void PurchaseAutocomplete()
    {
        if (GameManager.instance.balance >= autocompletePrice)
            GameManager.instance.AddBalance(-autocompletePrice);
        else
            return;
        
        Autocomplete autocomplete = Instantiate(autocompleteReferences.prefab, autocompleteReferences.parent)
            .GetComponent<Autocomplete>();
        autocompletes.Enqueue(autocomplete);

        autocompletePrice *= 2;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        toggleAnimator = GetComponent<Animator>();

        // setup events
        OnOpenShop = new UnityEvent();
        OnCloseShop = new UnityEvent();
    }

    private void Start()
    {
        isOpen = false;
        wildcharPrice = startingWildcharPrice;
        autocompletePrice = startingAutocompletePrice;

        // setup listeners
        OnOpenShop.AddListener(Open);
        OnCloseShop.AddListener(Close);
        GameManager.instance.OnWildchar.AddListener(UseWildchar);
        GameManager.instance.OnAutocomplete.AddListener(UseAutocomplete);
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey) && !isAnimating)
        {
            if (isOpen)
                OnCloseShop.Invoke();
            else
                OnOpenShop.Invoke();
        }
    }

    private void Open()
    {
        StartCoroutine(PlayOpen());
    }

    private IEnumerator PlayOpen()
    {
        isAnimating = true;
        toggleAnimator.Play("ShopOpen");
        yield return new WaitForSeconds(toggleAnimator.GetCurrentAnimatorStateInfo(0).length);
        isAnimating = false;
    }

    private void Close()
    {
        StartCoroutine(PlayClose());
    }

    private IEnumerator PlayClose()
    {
        isAnimating = true;
        toggleAnimator.Play("ShopClose");
        yield return new WaitForSeconds(toggleAnimator.GetCurrentAnimatorStateInfo(0).length);
        isAnimating = false;
    }

    private void UseWildchar()
    {
        if (wildchars.Count > 0)
        {
            // remove the wildchar in the front of the queue, activate it, then move it to the back
            Wildchar wildchar = wildchars.Dequeue();
            wildchar.Activate();
            wildchars.Enqueue(wildchar);
        }
        else
            GameManager.instance.OnIncorrectLetter.Invoke();
    }

    private void UseAutocomplete()
    {
        if (autocompletes.Count > 0)
        {
            // remove the autocomplete in the front of the queue, activate it, then move it to the back
            Autocomplete autocomplete = autocompletes.Dequeue();
            autocomplete.Activate();
            autocompletes.Enqueue(autocomplete);
        }
        else
            GameManager.instance.OnIncorrectLetter.Invoke();
    }
}

[System.Serializable]
public struct UpgradeReferences
{
    public Transform parent;
    public GameObject prefab;
}
