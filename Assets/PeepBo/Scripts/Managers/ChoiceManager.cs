using BackEnd;
using Naninovel;
using Naninovel.Commands;
using Naninovel.UI;
using PeepBo.Nani;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PeepBo.Managers
{
    public class ChoiceManager
    {
        public HogamChoice CurrentHogamChoice { get; set; }
        public string CurrentChoicesIndex { get; private set; } // ���� ������ ������ �ε���
        public bool IsPlayedChoices => ChoicedText != null; // �̹� ������ �����ߴ� ������ ��������
        public string ChoicedText { get; private set; } // ���� ������ ���� �ε������� �����ߴ� ������
        
        public PeepboChoiceHandlerPanel HandlerPanel { get; set; }

        public void Release()
        {
            CurrentChoicesIndex = default;
        }

        public void StartChoice(int index)
        {
            Debug.Log($"index : {index}");
            CurrentChoicesIndex = index.ToString();

            GameManager.UserDataManager.UserPlayData.playedEpisodeDataDict.TryGetValue(GameManager.DummyEpisode, out var playedEpisodeData);
            playedEpisodeData.choicedDict.TryGetValue(CurrentChoicesIndex, out var choicedText);
            Debug.Log($"already choiced text : {choicedText}");

            ChoicedText = choicedText;
        }

        public bool IsInteractable(string text)
        {
            if(IsPlayedChoices)
            {
                if (text == ChoicedText)
                    return true;
                return false;
            }
            return true;
        }

        public async UniTask SelectChoice(ChoiceState state, ChoiceHandlerButton buttonObject)
        {
            string episodeName = GameManager.DummyEpisode;
            string text = state.Summary;

            var playEpisodeDict = GameManager.UserDataManager.UserPlayData.playedEpisodeDataDict;
            playEpisodeDict.TryGetValue(episodeName, out var playedEpisodeData);
            if(!playedEpisodeData.choicedDict.ContainsKey(CurrentChoicesIndex))
            {
                Debug.Log("First Choiced");
                playedEpisodeData.choicedDict.Add(CurrentChoicesIndex, text);

                Param param = new Param();
                param.Add(GameManager.UserDataManager.UserPlayedEpisodeDataDictName, GameManager.UserDataManager.UserPlayData);

                Backend.GameData.Update(GameManager.UserDataManager.UserPlayedEpisodeDataTableName, new Where(), param);
            }

            HandlerPanel.OnClickChoice();

            //Debug.Log($"current : {CurrenHogamChoice.text}");
            //Debug.Log($"text : {state.Summary}");

            if (CurrentHogamChoice.IsHogamChoice(state.Summary)) // ȣ���� ����Ǵ� �������̸�
            {
                var button = buttonObject.GetComponent<Button>();
                var pressSprite = button.spriteState.pressedSprite;
                button.spriteState = new SpriteState { };

                var image = buttonObject.GetComponent<Image>();
                image.sprite = pressSprite;

                var rectTransform = buttonObject.GetComponent<RectTransform>();
                var cameraManager = Engine.GetService<ICameraManager>();
                Vector2 screenPos = cameraManager.UICamera.WorldToScreenPoint(rectTransform.position);
                //Vector2 screenPos = rectTransform.anchoredPosition;
                //custom.SetVariableValue("choiceButtonInfo", $"{screenPos.x} {screenPos.y} { rectTransform.sizeDelta.x} { rectTransform.sizeDelta.y}");

                var trigger = buttonObject.GetComponent<EventTrigger>(); // ȣ�� �˾�â �ߴµ��� ��ư �������� �̹��� ���ϴ°� ���� ����
                trigger.enabled = false;

                await GameManager.Hogam.ShowHogamUI(CurrentHogamChoice, buttonObject);

                var inputManager = Engine.GetService<IInputManager>();
                inputManager.ProcessInput = true;
            }
        }
    }
}