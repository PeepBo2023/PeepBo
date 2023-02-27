using PeepBo.Managers;

namespace PeepBo.UI.Scene
{
    public class UI_Scene : UI_Base
    {
		public override void Init()
		{
			GameManager.UI.SetCanvas(gameObject, false);
		}
	}
}
