﻿using Fiap.CloudGames.Fase1.Application.DTOs.Shared.ValueObjects;

namespace Fiap.CloudGames.Fase1.Application.DTOs.Shared;

public class ResultDto
{
    public bool Success { get; init; }
    public Error? Error { get; init; }
    public string? Message { get; init; }

    public static ResultDto Ok(string? message = null) => new ResultDto { Success = true, Message = message };
    public static ResultDto Fail(Error error) => new ResultDto { Success = false, Error = error };
}

public class ResultDto<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public Error? Error { get; init; }
    public string? Message { get; init; }

    public static ResultDto<T> Ok(T data, string? message = null) => new ResultDto<T> { Success = true, Data = data, Message = message };
    public static ResultDto<T> Fail(Error error) => new ResultDto<T> { Success = false, Error = error };
}