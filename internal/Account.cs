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

        public Account(string email, string password, string deviceUUID, string deviceName = "PC", string osVersion = "10.0", bool permanent = false, bool forced = false, bool locked = false)
        {
            if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException();
            }
            else
            {
                this.email = email;
                this.password = password;
            }

            if (String.IsNullOrEmpty(deviceUUID))
            {
                this.deviceUUID = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            }
            else
            {
                this.deviceUUID = Convert.ToBase64String(Encoding.UTF8.GetBytes(deviceUUID));
            }

            this.deviceName = deviceName;
            this.osVersion = osVersion;
            this.permanent = permanent;
            this.forced = forced;
        }
    }
}