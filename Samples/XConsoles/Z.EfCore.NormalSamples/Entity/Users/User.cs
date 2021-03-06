﻿using System;

namespace Z.EfCore.NormalSamples.Entity.Users
{
    public class User
    {
        public Guid Id { get; set; }
        #region Properties
        public string Username { get; set; }
        public string Email { get; set; }
        public string Mobilephone { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        #endregion
    }
}
