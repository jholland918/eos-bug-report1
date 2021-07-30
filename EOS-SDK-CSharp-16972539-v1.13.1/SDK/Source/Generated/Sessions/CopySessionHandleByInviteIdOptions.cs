// Copyright Epic Games, Inc. All Rights Reserved.
// This file is automatically generated. Changes to this file may be overwritten.

namespace Epic.OnlineServices.Sessions
{
	/// <summary>
	/// Input parameters for the <see cref="SessionsInterface.CopySessionHandleByInviteId" /> function.
	/// </summary>
	public class CopySessionHandleByInviteIdOptions
	{
		/// <summary>
		/// Invite ID for which to retrieve a session handle
		/// </summary>
		public string InviteId { get; set; }
	}

	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	internal struct CopySessionHandleByInviteIdOptionsInternal : ISettable, System.IDisposable
	{
		private int m_ApiVersion;
		private System.IntPtr m_InviteId;

		public string InviteId
		{
			set
			{
				Helper.TryMarshalSet(ref m_InviteId, value);
			}
		}

		public void Set(CopySessionHandleByInviteIdOptions other)
		{
			if (other != null)
			{
				m_ApiVersion = SessionsInterface.CopysessionhandlebyinviteidApiLatest;
				InviteId = other.InviteId;
			}
		}

		public void Set(object other)
		{
			Set(other as CopySessionHandleByInviteIdOptions);
		}

		public void Dispose()
		{
			Helper.TryMarshalDispose(ref m_InviteId);
		}
	}
}