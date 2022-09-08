using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEUtils;

namespace AOIClient
{
    public class GameEntry : MonoBehaviour
    {
        private void Awake()
        {
            //≈‰÷√log
            LogConfig logConfig = new LogConfig()
            {
                saveName = "AOIClientLog.txt",
                loggerEnum = LoggerType.Unity
            };
            PELog.InitSettings(logConfig);

            NetManager.Instance.Init();
            RoleManager.Instance.Init();
            AOICellManager.Instance.Init();
        }
    }
}