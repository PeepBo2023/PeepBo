using BackEnd;
using Naninovel;
using Naninovel.Commands;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;
using System.Linq;

namespace PeepBo.Managers
{
    public class CommandManager
    {
        public StringParameter ScriptName { get; set; } = null;
        public StringParameter ClickerName { get; set; } = null;

        public void SwitchToNovelByRoom(StringParameter scriptName, StringParameter label)
        {
            new SwitchToNovelMode { ScriptName = scriptName, Label = label }.ExecuteAsync().Forget();
        }

        public void SwitchToNovelByClicker()
        {
            new SwitchToNovelMode { ScriptName = ScriptName, Label = ClickerName }.ExecuteAsync().Forget();
        }
    }

    [CommandAlias("choice")]
    public class ChoicePeepbo : AddChoice
    {
        [ParameterAlias("target")]
        public StringParameter Target;

        [ParameterAlias("score")]
        public IntegerParameter Score;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            if (Target.HasValue && Score.HasValue) // 호감도 적용되는 선택지
                GameManager.Choice.CurrentHogamChoice = new HogamChoice(ChoiceSummary, Target, Score);

            return base.ExecuteAsync(asyncToken);
        }
    }

    [CommandAlias("startchoice")]
    public class StartChoice : Command
    {
        [ParameterAlias(NamelessParameterAlias), RequiredParameter]
        public IntegerParameter index;

        public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            GameManager.Choice.StartChoice(index);
        }
    }

    [CommandAlias("endscript")] // 스크립트 종료
	public class AfterEndScript : Command
	{
		public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
		{
            var scriptPlayer = Engine.GetService<IScriptPlayer>();
            scriptPlayer.Stop();

            // 나니 UI 삭제
            //var a = Engine.GetService<IUIManager>();
            //a.DestroyService();

            GameManager.Episode.OnEpisodeEnd();
		}
	}


	[CommandAlias("clicker")]
    public class SwitchToClickerMode : Command
    {
        [ParameterAlias("scriptname")] public StringParameter ScriptName; // 클리커를 실행 할 나니스크립트
        [ParameterAlias("clickerName")] public StringParameter ClickerName; // 실행 시킬 클리커UI 이름

        public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var showUI = new ShowUI { UINames = new List<string> { ClickerName } };
            showUI.ExecuteAsync(asyncToken).Forget();

            var scriptPlayer = Engine.GetService<IScriptPlayer>();
            scriptPlayer.Stop();

            var hidePrinter = new HidePrinter();
            hidePrinter.ExecuteAsync(asyncToken).Forget();

            GameManager.Command.ScriptName = ScriptName;
            GameManager.Command.ClickerName = ClickerName;
        }
    }

/*    [CommandAlias("vibrate")]
    public class Vibrate : Command
    {
        [ParameterAlias("time")] public IntegerParameter Time; // float가 없어서 int로 대체, 0.1초면 1에 해당
        public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            VibrateManager.Vibrate(Time*100);
        }
    }*/

    [CommandAlias("room")]
    public class SwitchToRoomMode : Command
    {
        [ParameterAlias("scriptname")] public StringParameter ScriptName; // 탐색을 완료하고 실행 할 나니스크립트
        [ParameterAlias("label")] public StringParameter Label; // 나니스크립트 내의 라벨

        public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var inputManager = Engine.GetService<IInputManager>();
            inputManager.ProcessInput = false;

            var scriptPlayer = Engine.GetService<IScriptPlayer>();
            scriptPlayer.Stop();

            GameManager.Room.EnterRoomMode(ScriptName, Label, asyncToken);
        }
    }

    [CommandAlias("novel")]
    public class SwitchToNovelMode : Command
    {
        public StringParameter ScriptName;
        public StringParameter Label;

        public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            // 1. Disable character control.
            //var controller = Object.FindObjectOfType<CharacterController3D>();
            //controller.IsInputBlocked = true;

            // 2. Switch cameras.
            //var advCamera = GameObject.Find("AdventureModeCamera").GetComponent<Camera>();
            //advCamera.enabled = false;
            //var naniCamera = Engine.GetService<ICameraManager>().Camera;
            //naniCamera.enabled = true;
            // 3. Load and play specified script (if assigned).
            if (Assigned(ScriptName))
            {
                var scriptPlayer = Engine.GetService<IScriptPlayer>();
                await scriptPlayer.PreloadAndPlayAsync(ScriptName, label: Label);
            }
            // 4. Enable Naninovel input.
            var inputManager = Engine.GetService<IInputManager>();
            inputManager.ProcessInput = true;
        }
    }

    [CommandAlias("stopscript")]
    public class StopScript : Command
    {
        public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var hidePrinter = new HidePrinter();
            //hidePrinter.ExecuteAsync(asyncToken).Forget();
            await hidePrinter.ExecuteAsync(asyncToken);

            // 1. Disable Naninovel input.
            var inputManager = Engine.GetService<IInputManager>();
            inputManager.ProcessInput = false;

            // 2. Stop script player.
            var scriptPlayer = Engine.GetService<IScriptPlayer>();
            scriptPlayer.Stop();
        }
    }

    [CommandAlias("find")]
    public class FindSomething : Command
    {
        [ParameterAlias("name")] public StringParameter Name;
        public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            GameManager.Room.OnFind(Name);
        }
    }

    [CommandAlias("roomcamera")]
    public class ModifyCamera : Command
    {

        public DecimalListParameter Offset;
        public DecimalParameter Roll;
        public DecimalListParameter Rotation;
        public DecimalParameter Zoom;

        [ParameterAlias("backobj")]
        public StringParameter BackObj;

        [ParameterAlias("ortho")]
        public BooleanParameter Orthographic;

        [ParameterAlias("toggle")]
        public StringListParameter ToggleTypeNames;

        [ParameterAlias("set")]
        public NamedBooleanListParameter SetTypeNames;

        [ParameterAlias("easing"), ConstantContext(typeof(EasingType))]
        public StringParameter EasingTypeName;

        [ParameterAlias("time"), ParameterDefaultValue("0.35")]
        public DecimalParameter Duration;

        private const string selectAllSymbol = "*";

        private readonly List<MonoBehaviour> componentsCache = new List<MonoBehaviour>();

        public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var cameraManager = Engine.GetService<ICameraManager>();

            var duration = Assigned(Duration) ? Duration.Value : cameraManager.Configuration.DefaultDuration;
            var easingType = cameraManager.Configuration.DefaultEasing;
            if (Assigned(EasingTypeName) && !Enum.TryParse(EasingTypeName, true, out easingType))
                LogWarningWithPosition($"Failed to parse `{EasingTypeName}` easing.");

            if (Assigned(Orthographic))
                cameraManager.Camera.orthographic = Orthographic;

            if (Assigned(ToggleTypeNames))
                foreach (var name in ToggleTypeNames)
                    DoForComponent(name, c => c.enabled = !c.enabled);

            if (Assigned(SetTypeNames))
                foreach (var kv in SetTypeNames)
                    if (kv.HasValue && !string.IsNullOrWhiteSpace(kv.Name) && kv.NamedValue.HasValue)
                        DoForComponent(kv.Name, c => c.enabled = kv.Value?.Value ?? false);

            var tasks = new List<UniTask>();

            //패닝되는 배경을 대응하기 위해 추가
            if (Assigned(Offset))
            {
                GameObject go = GameObject.Find(BackObj);

                float x = ArrayUtils.ToVector3(Offset, Vector3.zero).x + go.transform.localPosition.x;
                float y = ArrayUtils.ToVector3(Offset, Vector3.zero).y + go.transform.localPosition.y;
                float z = ArrayUtils.ToVector3(Offset, Vector3.zero).z + go.transform.localPosition.z;

                Vector3 pos = new Vector3(x, y, z);

                tasks.Add(cameraManager.ChangeOffsetAsync(pos, duration, easingType, asyncToken));
            }
            if (Assigned(Rotation)) tasks.Add(cameraManager.ChangeRotationAsync(Quaternion.Euler(
                Rotation.ElementAtOrDefault(0) ?? cameraManager.Rotation.eulerAngles.x,
                Rotation.ElementAtOrDefault(1) ?? cameraManager.Rotation.eulerAngles.y,
                Rotation.ElementAtOrDefault(2) ?? cameraManager.Rotation.eulerAngles.z), duration, easingType, asyncToken));
            else if (Assigned(Roll)) tasks.Add(cameraManager.ChangeRotationAsync(Quaternion.Euler(
                cameraManager.Rotation.eulerAngles.x,
                cameraManager.Rotation.eulerAngles.y,
                Roll), duration, easingType, asyncToken));
            if (Assigned(Zoom)) tasks.Add(cameraManager.ChangeZoomAsync(Zoom, duration, easingType, asyncToken));

            await UniTask.WhenAll(tasks);

            void DoForComponent(string componentName, Action<MonoBehaviour> action)
            {
                if (componentName == selectAllSymbol)
                {
                    cameraManager.Camera.gameObject.GetComponents(componentsCache);
                    componentsCache.ForEach(action);
                    return;
                }

                var cmp = cameraManager.Camera.gameObject.GetComponent(componentName) as MonoBehaviour;
                if (!cmp)
                {
                    LogWithPosition($"Failed to toggle `{componentName}` camera component; the component is not found on the camera's game object.");
                    return;
                }
                action.Invoke(cmp);
            }
        }
    }
}