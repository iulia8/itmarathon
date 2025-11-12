using CSharpFunctionalExtensions;
using Epam.ItMarathon.ApiService.Application.UseCases.User.Commands;
using Epam.ItMarathon.ApiService.Application.UseCases.User.Queries;
using Epam.ItMarathon.ApiService.Domain.Abstract;
using Epam.ItMarathon.ApiService.Domain.Shared.ValidationErrors;
using FluentValidation.Results;
using MediatR;
using RoomAggregate = Epam.ItMarathon.ApiService.Domain.Aggregate.Room.Room;

namespace Epam.ItMarathon.ApiService.Application.UseCases.User.Handlers
{
    public class DeleteUserHandler(IRoomRepository roomRepository)
        : IRequestHandler<DeleteUserRequest, Result<RoomAggregate, ValidationResult>>
    {
        ///<inheritdoc/>
        public async Task<Result<RoomAggregate, ValidationResult>> Handle(DeleteUserRequest request,
            CancellationToken cancellationToken)
        {
            // 1. Get room by user code
            var roomResult = await roomRepository.GetByUserCodeAsync(request.UserCode, cancellationToken);
            if (roomResult.IsFailure)
            {
                return roomResult;
            }

            // 2. Delete user by id in room's users - room.DeleteUser(userId)
            var room = roomResult.Value;
            var deleteResult = room.DeleteUser(request.UserId);
            if (deleteResult.IsFailure)
            {
                return deleteResult;
            }

            // 3. Update room in repository
            var updateResult = await roomRepository.UpdateAsync(room, cancellationToken);
            if (updateResult.IsFailure)
            {
                return Result.Failure<RoomAggregate, ValidationResult>(new BadRequestError([
                    new ValidationFailure(string.Empty, updateResult.Error)
                ]));
            }

            // 4. Get updated room
            var updatedRoomResult = await roomRepository.GetByUserCodeAsync(request.UserCode, cancellationToken);
            return updatedRoomResult;

            // var authUserResult = await userRepository.GetByCodeAsync(request.UserCode, cancellationToken,
            //     includeRoom: true, includeWishes: true);
            // if (authUserResult.IsFailure)
            // {
            //     return authUserResult.ConvertFailure<List<UserEntity>>();
            // }

            // if (request.UserId is null)
            // {
            //     // Get all users in room
            //     var roomId = authUserResult.Value.RoomId;
            //     var result = await userRepository.GetManyByRoomIdAsync(roomId, cancellationToken);
            //     return result;
            // }

            // // Otherwise, Get user by id
            // var requestedUserResult = await userRepository.GetByIdAsync(request.UserId.Value, cancellationToken,
            //     includeRoom: false, includeWishes: true);
            // if (requestedUserResult.IsFailure)
            // {
            //     return requestedUserResult.ConvertFailure<List<UserEntity>>();
            // }

            // if (requestedUserResult.Value.RoomId != authUserResult.Value.RoomId)
            // {
            //     return Result.Failure<List<UserEntity>, ValidationResult>(new NotAuthorizedError([
            //         new ValidationFailure("id", "User with userCode and user with Id belongs to different rooms.")
            //     ]));
            // }

            // return new List<UserEntity> { requestedUserResult.Value, authUserResult.Value };
        }
    }
}