using MonkeNotificationLib;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveWatch.Librarys
{
    public class Callbacks : MonoBehaviourPunCallbacks
    {
        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            base.OnMasterClientSwitched(newMasterClient);
            if (newMasterClient == PhotonNetwork.LocalPlayer)
            {
                NotificationController.AppendMessage("EveWatch", "You are now master client!".WrapColor("warning"));
            } 
        }
    }
}
