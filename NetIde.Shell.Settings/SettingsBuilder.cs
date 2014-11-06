using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using LinFu.DynamicProxy;
using NetIde.Shell.Interop;

namespace NetIde.Shell.Settings
{
    public static class SettingsBuilder
    {
        private static readonly Dictionary<MemberInfo, object> _defaultValueCache = new Dictionary<MemberInfo, object>();
        private static readonly object _syncRoot = new object();
        private static readonly ProxyFactory _factory = new ProxyFactory();
        private static readonly Dictionary<Type, object> _cache = new Dictionary<Type, object>();

        public static T GetSettings<T>(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");

            var settingsAttributes = typeof(T).GetCustomAttributes(typeof(SettingsAttribute), true);

            if (settingsAttributes.Length == 0)
                throw new InvalidOperationException(Labels.SettingsDoesNotSpecifyAttribute);

            var settingsAttribute = (SettingsAttribute)settingsAttributes[0];

            lock (_syncRoot)
            {
                object result;

                if (
                    settingsAttribute.Singleton &&
                    _cache.TryGetValue(typeof(T), out result)
                )
                    return (T)result;

                result = _factory.CreateProxy<T>(
                    new Interceptor<T>(
                        serviceProvider,
                        settingsAttribute.SettingsType,
                        settingsAttribute.Group,
                        settingsAttribute.Singleton
                    ),
                    typeof(IServiceProvider),
                    typeof(ISettings)
                );

                if (settingsAttribute.Singleton)
                    _cache.Add(typeof(T), result);

                return (T)result;
            }
        }

        private class Interceptor<T> : IInterceptor
        {
            private readonly IServiceProvider _serviceProvider;
            private readonly Type _settingsType;
            private readonly string _group;
            private readonly bool _singleton;
            private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();
            private readonly INiSettings _settings;
            private EventHandler _changed;
            private readonly object _syncRoot = new object();

            public Interceptor(IServiceProvider serviceProvider, Type settingsType, string group, bool singleton)
            {
                _serviceProvider = serviceProvider;
                _settingsType = settingsType;
                _group = @group;
                _singleton = singleton;

                _settings = (INiSettings)_serviceProvider.GetService(settingsType);
            }

            public object Intercept(InvocationInfo info)
            {
                lock (_syncRoot)
                {
                    if (info.TargetMethod.DeclaringType == typeof(IServiceProvider))
                    {
                        switch (info.TargetMethod.Name)
                        {
                            case "get_GetService":
                                return _serviceProvider;
                        }
                    }
                    else if (info.TargetMethod.DeclaringType == typeof(ISettings))
                    {
                        switch (info.TargetMethod.Name)
                        {
                            case "get_Group":
                                return _group;

                            case "get_Singleton":
                                return _singleton;

                            case "get_SettingsType":
                                return _settingsType;

                            case "add_Changed":
                                _changed += (EventHandler)info.Arguments[0];
                                return null;

                            case "remove_Changed":
                                _changed -= (EventHandler)info.Arguments[0];
                                break;

                            case "Reload":
                                _cache.Clear();
                                break;
                        }
                    }
                    else if (info.TargetMethod.DeclaringType == typeof(T))
                    {
                        if (
                            info.TargetMethod.IsSpecialName &&
                            info.TargetMethod.Name.Length > 4
                        )
                        {
                            string prefix = info.TargetMethod.Name.Substring(0, 4);

                            if (prefix == "get_")
                            {
                                return GetSettings(
                                    info.TargetMethod.Name.Substring(4),
                                    info.TargetMethod
                                );
                            }
                            if (prefix == "set_")
                            {
                                SetSetting(
                                    info.TargetMethod.Name.Substring(4),
                                    info.TargetMethod,
                                    info.Arguments[0]
                                );

                                var ev = _changed;
                                if (ev != null)
                                    ev(info.Target, EventArgs.Empty);
                                return null;
                            }
                        }
                    }
                    else if (info.TargetMethod.DeclaringType == typeof(object))
                    {
                        switch (info.TargetMethod.Name)
                        {
                            case "GetHashCode":
                                return GetHashCode();

                            case "Equals":
                                return Equals(info.Arguments[0]);
                        }
                    }

                    throw new InvalidOperationException(Labels.InvalidRequest);
                }
            }

            private void SetSetting(string name, MethodInfo member, object value)
            {
                string key = _group + "." + name;

                var type = member.GetParameters()[0].ParameterType;
                var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

                object defaultValue = GetDefaultValue(member);

                if (value == null || Equals(defaultValue, value))
                {
                    _settings.DeleteValue(key);

                    _cache.Remove(name);
                }
                else
                {
                    if (underlyingType == typeof(string))
                        _settings.SetValue(key, (string)value);
                    else if (underlyingType == typeof(string[]))
                        _settings.SetValue(key, SerializeStringArray((string[])value));
                    else if (underlyingType == typeof(int))
                        _settings.SetValue(key, (int)value);
                    else if (underlyingType == typeof(decimal))
                        _settings.SetValue(key, (decimal)value);
                    else if (underlyingType == typeof(Guid))
                        _settings.SetValue(key, (Guid)value);
                    else if (underlyingType == typeof(bool))
                        _settings.SetValue(key, (bool)value);
                    else if (underlyingType == typeof(Font))
                        _settings.SetValue(key, (Font)value);
                    else if (underlyingType == typeof(Color))
                        _settings.SetValue(key, (Color)value);
                    else if (typeof(Enum).IsAssignableFrom(underlyingType))
                        _settings.SetValue(key, (int)value);
                    else
                        throw new InvalidOperationException(Labels.InvalidRequest);

                    _cache[name] = value;
                }
            }

            private object GetSettings(string name, MethodInfo member)
            {
                object value;

                if (!_cache.TryGetValue(name, out value))
                {
                    string key = _group + "." + name;

                    var underlyingType = Nullable.GetUnderlyingType(member.ReturnType) ?? member.ReturnType;

                    if (underlyingType == typeof(string))
                        value = _settings.GetString(key);
                    else if (underlyingType == typeof(string[]))
                        value = DeserializeStringArray((string)_settings.GetString(key));
                    else if (underlyingType == typeof(int))
                        value = _settings.GetInt32(key);
                    else if (underlyingType == typeof(decimal))
                        value = _settings.GetDecimal(key);
                    else if (underlyingType == typeof(Guid))
                        value = _settings.GetGuid(key);
                    else if (underlyingType == typeof(bool))
                        value = _settings.GetBool(key);
                    else if (underlyingType == typeof(Font))
                        value = _settings.GetFont(key);
                    else if (underlyingType == typeof(Color))
                        value = _settings.GetColor(key);
                    else if (typeof(Enum).IsAssignableFrom(underlyingType))
                        value = GetEnum(underlyingType, key);
                    else
                        throw new InvalidOperationException(Labels.InvalidRequest);

                    if (value == null)
                        value = GetDefaultValue(member);

                    _cache[name] = value;
                }

                return value;
            }

            private string SerializeStringArray(string[] value)
            {
                if (value == null)
                    return null;

                var sb = new StringBuilder();

                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] != null)
                    {
                        foreach (char c in value[i])
                        {
                            switch (c)
                            {
                                case '\\':
                                    sb.Append("\\\\");
                                    break;

                                case '\n':
                                    sb.Append("\\n");
                                    break;

                                case '\r':
                                    sb.Append("\\r");
                                    break;

                                default:
                                    sb.Append(c);
                                    break;
                            }
                        }
                    }

                    // We always append a newline so that we can see whether
                    // the input didn't have any strings. When deserializing
                    // we assert that the last line is empty.

                    sb.Append(Environment.NewLine);
                }

                return sb.ToString();
            }

            private string[] DeserializeStringArray(string value)
            {
                if (value == null)
                    return null;

                string[] parts = value.Split('\n');
                var result = new List<string>();
                var sb = new StringBuilder();

                for (int i = 0; i < parts.Length; i++)
                {
                    sb.Clear();

                    string line = parts[i];
                    int end = line.Length;
                    if (line.Length > 0 && line[line.Length - 1] == '\r')
                        end--;

                    for (int j = 0; j < end; j++)
                    {
                        char c = line[j];
                        if (c == '\\')
                        {
                            Debug.Assert(j < end - 1);

                            switch (line[j + 1])
                            {
                                case '\\':
                                    j++;
                                    sb.Append('\\');
                                    break;

                                case 'n':
                                    j++;
                                    sb.Append('\n');
                                    break;

                                case 'r':
                                    j++;
                                    sb.Append('\r');
                                    break;

                                default:
                                    Debug.Fail("Unexpected escape character");
                                    sb.Append('\\');
                                    break;
                            }
                        }
                        else
                        {
                            sb.Append(c);
                        }
                    }

                    if (i == parts.Length - 1)
                    {
                        // The last line should be empty. Otherwise someone has
                        // tampered with the input.

                        Debug.Assert(line.Length == 0);
                        if (line.Length > 0)
                            result.Add(line);
                    }
                    else
                    {
                        result.Add(line);
                    }
                }

                return result.ToArray();
            }

            private object GetEnum(Type underlyingType, string key)
            {
                var value = _settings.GetInt32(key);

                if (value.HasValue)
                    return Enum.ToObject(underlyingType, value.Value);

                return null;
            }

            private object GetDefaultValue(MethodInfo member)
            {
                lock (_syncRoot)
                {
                    object value;

                    if (!_defaultValueCache.TryGetValue(member, out value))
                    {
                        var defaultValueAttribute = member.GetCustomAttributes(typeof(DefaultValueAttribute), true);

                        if (defaultValueAttribute.Length == 0)
                        {
                            var property = member.DeclaringType.GetProperty(member.Name.Substring(4));

                            defaultValueAttribute = property.GetCustomAttributes(typeof(DefaultValueAttribute), true);
                        }

                        if (defaultValueAttribute.Length > 0)
                        {
                            value = ((DefaultValueAttribute)defaultValueAttribute[0]).Value;
                        }
                        else
                        {
                            var type =
                                member.ReturnType == typeof(void)
                                ? member.GetParameters()[0].ParameterType
                                : member.ReturnType;

                            if (
                                type.IsValueType &&
                                Nullable.GetUnderlyingType(type) == null
                            ) {
                                // This is a value type that is not a Nullable<>.
                                // The GetUnderlyingType method returns null when
                                // the parameter is not a Nullable<>.
                                value = Activator.CreateInstance(type);
                            }
                        }

                        _defaultValueCache.Add(member, value);
                    }

                    return value;
                }
            }
        }
    }
}
