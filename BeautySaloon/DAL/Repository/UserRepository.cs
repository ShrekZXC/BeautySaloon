using BeautySaloon.DAL.Entity;
using BeautySaloon.Model;

namespace BeautySaloon.DAL.Repository;

public class UserRepository
{
    private readonly BeautySaloonDbContext _context;

    public UserRepository(BeautySaloonDbContext context)
    {
        _context = context;
    }

    public async Task<int> Create(User user)
    {
        var userEntity = new UserEntity()
        {
            FirstName = user.FirstName,
            SecondName = user.SecondName,
            LastName = user.LastName,
            Email = user.Email,
            Password = user.Password,
            Phone = user.Phone
        };

        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();

        return 1;
    }
}