using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Utility.Attributes;
using Project.Player;
using Project.Utility;

namespace Project.Networking
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class NetworkRotation : MonoBehaviour
    {
        [Header("References Values")]
        [SerializeField]
        [GreyOut]
        private float oldTankRotation;
        [SerializeField]
        [GreyOut]
        private float oldBarrelRotation;

        [Header("Class References")]
        [SerializeField]
        private PlayerManager playermanager;

        private NetworkIdentity networkIdentity;
        private PlayerRotation player;

        private float stillCounter = 0;

        void Start()
        {
            networkIdentity = GetComponent<NetworkIdentity>();

            player = new PlayerRotation();
            player.tankRotation = 0;
            player.barrelRotation = 0;

            if (!networkIdentity.IsControlling())
            {
                enabled = false;
            }

        }

        void Update()
        {
            if (networkIdentity.IsControlling())
            {
                if (oldTankRotation != transform.localEulerAngles.z || oldBarrelRotation != playermanager.GetLastRotation())
                {
                    oldTankRotation = transform.localEulerAngles.z;
                    oldBarrelRotation = playermanager.GetLastRotation();
                    stillCounter = 0;
                    sendData();
                }
                else
                {
                    stillCounter += Time.deltaTime;
                    if (stillCounter >= 1)
                    {
                        stillCounter = 0;
                        sendData();
                    }
                }
            }
        }

        private void sendData()
        {
            player.tankRotation = transform.localEulerAngles.z.TwoDecimals();
            player.barrelRotation = playermanager.GetLastRotation().TwoDecimals();


            networkIdentity.GetSocket().Emit("updateRotation", new JSONObject(JsonUtility.ToJson(player)));
        }
    }
}

