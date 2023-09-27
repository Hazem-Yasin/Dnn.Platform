﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace Dnn.ExchangeOnlineAuthProvider.Components;

using System.Text;

using DotNetNuke.Abstractions.Application;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;

using Microsoft.Identity.Client;

/// <summary>The token cache helper.</summary>
public class TokenCacheHelper
{
    private readonly IHostSettingsService hostSettingsService;
    private readonly int portalId;

    /// <summary>Initializes a new instance of the <see cref="TokenCacheHelper"/> class.</summary>
    /// <param name="portalId">The portal ID.</param>
    /// <param name="hostSettingsService">The host settings service.</param>
    public TokenCacheHelper(int portalId, IHostSettingsService hostSettingsService)
    {
        this.portalId = portalId;
        this.hostSettingsService = hostSettingsService;
    }

    /// <summary>Enable the token cache.</summary>
    /// <param name="tokenCache">The token cache instance.</param>
    public void EnableSerialization(ITokenCache tokenCache)
    {
        tokenCache.SetBeforeAccess(this.BeforeAccessNotification);
        tokenCache.SetAfterAccess(this.AfterAccessNotification);
    }

    private void BeforeAccessNotification(TokenCacheNotificationArgs args)
    {
        var authentication = this.GetAuthenticationData();
        if (!string.IsNullOrEmpty(authentication))
        {
            args.TokenCache.DeserializeMsalV3(Encoding.UTF8.GetBytes(authentication));
        }
    }

    private void AfterAccessNotification(TokenCacheNotificationArgs args)
    {
        if (args.HasStateChanged)
        {
            byte[] data = args.TokenCache.SerializeMsalV3();
            this.UpdateAuthenticationData(data);
        }
    }

    private string GetAuthenticationData()
    {
        if (this.portalId == Null.NullInteger)
        {
            return this.hostSettingsService.GetEncryptedString(Constants.AuthenticationSettingName, Config.GetDecryptionkey());
        }

        return PortalController.GetEncryptedString(Constants.AuthenticationSettingName, this.portalId, Config.GetDecryptionkey());
    }

    private void UpdateAuthenticationData(byte[] data)
    {
        var settingValue = Encoding.UTF8.GetString(data);
        if (this.portalId == Null.NullInteger)
        {
            this.hostSettingsService.UpdateEncryptedString(Constants.AuthenticationSettingName, settingValue, Config.GetDecryptionkey());
        }
        else
        {
            PortalController.UpdateEncryptedString(this.portalId, Constants.AuthenticationSettingName, settingValue, Config.GetDecryptionkey());
        }
    }
}
