using UnityEngine;
using System.Collections;

public abstract class Timeline {

    public abstract void GetUserTimeline(Twitter.AccessToken.AccessTokenResponse response, int numMaxStatus, SocialConnection.GetMessageListCallback<Message> callback);
    public abstract void GetPersonalTimeline(Twitter.AccessToken.AccessTokenResponse response, int numMaxStatus, SocialConnection.GetMessageListCallback<Message> callback);

}
