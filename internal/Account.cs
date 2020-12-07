using System;
using System.Text;

namespace LocoDotnet.Internal
{
    public struct Account
    {
        public readonly string email;
        public readonly string password;
        public readonly string deviceUUID;
        public readonly string deviceName;
        public readonly string osVersion;
        public readonly bool permanent;
        public readonly bool forced;
        public readonly bool autowithlock;
        public readonly bool autologin;

        public Account(string email, string password, string deviceUUID, string deviceName = "PC", string osVersion = "10.0", bool permanent = false, bool forced = false, bool locked = false)
        {
            this.email = email;
            this.password = password;

            if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException();
            }

            if (!String.IsNullOrEmpty(deviceUUID))
            {
                this.deviceUUID = Convert.ToBase64String(Encoding.UTF8.GetBytes(deviceUUID));
            }
            else
            {
                this.deviceUUID = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            }
            this.deviceName = deviceName;
            this.osVersion = osVersion;
            this.autologin = locked ? true : false;
            this.autowithlock = locked ? true : false;
            this.permanent = permanent;
            this.forced = forced;
        }
    }
}