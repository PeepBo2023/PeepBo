using Naninovel;
using Naninovel.Commands;
using PeepBo.UI.Popup;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PeepBo.Managers
{
    public class RoomManager
    {
        public Dictionary<string, Dictionary<string, bool>> roomInteractionDict = new Dictionary<string, Dictionary<string, bool>>();
        public StringParameter ScriptName { get; set; } = null;
        public StringParameter RoomBackLabel { get; set; } = null;
        public AsyncToken AsyncToken { get; set; } = default;

        private RoomModeUI roomModeUI;

        public void InitRoomModeUI(RoomModeUI target) => roomModeUI = target;

        public RoomManager()
        {
            // TODO : Serialization, json 구조화
            roomInteractionDict.Add(
                "Script101", new Dictionary<string, bool> 
                { 
                    {"동백꽃", false },
                    {"담벼락", true },
                    {"야생화", false},
                    {"집",false },
                    {"지붕",true }
                }
            );
            roomInteractionDict.Add(
                "Script206", new Dictionary<string, bool>
                {
                    {"문",false },
                    {"잡동사니",false },
                    {"나무판자", false },
                    {"여행가방", false },
                    {"전등",false },
                    {"박스",true },
                    {"가방",true },
                    {"잡동사니2",false },
                    {"선반",true },
                    {"노",false },
                    {"빨",false },
                    {"초",true },
                }
            );
        }


        public void OnFind(string name)
        {
            roomModeUI.OnFinishInteraction(name);
        }

        public List<string> GetFindList()
        {
            List<string> ret = new List<string>();
            roomInteractionDict.TryGetValue(ScriptName, out var list);
            list.ToList().ForEach((data) => { if (data.Value) ret.Add(data.Key); }); // 인터랙션 필수요소 추출
            return ret;
        }

        public void RoomInteractionOccured(string roomName, string interactionName)
        {
            GameManager.Command.SwitchToNovelByRoom(ScriptName, interactionName);
        }

        public void EnterRoomMode(StringParameter scriptName, StringParameter roomBackLabel, AsyncToken asyncToken = default)
        {
            ScriptName = scriptName;
            RoomBackLabel = roomBackLabel;
            AsyncToken = asyncToken;

            HideUI(new List<string> { "RightTopUI" }, asyncToken);
            ShowUI(new List<string> { "RoomModeUI" }, asyncToken);
        }

        public void ExitRoomMode() // 필수 요소를 다 찾은 상태
        {
            ShowUI(new List<string> { "RightTopUI" });
            HideUI(new List<string> { "RoomModeUI" });

            GameManager.Command.SwitchToNovelByRoom(ScriptName, RoomBackLabel);
        }

        public async void NoExitRoomMode() // 필수 요소를 아직 다 찾지 못한 상태
        {
            var name = new List<string> { "NoExitRoomModeUI" };

            var input = Engine.GetService<IInputManager>();
            input.ProcessInput = true;

            var showUI = new ShowUI { UINames = name, Duration = (DecimalParameter)0.5 };
            await showUI.ExecuteAsync();

            var hideUI = new HideUI { UINames = name, Duration = (DecimalParameter)0.5 };
            await hideUI.ExecuteAsync();

            input.ProcessInput = false;
        }

        private void HideUI(List<string> uiList, AsyncToken asyncToken = default)
        {
            if(uiList == null || uiList.Count == 0) return;

            var hideUI = new HideUI { UINames = uiList };
            hideUI.ExecuteAsync(asyncToken).Forget();
        }

        private void ShowUI(List<string> uiList, AsyncToken asyncToken = default)
        {
            if (uiList == null || uiList.Count == 0) return;

            var showUI = new ShowUI { UINames = uiList };
            showUI.ExecuteAsync(asyncToken).Forget();
        }
    }
}
