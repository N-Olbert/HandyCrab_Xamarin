﻿using HandyCrab.Business.Services;

namespace HandyCrab.Business
{
    /// <summary>
    /// Storage slot identifiers for usage within the <see cref="ISecureStorage"/>
    /// </summary>
    public enum SecureStorageSlot
    {
        /// <summary>
        /// Storage slot of the current user
        /// </summary>
        CurrentUser,

        /// <summary>
        /// Storage slot of the current user cookie
        /// </summary>
        CurrentUserCookie,
    }
}