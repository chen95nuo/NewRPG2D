using Assets.Script.Utility;
using System;
namespace Assets.Script.Battle
{
    public abstract class BaseSystem<T> : TSingleton<T>, IInitialize, IDisposable
    {
        public RoleBase CurrentRole;
        public abstract void Initialize();
        public abstract void ReDispose();

        public virtual void SetCurrentRole(RoleBase mole)
        {
            CurrentRole = mole;
        }

        public override void Init()
        {
            base.Init();
            Initialize();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        public override void Dispose()
        {
            base.Dispose();
            ReDispose();
        }
    }
}
