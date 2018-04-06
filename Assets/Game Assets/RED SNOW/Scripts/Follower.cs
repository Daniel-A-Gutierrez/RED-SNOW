using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour {

		public Transform target;
        public Vector3 offset = new Vector3(0f, 0, 0f);

        private void LateUpdate()
        {

            transform.position = target.position + offset;
			
        }
}
