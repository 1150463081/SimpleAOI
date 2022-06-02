using System;
using System.Collections.Generic;
using System.Text;

namespace AOIServer
{
    public enum ERoleState
    {
        None,
        OnLine,
        OffLine,
        Trusteeship,//托管
    }
    public class RoleEntity
    {
        public int RoleId { get; private set; }
        public ERoleState RoleState { get; private set; }

        public RoleEntity(int roleId)
        {
            RoleId = roleId;
            RoleState = ERoleState.OnLine;
        }
    }
}
