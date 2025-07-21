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

    void Start()
    {
        storyText.text = "";
        choiceText1.text = "";
        choiceText2.text = "";

        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);

        // Hook button clicks to handlers
        choiceButton1.onClick.AddListener(() => MakeChoice(0));
        choiceButton2.onClick.AddListener(() => MakeChoice(1));
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

        if (story.canContinue)
        {
            storyText.text = story.Continue().Trim();
        }

        if (story.currentChoices.Count > 0)
        {
            choiceButton1.gameObject.SetActive(true);
            choiceButton2.gameObject.SetActive(true);

            // Assign the choice texts, or empty if not enough choices
            choiceText1.text = story.currentChoices.Count > 0 ? story.currentChoices[0].text : "";
            choiceText2.text = story.currentChoices.Count > 1 ? story.currentChoices[1].text : "";
        }
        else
        {
            choiceButton1.gameObject.SetActive(false);
            choiceButton2.gameObject.SetActive(false);
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        if (!storyStarted) return;

        if (story.currentChoices.Count > choiceIndex)
        {
            story.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }
        else
        {
            Debug.LogError("Invalid choice index");
        }
    }
}
