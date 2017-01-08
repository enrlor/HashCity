using System.Collections;
using System.Collections.Generic;

public abstract class SocialConnection  {

    public delegate void AckCallback(bool success);
    public delegate void GetMessageListCallback<E>(bool success, List<E> list);
    public delegate void GetJSONCallback(bool success, string jsonEncoded);

    public Authorization authorization;
    public Message message;
    public Timeline timeline;
    public User user;
    public SocialSteam stream;

}
