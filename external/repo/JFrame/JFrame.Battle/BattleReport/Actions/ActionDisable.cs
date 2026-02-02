using System.Threading;

namespace JFramework
{
    public class ActionDisable : ActionState
    {
        public override string Name => nameof(ActionDisable);

        public override void OnEnter(BaseAction context)
        {
            base.OnEnter(context); //要先调用

        }


        public override void OnExit()
        {
            //Debug.Log("BatlleReady OnExit");
            base.OnExit();
        }
    }
}