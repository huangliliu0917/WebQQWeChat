﻿using iQQ.Net.WebQQCore.Im.Action;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;

namespace iQQ.Net.WebQQCore.Im.Module
{
    /// <summary>
    /// <para>群模块</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class GroupModule : AbstractModule
    {
        public override QQModuleType GetModuleType()
        {
            return QQModuleType.GROUP;
        }

        public IQQActionFuture GetGroupList(QQActionEventHandler listener = null)
        {
            return PushHttpAction(new GetGroupListAction(Context, listener));
        }

        public IQQActionFuture UpdateGroupMessageFilter(QQActionEventHandler listener = null)
        {
            return PushHttpAction(new UpdateGroupMessageFilterAction(Context, listener));
        }

        public IQQActionFuture GetGroupFace(QQGroup group, QQActionEventHandler listener = null)
        {
            return PushHttpAction(new GetGroupFaceAction(Context, listener, group));
        }

        public IQQActionFuture GetGroupInfo(QQGroup group, QQActionEventHandler listener = null)
        {
            return PushHttpAction(new GetGroupInfoAction(Context, listener, group));
        }

        public IQQActionFuture GetGroupGid(QQGroup group, QQActionEventHandler listener = null)
        {
            return PushHttpAction(new GetGroupAccoutAction(Context, listener, group));
        }

        public IQQActionFuture GetMemberStatus(QQGroup group, QQActionEventHandler listener = null)
        {
            return PushHttpAction(new GetGroupMemberStatusAction(Context, listener, group));
        }

        public IQQActionFuture SearchGroup(QQGroupSearchList resultList, QQActionEventHandler listener = null)
        {
            return PushHttpAction(new SearchGroupInfoAction(Context, listener, resultList));
        }
    }

}
