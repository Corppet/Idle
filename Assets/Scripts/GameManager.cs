using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager instance { private set; get; }

    [HideInInspector] public UnityEvent OnWildchar;
    [HideInInspector] public UnityEvent OnAutocomplete;
    [HideInInspector] public UnityEvent OnAutofill;

    [HideInInspector] public UnityEvent OnIncorrectLetter;
    [HideInInspector] public UnityEvent OnCorrectLetter;
    [HideInInspector] public UnityEvent OnCompleteWord;

    [HideInInspector] public int charAmplifier;
    [HideInInspector] public int balance;

    [HideInInspector] public string currentWord;
    [HideInInspector] public string remainingString;
    [HideInInspector] public string completedString;

    [Tooltip("Text file containing all the possible words. " +
        "Each line should be a single unique word.")]
    [SerializeField] private TextAsset wordBank;
    [SerializeField] private Color completedStringColor = Color.yellow;

    [Space(10)]

    [Header("References")]
    [SerializeField] private TMP_Text wordText;
    [SerializeField] private TMP_Text balanceText;

    private List<string> availableWords;
    private List<string> usedWords;

    public void AddBalance(int value)
    {
        balance += value;
        balanceText.text = balance.ToString();
    }

    public void EnterLetter(char letter)
    {
        // Special Case: Wildchar ('.')
        if (letter == '.')
        {
            OnWildchar.Invoke();
            return;
        }
        // Special Case: Autocomplete ('*')
        else if (letter == '*')
        {
            OnAutocomplete.Invoke();
            return;
        }
        // Special Case: Spacebar (' ')
        else if (letter == ' ')
        {
            OnCompleteWord.Invoke();
            return;
        }
        // wrong letter entered
        else if (remainingString.Length == 0 || letter != remainingString[0])
        {
            OnIncorrectLetter.Invoke();
            return;
        }

        // correct letter entered
        AddBalance(charAmplifier);
        completedString += remainingString[0];
        remainingString = remainingString.Substring(1);
        UpdateText();
        OnCorrectLetter.Invoke();
    }

    public void FinishWord()
    {
        if (remainingString == string.Empty || remainingString[0] == '\r')
            AddBalance(currentWord.Length * charAmplifier);

        SetNewWord();
    }

    public void UpdateText()
    {
        wordText.text = "<color=#" + ColorUtility.ToHtmlStringRGB(completedStringColor) + ">" 
            + completedString + "</color>" + remainingString;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // setup UnityEvents
        OnWildchar = new UnityEvent();
        OnAutocomplete = new UnityEvent();
        OnAutofill = new UnityEvent();

        OnIncorrectLetter = new UnityEvent();
        OnCorrectLetter = new UnityEvent();
        OnCompleteWord = new UnityEvent();
    }

    private void Start()
    {
        ProcessWordBank();
        SetNewWord();
        charAmplifier = 1;
        balance = 0;
        balanceText.text = balance.ToString();


        // add listeners
        OnIncorrectLetter.AddListener(FinishWord);
        OnCompleteWord.AddListener(FinishWord);
    }

    private void Update()
    {
        CheckInput();
        
        if (Input.GetKeyDown(KeyCode.Backslash) && Input.GetKey(KeyCode.P))
            AddBalance(1000);
    }
    
    /// <summary>
    /// Parse the word bank and store the words in a list.
    /// </summary>
    private void ProcessWordBank()
    {
        // each line in the wordBank is a new and unique word
        string[] words = wordBank.text.Split('\n');
        availableWords = new List<string>(words);

        usedWords = new List<string>();
    }

    private string GetNewWord()
    {
        // if there are no more words, reset the list
        if (availableWords.Count == 0)
        {
            availableWords = new List<string>(usedWords);
            usedWords.Clear();
        }

        // word from the available list at random
        int index = Random.Range(0, availableWords.Count);
        string word = availableWords[index];
        availableWords.RemoveAt(index);
        usedWords.Add(word);

        return word;
    }

    private void SetNewWord()
    {
        currentWord = GetNewWord();
        remainingString = currentWord;
        completedString = string.Empty;
        wordText.text = currentWord;
    }
    

    private void CheckInput()
    {
        if (Input.anyKeyDown && !ShopManager.instance.isOpen)
        {
            Debug.Log(Input.inputString);
            foreach (char letter in Input.inputString)
                EnterLetter(letter);
        }
    }
}
