using AutoMapper;
using CaptureMatchApi.Entities;
using CaptureMatchApi.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CaptureMatchApi.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> userManager;

        public UnitOfWork(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IUserRepository UserRepository => new UserRepository(_context, _mapper, userManager);

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0; 
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}
