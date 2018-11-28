using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSFrame
{
    public class SystemManager:BaseBehaviour
    {
        public override void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            this.gameObject.AddComponent<ThreadManager>();
        }
    }
}
