// Copyright Epic Games, Inc. All Rights Reserved.
// This file is automatically generated. Changes to this file may be overwritten.

namespace Epic.OnlineServices.Ecom
{
	/// <summary>
	/// Input parameters for the <see cref="Transaction.CopyEntitlementByIndex" /> function.
	/// </summary>
	public class TransactionCopyEntitlementByIndexOptions
	{
		/// <summary>
		/// The index of the entitlement to get
		/// </summary>
		public uint EntitlementIndex { get; set; }
	}

	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	internal struct TransactionCopyEntitlementByIndexOptionsInternal : ISettable, System.IDisposable
	{
		private int m_ApiVersion;
		private uint m_EntitlementIndex;

		public uint EntitlementIndex
		{
			set
			{
				m_EntitlementIndex = value;
			}
		}

		public void Set(TransactionCopyEntitlementByIndexOptions other)
		{
			if (other != null)
			{
				m_ApiVersion = Transaction.TransactionCopyentitlementbyindexApiLatest;
				EntitlementIndex = other.EntitlementIndex;
			}
		}

		public void Set(object other)
		{
			Set(other as TransactionCopyEntitlementByIndexOptions);
		}

		public void Dispose()
		{
		}
	}
}