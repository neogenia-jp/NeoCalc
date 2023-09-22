using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;

namespace NeoCalc.WebCalc.Mvc
{
    public class ControllerEx : Controller
    {
        private SessionProxy? _sessionProxy = null;
        public SessionProxy Session
        {
            get
            {
                if (_sessionProxy == null)
                {
                    _sessionProxy = new SessionProxy(this.HttpContext.Session);
                }
                return _sessionProxy;
            }
        }

    }

    public class SessionProxy
    {
        private ISession _session;

        internal SessionProxy(ISession s)
        {
            _session = s;
        }

        public Object? this[string key]
        {
            get
            {
                try
                {
                    string? value = _session.GetString(key) ?? string.Empty;
                    var obj = JsonSerializer.Deserialize<Object>(value);
                    return obj;
                } catch(Exception ex)
                {
                    return null;
                }
            }
            set
            {
                string? json;
                try
                {
                    json = JsonSerializer.Serialize(value);

                } catch(Exception ex)
                {
                    json = "{}";
                }
                _session.SetString(key, json);
            }
        }
    }
}

