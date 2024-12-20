using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConnectData
{
    private static readonly string Host = "http://77.91.78.231:8081/";
    public static readonly string RegistrationUrl = $"{Host}auth/register";
    public static readonly string LoginUrl = $"{Host}auth/login";
    public static readonly string UserWorksUrl = $"{Host}works/";
    public static readonly string AddWorkBaseUrl = $"{Host}works/";

    public static string GetAddWorkUrl(int userId)
    {
        return $"{AddWorkBaseUrl}{userId}/add";
    }

    public static string GetUserWorksUrl(int userId)
    {
        return $"{UserWorksUrl}{userId}";
    }

    public static string GetDeleteWorkUrl(int userId, int workId)
    {
        return $"{Host}works/{userId}/{workId}/delete";
    }

    public static string GetUserAvatarUrl(int userId)
    {
        return $"{Host}user/{userId}/avatar";
    }

    public static string GetUserRoomUrl(int currentRoomID)
    {
        return $"{Host}room/{currentRoomID}/works";
    }

    public static string GetLikeWorkUrl(int workId)
    {
        return $"{Host}works/{workId}/like";
    }

    public static string GetValidationUrl(int workId, string type)
    {
        return $"{Host}works/{workId}/validate/{type}";
    }

    public static string GetAddWorkUrl(int roomId, int slot)
    {
        return $"{Host}room/{roomId}/{slot}/add_work/";
    }

}