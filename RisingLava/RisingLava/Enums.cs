internal enum EventCodes : int
{
    StartLava = 135, // Server -> Client
    SendMessage, // Server -> Client
    MoveLava, // Server -> Client
    EndLava, // Server -> Client

    /*Request_Has_Mod, // Server -> Client
    Response_Has_Mod, // Client -> Server*/
}