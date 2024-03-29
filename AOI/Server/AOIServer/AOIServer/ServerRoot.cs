﻿using PEUtils;
using System;
using System.Collections.Generic;
using System.Text;
using PENet;
using AOICellProtocol;
using System.Collections.Concurrent;
using AOICell;
using System.Numerics;

namespace AOIServer
{
    public partial class ServerRoot
    {
        private static ServerRoot instance;
        public static ServerRoot Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ServerRoot();
                }
                return instance;
            }
        }
        AsyncNet<ServerSession, Pkg> asyncNet = new AsyncNet<ServerSession, Pkg>();
        ConcurrentQueue<NetPkg> pkgQueue = new ConcurrentQueue<NetPkg>();
        ConcurrentDictionary<OperateCode, Action<NetPkg>> handlerDict = new ConcurrentDictionary<OperateCode, Action<NetPkg>>();
        ConcurrentDictionary<string, ServerSession> sessionDict = new ConcurrentDictionary<string, ServerSession>();
        //场景关卡
        public BattleStage BattleStage { get; private set; }


        private static int cIndex = 1000;
        public int SpawnClientId()
        {
            return cIndex++;
        }
        private static int sIndex = 2000;
        public int SpawnServerId()
        {
            return sIndex++;
        }
        //创建机器人
        public void CreateRobotRole()
        {
            Random random = new Random();
            var rdx = random.Next(AOICfg.borderLeft, AOICfg.borderRight);
            var rdz = random.Next(AOICfg.borderDown, AOICfg.borderUp);
            var roleId = SpawnServerId();
            var robotEntity = new RoleEntity(roleId, null, AOICell.EDriveType.Server);
            robotEntity.TargetPos = new Vector3(rdx, 0, rdz);
            BattleStage.EnterStage(robotEntity);
            robotEntity.StartRandomMove();
        }

        public void Init()
        {
            asyncNet.StartAsServer("127.0.0.1", 0415);
            BattleStage = new BattleStage();
            BattleStage.Init();
            InitHandler();
        }
        public void Tick()
        {
            BattleStage?.Tick();
            while (!pkgQueue.IsEmpty)
            {
                if (pkgQueue.TryDequeue(out var netPkg))
                {
                    if (handlerDict.TryGetValue(netPkg.pkg.operateCode, out var handler))
                    {
                        handler?.Invoke(netPkg);
                    }
                    else
                    {
                        PELog.Error($"操作{netPkg.pkg.operateCode}触发失败");
                    }
                }
                else
                {
                    PELog.Error("Dequeue Queue Error!!!");
                }
            }
        }
        public void Destroy()
        {
            BattleStage.Destroy();
        }
        public void AddNetPkg(NetPkg netPkg)
        {
            pkgQueue.Enqueue(netPkg);
        }
        public void AddHandler(OperateCode operateCode, Action<NetPkg> handler)
        {
            handlerDict[operateCode] = handler;
        }
        public void AddSession(string sessionId, ServerSession session)
        {
            sessionDict[sessionId] = session;
        }
        public void RemoveSession(string sessionId)
        {
            sessionDict.TryRemove(sessionId ,out var session);
        }
        public void SendMsg8Session(string sessionId,AsyncMsg msg)
        {
            if(sessionDict.TryGetValue(sessionId,out var session))
            {
                session.SendMsg(msg);
            }
            else
            {
                PELog.Error("No Session To [{0}]", sessionId);
            }
        }

        private void InitHandler()
        {
            new LoginHandler();
            new EntityMoveHandler();
            new EntityExitHandler();
        }
    }
}
