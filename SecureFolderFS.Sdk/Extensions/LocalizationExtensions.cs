﻿using CommunityToolkit.Mvvm.DependencyInjection;
using SecureFolderFS.Sdk.Services;

namespace SecureFolderFS.Sdk.Extensions
{
    public static class LocalizationExtensions
    {
        private static ILocalizationService? FallbackLocalizationService;

        public static string? ToLocalized(this string resourceKey, ILocalizationService? localizationService = null)
        {
            if (localizationService is null)
            {
                FallbackLocalizationService ??= Ioc.Default.GetService<ILocalizationService>();
                return FallbackLocalizationService?.LocalizeFromResourceKey(resourceKey) ?? string.Empty;
            }

            return localizationService.LocalizeFromResourceKey(resourceKey);
        }
    }
}