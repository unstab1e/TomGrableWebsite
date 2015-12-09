using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Coveo.Framework.Utils;
using RestSharp;
using SimpleImpersonation;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.StringExtensions;
//using TomGrable.Website.Data;

namespace TomGrable.Website.Extensions.Tasks
{
    public class CesSecurityCacheUpdate
    {
        protected static bool CesAdminAvailable = false;
        protected static Type CesAdminType;

        static CesSecurityCacheUpdate()
        {
            // Check if the ProgID exists on this machine.
            CesAdminType = TypeDelegator.GetTypeFromProgID("CESAdmin.Admin.7.0");
            if (CesAdminType != null) CesAdminAvailable = true;
        }

        #region Properties

        protected string _RestBaseUrl = null;
        protected string RestBaseUrl
        {
            get
            {
                if (_RestBaseUrl == null)
                {
                    // Read the IsSecurityCacheUpdateInProgress Base URL from config.
                    _RestBaseUrl = Settings.GetSetting("Wacom.Ces.IsSecurityCacheUpdateInProgress.BaseUrl");
                    if (_RestBaseUrl == null)
                    {
                        Log.Warn("Unable to find a value for Wacom.Ces.IsSecurityCacheUpdateInProgress.BaseUrl in config - using default!", this);
                        _RestBaseUrl = "http://localhost:8081/Index/Mirrors";
                    }
                }

                return _RestBaseUrl;
            }
        }

        protected string _RestResource = null;
        protected string RestResource
        {
            get
            {
                if (_RestResource == null)
                {
                    // Read the IsSecurityCacheUpdateInProgress Resource from config.
                    _RestResource = Settings.GetSetting("Wacom.Ces.IsSecurityCacheUpdateInProgress.Resource");
                    if (_RestResource == null) {
                        Log.Warn("Unable to find a value for Wacom.Ces.IsSecurityCacheUpdateInProgress.Resource in config - using default!", this);
                        _RestResource = "IsSecurityCacheUpdateInProgress.aspx";
                    }
                }

                return _RestResource;
            }
        }

        protected string _CesImpersonateUsername = null;
        protected string CesImpersonateUsername
        {
            get
            {
                if (_CesImpersonateUsername == null)
                {
                    // Read the impersonate username from config.
                    _CesImpersonateUsername = Settings.GetSetting("Wacom.Ces.ImpersonateUsername");
                    if (_CesImpersonateUsername == null) {
                        Log.Warn("Unable to find a value for Wacom.Ces.ImpersonateUsername in config - using default!", this);
                        _CesImpersonateUsername = "lokal";
                    }
                }

                return _CesImpersonateUsername;
            }
        }

        protected string _CesImpersonatePassword = null;
        protected string CesImpersonatePassword
        {
            get
            {
                if (_CesImpersonatePassword == null)
                {
                    // Read the impersonate password from config.
                    _CesImpersonatePassword = Settings.GetSetting("Wacom.Ces.ImpersonatePassword");
                    if (_CesImpersonatePassword == null) {
                        Log.Warn("Unable to find a value for Wacom.Ces.ImpersonatePassword in config - using default!", this);
                        _CesImpersonatePassword = "Wacom2015!";
                    }
                }

                return _CesImpersonatePassword;
            }
        }

        #endregion Properties

        public void Run()
        {
            // If CesAdmin isn't available on this machine, do nothing.
            if (!CesAdminAvailable) return;

            try
            {
                // If a request is already running, we do nothing until next time this scheduled task runs.
                if (IsUpdateRunning()) return;

//                // Create a list.
//                var events = new List<EventQueue>();

//                // Look for any queued updates that need to be processed.
//                using (var context = new WacomAppDataContext())
//                {
//                    // Grab the queue'd events.
//                    events.AddRange(context.EventQueues);

//                    // If there weren't any queued, we do nothing.
//                    if (!events.Any()) return;

//                    // TODO: Even worse than below, we are now having to shell to an EXE that does the impersonating!
//                    string updateExe = Settings.GetSetting("Wacom.Ces.SecurityCacheUpdate.EXE", "C:\\Sitecore\\UpdateCoveoSecurityCache.exe");
//                    string updateArgs = "{0} {1}".FormatWith(CesImpersonateUsername, CesImpersonatePassword);

//                    var process = new System.Diagnostics.Process();
//                    process.StartInfo.FileName = updateExe;
//                    process.StartInfo.Arguments = updateArgs;
//                    process.StartInfo.UseShellExecute = false;
//                    process.StartInfo.RedirectStandardError = true;
//                    process.StartInfo.RedirectStandardOutput = true;
//                    process.StartInfo.CreateNoWindow = true;
//                    process.Start();
//                    Log.Debug("CesSecurityCacheUpdate - {0} started".FormatWith(updateExe));

//                    string stdout = process.StandardOutput.ReadToEnd();
//                    string stderr = process.StandardError.ReadToEnd();
//                    Log.Debug("CesSecurityCacheUpdate - output: {0}".FormatWith(stdout));
//                    Log.Debug("CesSecurityCacheUpdate - error output: {0}".FormatWith(stderr));

//                    process.WaitForExit();
//                    Log.Debug("CesSecurityCacheUpdate - Process has exited");

///*
//                    // TODO: Fix this once Coveo gives us an alternative to impersonating an administrator.
//                    using (Impersonation.LogonUser(".", CesImpersonateUsername, CesImpersonatePassword, LogonType.Interactive))
//                    {
//                        // Create and initialize the CesAdmin COM object.
//                        dynamic cesAdmin = Activator.CreateInstance(CesAdminType);
//                        cesAdmin.Connect("localhost", "default");
//                        cesAdmin.Refresh();

//                        // Trigger Coveo to update the security cache.
//                        cesAdmin.UpdateFileSecurityCache(true);
//                    }
//*/

//                    // Wait until the update completes.
//                    var running = true;
//                    while (running)
//                    {
//                        // Wait 10 seconds.
//                        Thread.Sleep(10000);

//                        // Check if the update is running.
//                        running = IsUpdateRunning();
//                    }

//                    // Delete the events that were processed.
//                    context.EventQueues.DeleteAllOnSubmit(events);
//                    context.SubmitChanges();
//                }
            }
            catch (Exception ex)
            {
                Log.Error("could not run the CesSecurityCacheUpdate task", ex, this);
            }
        }

        public bool IsUpdateRunning()
        {
            // Make a REST request to the Coveo server to determine if an update is already in progress.
            var restClient = new RestClient(RestBaseUrl);
            var restRequest = new RestRequest(RestResource);
            var response = restClient.Execute<bool>(restRequest);
            return response.Data;
        }
    }
}
