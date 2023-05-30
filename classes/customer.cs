using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarAgency.classes
{
    internal abstract class customer
    {
        private static customer _instance;
        private static readonly object _lock = new object();

        private int userid { get; set; }
        private string username { get; set; }
        private string password { get; set; }
        private string email { get; set; }
        private string phone { get; set; }

        protected customer(int userid, string username, string password, string email, string phone)
        {
            this.userid = userid;
            this.username = username;
            this.password = password;
            this.email = email;
            this.phone = phone;
        }

        protected customer()
        {
        }

        public static customer GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new customer_login();
                    }
                }
            }
            return _instance;
        }

        public abstract bool login(string username, string password);

        public abstract bool signup(string username, string password, string email);
    }
}
