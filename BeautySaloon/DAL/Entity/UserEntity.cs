﻿using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.DAL.Entity;

public class UserEntity : BaseEntity
{
    public string FirstName { get; set; }
    
    public string SecondName { get; set; }
    
    public string? LastName { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public string? Phone { get; set; }
    
    public string Salt { get; set; }
    
    public RoleEntity Role { get; set; }
}