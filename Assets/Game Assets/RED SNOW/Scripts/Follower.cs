using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof(Camera))]
public class Follower : MonoBehaviour
{
        private Camera cam;
		public Transform target;
        public Vector3 offset = new Vector3(0f, 0, 0f);
        public float borderRatio;
        Vector2 borderRect;
        public bool isCool;
        public GameObject background;
        float aspectRatio;
        float orthoSize ;

        void Start()
        {
            cam = GetComponent<Camera>();
            aspectRatio = cam.aspect;
            orthoSize = cam.orthographicSize;
            borderRect = new Vector2(aspectRatio*orthoSize*2,orthoSize*2);
        }


        private void LateUpdate()
        {
            if(isCool == false)
            {
                transform.position = target.position + offset;
            }
            if(isCool == true)
            {
                LayerMask mask = LayerMask.GetMask( "Ground", "CameraGround" );
                RaycastHit2D hit = Physics2D.Raycast(target.position,-Vector3.up, 100, mask);
                float scaleFactor;
                Vector2 bkgdScale;
                offset = new Vector3(offset.x,-hit.distance/2,offset.z);
                transform.position = target.position + offset;
                if(Mathf.Abs( CalculateZoom(target.position , cam.transform.position, borderRect))*borderRatio/orthoSize > borderRatio)
                {
                    scaleFactor = CalculateZoom(target.position , transform.position, borderRect) / cam.GetComponent<Camera>().orthographicSize;
                    bkgdScale = new Vector2(background.transform.localScale.x, background.transform.localScale.y) * scaleFactor;
                    background.transform.localScale = new Vector3(bkgdScale.x, bkgdScale.y, background.transform.localScale.z);
                    cam.GetComponent<Camera>().orthographicSize = CalculateZoom(target.position , transform.position, borderRect);
                }     
            }
        }


        float CalculateZoom(Vector2 targetPos, Vector2 cameraPos , Vector2 ortho)
            {
                Vector2 relativePos = targetPos-cameraPos;
                if(Mathf.Abs(relativePos.y/relativePos.x) > Mathf.Abs(ortho.x/ortho.y))
                {
                    return Mathf.Abs(2*relativePos.y/ortho.y)/borderRatio*orthoSize;
                }
                else
                {                    
                    return Mathf.Abs(2*relativePos.x/ortho.x)/borderRatio*orthoSize;
                }
            }        
}
