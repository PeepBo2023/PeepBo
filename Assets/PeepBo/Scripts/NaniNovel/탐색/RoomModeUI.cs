using Naninovel;
using Naninovel.Commands;
using Naninovel.UI;
using PeepBo.Managers;
using PeepBo.UI.Popup;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomModeUI : MonoBehaviour
{
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Sprite findSprite;
    [SerializeField] private Sprite exitSprite;
    [SerializeField] private List<GameObject> findObjectList;

    private bool isEnd = false;
    private int findCount = 0;
    private List<string> findList = new List<string>();
    private Dictionary<string, GameObject> findObjectDict = new Dictionary<string, GameObject>();
    private Dictionary<string, bool> alreadyFindDict = new Dictionary<string, bool>();

    bool isShow = false;

    private void Start()
    {
        var customUI = GetComponent<CustomUI>();
        customUI.OnVisibilityChanged += CustomUI_OnVisibilityChanged;
    }

    private void CustomUI_OnVisibilityChanged(bool _isShow)
    {
        if (_isShow)
            OnShow();
        else
            OnHide();
    }

    private void OnShow()
    {
        if (isShow) return;

        isShow = true;

        GameManager.Room.InitRoomModeUI(this);

        TryTutorialSetUp();
        Init();
    }

    private void OnHide()
    {
        isShow = false;

        var scriptPlayer = Engine.GetService<IScriptPlayer>();
        if (scriptPlayer?.PlayedScript?.name == "Script101")
            tutorialButton.gameObject.SetActive(false);
    }

    private void Init()
    {
        exitButton.onClick.AddListener(OnClickExitButton);
        findList = GameManager.Room.GetFindList();
        for (int i = 0; i < findList.Count; i++)
        {
            var obj = findObjectList[i];
            obj.SetActive(true); // 활성화
            obj.transform.name = findList[i];

            findObjectDict.Add(findList[i], obj);
            alreadyFindDict.Add(findList[i], false);
        }
    }

    private void TryTutorialSetUp()
    {
        var scriptPlayer = Engine.GetService<IScriptPlayer>();
        if(scriptPlayer.PlayedScript.name == "Script101")
        {
            tutorialButton.gameObject.SetActive(true);
            GameManager.UI.ShowPopupUI<UI_RoomTutorialPopup>(); // 1-1이면 강제로 튜토리얼 뜨게 함
        }
    }

    public void OnFinishInteraction(string interactionName)
    {
        findObjectDict.TryGetValue(interactionName, out GameObject obj);
        alreadyFindDict.TryGetValue(interactionName, out bool alreadyFind);
        if (alreadyFind) return;

        obj.GetComponentInChildren<Image>().sprite = findSprite;
        obj.GetComponentInChildren<TextMeshProUGUI>().text = obj.name;

        alreadyFindDict[interactionName] = true;
        findCount++;

        if(findCount == findList.Count)
        {
            var image = exitButton.GetComponent<Image>();
            image.sprite = exitSprite;

            var text = exitButton.GetComponentInChildren<TextMeshProUGUI>();
            text.color = Color.white;
            //panelText.text = "이만하면 된 것 같아";
            isEnd = true;
        }
    }

    public void OnClickExitButton()
    {
        if (isEnd)
            GameManager.Room.ExitRoomMode();
        else
            GameManager.Room.NoExitRoomMode();
    }
}
