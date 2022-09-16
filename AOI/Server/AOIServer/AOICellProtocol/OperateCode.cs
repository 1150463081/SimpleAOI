using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOICellProtocol
{
    public enum OperateCode
    {
        None=0,

        S2CConnect=10,

        C2SLogin=100,
        S2CLogin=101,

        //更新AOI数据
        S2CUpdateAOI=110,

        //更新位置信息
        C2SEntityMove=120,

        //玩家离开
        C2SEntiyExit=130,

        //创建宫格
        S2CCreateCell=140,
        S2CInitExistCell=141,
    }
}
