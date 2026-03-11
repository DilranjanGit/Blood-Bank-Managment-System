using AutoMapper;
using BloodBank.Api.Models;
using BloodBank.Api.DTOs;
using BloodBank.Api.Repositories;

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
    public Task<CreateUserDto> AddUser(CreateUserDto user)
    {
         var addedUser = _userRepository.AddUser(user).Result;
         return Task.FromResult(_mapper.Map<CreateUserDto>(addedUser));
    }

    public Task<IEnumerable<CreateUserDto>> GetAllUsers()
    {
        var users = _userRepository.GetAllUsers().Result;
        return Task.FromResult(_mapper.Map<IEnumerable<CreateUserDto>>(users));
    }

    public Task<CreateUserDto> GetUserByEmail(string email)
    {
        var user = _userRepository.GetUserByEmail(email).Result;
        return Task.FromResult(_mapper.Map<CreateUserDto>(user));
       }

    public Task<CreateUserDto> GetUserById(int id)
    {
        var user = _userRepository.GetUserById(id).Result;
        return Task.FromResult(_mapper.Map<CreateUserDto>(user));
    }
}