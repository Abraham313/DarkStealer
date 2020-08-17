using System;

namespace CG.Web.MegaApiClient
{
	// Token: 0x02000148 RID: 328
	public enum ApiResultCode
	{
		// Token: 0x04000678 RID: 1656
		Ok,
		// Token: 0x04000679 RID: 1657
		InternalError = -1,
		// Token: 0x0400067A RID: 1658
		BadArguments = -2,
		// Token: 0x0400067B RID: 1659
		RequestFailedRetry = -3,
		// Token: 0x0400067C RID: 1660
		TooManyRequests = -4,
		// Token: 0x0400067D RID: 1661
		RequestFailedPermanetly = -5,
		// Token: 0x0400067E RID: 1662
		ToManyRequestsForThisResource = -6,
		// Token: 0x0400067F RID: 1663
		ResourceAccessOutOfRange = -7,
		// Token: 0x04000680 RID: 1664
		ResourceExpired = -8,
		// Token: 0x04000681 RID: 1665
		ResourceNotExists = -9,
		// Token: 0x04000682 RID: 1666
		CircularLinkage = -10,
		// Token: 0x04000683 RID: 1667
		AccessDenied = -11,
		// Token: 0x04000684 RID: 1668
		ResourceAlreadyExists = -12,
		// Token: 0x04000685 RID: 1669
		RequestIncomplete = -13,
		// Token: 0x04000686 RID: 1670
		CryptographicError = -14,
		// Token: 0x04000687 RID: 1671
		BadSessionId = -15,
		// Token: 0x04000688 RID: 1672
		ResourceAdministrativelyBlocked = -16,
		// Token: 0x04000689 RID: 1673
		QuotaExceeded = -17,
		// Token: 0x0400068A RID: 1674
		ResourceTemporarilyNotAvailable = -18,
		// Token: 0x0400068B RID: 1675
		TooManyConnectionsOnThisResource = -19,
		// Token: 0x0400068C RID: 1676
		FileCouldNotBeWrittenTo = -20,
		// Token: 0x0400068D RID: 1677
		FileCouldNotBeReadFrom = -21,
		// Token: 0x0400068E RID: 1678
		InvalidOrMissingApplicationKey = -22,
		// Token: 0x0400068F RID: 1679
		TwoFactorAuthenticationError = -26
	}
}
