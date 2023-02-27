using PeepBo.Managers;
using PeepBo.UI.Scene;
using Naninovel;

namespace PeepBo.Scene
{ 
    public class InGameScene : BaseScene
    {
        private void Start()
                => Init();

        protected override void Init()
        {
            base.Init();

            GameManager.Episode.InitEpisodeManager();
        }
    }
}
