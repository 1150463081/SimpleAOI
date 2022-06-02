using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AOICellProtocol;

namespace AOIClient {
    public class MainPanel : MonoBehaviour
    {
        private Button btn_login;
        private Text txt_id;

        private void Awake()
        {
            btn_login = transform.Find("btn_login").GetComponent<Button>();
            txt_id= transform.Find("txt_id").GetComponent<Text>();

            btn_login.onClick.AddListener(LoginBtnClickHandler);

            NetManager.Instance.AddNetMsgLisener(OperateCode.S2CLogin, LoginSuccessHandler);
        }

        private void LoginBtnClickHandler()
        {
            NetManager.Instance.SendMsg(new Pkg_C2SLogin()
            {
                operateCode = OperateCode.C2SLogin,
                usernName = "lululu",
                sessionId = NetManager.Instance.SessionId
            });
        }

        private void LoginSuccessHandler(Pkg pkg)
        {
            Pkg_S2CLogin mPkg = pkg as Pkg_S2CLogin;
            txt_id.text = mPkg.roleId.ToString();
        }
    }
}