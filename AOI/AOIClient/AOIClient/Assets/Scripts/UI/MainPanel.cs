using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AOICellProtocol;

namespace AOIClient {
    public class MainPanel : MonoBehaviour
    {
        private Button btn_login;

        private void Awake()
        {
            btn_login = transform.Find("btn_login").GetComponent<Button>();

            btn_login.onClick.AddListener(LoginBtnClickHandler);
        }

        private void LoginBtnClickHandler()
        {
            NetManager.Instance.SendMsg(new Pkg_C2SLogin()
            {
                operateCode = OperateCode.Login,
                usernName = "lululu"
            });
        }
    }
}