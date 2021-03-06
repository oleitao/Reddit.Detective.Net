// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Newtonsoft.Json;
using Reddit.Detective.Net.Serializer.Converters;

namespace Reddit.Detective.Net.Serializer
{
    public static class ParameterSerializer
    {
        public static IList<Dictionary<string, object>> ToDictionary<TSourceType>(IList<TSourceType> source)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            string json = JsonConvert.SerializeObject(source, settings);

            return JsonConvert.DeserializeObject<IList<Dictionary<string, object>>>(json, new CustomDictionaryConverter());
        }
    }
}
