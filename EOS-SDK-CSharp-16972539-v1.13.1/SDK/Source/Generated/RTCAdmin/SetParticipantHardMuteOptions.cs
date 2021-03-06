// Copyright Epic Games, Inc. All Rights Reserved.
// This file is automatically generated. Changes to this file may be overwritten.

namespace Epic.OnlineServices.RTCAdmin
{
	/// <summary>
	/// Input parameters for the <see cref="RTCAdminInterface.SetParticipantHardMute" /> function.
	/// </summary>
	public class SetParticipantHardMuteOptions
	{
		/// <summary>
		/// Room to kick the participant from
		/// </summary>
		public string RoomName { get; set; }

		/// <summary>
		/// Product User ID of the participant to hard mute for every participant in the room.
		/// </summary>
		public ProductUserId TargetUserId { get; set; }

		/// <summary>
		/// Hard mute status (Mute on or off)
		/// </summary>
		public bool Mute { get; set; }
	}

	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	internal struct SetParticipantHardMuteOptionsInternal : ISettable, System.IDisposable
	{
		private int m_ApiVersion;
		private System.IntPtr m_RoomName;
		private System.IntPtr m_TargetUserId;
		private int m_Mute;

		public string RoomName
		{
			set
			{
				Helper.TryMarshalSet(ref m_RoomName, value);
			}
		}

		public ProductUserId TargetUserId
		{
			set
			{
				Helper.TryMarshalSet(ref m_TargetUserId, value);
			}
		}

		public bool Mute
		{
			set
			{
				Helper.TryMarshalSet(ref m_Mute, value);
			}
		}

		public void Set(SetParticipantHardMuteOptions other)
		{
			if (other != null)
			{
				m_ApiVersion = RTCAdminInterface.SetparticipanthardmuteApiLatest;
				RoomName = other.RoomName;
				TargetUserId = other.TargetUserId;
				Mute = other.Mute;
			}
		}

		public void Set(object other)
		{
			Set(other as SetParticipantHardMuteOptions);
		}

		public void Dispose()
		{
			Helper.TryMarshalDispose(ref m_RoomName);
			Helper.TryMarshalDispose(ref m_TargetUserId);
		}
	}
}