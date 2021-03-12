using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Networking;

namespace Project.Player
{
    public class PlayerManager : MonoBehaviour
    {

        const float BARREL_PIVOT_OFFSET = 90.0f;

        [Header("Data")]
        [SerializeField]
        private float speed = 2;
        [SerializeField]
        private float rotation = 60;

        [Header("Object References")]
        [SerializeField]
        private Transform barrelPivot;

        [Header("Class References")]
        [SerializeField]
        private NetworkIdentity networkIdentity;

        private float lastRotation;

        void Start()
        {

        }


        void Update()
        {
            if (networkIdentity.IsControlling())
            {
                checkMovement();
                checkAiming();
            }
        }

        public float GetLastRotation()
        {
            return lastRotation;
        }

        public void SetRotation(float Value)
        {
            barrelPivot.rotation = Quaternion.Euler(0, 0, Value + BARREL_PIVOT_OFFSET);
        }

        private void checkMovement()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            transform.position += -transform.up * vertical * speed * Time.deltaTime;
            transform.Rotate(new Vector3(0, 0, -horizontal * rotation * Time.deltaTime));
        }

        private void checkAiming()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dif = mousePosition - transform.position;
            dif.Normalize();
            float rot = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;

            lastRotation = rot;
            barrelPivot.rotation = Quaternion.Euler(0, 0, rot + BARREL_PIVOT_OFFSET);
        }
    }

}
