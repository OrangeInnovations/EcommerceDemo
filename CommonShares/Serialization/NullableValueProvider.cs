using System;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace CommonShares.Serialization
{
    public class NullableValueProvider : IValueProvider
    {
        private readonly object _defaultValue;
        private readonly IValueProvider _underlyingValueProvider;


        public NullableValueProvider(MemberInfo memberInfo, Type underlyingType)
        {
            _underlyingValueProvider = new Newtonsoft.Json.Serialization.ExpressionValueProvider(memberInfo); //DynamicValueProvider(memberInfo);
            _defaultValue = Activator.CreateInstance(underlyingType);
        }

        public void SetValue(object target, object value)
        {
            _underlyingValueProvider.SetValue(target, value);
        }

        public object GetValue(object target)
        {
            return _underlyingValueProvider.GetValue(target) ?? _defaultValue;
        }
    }
}