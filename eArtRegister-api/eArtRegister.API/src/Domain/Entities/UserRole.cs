﻿using System;

namespace eArtRegister.API.Domain.Entities
{
    public class UserRole
    {
        public Guid UserId { get; set; }

        public Guid RoleId { get; set; }
    }
}
