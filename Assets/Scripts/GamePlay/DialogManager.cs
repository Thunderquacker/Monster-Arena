using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] int lettersPerSecond = 20;

    public event Action OnShowDialog;
    public event Action OnCloseDialog;

    public static DialogManager Instance { get; private set; }

    Dialog dialog;
    Action onDialogFinished;
    int currentLine = 0;
    bool isTyping;

    public bool IsShowing { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        dialogBox.SetActive(false);
    }

    public IEnumerator ShowDialog(Dialog dialog, Action onFinished = null)
    {
       
        yield return new WaitForEndOfFrame();

        OnShowDialog?.Invoke();

        IsShowing = true;
        this.dialog = dialog;
        onDialogFinished = onFinished;
        currentLine = 0;

        dialogBox.SetActive(true);
        yield return StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
    }

    public void HandleUpdate()
    {
        if (!IsShowing)
            return;

        if (Input.GetKeyDown(KeyCode.Z) && !isTyping)
        {
            currentLine++;

            if (currentLine < dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }
            else
            {
                EndDialog();
            }
        }
    }

    IEnumerator TypeDialog(string line)
    {
        isTyping = true;
        dialogText.text = "";

        foreach (var letter in line)
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        isTyping = false;
    }

    void EndDialog()
    {
        currentLine = 0;
        IsShowing = false;
        dialogBox.SetActive(false);

        onDialogFinished?.Invoke();
        onDialogFinished = null;

        OnCloseDialog?.Invoke();
    }
}
