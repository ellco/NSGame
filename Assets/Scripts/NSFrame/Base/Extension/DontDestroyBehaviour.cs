using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NSFrame
{
    /// <summary>
    /// 
    /// </summary>
    public class DontDestroyBehaviour : BaseBehaviour
    {
        public override void Awake()
        {
            GameObject.DontDestroyOnLoad(this);
        }

    }
}
