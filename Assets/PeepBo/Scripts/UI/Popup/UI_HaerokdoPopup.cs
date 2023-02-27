using UnityEngine;
using PeepBo.Utils;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using PeepBo.Managers;
using UnityEngine.SceneManagement;
using Naninovel;
using System.Threading.Tasks;

namespace PeepBo.UI.Popup
{
    public class UI_HaerokdoPopup : UI_Popup
    {
        public string[] explanations = {
            "위치: 대한민국 충남 태안\n면적: 0.9km²\n인구수: 50명\n\n- 행정구역은 충남 태안군 인근 섬\n\n- 해록도로 갈 수 있는 배는 가의도에서 일주일에 한번 오전 7시에 출항하며 약 1시간 정도 소요된다\n\n- 선착장에는 등대가 하나 있으나 많이 노후 되어있고 등대관리원은 주민들이 돌아가며 맡는 편\n\n- 평지가 거의 없고 섬 중앙으로 갈 수록 경사가 급하다.",
            "\n- 동쪽은 절벽이다.\n\n- 상록수림이 우거져 생태보전 특별도서로 지정됐다.\n\n- 방문객이 극히 제한되어있으며, 일반 방문객은 섬의 허가가 있어야만 출입할 수 있다.\n\n- 물자선은 한 달에 한번 둘째 주 화요일에 섬에 방문한다.\n\n- 오랫동안 타지역 어선들의 불법 조업에 시달린 탓에 외지인을 극도로 경계한다.",
            "\n- 전력이 부족해 집마다 밤에도 불을 잘 켜지 않고 가로등도 거의 없는 편이지만 달빛이 밝아 주민들 모두 큰 불편함 없이 살고 있다.\n\n- 종교시설로 개신교인 ‘선락원’이 섬 중앙에 위치해있다\n\n- 선락원에서 운영하는 보육원 덕분에 복지재단에서 해록도에 물질적인 후원을 많이 하고 있다.\n\n- 물자선이 오가지만 그리 풍족하지 않아 생필품을 판매하지 않고 주민들끼리 나눠쓴다.",
            "\n- 기후가 좋아 농업 어업 목축업 모두 무리가 없어 식재료는 대부분 자급자족하고 있다.\n\n- 주민조사를 위해 1년에 한 번 파견오는 공무원만이 유일한 방문객"
        };
        enum Texts
        {
            explanation,
        }
        enum GameObjects
        {
            CloseButton,
            list,
            before,
            after,
        }

        public override void Init()
        {
            base.Init();
            BindObjects();

            TextMeshProUGUI exp = GetTextPro((int)Texts.explanation);
            exp.text = explanations[0];
        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));
            Bind<TextMeshProUGUI>(typeof(Texts));

            GameObject closeButton = GetObject((int)GameObjects.CloseButton);
            AddUIEvent(closeButton, OnClickCloseButton, Define.UIEvent.Click);
            AddButtonAnim(closeButton);

            GameObject tolist = GetObject((int)GameObjects.list);
            AddUIEvent(tolist, OnClickListButton, Define.UIEvent.Click);
            AddButtonAnim(tolist);

            GameObject tobefore = GetObject((int)GameObjects.before);
            AddUIEvent(tobefore, OnClickBeforeButton, Define.UIEvent.Click);
            AddButtonAnim(tobefore);

            GameObject toafter = GetObject((int)GameObjects.after);
            AddUIEvent(toafter, OnClickAfterButton, Define.UIEvent.Click);
            AddButtonAnim(toafter);

        }

        private void OnClickListButton(PointerEventData evt)
        {
            ClosePopupUI();
        }
        private void OnClickCloseButton(PointerEventData evt)
        {
            ClosePopupUI();
        }
        private void OnClickBeforeButton(PointerEventData evt)
        {
            TextMeshProUGUI exp = GetTextPro((int)Texts.explanation);
            int textIdx = explanations.IndexOf(exp.text.ToString());

            if (textIdx <= 0)
            {
                textIdx = explanations.Length;
            }
            textIdx--;
            exp.text = explanations[textIdx];
        }
        private void OnClickAfterButton(PointerEventData evt)
        {
            TextMeshProUGUI exp = GetTextPro((int)Texts.explanation);
            int textIdx = explanations.IndexOf(exp.text.ToString());

            if (textIdx >= explanations.Length - 1)
            {
                textIdx = -1;
            }
            textIdx++;
            exp.text = explanations[textIdx];
        }
    }
}