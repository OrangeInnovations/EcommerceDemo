using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace CommonShares.Serialization
{
    public class SpecialContractResolver : DefaultContractResolver
    {
        protected override IValueProvider CreateMemberValueProvider(MemberInfo member)
        {
            if (member.MemberType == MemberTypes.Property)
            {
                var pi = (PropertyInfo)member;
                if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return new NullableValueProvider(member, pi.PropertyType.GetGenericArguments().First());
                }
            }
            else if (member.MemberType == MemberTypes.Field)
            {
                var fi = (FieldInfo)member;
                if (fi.FieldType.IsGenericType && fi.FieldType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    return new NullableValueProvider(member, fi.FieldType.GetGenericArguments().First());
            }

            return base.CreateMemberValueProvider(member);
        }
    }
}
