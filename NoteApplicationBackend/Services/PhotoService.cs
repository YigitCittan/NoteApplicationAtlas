    using System.Net.Mime;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using NoteApplicationBackend.Data;
    using NoteApplicationBackend.DTOs;
    using NoteApplicationBackend.Models;
    using NoteApplicationBackend.Repositories;

    namespace NoteApplicationBackend.Services
    {
        public class PhotoService
        {
            private readonly UserManager<IdentityUser> _userManager;
            private readonly ApplicationDbContext _context;

            public PhotoService(UserManager<IdentityUser> userManager,
                                ApplicationDbContext context)
            {
                _userManager = userManager;
                _context = context;
            }

            // Get user by email from db
            
            public async Task<UserPhoto> AddProfilePhoto(string userId, string base64)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    // Kullanıcı bulunamadı hatası
                    throw new Exception("Kullanıcı bulunamadı.");
                }

                if (base64.StartsWith("data:image/jpeg;base64,"))
                {
                    base64 = base64.Replace("data:image/jpeg;base64,", "");
                }   

                byte[] photoData;
                try
                {
                    photoData = Convert.FromBase64String(base64);
                }
                catch (FormatException ex)
                {
                    // Base64 kodu geçersiz hatası
                    throw new FormatException("Geçersiz base64 kodu.", ex);
                }

                UserPhoto userPhoto = new UserPhoto
                {
                    UserId = userId,
                    PhotoData = photoData
                };

                if (_context.userPhoto != null)
                {
                    _context.userPhoto.Add(userPhoto);
                    _context.SaveChanges();
                    return userPhoto;
                }
                else
                {
                    // Fotoğraf ekleme işlemi sırasında bir hata oluştu hatası
                    throw new Exception("Fotoğraf ekleme işlemi sırasında bir hata oluştu.");
                }
            }

            public async Task<string> GetUserPhoto(string userId)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    // Kullanıcı bulunamadı hatası
                    throw new Exception("Kullanıcı bulunamadı.");
                }
                var existingPhoto = await _context.userPhoto.FirstOrDefaultAsync(photo => photo.UserId == userId);

                if (existingPhoto != null)
                {
                byte[] photoData = existingPhoto.PhotoData;
                string base64String = Convert.ToBase64String(photoData);
                string prefixedBase64 = $"data:image/jpeg;base64,{base64String}";
                
                return prefixedBase64;
                }
                else
                {
                    // Kullanıcıya ait fotoğraf bulunamadı hatası
                    throw new Exception("Kullanıcıya ait fotoğraf bulunamadı.");
                }
            }

    
            
            public async Task DeleteProfilePhoto(string userId)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    // Kullanıcı bulunamadı hatası
                    throw new Exception("Kullanıcı bulunamadı.");
                }

                var userPhoto = await _context.userPhoto.FirstOrDefaultAsync(photo => photo.UserId == userId);
                if (userPhoto == null)
                {
                    // Kullanıcıya ait profil fotoğrafı bulunamadı hatası
                    throw new Exception("Kullanıcıya ait profil fotoğrafı bulunamadı.");
                }

                _context.userPhoto.Remove(userPhoto);
                _context.SaveChanges();
            }
            public async Task<UserPhoto> UpdateProfilePhoto(string userId, string base64)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    // Kullanıcı bulunamadı hatası
                    throw new Exception("Kullanıcı bulunamadı.");
                }

                if (base64.StartsWith("data:image/jpeg;base64,"))
                {
                    base64 = base64.Replace("data:image/jpeg;base64,", "");
                }   

                byte[] photoData;
                try
                {
                    photoData = Convert.FromBase64String(base64);
                }
                catch (FormatException ex)
                {
                    // Base64 kodu geçersiz hatası
                    throw new FormatException("Geçersiz base64 kodu.", ex);
                }

                var existingPhoto = await _context.userPhoto.FirstOrDefaultAsync(photo => photo.UserId == userId);
                if (existingPhoto != null)
                {
                    existingPhoto.PhotoData = photoData;
                    await _context.SaveChangesAsync();
                    return existingPhoto;
                }
                else
                {
                    // Kullanıcıya ait profil fotoğrafı bulunamadı hatası
                    throw new Exception("Kullanıcıya ait profil fotoğrafı bulunamadı.");
                }
            }

        
        }
        
    }