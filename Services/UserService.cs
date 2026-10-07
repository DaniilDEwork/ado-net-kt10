using Kt10Validation.Data;
using Kt10Validation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Kt10Validation.Services;

public class UserService(AppDbContext db, PasswordHasher<AppUser> hasher, UserWriteLock writeLock)
{
    public List<UserResponse> GetAll()
    {
        return db.Users.AsNoTracking().OrderBy(user => user.Id)
            .Select(user => new UserResponse { Id = user.Id, UserName = user.UserName, Email = user.Email }).ToList();
    }

    public UserResponse? GetById(int id)
    {
        var user = db.Users.AsNoTracking().FirstOrDefault(user => user.Id == id);
        return user is null ? null : ToResponse(user);
    }

    public UserResponse Create(UserRequest request, string? password = null)
    {
        lock (writeLock.SyncRoot)
        {
            var name = request.UserName;
            var email = request.Email.ToLowerInvariant();
            CheckDuplicates(name, email);
            var user = new AppUser { UserName = name, Email = email };
            if (password is not null) user.PasswordHash = hasher.HashPassword(user, password);
            db.Users.Add(user);
            db.SaveChanges();
            return ToResponse(user);
        }
    }

    public UserResponse? Update(int id, UserRequest request)
    {
        lock (writeLock.SyncRoot)
        {
            var user = db.Users.Find(id);
            if (user is null) return null;
            var email = request.Email.ToLowerInvariant();
            CheckDuplicates(request.UserName, email, id);
            user.UserName = request.UserName;
            user.Email = email;
            db.SaveChanges();
            return ToResponse(user);
        }
    }

    public bool Delete(int id)
    {
        lock (writeLock.SyncRoot)
        {
            var user = db.Users.Find(id);
            if (user is null) return false;
            db.Users.Remove(user);
            db.SaveChanges();
            return true;
        }
    }

    private void CheckDuplicates(string name, string email, int? excludedId = null)
    {
        if (db.Users.Any(user => user.Id != excludedId && user.UserName.ToUpperInvariant() == name.ToUpperInvariant()))
        {
            throw new InvalidOperationException("Это имя пользователя уже занято");
        }
        if (db.Users.Any(user => user.Id != excludedId && user.Email == email))
        {
            throw new InvalidOperationException("Эта электронная почта уже используется");
        }
    }

    private static UserResponse ToResponse(AppUser user)
    {
        return new UserResponse { Id = user.Id, UserName = user.UserName, Email = user.Email };
    }
}
