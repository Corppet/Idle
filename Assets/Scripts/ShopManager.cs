using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Animation))]
public class ShopManager : MonoBehaviour
{
    [HideInInspector] public static ShopManager instance { private set; get; }

    [HideInInspector] public bool isOpen { private set; get; }

    [HideInInspector] public UnityEvent OnOpenShop;
    [HideInInspector] public UnityEvent OnCloseShop;

    [HideInInspector] public List<Wildchar> wildchars;
    [HideInInspector] public List<Autocomplete> autocompletes;
    [HideInInspector] public List<Autofill> autofills;

    [Header("Keybinds")]
    [SerializeField] private KeyCode toggleKey = KeyCode.Tab;
    [SerializeField] private KeyCode purchaseWildcharKey = KeyCode.Alpha1;
    [SerializeField] private KeyCode purchaseAutocompleteKey = KeyCode.Alpha2;
    [SerializeField] private KeyCode purchaseAutofillKey = KeyCode.Alpha3;

    [Space(5)]

    [Header("Upgrade Settings")]
    [Range(0, 100)]
    [SerializeField] private int startingWildcharPrice = 10;
    [Range(0, 1000)]
    [SerializeField] private int startingAutocompletePrice = 100;
    [Range(0, 500)]
    [SerializeField] private int startingAutofillPrice = 50;

    [Space(10)]

    [Header("References")]
    [SerializeField] private UpgradeReferences wildcharReferences;
    [SerializeField] private UpgradeReferences autocompleteReferences;
    [SerializeField] private UpgradeReferences autofillReferences;

    private int wildcharPrice;
    private int autocompletePrice;
    private int autofillPrice;

    private Animation toggleAnimation;

    public void PurchaseWildchar()
    {
        if (GameManager.instance.balance < wildcharPrice)
            return;
            
        GameManager.instance.AddBalance(-wildcharPrice);
        
        Wildchar wildchar = Instantiate(wildcharReferences.prefab, wildcharReferences.parent)
            .GetComponent<Wildchar>();
        wildchar.ID = wildchars.Count;
        wildchars.Add(wildchar);

        wildcharPrice *= 2;
        wildcharReferences.priceText.text = wildcharPrice.ToString();
    }

    public void PurchaseAutocomplete()
    {
        if (GameManager.instance.balance < autocompletePrice)
            return;
            
        GameManager.instance.AddBalance(-autocompletePrice);
        
        Autocomplete autocomplete = Instantiate(autocompleteReferences.prefab, autocompleteReferences.parent)
            .GetComponent<Autocomplete>();
        autocomplete.ID = autocompletes.Count;
        autocompletes.Add(autocomplete);

        autocompletePrice *= 2;
        autocompleteReferences.priceText.text = autocompletePrice.ToString();
    }

    public void PurchaseAutofill()
    {
        if (GameManager.instance.balance < autofillPrice)
            return;
            
        GameManager.instance.AddBalance(-autofillPrice);
        
        Autofill autofill = Instantiate(autofillReferences.prefab, autofillReferences.parent)
            .GetComponent<Autofill>();
        autofill.ID = autofills.Count;
        autofills.Add(autofill);

        autofillPrice *= 2;
        autofillReferences.priceText.text = autofillPrice.ToString();
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        toggleAnimation = GetComponent<Animation>();

        // setup events
        OnOpenShop = new UnityEvent();
        OnCloseShop = new UnityEvent();
    }

    private void Start()
    {
        isOpen = false;
        wildcharPrice = startingWildcharPrice;
        autocompletePrice = startingAutocompletePrice;
        autofillPrice = startingAutofillPrice;

        // setup listeners
        OnOpenShop.AddListener(Open);
        OnCloseShop.AddListener(Close);

        GameManager gm = GameManager.instance;
        gm.OnWildchar.AddListener(UseWildchar);
        gm.OnAutocomplete.AddListener(UseAutocomplete);
        gm.OnAutofill.AddListener(UseAutofill);

        // setup upgrade prices
        wildcharReferences.priceText.text = wildcharPrice.ToString();
        autocompleteReferences.priceText.text = autocompletePrice.ToString();
        autofillReferences.priceText.text = autofillPrice.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey) && !toggleAnimation.isPlaying)
        {
            if (isOpen)
                OnCloseShop.Invoke();
            else
                OnOpenShop.Invoke();
        }

        if (isOpen)
        {
            if (Input.GetKeyDown(purchaseWildcharKey))
                PurchaseWildchar();
            else if (Input.GetKeyDown(purchaseAutocompleteKey))
                PurchaseAutocomplete();
            else if (Input.GetKeyDown(purchaseAutofillKey))
                PurchaseAutofill();
        }

        UseAutofill();
    }

    private void Open()
    {
        toggleAnimation.Play("ShopOpen");
        isOpen = true;
    }

    private void Close()
    {
        toggleAnimation.Play("ShopClose");
        isOpen = false;
    }

    private void UseWildchar()
    {
        if (wildchars.Count > 0 && !wildchars[0].isOnCooldown)
        {
            // remove the wildchar in the front of the queue, activate it, then move it to the back
            Wildchar wildchar = wildchars[0];
            wildchars.RemoveAt(0);
            wildchar.Activate();
            wildchars.Add(wildchar);
        }
        else
        {
            GameManager.instance.OnIncorrectLetter.Invoke();
        }
    }

    private void UseAutocomplete()
    {
        if (autocompletes.Count > 0 && !autocompletes[0].isOnCooldown)
        {
            // remove the autocomplete in the front of the queue, activate it, then move it to the back
            Autocomplete autocomplete = autocompletes[0];
            autocomplete.Activate();
            autocompletes.Add(autocomplete);
        }
        else
        {
            GameManager.instance.OnIncorrectLetter.Invoke();
        }
    }

    private void UseAutofill()
    {
        if (autofills.Count > 0 && !autofills[0].isOnCooldown)
        {
            // remove the autofill in the front of the queue, activate it, then move it to the back
            Autofill autofill = autofills[0];
            autofills.RemoveAt(0);
            autofill.Activate();
            autofills.Add(autofill);
        }
    }
}

[System.Serializable]
public struct UpgradeReferences
{
    public Transform parent;
    public GameObject prefab;
    public TMP_Text priceText;
    public TMP_Text countText;
    public Slider timerSlider;
}
