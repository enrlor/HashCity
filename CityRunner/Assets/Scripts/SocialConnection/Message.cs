using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using LitJson;
using System;

public abstract class Message {

    public string creationDate { get; private set; }
    public string id { get; private set; }
    public string text { get; private set; }
    public User author { get; private set; }

    public Message(string d, string i, string t, User user)
    {

        DateTime dt = DateTime.ParseExact(d,
                                          "ddd MMM dd HH:mm:ss zzz yyyy",
                                          CultureInfo.InvariantCulture,
                                          DateTimeStyles.AdjustToUniversal);

        this.creationDate = dt.ToShortDateString();
        this.id = i;
        this.text = t;
        this.author = user;

    }

    public Message() { }

    public abstract void Post(Twitter.AccessToken.AccessTokenResponse response, string text, SocialConnection.AckCallback callback);
    public abstract void Reply(Twitter.AccessToken.AccessTokenResponse response, string text, string id, SocialConnection.AckCallback callback);
    public abstract void Repost(Twitter.AccessToken.AccessTokenResponse response, string text, string id, SocialConnection.AckCallback callback);
    public abstract void SearchFromQuery(Twitter.AccessToken.AccessTokenResponse response, string query, string lastID, SocialConnection.GetJSONCallback callback);



}
