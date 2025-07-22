using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.UI;

public class npcInk : MonoBehaviour
{
    [SerializeField] private TextAsset inkJSONAsset;
    [SerializeField] private TextMeshProUGUI storyText;

    [SerializeField] private TextMeshProUGUI choiceText1;
    [SerializeField] private TextMeshProUGUI choiceText2;

    [SerializeField] private Button choiceButton1;
    [SerializeField] private Button choiceButton2;

    private Story story;
    private bool storyStarted = false;
    private gunAiming gunAimingRef;

    void Start()
    {
        storyText.text = "";
        choiceText1.text = "";
        choiceText2.text = "";

        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);

        choiceButton1.onClick.AddListener(() => MakeChoice(0));
        choiceButton2.onClick.AddListener(() => MakeChoice(1));

        gunAimingRef = FindObjectOfType<gunAiming>();
    }

    public void StartStory()
    {
        if (inkJSONAsset != null)
        {
            story = new Story(inkJSONAsset.text);
            storyStarted = true;
            ContinueStory();
        }
        else
        {
            Debug.LogError("Ink JSON asset not assigned!");
        }
    }

    public void ContinueStory()
    {
        if (!storyStarted) return;

        // Keep continuing until we find actual content
        while (story.canContinue)
        {
            string text = story.Continue().Trim();

            // Skip empty or choice echo lines
            if (!string.IsNullOrEmpty(text))
            {
                storyText.text = text;
                break;
            }
        }

        if (story.currentChoices.Count > 0)
        {
            choiceButton1.gameObject.SetActive(true);
            choiceButton2.gameObject.SetActive(true);

            choiceText1.text = story.currentChoices.Count > 0 ? story.currentChoices[0].text : "";
            choiceText2.text = story.currentChoices.Count > 1 ? story.currentChoices[1].text : "";


            if (gunAimingRef != null)
            {
                gunAimingRef.LockCursor(false);
            }
        }
        else
        {
            choiceButton1.gameObject.SetActive(false);
            choiceButton2.gameObject.SetActive(false);


           // choiceText1.gameObject.SetActive(false);
           // choiceText2.gameObject.SetActive(false);

        }
    }


    public void MakeChoice(int choiceIndex)
    {
        if (!storyStarted) return;

        if (story.currentChoices.Count > choiceIndex)
        {
            story.ChooseChoiceIndex(choiceIndex);

            // Skip the line that contains the player's choice
            if (story.canContinue)
            {
                story.Continue(); // This consumes the choice text but doesn't show it

            }

            ContinueStory();

            if (gunAimingRef != null)
            {
                gunAimingRef.LockCursor(true);
            }

            choiceText1.text = story.currentChoices.Count > 0 ? story.currentChoices[0].text : "";
            choiceText2.text = story.currentChoices.Count > 1 ? story.currentChoices[1].text : "";



        }
        else
        {
            Debug.LogError("Invalid choice index");
        }
    }

    public bool IsStoryFinished()
    {
        return story != null && !story.canContinue && story.currentChoices.Count == 0;
    }

    public void ResetUI()
    {
        storyText.text = "";
        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);
    }


}
