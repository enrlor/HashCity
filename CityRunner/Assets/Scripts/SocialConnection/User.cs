using System;
using System.Globalization;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class User
{

    public string id { get; private set; }
    public string name { get; private set; }
    public string creationDate { get; private set; }
    public string language { get; private set; }


    public User(string i, string n, string date, string lang)
    {

        this.id = i;
        this.name = n;
        this.language = lang;
        this.creationDate = DateTime.ParseExact(date,
                                                "ddd MMM dd HH:mm:ss zzz yyyy",
                                                CultureInfo.InvariantCulture,
                                                DateTimeStyles.AdjustToUniversal).ToShortDateString();


    }

    public User() { }

    public abstract void GetUser(Twitter.AccessToken.AccessTokenResponse response, string name, SocialConnection.GetJSONCallback callback);


}
