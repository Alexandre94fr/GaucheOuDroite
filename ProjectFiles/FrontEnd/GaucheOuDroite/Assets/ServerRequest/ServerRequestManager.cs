using System;
using System.Text;
using System.Collections;

using UnityEngine;

using UnityEngine.Networking;
using Newtonsoft.Json;


public static class ServerRequestManager
{
    public enum RequestType
    {
        /// <summary> To read BackEnd's values. </summary>
        Get,

        /// <summary> To read BackEnd header values. </summary>
        // Head, // Not necessary for our project.

        /// <summary> To create on the BackEnd. </summary>
        Post,

        /// <summary> To replace completely BackEnd's values. </summary>
        Put,

        /// <summary> To read BackEnd values. </summary>
        // Create, // Not necessary for our project, Post is enough.

        /// <summary> To delete BackEnd values. </summary>
        Delete
    }


    public static bool IS_DEBUG_MODE_ON = true;

    public static string CLASS_NAME = typeof(ServerRequestManager).Name;


    public static string AuthenticationToken = null;


    public static string RequestResponseToString(UnityWebRequest p_request)
    {
        return
        $"Request response received. Data:\n" +
        $"- Url: {p_request.url}\n" +
        $"- Request type: {p_request.method}\n" +
        $"- Result: {p_request.result}\n" +
        $"- HTTP error: {p_request.error}\n" +
        $"- Response code: {p_request.responseCode}\n" +
        $"- Response body (formated):\n" +
        $"{JsonConvert.DeserializeObject(p_request.downloadHandler.text)}";
    }

    /// <summary>
    /// Sends an HTTP request to the BackEnd API and automatically handles:
    /// <list type="bullet">
    /// <item><description>Request creation (GET, POST, PUT, DELETE).</description></item>
    /// <item><description>JSON serialization of the request body.</description></item>
    /// <item><description>Authentication token injection when required.</description></item>
    /// <item><description>Sending the request and waiting for the server response.</description></item>
    /// <item><description>JSON deserialization of the response body.</description></item>
    /// <item><description>Invoking success or error callbacks depending on the request result.</description></item>
    /// </list>
    /// 
    /// This method should be used as the main entry point for all communications
    /// between the FrontEnd and the BackEnd.
    ///
    /// <para>
    /// The server response body is automatically deserialized into the
    /// specified <typeparamref name="TResponse"/> type.
    /// </para>
    ///
    /// <para>
    /// <b>BEWARE:</b> This method is a coroutine and must be started using
    /// <see cref="MonoBehaviour.StartCoroutine(IEnumerator)"/>.
    /// </para>
    ///
    /// <example>
    /// Code example: Sending a login request:
    /// <code>
    /// StartCoroutine(ServerRequestManager.SendRequest&lt;LogInResultDTO&gt;(
    ///     "authentication/log-in",
    ///     ServerRequestManager.RequestType.Post,
    ///     new LogInDTO
    ///     {
    ///         Username = username,
    ///         Password = password
    ///     },
    ///     p_isAuthenticationTokenNeeded: false,
    ///
    ///     result =>
    ///     {
    ///         Debug.Log($"Successfully log in as {result.Username}");
    ///
    ///         ServerRequestManager.AuthenticationToken = result.Token;
    ///     },
    ///
    ///     request =>
    ///     {
    ///         Debug.LogError(
    ///             $"Failed to log in. Request failed.\n" +
    ///             ServerRequestManager.RequestResponseToString(request)
    ///         );
    ///     })
    /// );
    /// </code>
    /// </example>
    /// </summary>
    /// 
    /// <typeparam name="TResponse"> Type expected from the server response body. </typeparam>
    /// <param name="p_route"> API route relative to "/api/". Example: "authentication/log-in". </param>
    /// <param name="p_requestType"> HTTP request type to send. </param>
    /// <param name="p_requestBody"> Object to serialize as JSON and send to the server. Can be null for requests that do not require a body. </param>
    /// <param name="p_isAuthenticationTokenNeeded"> Indicates whether the request requires an AuthenticationToken. 
    /// If true, the value stored in <see cref="AuthenticationToken"/> will be added to the request headers. 
    /// Be sure that <see cref="AuthenticationToken"/> as been set before passing true.
    /// A warning will be print out if <see cref="AuthenticationToken"/> is equal to null or "". </param>
    /// <param name="p_onSuccess"> Callback invoked when the request succeeds. Receives the deserialized server response. </param>
    /// <param name="p_onError"> Callback invoked when the request fails. Receives the original UnityWebRequest for error inspection. </param>
    /// 
    /// <returns> IEnumerator used by Unity's coroutine system. </returns>
    public static IEnumerator SendRequest<TResponse>(
        string p_route,
        RequestType p_requestType,

        object p_requestBody,
        bool p_isAuthenticationTokenNeeded,

        Action<TResponse> p_onSuccess,
        Action<UnityWebRequest> p_onError
    )
    {
        // -- Creating the request -- //

        // http://localhost:5131 For local http request
        // https://localhost:7280 For local https request

        string url = $"https://localhost:7280/api/{p_route}";

        string requestType = p_requestType.ToString().ToUpper();

        if (IS_DEBUG_MODE_ON)
            Debug.Log($"DEBUG: [{CLASS_NAME}] Creating a request.\n{requestType} request for '{url}'.");

        UnityWebRequest request = new(
            url,
            requestType
        );

        // -- Preparing request's body (content) -- //

        if (IS_DEBUG_MODE_ON)
            Debug.Log($"DEBUG: [{CLASS_NAME}] Preparing request's body (content).\n{requestType} request for '{url}'.");

        // Converting the raw body in Json
        string requestBodyJson = JsonConvert.SerializeObject(p_requestBody);

        byte[] body = Encoding.UTF8.GetBytes(requestBodyJson);

        // Will store the data when we send the request to the server
        request.uploadHandler = new UploadHandlerRaw(body);

        // Will store the data when the server will respond
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader(
            "Content-Type", // To tell the BackEnd (server) that we stored a body (content) in Json inside the request, so he can detect it.
            "application/json"
        );

        if (p_isAuthenticationTokenNeeded)
        {
            if (IS_DEBUG_MODE_ON)
                Debug.Log($"DEBUG: [{CLASS_NAME}] Adding to request's body (content) an AuthenticationToken.\n{requestType} request for '{url}'.");

            if (string.IsNullOrEmpty(AuthenticationToken))
            {
                Debug.LogWarning(
                    $"WARNING: [{CLASS_NAME}] Tried to send a server request containing an AuthenticationToken when none have been saved. " +
                    $"Try logging in first. Request canceled. Returning."
                );

                yield break;
            }

            request.SetRequestHeader(
                "Authorization", // To tell the BackEnd (server) that we stored an AuthenticationToken inside the request, so he can detect it.
                $"Bearer {AuthenticationToken}"
            );
        }

        // -- Sending the request and waiting for a response -- //

        if (IS_DEBUG_MODE_ON)
            Debug.Log($"DEBUG: [{CLASS_NAME}] Sending the request to the server.\n{requestType} request for '{url}'.");

        yield return request.SendWebRequest();

        // -- Handling server response -- //

        if (IS_DEBUG_MODE_ON)
            Debug.Log($"DEBUG: [{CLASS_NAME}] Handling server's request response.\n{requestType} request for '{url}'.");

        // Getting and converting the server response body 
        TResponse responseBody;

        try
        {
            responseBody = JsonConvert.DeserializeObject<TResponse>(request.downloadHandler.text);
        }
        catch (Exception exception)
        {
            Debug.LogWarning($"WARNING: [{CLASS_NAME}] Caught an error while trying to deserialize the server response content into '{typeof(TResponse)}' type. Returning.\nError: {exception}");
            yield break;
        }
        
        if (IS_DEBUG_MODE_ON)
            Debug.Log($"DEBUG: [{CLASS_NAME}] {RequestResponseToString(request)}");

        // Invoking the right Action depending of the success of the request
        if (request.result == UnityWebRequest.Result.Success)
        {
            p_onSuccess?.Invoke(responseBody);
        }
        else
        {
            p_onError?.Invoke(request);
        }
    }
}