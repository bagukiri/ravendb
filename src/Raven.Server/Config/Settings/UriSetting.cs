﻿using System;
using System.Collections.Generic;
namespace Raven.Server.Config.Settings
{
    public struct UriSetting
    {
        public static readonly Type TypeOf = typeof(UriSetting);
        public static readonly Type NullableTypeOf = typeof(UriSetting?);

        public UriSetting(string uri)
        {
            UriValue = uri;

            if (string.IsNullOrWhiteSpace(UriValue))
            {
                UriValue = null;
                return;
            }

            if (System.Uri.TryCreate(uri, UriKind.RelativeOrAbsolute, out _) == false)
                throw new ArgumentException($"{uri} is not a valid URI.");
        }

        public readonly string UriValue;
    }
}
