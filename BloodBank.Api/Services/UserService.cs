using AutoMapper;
using BloodBank.Api.Models;
using BloodBank.Api.DTOs;
using BloodBank.Api.Repositories;
using BloodBank.Api.Security;

namespace BloodBank.Api.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    public async Task<UserDto> AddUser(CreateUserDto user)
    {
        // Check email uniqueness
        var exists = await _userRepository.GetUserByEmail(user.Email) != null;
        if (exists) throw new InvalidOperationException("Email already registered.");

        //Role validation (Administrator/Staf/Donor)
        var role = await _userRepository.GetRoleById(user.RoleId);
        if (role == null ) throw new ArgumentException("Invalid role.");
        
        // Hash password
        var hashed = PasswordHasher.HashPassword(user.Password);
        var userEntity = new User
        {
            FullName = user.FullName,
            Email = user.Email,
            PasswordHash = hashed,
            Phone = user.Phone,
            RoleId = user.RoleId
        };
         var addedUser = await _userRepository.AddUser(userEntity);
         return _mapper.Map<UserDto>(addedUser);
    }

    public async Task<IEnumerable<UserDto>> GetAllUsers()
    {
        var users = await _userRepository.GetAllUsers();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto> GetUserByEmail(string email)
    {
        var user = await _userRepository.GetUserByEmail(email);
        return _mapper.Map<UserDto>(user);
       }

    public async Task<UserDto> GetUserById(int id)
    {
        var user = await _userRepository.GetUserById(id);
        return _mapper.Map<UserDto>(user)   ;
    }
}