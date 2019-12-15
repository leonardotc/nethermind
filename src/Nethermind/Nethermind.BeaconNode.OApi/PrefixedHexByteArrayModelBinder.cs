﻿using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Nethermind.BeaconNode.OApi
{
    public class PrefixedHexByteArrayModelBinder : IModelBinder
    {
        private const string Prefix = "0x";

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            string modelName = bindingContext.ModelName;
            ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                // no entry
                return Task.CompletedTask;
            }

            ModelStateDictionary modelState = bindingContext.ModelState;
            modelState.SetModelValue(modelName, valueProviderResult);

            ModelMetadata metadata = bindingContext.ModelMetadata;
            Type type = metadata.UnderlyingOrModelType;

            string value = valueProviderResult.FirstValue;
            CultureInfo culture = valueProviderResult.Culture;

            object? model;
            if (string.IsNullOrWhiteSpace(value))
            {
                model = null;
            }
            else if (type == typeof(byte[]))
            {
                if (value.StartsWith(Prefix))
                {
                    model = Enumerable.Range(Prefix.Length, value.Length - Prefix.Length)
                            .Where(x => x % 2 == 0)
                            .Select(x => Convert.ToByte(value.Substring(x, 2), 16))
                            .ToArray();
                }
                else
                {
                    throw new FormatException("Byte value must start with '0x'");
                }
            }
            else
            {
                // unreachable
                throw new NotSupportedException();
            }

            // When converting value, a null model may indicate a failed conversion for an otherwise required
            // model (can't set a ValueType to null). This detects if a null model value is acceptable given the
            // current bindingContext. If not, an error is logged.
            if (model == null && !metadata.IsReferenceOrNullableType)
            {
                modelState.TryAddModelError(
                    modelName,
                    metadata.ModelBindingMessageProvider.ValueMustNotBeNullAccessor(
                        valueProviderResult.ToString()));
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Success(model);
            }

            return Task.CompletedTask;
        }
    }
}