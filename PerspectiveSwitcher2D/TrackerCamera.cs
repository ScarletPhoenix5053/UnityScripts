using UnityEngine;
using System.Collections;

namespace Sierra.PerspectiveSwitcher2D
{
    /// <summary>
    /// Inherits from Sierra.Camera
    /// </summary>
    public class TrackerCamera : Camera.CameraController
    {
        public PerspectiveSwitcher PerspectiveSwitcher;
        
        protected override void Update()
        {
            if (!Transitioning)
            {
               TrackTarget(LookAt);
            }
            else
            {
               transform.LookAt(LookAt);
            }
        }

        private void TrackTarget(Transform target)
        {           
            switch (PerspectiveSwitcher.CurrentPerspective)
            {
                case PerspectiveSwitcher.CubePerspective.top:
                    TeleportTo(LookAt.position + Vector3.up * DistToSubject);
                    break;
                case PerspectiveSwitcher.CubePerspective.bottom:
                    TeleportTo(LookAt.position + Vector3.down * DistToSubject);
                    break;
                case PerspectiveSwitcher.CubePerspective.left:
                    TeleportTo(LookAt.position + Vector3.left * DistToSubject);
                    break;
                case PerspectiveSwitcher.CubePerspective.right:
                    TeleportTo(LookAt.position + Vector3.right * DistToSubject);
                    break;
                case PerspectiveSwitcher.CubePerspective.back:
                    TeleportTo(LookAt.position + Vector3.back * DistToSubject);
                    break;
                case PerspectiveSwitcher.CubePerspective.front:
                    TeleportTo(LookAt.position + Vector3.forward * DistToSubject);
                    break;
                default:
                    break;
            }

            transform.LookAt(LookAt);
        }
    }
}
