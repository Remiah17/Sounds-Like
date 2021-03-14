using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListElement : MonoBehaviour
{
    public Text RoomNameText;
    public Text PlayersNumberText;
    public string RoomGameID;
    public Image BodyImage;
    public Image UpperTorsoImage;
    public Image LowerTorsoImage;
    public Image HairImage;
    public Image EyesBGImage;
    public Image PupilsImage;
    public Image EyeLashesImage;
    public Image EyebrowsImage;
    public Image NoseImage;
    public Image MouthImage;
    public Text HostName;
    public AvatarDatabaseScriptableObject AvatarDatabase;
    public void NewRoomListElement(string _roomName, int _numberofPlayer, int _maxPlayers, string _gameID)
    {
        RoomNameText.text = _roomName;
        PlayersNumberText.text = _numberofPlayer + " / " + _maxPlayers;
        RoomGameID = _gameID;
    }

    public void NewAvatarElement(int bodyID, Color bodyColor, int UTID, Color UTColor, int LTID, Color LTColor, int hairID, Color hairColor, int eyesID, Color pupilColor, int eyelashesID, int eyebrowsID, Color eyebrowsColor, int noseID, int mouthID, string hostName)
    {

        BodyImage.color = bodyColor;
        BodyImage.sprite = AvatarDatabase.BodySprite[bodyID - 1];
        UpperTorsoImage.color = UTColor;
        UpperTorsoImage.sprite = AvatarDatabase.UpperTorsoSprite[UTID - 1];
        LowerTorsoImage.color = LTColor;
        LowerTorsoImage.sprite = AvatarDatabase.LowerTorsoSprite[LTID - 1];
        HairImage.sprite = AvatarDatabase.HairSprite[hairID - 1];
        HairImage.color = hairColor;
        EyesBGImage.sprite = AvatarDatabase.EyeBG[eyesID - 1];
        PupilsImage.color = pupilColor;
        EyeLashesImage.sprite = AvatarDatabase.EyeLashes[eyelashesID - 1];
        EyebrowsImage.sprite = AvatarDatabase.EyebrowSprite[eyebrowsID - 1];
        EyebrowsImage.color = eyebrowsColor;
        NoseImage.sprite = AvatarDatabase.NoseSprite[noseID - 1];
        MouthImage.sprite = AvatarDatabase.MouthSprite[mouthID - 1];
        HostName.text = hostName;
    }
}
