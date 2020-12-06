using System;
using System.Text;

namespace LocoDotnet.Internal {
    public struct Account {
        private string email;
        private string password;
        private string deviceUUID;
        private string deviceName;
        private string osVersion;
        private bool permanent;
        private bool forced;

        public Account(string email, string password, string deviceUUID, string deviceName, string osVersion, bool? permanent, bool? forced) {
            this.email = email;
            this.password = password;

            if(String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password)) {
                throw new ArgumentNullException();
            }

            if(!String.IsNullOrEmpty(deviceUUID)) {
                this.deviceUUID = Convert.ToBase64String(Encoding.UTF8.GetBytes(deviceUUID));
            } else {
                this.deviceUUID = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            }

            if(!String.IsNullOrEmpty(deviceName)) {
                this.deviceName = deviceName;
            } else {
                this.deviceName = "PC";
            }

            if(!String.IsNullOrEmpty(osVersion)) {
                this.osVersion = osVersion;
            } else {
                this.osVersion = "10.0";
            }

            if(permanent.HasValue) {
                this.permanent = permanent.Value;
            } else {
                this.permanent = false;
            }

            if(forced.HasValue) {
                this.forced = forced.Value;
            } else {
                this.forced = false;
            }
        }

        public string Email {
            get {
                return email;
            }
        }

        public string Password {
            get {
                return password;
            }
        }

        public string DeviceUUID {
            get {
                return deviceUUID;
            }
        }

        public string DeviceName {
            get {
                return deviceName;
            }
        }

        public string OsVersion {
            get {
                return osVersion;
            }
        }

        public bool Permanent {
            get {
                return permanent;
            }
        }

        public bool Forced {
            get {
                return forced;
            }
        }
    }
}