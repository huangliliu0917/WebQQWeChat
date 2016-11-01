﻿using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Utility.HttpAction;
using Utility.HttpAction.Action;
using Utility.HttpAction.Event;
using WebWeChat.Im.Action;
using WebWeChat.Im.Core;
using WebWeChat.Im.Event;
using WebWeChat.Im.Module.Interface;
using WebWeChat.Im.Service.Impl;

namespace WebWeChat.Im.Module.Impl
{
    public class LoginModule : WeChatModule, ILoginModule
    {
        public LoginModule(IWeChatContext context) : base(context)
        {
        }

        public Task<ActionEvent> Login(ActionEventListener listener = null)
        {
            var future = new WeChatActionFuture(Context, listener)
                .PushAction<GetUuidAction>()
                .PushAction<GetQRCodeAction>((sender, @event) =>
                {
                    if (@event.Type == ActionEventType.EvtOK)
                    {
                        var verify = (Image)@event.Target;
                        Context.FireNotify(new WeChatNotifyEvent(WeChatNotifyEventType.QRCodeReady, verify));
                    }
                })
                .PushAction<WatiForLoginAction>((sender, @event) =>
                {
                    if (@event.Type == ActionEventType.EvtRepeat)
                    {
                        Context.FireNotify(new WeChatNotifyEvent(WeChatNotifyEventType.QRCodeSuccess));
                    }
                    else if (@event.Type == ActionEventType.EvtError)
                    {
                        Context.FireNotify(new WeChatNotifyEvent(WeChatNotifyEventType.QRCodeInvalid));
                    }
                })
                .PushAction<WebLoginAction>((sender, @event) =>
                {
                    if (@event.Type == ActionEventType.EvtOK)
                    {
                        Context.FireNotify(new WeChatNotifyEvent(WeChatNotifyEventType.LoginSuccess));
                        AfterLogin();
                        BeginSyncCheck();
                    }
                });
            return future.ExecuteAsync();
        }

        private void AfterLogin()
        {
            var future = new WeChatActionFuture(Context)
                .PushAction<WebwxInitAction>()
                .PushAction<StatusNotifyAction>()
                .PushAction<GetContactAction>()
                .PushAction<BatchGetContactAction>()
                .ExecuteAsync();
        }

        private void BeginSyncCheck()
        {
            var future = new WeChatActionFuture(Context);
            future.PushAction<SyncCheckAction>((sender, @event) =>
            {
                if (@event.Type != ActionEventType.EvtOK) return;
                var result = (SyncCheckResult)@event.Target;
                switch (result)
                {
                    case SyncCheckResult.Nothing:
                        break;
                    case SyncCheckResult.NewMsg:
                        break;
                    case SyncCheckResult.UsingPhone:
                        break;
                    case SyncCheckResult.RedEnvelope:
                        break;
                    case SyncCheckResult.Offline:
                        break;
                    case SyncCheckResult.Kicked:
                        break;
                }
                if (Context.GetModule<SessionModule>().State == SessionState.Online)
                {
                    future.PushAction(sender);
                }
            }).ExecuteAsync();
        }
    }
}